// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletTokenBalancesStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.DefiLlama.Interfaces.Models;
using Nomis.Utils.Contracts.Stats;

namespace Nomis.DefiLlama.Interfaces.Stats
{
    /// <summary>
    /// Wallet hold token balances stats.
    /// </summary>
    public interface IWalletTokenBalancesStats :
        IWalletStats
    {
        /// <summary>
        /// Hold token balances.
        /// </summary>
        public IEnumerable<TokenBalanceData>? TokenBalances { get; init; }

        /// <summary>
        /// Wallet hold tokens total balance (USD).
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public decimal HoldTokensBalanceUSD { get; }

        /// <summary>
        /// Get wallet DEX token balances stats score.
        /// </summary>
        /// <returns>Returns DEX token balances stats score.</returns>
        public new double GetScore()
        {
            // TODO - add calculation
            return 0;
        }
    }
}