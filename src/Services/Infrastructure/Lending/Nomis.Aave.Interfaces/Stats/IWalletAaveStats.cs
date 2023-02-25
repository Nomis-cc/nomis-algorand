// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletAaveStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Aave.Interfaces.Responses;
using Nomis.Utils.Contracts.Stats;

namespace Nomis.Aave.Interfaces.Stats
{
    /// <summary>
    /// Wallet Aave protocol stats.
    /// </summary>
    public interface IWalletAaveStats :
        IWalletStats
    {
        /// <summary>
        /// Aave user account data.
        /// </summary>
        public AaveUserAccountDataResponse? AaveData { get; set; }

        /// <summary>
        /// Get wallet Aave protocol stats score.
        /// </summary>
        /// <returns>Returns wallet Aave protocol stats score.</returns>
        public new double GetScore()
        {
            // TODO - add calculation
            return 0;
        }
    }
}