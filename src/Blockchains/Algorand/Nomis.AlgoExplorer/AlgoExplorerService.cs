// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerService.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.AlgoExplorer.Calculators;
using Nomis.AlgoExplorer.Interfaces;
using Nomis.AlgoExplorer.Interfaces.Extensions;
using Nomis.AlgoExplorer.Settings;
using Nomis.Blockchain.Abstractions;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Extensions;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.Blockchain.Abstractions.Requests;
using Nomis.Blockchain.Abstractions.Stats;
using Nomis.Coingecko.Interfaces;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Contracts;
using Nomis.DefiLlama.Interfaces.Extensions;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Domain.Scoring.Entities;
using Nomis.ScoringService.Interfaces;
using Nomis.SoulboundTokenService.Interfaces;
using Nomis.Utils.Contracts.Services;
using Nomis.Utils.Wrapper;

namespace Nomis.AlgoExplorer
{
    /// <inheritdoc cref="IAlgorandScoringService"/>
    internal sealed class AlgoExplorerService :
        BlockchainDescriptor,
        IAlgorandScoringService,
        IHasDefiLlamaChainId,
        ITransientService
    {
        private readonly IAlgoExplorerClient _client;
        private readonly ICoingeckoService _coingeckoService;
        private readonly IScoringService _scoringService;
        private readonly INonEvmSoulboundTokenService _soulboundTokenService;
        private readonly IDefiLlamaService _defiLlamaService;

        /// <summary>
        /// Initialize <see cref="AlgoExplorerService"/>.
        /// </summary>
        /// <param name="settings"><see cref="AlgoExplorerSettings"/>.</param>
        /// <param name="client"><see cref="IAlgoExplorerClient"/>.</param>
        /// <param name="coingeckoService"><see cref="ICoingeckoService"/>.</param>
        /// <param name="scoringService"><see cref="IScoringService"/>.</param>
        /// <param name="soulboundTokenService"><see cref="INonEvmSoulboundTokenService"/>.</param>
        /// <param name="defiLlamaService"><see cref="IDefiLlamaService"/>.</param>
        public AlgoExplorerService(
            IOptions<AlgoExplorerSettings> settings,
            IAlgoExplorerClient client,
            ICoingeckoService coingeckoService,
            IScoringService scoringService,
            INonEvmSoulboundTokenService soulboundTokenService,
            IDefiLlamaService defiLlamaService)
            : base(settings.Value.BlockchainDescriptor)
        {
            _client = client;
            _coingeckoService = coingeckoService;
            _scoringService = scoringService;
            _soulboundTokenService = soulboundTokenService;
            _defiLlamaService = defiLlamaService;
        }

        /// <inheritdoc />
        public string DefiLLamaChainId => "algorand";

        /// <inheritdoc/>
        public async Task<Result<TWalletScore>> GetWalletStatsAsync<TWalletStatsRequest, TWalletScore, TWalletStats, TTransactionIntervalData>(
            TWalletStatsRequest request,
            CancellationToken cancellationToken = default)
            where TWalletStatsRequest : WalletStatsRequest
            where TWalletScore : IWalletScore<TWalletStats, TTransactionIntervalData>, new()
            where TWalletStats : class, IWalletCommonStats<TTransactionIntervalData>, new()
            where TTransactionIntervalData : class, ITransactionIntervalData
        {
            var account = await _client.GetAccountDataAsync(request.Address);
            var balanceWei = account.Amount;
            decimal usdBalance =
                (await _defiLlamaService.GetTokensPriceAsync(new List<string> { "coingecko:algorand" }))?.TokensPrices["coingecko:algorand"].Price * balanceWei.ToAlgo() ?? 0;
            var transactions = (await _client.GetTransactionsAsync(request.Address)).ToList();
            var assets = account.Assets;

            #region Tokens data

            var tokensData = new List<(string TokenContractId, string TokenContractIdWithBlockchain, decimal? Balance)>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var assetsWithBalance = assets.Select(a => new
                {
                    a.AssetId,
                    a.Amount
                })
                .DistinctBy(a => a.AssetId);
                foreach (var assetWithBalance in assetsWithBalance)
                {
                    decimal tokenBalance = (decimal)assetWithBalance.Amount;
                    if (tokenBalance > 0)
                    {
                        tokensData.Add((assetWithBalance.AssetId.ToString(), $"{DefiLLamaChainId}:{assetWithBalance.AssetId}", tokenBalance));
                    }
                }
            }

            #endregion Tokens data

            #region Tokens balances (DefiLlama)

            var tokenBalances = new List<TokenBalanceData>();
            if ((request as IWalletTokensBalancesRequest)?.GetHoldTokensBalances == true)
            {
                var tokenPrices = await _defiLlamaService.GetTokensPriceAsync(
                    tokensData.Select(t => t.TokenContractIdWithBlockchain).ToList(),
                    (request as IWalletTokensBalancesRequest)?.SearchWidthInHours ?? 6);
                if (tokenPrices != null)
                {
                    tokenBalances.AddRange(tokenPrices.GetTokenBalanceData(tokensData).ToList());
                }
            }

            #endregion Tokens balances

            var walletStats = new AlgorandStatCalculator(
                    request.Address,
                    (decimal)balanceWei,
                    usdBalance,
                    account,
                    transactions,
                    assets,
                    tokenBalances)
                .GetStats() as TWalletStats;

            double score = walletStats!.GetScore<TWalletStats, TTransactionIntervalData>();
            var scoringData = new ScoringData(request.Address, request.Address, ChainId, score, JsonSerializer.Serialize(walletStats));
            await _scoringService.SaveScoringDataToDatabaseAsync(scoringData, cancellationToken);

            // getting signature
            ushort mintedScore = (ushort)(score * 10000);
            var signatureResult = _soulboundTokenService.GetSoulboundTokenSignature(new()
            {
                Score = mintedScore,
                ScoreType = request.ScoreType,
                To = request.Address,
                Nonce = request.Nonce,
                ChainId = ChainId,
                ContractAddress = SBTContractAddresses?.ContainsKey(request.ScoreType) == true ? SBTContractAddresses?[request.ScoreType] : null,
                Deadline = request.Deadline
            });

            var messages = signatureResult.Messages;
            messages.Add($"Got {ChainName} wallet {request.ScoreType.ToString()} score.");
            return await Result<TWalletScore>.SuccessAsync(
                new()
                {
                    Address = request.Address,
                    Stats = walletStats,
                    Score = score,
                    MintedScore = mintedScore,
                    Signature = signatureResult.Data.Signature
                }, messages);
        }
    }
}