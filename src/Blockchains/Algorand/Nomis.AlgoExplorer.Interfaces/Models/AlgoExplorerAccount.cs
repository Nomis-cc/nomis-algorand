// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerAccount.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AlgoExplorer.Interfaces.Models
{
    /// <summary>
    /// AlgoExplorer account data.
    /// </summary>
    public class AlgoExplorerAccount
    {
        /// <summary>
        /// Address.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Amount without pending rewards.
        /// </summary>
        [JsonPropertyName("amount-without-pending-rewards")]
        public BigInteger AmountWithoutPendingRewards { get; set; }

        /// <summary>
        /// Assets.
        /// </summary>
        [JsonPropertyName("assets")]
        public IList<AlgoExplorerAccountAsset> Assets { get; set; } = new List<AlgoExplorerAccountAsset>();

        /// <summary>
        /// Status.
        /// </summary>
        [JsonPropertyName("status")]
        public string? Status { get; set; }

        /// <summary>
        /// Total created apps.
        /// </summary>
        [JsonPropertyName("total-created-apps")]
        public int TotalCreatedApps { get; set; }

        /// <summary>
        /// Total created assets.
        /// </summary>
        [JsonPropertyName("total-created-assets")]
        public int TotalCreatedAssets { get; set; }
    }
}