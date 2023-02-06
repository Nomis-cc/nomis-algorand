// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgorandWalletStatsRequest.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Nomis.Blockchain.Abstractions.Contracts;
using Nomis.Blockchain.Abstractions.Requests;

namespace Nomis.AlgoExplorer.Interfaces.Requests
{
    /// <summary>
    /// Request for getting the wallet stats for Algorand.
    /// </summary>
    public sealed class AlgorandWalletStatsRequest :
        WalletStatsRequest,
        IWalletTokensBalancesRequest
    {
        /// <inheritdoc />
        /// <example>true</example>
        [FromQuery]
        public bool GetHoldTokensBalances { get; set; } = true;

        /// <inheritdoc />
        /// <example>6</example>
        [FromQuery]
        [Range(typeof(int), "1", "8760")]
        public int SearchWidthInHours { get; set; } = 6;
    }
}