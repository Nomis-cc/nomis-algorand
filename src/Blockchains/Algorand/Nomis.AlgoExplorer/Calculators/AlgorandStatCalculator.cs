// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgorandStatCalculator.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

using Nomis.AlgoExplorer.Interfaces.Extensions;
using Nomis.AlgoExplorer.Interfaces.Models;
using Nomis.Blockchain.Abstractions.Calculators;
using Nomis.Blockchain.Abstractions.Models;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Utils.Extensions;

namespace Nomis.AlgoExplorer.Calculators
{
    /// <summary>
    /// Algorand wallet stats calculator.
    /// </summary>
    internal sealed class AlgorandStatCalculator :
        IStatCalculator<AlgorandWalletStats, AlgorandTransactionIntervalData>
    {
        private readonly string _address;
        private readonly decimal _balance;
        private readonly decimal _usdBalance;
        private readonly AlgoExplorerAccount _account;
        private readonly IEnumerable<AlgoExplorerTransaction> _transactions;
        private readonly IEnumerable<AlgoExplorerAccountAsset> _assets;
        private readonly IEnumerable<TokenBalanceData>? _tokenBalances;

        public AlgorandStatCalculator(
            string address,
            decimal balance,
            decimal usdBalance,
            AlgoExplorerAccount account,
            IEnumerable<AlgoExplorerTransaction> transactions,
            IEnumerable<AlgoExplorerAccountAsset> assets,
            IEnumerable<TokenBalanceData>? tokenBalances)
        {
            _address = address;
            _balance = balance;
            _usdBalance = usdBalance;
            _account = account;
            _transactions = transactions;
            _assets = assets;
            _tokenBalances = tokenBalances;
        }

        public AlgorandWalletStats GetStats()
        {
            if (!_transactions.Any())
            {
                return new()
                {
                    NoData = true
                };
            }

            var intervals = IStatCalculator
                .GetTransactionsIntervals(_transactions.Select(x => x.RoundTime.ToString().ToDateTime())).ToList();
            if (intervals.Count == 0)
            {
                return new()
                {
                    NoData = true
                };
            }

            var monthAgo = DateTime.Now.AddMonths(-1);
            var yearAgo = DateTime.Now.AddYears(-1);

            int contractsCreated = _account.TotalCreatedApps + _account.TotalCreatedAssets;
            var totalTokens = _assets.Select(x => x.AssetId).Distinct();

            var turnoverIntervalsDataList =
                _transactions.Select(x => new TurnoverIntervalsData(
                    x.RoundTime.ToString().ToDateTime(),
                    x.AssetTransfer?.Amount ?? new BigInteger(0),
                    x.Sender?.Equals(_address, StringComparison.InvariantCultureIgnoreCase) == true));
            var turnoverIntervals = IStatCalculator<AlgorandTransactionIntervalData>
                .GetTurnoverIntervals(turnoverIntervalsDataList, _transactions.Min(x => x.RoundTime.ToString().ToDateTime())).ToList();

            return new()
            {
                NativeBalance = _balance.ToAlgo(),
                NativeBalanceUSD = _usdBalance,
                WalletAge = IStatCalculator
                    .GetWalletAge(_transactions.Select(x => x.RoundTime.ToString().ToDateTime())),
                TotalTransactions = _transactions.Count(),
                TotalRejectedTransactions = 0,
                MinTransactionTime = intervals.Min(),
                MaxTransactionTime = intervals.Max(),
                AverageTransactionTime = intervals.Average(),
                WalletTurnover = _transactions.Sum(x => (decimal)(x.AssetTransfer?.Amount ?? 0)).ToAlgo(),
                TurnoverIntervals = turnoverIntervals,
                BalanceChangeInLastMonth = IStatCalculator<AlgorandTransactionIntervalData>.GetBalanceChangeInLastMonth(turnoverIntervals),
                BalanceChangeInLastYear = IStatCalculator<AlgorandTransactionIntervalData>.GetBalanceChangeInLastYear(turnoverIntervals),
                LastMonthTransactions = _transactions.Count(x => x.RoundTime.ToString().ToDateTime() > monthAgo),
                LastYearTransactions = _transactions.Count(x => x.RoundTime.ToString().ToDateTime() > yearAgo),
                TimeFromLastTransaction = (int)((DateTime.UtcNow - _transactions.OrderBy(x => x.RoundTime).Last().RoundTime.ToString().ToDateTime()).TotalDays / 30),
                DeployedContracts = contractsCreated,
                TokensHolding = totalTokens.Count(),
                TokenBalances = _tokenBalances?.OrderByDescending(b => b.TotalAmountPrice)
            };
        }
    }
}