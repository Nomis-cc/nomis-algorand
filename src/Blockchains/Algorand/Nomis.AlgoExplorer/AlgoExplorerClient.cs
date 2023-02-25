// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorerClient.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Net.Http.Json;
using System.Text.Json;

using Microsoft.Extensions.Options;
using Nomis.AlgoExplorer.Interfaces;
using Nomis.AlgoExplorer.Interfaces.Models;
using Nomis.AlgoExplorer.Settings;
using Nomis.Blockchain.Abstractions.Converters;
using Nomis.Utils.Exceptions;

namespace Nomis.AlgoExplorer
{
    /// <inheritdoc cref="IAlgoExplorerClient"/>
    internal sealed class AlgoExplorerClient :
        IAlgoExplorerClient
    {
        private const int ItemsFetchLimit = 1000;

        private readonly HttpClient _nodeClient;
        private readonly HttpClient _indexerClient;

        /// <summary>
        /// Initialize <see cref="AlgoExplorerClient"/>.
        /// </summary>
        /// <param name="algoExplorerSettings"><see cref="AlgoExplorerSettings"/>.</param>
        public AlgoExplorerClient(
            IOptions<AlgoExplorerSettings> algoExplorerSettings)
        {
            _nodeClient = new()
            {
                BaseAddress = new(algoExplorerSettings.Value.NodeApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(algoExplorerSettings.Value.NodeApiBaseUrl)))
            };
            _indexerClient = new()
            {
                BaseAddress = new(algoExplorerSettings.Value.IndexerApiBaseUrl ??
                                  throw new ArgumentNullException(nameof(algoExplorerSettings.Value.IndexerApiBaseUrl)))
            };
        }

        /// <inheritdoc/>
        public async Task<AlgoExplorerAccount> GetAccountDataAsync(string address)
        {
            string request =
                $"/v2/accounts/{address}";

            var response = await _nodeClient.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AlgoExplorerAccount>(new JsonSerializerOptions
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account balance.");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<AlgoExplorerTransaction>> GetTransactionsAsync(string address)
        {
            var result = new List<AlgoExplorerTransaction>();
            var transactionsData = await GetTransactionList(address).ConfigureAwait(false);
            result.AddRange(transactionsData.Transactions ?? new List<AlgoExplorerTransaction>());
            while (!string.IsNullOrWhiteSpace(transactionsData.NextToken))
            {
                transactionsData = await GetTransactionList(address, transactionsData.NextToken).ConfigureAwait(false);
                result.AddRange(transactionsData.Transactions ?? new List<AlgoExplorerTransaction>());
            }

            return result;
        }

        private async Task<AlgoExplorerTransactions> GetTransactionList(
            string address,
            string? next = null)
        {
            string request =
                $"/v2/accounts/{address}/transactions?limit={ItemsFetchLimit}";

            if (!string.IsNullOrWhiteSpace(next))
            {
                request = $"{request}&next={next}";
            }

            var response = await _indexerClient.GetAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<AlgoExplorerTransactions>(new JsonSerializerOptions
                   {
                       Converters = { new BigIntegerConverter() }
                   })
.ConfigureAwait(false) ?? throw new CustomException("Can't get account transactions.");
        }
    }
}