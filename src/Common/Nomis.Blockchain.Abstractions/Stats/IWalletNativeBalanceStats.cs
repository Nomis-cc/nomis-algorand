// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletNativeBalanceStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Utils.Contracts.Stats;

namespace Nomis.Blockchain.Abstractions.Stats
{
    /// <summary>
    /// Wallet native token balance stats.
    /// </summary>
    public interface IWalletNativeBalanceStats :
        IWalletStats
    {
        private const double BalancePercents = 26.88 / 100;
        private const double WalletTurnoverPercents = 16.31 / 100;

        /// <summary>
        /// Native token symbol.
        /// </summary>
        public string NativeToken { get; }

        /// <summary>
        /// Wallet balance (Native token).
        /// </summary>
        public decimal NativeBalance { get; init; }

        /// <summary>
        /// Wallet balance (Native token in USD).
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public decimal NativeBalanceUSD { get; init; }

        /// <summary>
        /// The balance change value in the last month (Native token).
        /// </summary>
        public decimal BalanceChangeInLastMonth { get; init; }

        /// <summary>
        /// The balance change value in the last year (Native token).
        /// </summary>
        public decimal BalanceChangeInLastYear { get; init; }

        /// <summary>
        /// The movement of funds on the wallet (Native token).
        /// </summary>
        public decimal WalletTurnover { get; init; }

        /// <summary>
        /// Get wallet native token balance stats score.
        /// </summary>
        /// <returns>Returns wallet native token balance stats score.</returns>
        public new double GetScore()
        {
            double result = BalanceScore(NativeBalance) / 100 * BalancePercents;
            result += WalletTurnoverScore(WalletTurnover) / 100 * WalletTurnoverPercents;

            return result;
        }

        private static double BalanceScore(decimal balance)
        {
            return balance switch
            {
                < 0m => 7.7,
                < 0.2m => 23.05,
                < 0.4m => 22.23,
                < 0.7m => 65.98,
                _ => 100.0
            };
        }

        private static double WalletTurnoverScore(decimal turnover)
        {
            return turnover switch
            {
                < 10 => 2.76,
                < 50 => 6.38,
                < 100 => 14.71,
                < 1000 => 33.27,
                _ => 60.07
            };
        }
    }
}