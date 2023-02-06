// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.Utils.Contracts.Stats
{
    /// <summary>
    /// Wallet stats.
    /// </summary>
    public interface IWalletStats
    {
        /// <summary>
        /// Get wallet default stats score.
        /// </summary>
        /// <returns>Returns wallet default stats score.</returns>
        public double GetScore()
        {
            return 0;
        }

        /// <summary>
        /// Get wallet default adjusting score multiplier.
        /// </summary>
        /// <returns>Returns wallet default adjusting score multiplier.</returns>
        public double GetAdjustingScoreMultiplier()
        {
            return 1;
        }
    }
}