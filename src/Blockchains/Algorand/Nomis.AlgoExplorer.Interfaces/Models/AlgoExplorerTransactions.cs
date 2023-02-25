// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerTransactions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Text.Json.Serialization;

namespace Nomis.AlgoExplorer.Interfaces.Models
{
    /// <summary>
    /// AlgoExplorer transactions data.
    /// </summary>
    public class AlgoExplorerTransactions
    {
        /// <summary>
        /// Current round.
        /// </summary>
        [JsonPropertyName("current-round")]
        public BigInteger CurrentRound { get; set; }

        /// <summary>
        /// Next token.
        /// </summary>
        [JsonPropertyName("next-token")]
        public string? NextToken { get; set; }

        /// <summary>
        /// Transactions.
        /// </summary>
        [JsonPropertyName("transactions")]
        public IList<AlgoExplorerTransaction> Transactions { get; set; } = new List<AlgoExplorerTransaction>();
    }
}