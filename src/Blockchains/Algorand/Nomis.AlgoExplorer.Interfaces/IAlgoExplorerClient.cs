// ------------------------------------------------------------------------------------------------------
// <copyright file="IAlgoExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AlgoExplorer.Interfaces.Models;

namespace Nomis.AlgoExplorer.Interfaces
{
    /// <summary>
    /// AlgoExplorer client.
    /// </summary>
    public interface IAlgoExplorerClient
    {
        /// <summary>
        /// Get the account data by address.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns <see cref="AlgoExplorerAccount"/>.</returns>
        Task<AlgoExplorerAccount> GetAccountDataAsync(string address);

        /// <summary>
        /// Get list of transactions of the given account.
        /// </summary>
        /// <param name="address">Account address.</param>
        /// <returns>Returns list of <see cref="AlgoExplorerTransaction"/>.</returns>
        Task<IEnumerable<AlgoExplorerTransaction>> GetTransactionsAsync(string address);
    }
}