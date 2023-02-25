// ------------------------------------------------------------------------------------------------------
// <copyright file="IWalletSnapshotStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using Nomis.Snapshot.Interfaces.Models;
using Nomis.Utils.Contracts.Stats;

namespace Nomis.Snapshot.Interfaces.Stats
{
    /// <summary>
    /// Wallet Snapshot protocol stats.
    /// </summary>
    public interface IWalletSnapshotStats :
        IWalletStats
    {
        /// <summary>
        /// The Snapshot protocol votes.
        /// </summary>
        public IEnumerable<SnapshotProtocolVoteData>? SnapshotVotes { get; init; }

        /// <summary>
        /// The Snapshot protocol proposals.
        /// </summary>
        public IEnumerable<SnapshotProtocolProposalData>? SnapshotProposals { get; init; }

        /// <summary>
        /// Get wallet Snapshot protocol stats score.
        /// </summary>
        /// <returns>Returns wallet Snapshot protocol stats score.</returns>
        public new double GetScore()
        {
            // TODO - add calculation
            return 0;
        }
    }
}