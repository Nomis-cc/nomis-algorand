// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerTransaction.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AlgoExplorer.Interfaces.Models
{
    /// <summary>
    /// AlgoExplorer transaction data.
    /// </summary>
    public class AlgoExplorerTransaction
    {
        /// <summary>
        /// Asset transfer transaction data.
        /// </summary>
        [JsonPropertyName("asset-transfer-transaction")]
        public AlgoExplorerTransactionAssetTransfer? AssetTransfer { get; set; }

        /// <summary>
        /// Genesis hash.
        /// </summary>
        [JsonPropertyName("genesis-hash")]
        public string? GenesisHash { get; set; }

        /// <summary>
        /// Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Round time.
        /// </summary>
        [JsonPropertyName("round-time")]
        public BigInteger RoundTime { get; set; }

        /// <summary>
        /// Sender.
        /// </summary>
        [JsonPropertyName("sender")]
        public string? Sender { get; set; }

        /// <summary>
        /// Transaction type.
        /// </summary>
        [JsonPropertyName("tx-type")]
        public string? TxType { get; set; }
    }
}