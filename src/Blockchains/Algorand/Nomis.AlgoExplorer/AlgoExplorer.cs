// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgoExplorer.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Microsoft.Extensions.DependencyInjection;
using Nomis.AlgoExplorer.Extensions;
using Nomis.AlgoExplorer.Interfaces;

namespace Nomis.AlgoExplorer
{
    /// <summary>
    /// AlgoExplorer service registrar.
    /// </summary>
    public sealed class AlgoExplorer :
        IAlgorandServiceRegistrar
    {
        /// <inheritdoc/>
        public IServiceCollection RegisterService(
            IServiceCollection services)
        {
            return services
                .AddAlgoExplorerService();
        }
    }
}