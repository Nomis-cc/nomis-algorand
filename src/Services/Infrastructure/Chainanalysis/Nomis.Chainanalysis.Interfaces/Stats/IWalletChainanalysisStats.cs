// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletChainanalysisStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.Utils.Contracts.Stats;

namespace Nomis.Chainanalysis.Interfaces.Stats
{
    /// <summary>
    /// Wallet Chainanalysis sanctions reporting service stats.
    /// </summary>
    public interface IWalletChainanalysisStats :
        IWalletStats
    {
        /// <summary>
        /// The Chainanalysis sanctions reports.
        /// </summary>
        public IEnumerable<ChainanalysisReport>? ChainanalysisReports { get; init; }

        /// <summary>
        /// Get wallet Chainanalysis stats score.
        /// </summary>
        /// <returns>Returns wallet Chainanalysis protocol stats score.</returns>
        public new double GetScore()
        {
            return 0;
        }

        /// <summary>
        /// Get wallet Chainanalysis adjusting score multiplier.
        /// </summary>
        /// <returns>Returns wallet Chainanalysis adjusting score multiplier.</returns>
        public new double GetAdjustingScoreMultiplier()
        {
            // TODO - add
            return 1;
        }
    }
}