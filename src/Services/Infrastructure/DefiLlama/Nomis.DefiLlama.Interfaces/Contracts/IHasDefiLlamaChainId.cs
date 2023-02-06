// ------------------------------------------------------------------------------------------------------
// <copyright file="IHasDefiLlamaChainId.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

namespace Nomis.DefiLlama.Interfaces.Contracts
{
    /// <summary>
    /// Has DefiLlama chain id.
    /// </summary>
    public interface IHasDefiLlamaChainId
    {
        /// <summary>
        /// DefiLlama chain id for getting token prices.
        /// </summary>
        public string DefiLLamaChainId { get; }
    }
}