// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgorandController.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nomis.AlgoExplorer.Interfaces;
using Nomis.AlgoExplorer.Interfaces.Models;
using Nomis.AlgoExplorer.Interfaces.Requests;
using Nomis.Api.Common.Swagger.Examples;
using Nomis.SoulboundTokenService.Interfaces.Enums;
using Nomis.Utils.Wrapper;
using Swashbuckle.AspNetCore.Annotations;

namespace Nomis.Api.Algorand
{
    /// <summary>
    /// A controller to aggregate all Algorand-related actions.
    /// </summary>
    [Route(BasePath)]
    [ApiVersion("1")]
    [SwaggerTag("Algorand blockchain.")]
    public sealed class AlgorandController :
        ControllerBase
    {
        /// <summary>
        /// Base path for routing.
        /// </summary>
        internal const string BasePath = "api/v{version:apiVersion}/algorand";

        /// <summary>
        /// Common tag for Algorand actions.
        /// </summary>
        internal const string AlgorandTag = "Algorand";

        private readonly ILogger<AlgorandController> _logger;
        private readonly IAlgorandScoringService _scoringService;

        /// <summary>
        /// Initialize <see cref="AlgorandController"/>.
        /// </summary>
        /// <param name="scoringService"><see cref="IAlgorandScoringService"/>.</param>
        /// <param name="logger"><see cref="ILogger{T}"/>.</param>
        public AlgorandController(
            IAlgorandScoringService scoringService,
            ILogger<AlgorandController> logger)
        {
            _scoringService = scoringService ?? throw new ArgumentNullException(nameof(scoringService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get Nomis Score for given wallet address.
        /// </summary>
        /// <param name="request">Request for getting the wallet stats.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/>.</param>
        /// <returns>An Nomis Score value and corresponding statistical data.</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/v1/algorand/wallet/5NPJT5RZD7SLGHON3KO5N4I7LOFN3XVHKI62WXZO3OS666BPG5RLQ7SVHQ/score?scoreType=0&amp;nonce=0&amp;deadline=133160867380732039
        /// </remarks>
        /// <response code="200">Returns Nomis Score and stats.</response>
        /// <response code="400">Address not valid.</response>
        /// <response code="404">No data found.</response>
        /// <response code="500">Unknown internal error.</response>
        [HttpGet("wallet/{address}/score", Name = "GetAlgorandWalletScore")]
        [AllowAnonymous]
        [SwaggerOperation(
            OperationId = "GetAlgorandWalletScore",
            Tags = new[] { AlgorandTag })]
        [ProducesResponseType(typeof(Result<AlgorandWalletScore>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(RateLimitResult), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ErrorResult<string>), StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> GetAlgorandWalletScoreAsync(
            [Required(ErrorMessage = "Request should be set")] AlgorandWalletStatsRequest request,
            CancellationToken cancellationToken = default)
        {
            switch (request.ScoreType)
            {
                case ScoreType.Finance:
                    return Ok(await _scoringService.GetWalletStatsAsync<AlgorandWalletStatsRequest, AlgorandWalletScore, AlgorandWalletStats, AlgorandTransactionIntervalData>(request, cancellationToken));
                default:
                    throw new NotImplementedException();
            }
        }
    }
}