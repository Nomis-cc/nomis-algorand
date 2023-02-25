// ------------------------------------------------------------------------------------------------------
// <copyright file="BaseEvmWalletStats.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Nomis.Blockchain.Abstractions.Models;
using Nomis.Chainanalysis.Interfaces.Models;
using Nomis.Chainanalysis.Interfaces.Stats;
using Nomis.DefiLlama.Interfaces.Models;
using Nomis.DefiLlama.Interfaces.Stats;
using Nomis.Greysafe.Interfaces.Models;
using Nomis.Greysafe.Interfaces.Stats;
using Nomis.Snapshot.Interfaces.Models;
using Nomis.Snapshot.Interfaces.Stats;

namespace Nomis.Blockchain.Abstractions.Stats
{
    /// <summary>
    /// Base EVM wallet stats.
    /// </summary>
    public abstract class BaseEvmWalletStats<TTransactionIntervalData> :
        IWalletCommonStats<TTransactionIntervalData>,
        IWalletNativeBalanceStats,
        IWalletTokenBalancesStats,
        IWalletTransactionStats,
        IWalletTokenStats,
        IWalletContractStats,
        IWalletSnapshotStats,
        IWalletGreysafeStats,
        IWalletChainanalysisStats
        where TTransactionIntervalData : class, ITransactionIntervalData
    {
        /// <inheritdoc/>
        [JsonPropertyOrder(-20)]
        public bool NoData { get; init; }

        /// <inheritdoc/>
        [JsonPropertyOrder(-19)]
        public virtual string NativeToken => "Native token";

        /// <inheritdoc/>
        [Display(Description = "Amount of deployed smart-contracts", GroupName = "number")]
        public int DeployedContracts { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Wallet native token balance", GroupName = "Native token")]
        [JsonPropertyOrder(-18)]
        public decimal NativeBalance { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Wallet native token balance", GroupName = "USD")]
        [JsonPropertyOrder(-17)]
        public decimal NativeBalanceUSD { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Wallet hold tokens total balance", GroupName = "USD")]
        [JsonPropertyOrder(-16)]
        public decimal HoldTokensBalanceUSD => TokenBalances?.Sum(b => b.TotalAmountPrice) ?? 0;

        /// <inheritdoc/>
        [Display(Description = "The movement of funds on the wallet", GroupName = "Native token")]
        [JsonPropertyOrder(-15)]
        public decimal WalletTurnover { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The balance change value in the last month", GroupName = "Native token")]
        [JsonPropertyOrder(-14)]
        public decimal BalanceChangeInLastMonth { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The balance change value in the last year", GroupName = "Native token")]
        [JsonPropertyOrder(-13)]
        public decimal BalanceChangeInLastYear { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Wallet age", GroupName = "months")]
        [JsonPropertyOrder(-12)]
        public int WalletAge { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Total transactions on wallet", GroupName = "number")]
        [JsonPropertyOrder(-11)]
        public int TotalTransactions { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Total rejected transactions on wallet", GroupName = "number")]
        [JsonPropertyOrder(-10)]
        public int TotalRejectedTransactions { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Average time interval between transactions", GroupName = "hours")]
        [JsonPropertyOrder(-9)]
        public double AverageTransactionTime { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Maximum time interval between transactions", GroupName = "hours")]
        [JsonPropertyOrder(-8)]
        public double MaxTransactionTime { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Minimal time interval between transactions", GroupName = "hours")]
        [JsonPropertyOrder(-7)]
        public double MinTransactionTime { get; init; }

        /// <inheritdoc/>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<TTransactionIntervalData>? TurnoverIntervals { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Time since last transaction", GroupName = "months")]
        [JsonPropertyOrder(-6)]
        public int TimeFromLastTransaction { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Last month transactions", GroupName = "number")]
        [JsonPropertyOrder(-5)]
        public int LastMonthTransactions { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Last year transactions on wallet", GroupName = "number")]
        [JsonPropertyOrder(-4)]
        public int LastYearTransactions { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Average transaction per months", GroupName = "number")]
        [JsonPropertyOrder(-3)]
        public double TransactionsPerMonth => WalletAge != 0 ? (double)TotalTransactions / WalletAge : 0;

        /// <inheritdoc/>
        [Display(Description = "Value of all holding tokens", GroupName = "number")]
        [JsonPropertyOrder(-2)]
        public int TokensHolding { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The Snapshot protocol votes", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<SnapshotProtocolVoteData>? SnapshotVotes { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The Snapshot protocol proposals", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<SnapshotProtocolProposalData>? SnapshotProposals { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The Greysafe scam reports data", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<GreysafeReport>? GreysafeReports { get; init; }

        /// <inheritdoc/>
        [Display(Description = "The Greysafe sanctions reports data", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IEnumerable<ChainanalysisReport>? ChainanalysisReports { get; init; }

        /// <inheritdoc/>
        [Display(Description = "Hold tokens balances", GroupName = "collection")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyOrder(-1)]
        public IEnumerable<TokenBalanceData>? TokenBalances { get; init; }

        /// <inheritdoc/>
        [JsonPropertyOrder(int.MaxValue)]
        public virtual IDictionary<string, PropertyData> StatsDescriptions => GetType()
            .GetProperties()
            .Where(prop => !ExcludedStatDescriptions.Contains(prop.Name) && Attribute.IsDefined(prop, typeof(DisplayAttribute)) && !Attribute.IsDefined(prop, typeof(JsonIgnoreAttribute)))
            .ToDictionary(p => p.Name, p => new PropertyData(p, NativeToken));

        /// <inheritdoc/>
        [JsonIgnore]
        public virtual IEnumerable<Func<double>> AdditionalScores => new List<Func<double>>
        {
            () => (this as IWalletSnapshotStats).GetScore()
        };

        /// <inheritdoc/>
        [JsonIgnore]
        public virtual IEnumerable<Func<double>> AdjustingScoreMultipliers => new List<Func<double>>
        {
            () => (this as IWalletGreysafeStats).GetAdjustingScoreMultiplier(),
            () => (this as IWalletChainanalysisStats).GetAdjustingScoreMultiplier()
        };

        /// <inheritdoc cref="IWalletCommonStats{ITransactionIntervalData}.ExcludedStatDescriptions"/>
        [JsonIgnore]
        public virtual IEnumerable<string> ExcludedStatDescriptions =>
            IWalletCommonStats<TTransactionIntervalData>.ExcludedStatDescriptions.Union(new List<string>
            {
                nameof(NativeToken),
                nameof(SnapshotProposals),
                nameof(SnapshotVotes),
                nameof(TokenBalances),
                nameof(GreysafeReports),
                nameof(ChainanalysisReports)
            });
    }
}