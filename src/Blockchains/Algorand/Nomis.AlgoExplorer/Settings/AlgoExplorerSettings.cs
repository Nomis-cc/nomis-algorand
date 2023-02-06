// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerSettings.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Blockchain.Abstractions.Contracts;

namespace Nomis.AlgoExplorer.Settings
{
    /// <summary>
    /// Algorand Explorer settings.
    /// </summary>
    internal class AlgoExplorerSettings :
        IBlockchainSettings
    {
        /// <summary>
        /// Node v2 API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://algoexplorer.io/api-dev/v2"/>
        /// </remarks>
        public string? NodeApiBaseUrl { get; set; }

        /// <summary>
        /// Indexer v2 API base URL.
        /// </summary>
        /// <remarks>
        /// <see href="https://algoexplorer.io/api-dev/indexer-v2"/>
        /// </remarks>
        public string? IndexerApiBaseUrl { get; set; }

        /// <inheritdoc />
        public BlockchainDescriptor? BlockchainDescriptor { get; set; }
    }
}