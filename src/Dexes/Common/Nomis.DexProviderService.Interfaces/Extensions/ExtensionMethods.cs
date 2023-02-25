// ------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.DefiLlama.Interfaces;
using Nomis.DefiLlama.Interfaces.Extensions;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Responses;
using Nomis.DexProviderService.Interfaces.Contracts;

namespace Nomis.DexProviderService.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get tokens balances.
        /// </summary>
        /// <param name="service"><see cref="IDefiLlamaService"/>.</param>
        /// <param name="request">Wallet tokens balance request.</param>
        /// <param name="tokensData">Tokens data.</param>
        /// <param name="dexTokensData">DEX tokens data.</param>
        /// <returns>Returns tokens balances.</returns>
        public static async Task<IReadOnlyList<TokenBalanceData>> TokensBalancesAsync(
            this IDefiLlamaService service,
            IWalletTokensBalancesRequest? request,
            IList<(string TokenContractId, string? TokenContractIdWithBlockchain, BigInteger? Balance)> tokensData,
            IReadOnlyList<DexTokenData> dexTokensData)
        {
            var tokenBalances = new List<TokenBalanceData>();
            if (request?.GetHoldTokensBalances == true)
            {
                var tokenPrices = await service.TokensPriceAsync(
                    tokensData.Select(t => t.TokenContractIdWithBlockchain).ToList(),
                    request.SearchWidthInHours).ConfigureAwait(false);
                var tokenBalancesData = tokenPrices != null
                    ? tokenPrices.TokenBalanceData(tokensData).ToList()
                    : new DefiLlamaTokensPriceResponse().TokenBalanceData(tokensData).ToList();

                foreach (var tokenBalanceData in tokenBalancesData)
                {
                    var dexTokenData = dexTokensData
                        .FirstOrDefault(t => t.Id?.Equals(tokenBalanceData.TokenId, StringComparison.OrdinalIgnoreCase) == true);

                    if (dexTokenData != null)
                    {
                        tokenBalanceData.Decimals ??= int.TryParse(dexTokenData.Decimals, out int decimals) ? decimals : null;
                        tokenBalanceData.Symbol ??= dexTokenData.Symbol;
                    }
                }

                tokenBalances.AddRange(tokenBalancesData);
            }

            return tokenBalances;
        }
    }
}