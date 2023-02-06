// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerAccountAsset.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AlgoExplorer.Interfaces.Models
{
    /// <summary>
    /// AlgoExplorer account asset data.
    /// </summary>
    public class AlgoExplorerAccountAsset
    {
        /// <summary>
        /// Amount.
        /// </summary>
        [JsonPropertyName("amount")]
        public BigInteger Amount { get; set; }

        /// <summary>
        /// Asset id.
        /// </summary>
        [JsonPropertyName("asset-id")]
        public BigInteger AssetId { get; set; }

        /// <summary>
        /// Is frozen.
        /// </summary>
        [JsonPropertyName("is-frozen")]
        public bool IsFrozen { get; set; }
    }
}