// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgorandExtensions.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.AlgoExplorer.Interfaces;
using Nomis.Api.Algorand.Settings;
using Nomis.Api.Common.Extensions;
using Nomis.ScoringService.Interfaces.Builder;

namespace Nomis.Api.Algorand.Extensions
{
    /// <summary>
    /// Algorand extension methods.
    /// </summary>
    public static class AlgorandExtensions
    {
        /// <summary>
        /// Add Algorand blockchain.
        /// </summary>
        /// <typeparam name="TServiceRegistrar">The service registrar type.</typeparam>
        /// <param name="optionsBuilder"><see cref="IScoringOptionsBuilder"/>.</param>
        /// <returns>Returns <see cref="IScoringOptionsBuilder"/>.</returns>
        public static IScoringOptionsBuilder WithAlgorandBlockchain<TServiceRegistrar>(
            this IScoringOptionsBuilder optionsBuilder)
            where TServiceRegistrar : IAlgorandServiceRegistrar, new()
        {
            return optionsBuilder
                .With<AlgorandAPISettings, TServiceRegistrar>();
        }
    }
}