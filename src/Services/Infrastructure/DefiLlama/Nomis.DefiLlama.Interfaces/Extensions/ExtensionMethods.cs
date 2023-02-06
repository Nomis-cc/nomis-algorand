// ------------------------------------------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.DefiLlama.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Responses;

namespace Nomis.DefiLlama.Interfaces.Extensions
{
    /// <summary>
    /// DefiLlama extension methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Get token balance data.
        /// </summary>
        /// <param name="tokenPrices">DefiLlama token prices response.</param>
        /// <param name="tokensData">Tokens data.</param>
        public static IEnumerable<TokenBalanceData> GetTokenBalanceData(
            this DefiLlamaTokensPriceResponse tokenPrices,
            List<(string TokenContractId, string TokenContractIdWithBlockchain, decimal? Balance)> tokensData)
        {
            return tokensData
                .Where(t => tokenPrices.TokensPrices.ContainsKey(t.TokenContractIdWithBlockchain))
                .Select(t => new TokenBalanceData(tokenPrices.TokensPrices[t.TokenContractIdWithBlockchain], t.TokenContractId, t.TokenContractIdWithBlockchain, t.Balance));
        }
    }
}