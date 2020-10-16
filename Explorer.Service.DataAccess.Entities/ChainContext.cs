using Thor.Framework.Data.DbContext.Relational;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class ChainContext : BaseDbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<AccountMetadata> AccountMetadata { get; set; }
        public virtual DbSet<ActionTrace> ActionTrace { get; set; }
        public virtual DbSet<ActionTraceAuthSequence> ActionTraceAuthSequence { get; set; }
        public virtual DbSet<ActionTraceAuthorization> ActionTraceAuthorization { get; set; }
        public virtual DbSet<ActionTraceRamDelta> ActionTraceRamDelta { get; set; }
        public virtual DbSet<BlockInfo> BlockInfo { get; set; }
        public virtual DbSet<Code> Code { get; set; }
        public virtual DbSet<ContractIndex128> ContractIndex128 { get; set; }
        public virtual DbSet<ContractIndex256> ContractIndex256 { get; set; }
        public virtual DbSet<ContractIndex64> ContractIndex64 { get; set; }
        public virtual DbSet<ContractIndexDouble> ContractIndexDouble { get; set; }
        public virtual DbSet<ContractIndexLongDouble> ContractIndexLongDouble { get; set; }
        public virtual DbSet<ContractRow> ContractRow { get; set; }
        public virtual DbSet<ContractTable> ContractTable { get; set; }
        public virtual DbSet<GeneratedTransaction> GeneratedTransaction { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<PermissionLink> PermissionLink { get; set; }
        public virtual DbSet<ProtocolState> ProtocolState { get; set; }
        public virtual DbSet<ReceivedBlock> ReceivedBlock { get; set; }
        public virtual DbSet<ResourceLimits> ResourceLimits { get; set; }
        public virtual DbSet<ResourceLimitsConfig> ResourceLimitsConfig { get; set; }
        public virtual DbSet<ResourceLimitsState> ResourceLimitsState { get; set; }
        public virtual DbSet<ResourceUsage> ResourceUsage { get; set; }
        public virtual DbSet<TransactionTrace> TransactionTrace { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlHasEnum("chain", "EnumTransactionStatus", new[] { "executed", "soft_fail", "hard_fail", "delayed", "expired" });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Name})
                    .HasName("account_pkey");

                entity.ToTable("account", "chain");

                entity.HasIndex(e => new {e.Name, e.BlockNum, e.Present})
                    .HasName("account_name_block_present_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(13);

                entity.Property(e => e.Abi).HasColumnName("abi");

                entity.Property(e => e.CreationDate).HasColumnName("creation_date");
            });

            modelBuilder.Entity<AccountMetadata>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Name})
                    .HasName("account_metadata_pkey");

                entity.ToTable("account_metadata", "chain");

                entity.HasIndex(e => new {e.Name, e.BlockNum, e.Present})
                    .HasName("acctmeta_name_block_present_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(13);

                entity.Property(e => e.CodeCodeHash)
                    .HasColumnName("code_code_hash")
                    .HasMaxLength(64);

                entity.Property(e => e.CodePresent).HasColumnName("code_present");

                entity.Property(e => e.CodeVmType).HasColumnName("code_vm_type");

                entity.Property(e => e.CodeVmVersion).HasColumnName("code_vm_version");

                entity.Property(e => e.LastCodeUpdate).HasColumnName("last_code_update");

                entity.Property(e => e.Privileged).HasColumnName("privileged");
            });

            modelBuilder.Entity<ActionTrace>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.TransactionId, e.ActionOrdinal})
                    .HasName("action_trace_pkey");

                entity.ToTable("action_trace", "chain");

                entity.HasIndex(e => new {e.TransactionId, e.BlockNum, e.ActionOrdinal})
                    .HasName("transaction_idx");

                entity.HasIndex(e => new {e.Receiver, e.BlockNum, e.TransactionId, e.ActionOrdinal})
                    .HasName("receipt_receiver_idx");

                entity.HasIndex(e => new
                        {e.ActName, e.Receiver, e.ActAccount, e.BlockNum, e.TransactionId, e.ActionOrdinal})
                    .HasName("at_range_name_receiver_account_block_trans_action_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(64);

                entity.Property(e => e.ActionOrdinal).HasColumnName("action_ordinal");

                entity.Property(e => e.ActAccount)
                    .HasColumnName("act_account")
                    .HasMaxLength(13);

                entity.Property(e => e.ActData).HasColumnName("act_data");

                entity.Property(e => e.ActName)
                    .HasColumnName("act_name")
                    .HasMaxLength(13);

                entity.Property(e => e.Console)
                    .HasColumnName("console")
                    .HasColumnType("character varying");

                entity.Property(e => e.ContextFree).HasColumnName("context_free");

                entity.Property(e => e.CreatorActionOrdinal).HasColumnName("creator_action_ordinal");

                entity.Property(e => e.Elapsed).HasColumnName("elapsed");

                entity.Property(e => e.ErrorCode)
                    .HasColumnName("error_code")
                    .HasColumnType("numeric");

                entity.Property(e => e.Except)
                    .HasColumnName("except")
                    .HasColumnType("character varying");

                entity.Property(e => e.ReceiptAbiSequence).HasColumnName("receipt_abi_sequence");

                entity.Property(e => e.ReceiptActDigest)
                    .HasColumnName("receipt_act_digest")
                    .HasMaxLength(64);

                entity.Property(e => e.ReceiptCodeSequence).HasColumnName("receipt_code_sequence");

                entity.Property(e => e.ReceiptGlobalSequence)
                    .HasColumnName("receipt_global_sequence")
                    .HasColumnType("numeric");

                entity.Property(e => e.ReceiptPresent).HasColumnName("receipt_present");

                entity.Property(e => e.ReceiptReceiver)
                    .HasColumnName("receipt_receiver")
                    .HasMaxLength(13);

                entity.Property(e => e.ReceiptRecvSequence)
                    .HasColumnName("receipt_recv_sequence")
                    .HasColumnType("numeric");

                entity.Property(e => e.Receiver)
                    .HasColumnName("receiver")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<ActionTraceAuthSequence>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.TransactionId, e.ActionOrdinal, e.Ordinal})
                    .HasName("action_trace_auth_sequence_pkey");

                entity.ToTable("action_trace_auth_sequence", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(64);

                entity.Property(e => e.ActionOrdinal).HasColumnName("action_ordinal");

                entity.Property(e => e.Ordinal).HasColumnName("ordinal");

                entity.Property(e => e.Account)
                    .HasColumnName("account")
                    .HasMaxLength(13);

                entity.Property(e => e.Sequence)
                    .HasColumnName("sequence")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<ActionTraceAuthorization>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.TransactionId, e.ActionOrdinal, e.Ordinal})
                    .HasName("action_trace_authorization_pkey");

                entity.ToTable("action_trace_authorization", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(64);

                entity.Property(e => e.ActionOrdinal).HasColumnName("action_ordinal");

                entity.Property(e => e.Ordinal).HasColumnName("ordinal");

                entity.Property(e => e.Actor)
                    .HasColumnName("actor")
                    .HasMaxLength(13);

                entity.Property(e => e.Permission)
                    .HasColumnName("permission")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<ActionTraceRamDelta>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.TransactionId, e.ActionOrdinal, e.Ordinal})
                    .HasName("action_trace_ram_delta_pkey");

                entity.ToTable("action_trace_ram_delta", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.TransactionId)
                    .HasColumnName("transaction_id")
                    .HasMaxLength(64);

                entity.Property(e => e.ActionOrdinal).HasColumnName("action_ordinal");

                entity.Property(e => e.Ordinal).HasColumnName("ordinal");

                entity.Property(e => e.Account)
                    .HasColumnName("account")
                    .HasMaxLength(13);

                entity.Property(e => e.Delta).HasColumnName("delta");
            });

            modelBuilder.Entity<BlockInfo>(entity =>
            {
                entity.HasKey(e => e.BlockNum)
                    .HasName("block_info_pkey");

                entity.ToTable("block_info", "chain");

                entity.Property(e => e.BlockNum)
                    .HasColumnName("block_num")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActionMroot)
                    .HasColumnName("action_mroot")
                    .HasMaxLength(64);

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(64);

                entity.Property(e => e.Confirmed).HasColumnName("confirmed");

                entity.Property(e => e.NewProducersVersion).HasColumnName("new_producers_version");

                entity.Property(e => e.Previous)
                    .HasColumnName("previous")
                    .HasMaxLength(64);

                entity.Property(e => e.Producer)
                    .HasColumnName("producer")
                    .HasMaxLength(13);

                entity.Property(e => e.ScheduleVersion).HasColumnName("schedule_version");

                entity.Property(e => e.Timestamp).HasColumnName("timestamp");

                entity.Property(e => e.TransactionMroot)
                    .HasColumnName("transaction_mroot")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Code>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.VmType, e.VmVersion, e.CodeHash})
                    .HasName("code_pkey");

                entity.ToTable("code", "chain");

                entity.HasIndex(e => new {e.VmType, e.VmVersion, e.CodeHash, e.BlockNum, e.Present})
                    .HasName("code_type_ver_hash_block_present_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.VmType).HasColumnName("vm_type");

                entity.Property(e => e.VmVersion).HasColumnName("vm_version");

                entity.Property(e => e.CodeHash)
                    .HasColumnName("code_hash")
                    .HasMaxLength(64);

                entity.Property(e => e.Code1).HasColumnName("code");
            });

            modelBuilder.Entity<ContractIndex128>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_index128_pkey");

                entity.ToTable("contract_index128", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.SecondaryKey)
                    .HasColumnName("secondary_key")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<ContractIndex256>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_index256_pkey");

                entity.ToTable("contract_index256", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.SecondaryKey)
                    .HasColumnName("secondary_key")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ContractIndex64>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_index64_pkey");

                entity.ToTable("contract_index64", "chain");

                entity.HasIndex(
                        e => new {e.Code, e.Table, e.Scope, e.SecondaryKey, e.PrimaryKey, e.BlockNum, e.Present})
                    .HasName("contract_index64_code_table_scope_sk_pk_block_num_prese_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.SecondaryKey)
                    .HasColumnName("secondary_key")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<ContractIndexDouble>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_index_double_pkey");

                entity.ToTable("contract_index_double", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.SecondaryKey).HasColumnName("secondary_key");
            });

            modelBuilder.Entity<ContractIndexLongDouble>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_index_long_double_pkey");

                entity.ToTable("contract_index_long_double", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.SecondaryKey).HasColumnName("secondary_key");
            });

            modelBuilder.Entity<ContractRow>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table, e.PrimaryKey})
                    .HasName("contract_row_pkey");

                entity.ToTable("contract_row", "chain");

                entity.HasIndex(e => new {e.Code, e.Table, e.PrimaryKey, e.Scope, e.BlockNum, e.Present})
                    .HasName("contract_row_code_table_primary_key_scope_block_num_prese_idx");

                entity.HasIndex(e => new {e.Code, e.Table, e.Scope, e.PrimaryKey, e.BlockNum, e.Present})
                    .HasName("contract_row_code_table_scope_primary_key_block_num_prese_idx");

                entity.HasIndex(e => new {e.Scope, e.Table, e.PrimaryKey, e.Code, e.BlockNum, e.Present})
                    .HasName("contract_row_scope_table_primary_key_code_block_num_prese_idx");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.PrimaryKey)
                    .HasColumnName("primary_key")
                    .HasColumnType("numeric");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.Value).HasColumnName("value");
            });

            modelBuilder.Entity<ContractTable>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Code, e.Scope, e.Table})
                    .HasName("contract_table_pkey");

                entity.ToTable("contract_table", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.Scope)
                    .HasColumnName("scope")
                    .HasMaxLength(13);

                entity.Property(e => e.Table)
                    .HasColumnName("table")
                    .HasMaxLength(13);

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<GeneratedTransaction>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Sender, e.SenderId})
                    .HasName("generated_transaction_pkey");

                entity.ToTable("generated_transaction", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Sender)
                    .HasColumnName("sender")
                    .HasMaxLength(13);

                entity.Property(e => e.SenderId)
                    .HasColumnName("sender_id")
                    .HasColumnType("numeric");

                entity.Property(e => e.PackedTrx).HasColumnName("packed_trx");

                entity.Property(e => e.Payer)
                    .HasColumnName("payer")
                    .HasMaxLength(13);

                entity.Property(e => e.TrxId)
                    .HasColumnName("trx_id")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Owner, e.Name})
                    .HasName("permission_pkey");

                entity.ToTable("permission", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Owner)
                    .HasColumnName("owner")
                    .HasMaxLength(13);

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(13);

                entity.Property(e => e.AuthThreshold).HasColumnName("auth_threshold");

                entity.Property(e => e.LastUpdated).HasColumnName("last_updated");

                entity.Property(e => e.Parent)
                    .HasColumnName("parent")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<PermissionLink>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Account, e.Code, e.MessageType})
                    .HasName("permission_link_pkey");

                entity.ToTable("permission_link", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Account)
                    .HasColumnName("account")
                    .HasMaxLength(13);

                entity.Property(e => e.Code)
                    .HasColumnName("code")
                    .HasMaxLength(13);

                entity.Property(e => e.MessageType)
                    .HasColumnName("message_type")
                    .HasMaxLength(13);

                entity.Property(e => e.RequiredPermission)
                    .HasColumnName("required_permission")
                    .HasMaxLength(13);
            });

            modelBuilder.Entity<ProtocolState>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present})
                    .HasName("protocol_state_pkey");

                entity.ToTable("protocol_state", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");
            });

            modelBuilder.Entity<ReceivedBlock>(entity =>
            {
                entity.HasKey(e => e.BlockNum)
                    .HasName("received_block_pkey");

                entity.ToTable("received_block", "chain");

                entity.Property(e => e.BlockNum)
                    .HasColumnName("block_num")
                    .ValueGeneratedNever();

                entity.Property(e => e.BlockId)
                    .HasColumnName("block_id")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<ResourceLimits>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Owner})
                    .HasName("resource_limits_pkey");

                entity.ToTable("resource_limits", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Owner)
                    .HasColumnName("owner")
                    .HasMaxLength(13);

                entity.Property(e => e.CpuWeight).HasColumnName("cpu_weight");

                entity.Property(e => e.NetWeight).HasColumnName("net_weight");

                entity.Property(e => e.RamBytes).HasColumnName("ram_bytes");
            });

            modelBuilder.Entity<ResourceLimitsConfig>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present})
                    .HasName("resource_limits_config_pkey");

                entity.ToTable("resource_limits_config", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.AccountCpuUsageAverageWindow).HasColumnName("account_cpu_usage_average_window");

                entity.Property(e => e.AccountNetUsageAverageWindow).HasColumnName("account_net_usage_average_window");

                entity.Property(e => e.CpuLimitParametersContractRateDenominator)
                    .HasColumnName("cpu_limit_parameters_contract_rate_denominator")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuLimitParametersContractRateNumerator)
                    .HasColumnName("cpu_limit_parameters_contract_rate_numerator")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuLimitParametersExpandRateDenominator)
                    .HasColumnName("cpu_limit_parameters_expand_rate_denominator")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuLimitParametersExpandRateNumerator)
                    .HasColumnName("cpu_limit_parameters_expand_rate_numerator")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuLimitParametersMax)
                    .HasColumnName("cpu_limit_parameters_max")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuLimitParametersMaxMultiplier)
                    .HasColumnName("cpu_limit_parameters_max_multiplier");

                entity.Property(e => e.CpuLimitParametersPeriods).HasColumnName("cpu_limit_parameters_periods");

                entity.Property(e => e.CpuLimitParametersTarget)
                    .HasColumnName("cpu_limit_parameters_target")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersContractRateDenominator)
                    .HasColumnName("net_limit_parameters_contract_rate_denominator")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersContractRateNumerator)
                    .HasColumnName("net_limit_parameters_contract_rate_numerator")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersExpandRateDenominator)
                    .HasColumnName("net_limit_parameters_expand_rate_denominator")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersExpandRateNumerator)
                    .HasColumnName("net_limit_parameters_expand_rate_numerator")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersMax)
                    .HasColumnName("net_limit_parameters_max")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetLimitParametersMaxMultiplier)
                    .HasColumnName("net_limit_parameters_max_multiplier");

                entity.Property(e => e.NetLimitParametersPeriods).HasColumnName("net_limit_parameters_periods");

                entity.Property(e => e.NetLimitParametersTarget)
                    .HasColumnName("net_limit_parameters_target")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<ResourceLimitsState>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present})
                    .HasName("resource_limits_state_pkey");

                entity.ToTable("resource_limits_state", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.AverageBlockCpuUsageConsumed)
                    .HasColumnName("average_block_cpu_usage_consumed")
                    .HasColumnType("numeric");

                entity.Property(e => e.AverageBlockCpuUsageLastOrdinal)
                    .HasColumnName("average_block_cpu_usage_last_ordinal");

                entity.Property(e => e.AverageBlockCpuUsageValueEx)
                    .HasColumnName("average_block_cpu_usage_value_ex")
                    .HasColumnType("numeric");

                entity.Property(e => e.AverageBlockNetUsageConsumed)
                    .HasColumnName("average_block_net_usage_consumed")
                    .HasColumnType("numeric");

                entity.Property(e => e.AverageBlockNetUsageLastOrdinal)
                    .HasColumnName("average_block_net_usage_last_ordinal");

                entity.Property(e => e.AverageBlockNetUsageValueEx)
                    .HasColumnName("average_block_net_usage_value_ex")
                    .HasColumnType("numeric");

                entity.Property(e => e.TotalCpuWeight)
                    .HasColumnName("total_cpu_weight")
                    .HasColumnType("numeric");

                entity.Property(e => e.TotalNetWeight)
                    .HasColumnName("total_net_weight")
                    .HasColumnType("numeric");

                entity.Property(e => e.TotalRamBytes)
                    .HasColumnName("total_ram_bytes")
                    .HasColumnType("numeric");

                entity.Property(e => e.VirtualCpuLimit)
                    .HasColumnName("virtual_cpu_limit")
                    .HasColumnType("numeric");

                entity.Property(e => e.VirtualNetLimit)
                    .HasColumnName("virtual_net_limit")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<ResourceUsage>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.Present, e.Owner})
                    .HasName("resource_usage_pkey");

                entity.ToTable("resource_usage", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.Present).HasColumnName("present");

                entity.Property(e => e.Owner)
                    .HasColumnName("owner")
                    .HasMaxLength(13);

                entity.Property(e => e.CpuUsageConsumed)
                    .HasColumnName("cpu_usage_consumed")
                    .HasColumnType("numeric");

                entity.Property(e => e.CpuUsageLastOrdinal).HasColumnName("cpu_usage_last_ordinal");

                entity.Property(e => e.CpuUsageValueEx)
                    .HasColumnName("cpu_usage_value_ex")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetUsageConsumed)
                    .HasColumnName("net_usage_consumed")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetUsageLastOrdinal).HasColumnName("net_usage_last_ordinal");

                entity.Property(e => e.NetUsageValueEx)
                    .HasColumnName("net_usage_value_ex")
                    .HasColumnType("numeric");

                entity.Property(e => e.RamUsage)
                    .HasColumnName("ram_usage")
                    .HasColumnType("numeric");
            });

            modelBuilder.Entity<TransactionTrace>(entity =>
            {
                entity.HasKey(e => new {e.BlockNum, e.TransactionOrdinal})
                    .HasName("transaction_trace_pkey");

                entity.ToTable("transaction_trace", "chain");

                entity.Property(e => e.BlockNum).HasColumnName("block_num");

                entity.Property(e => e.TransactionOrdinal).HasColumnName("transaction_ordinal");

                entity.Property(e => e.AccountRamDeltaAccount)
                    .HasColumnName("account_ram_delta_account")
                    .HasMaxLength(13);

                entity.Property(e => e.AccountRamDeltaDelta).HasColumnName("account_ram_delta_delta");

                entity.Property(e => e.AccountRamDeltaPresent).HasColumnName("account_ram_delta_present");

                entity.Property(e => e.CpuUsageUs).HasColumnName("cpu_usage_us");

                entity.Property(e => e.Elapsed).HasColumnName("elapsed");

                entity.Property(e => e.ErrorCode)
                    .HasColumnName("error_code")
                    .HasColumnType("numeric");

                entity.Property(e => e.Except)
                    .HasColumnName("except")
                    .HasColumnType("character varying");

                entity.Property(e => e.FailedDtrxTrace)
                    .HasColumnName("failed_dtrx_trace")
                    .HasMaxLength(64);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(64);

                entity.Property(e => e.NetUsage)
                    .HasColumnName("net_usage")
                    .HasColumnType("numeric");

                entity.Property(e => e.NetUsageWords).HasColumnName("net_usage_words");

                entity.Property(e => e.PartialContextFreeData).HasColumnName("partial_context_free_data");

                entity.Property(e => e.PartialDelaySec).HasColumnName("partial_delay_sec");

                entity.Property(e => e.PartialExpiration).HasColumnName("partial_expiration");

                entity.Property(e => e.PartialMaxCpuUsageMs).HasColumnName("partial_max_cpu_usage_ms");

                entity.Property(e => e.PartialMaxNetUsageWords).HasColumnName("partial_max_net_usage_words");

                entity.Property(e => e.PartialPresent).HasColumnName("partial_present");

                entity.Property(e => e.PartialRefBlockNum).HasColumnName("partial_ref_block_num");

                entity.Property(e => e.PartialRefBlockPrefix).HasColumnName("partial_ref_block_prefix");

                entity.Property(e => e.PartialSignatures)
                    .HasColumnName("partial_signatures")
                    .HasColumnType("character varying[]");

                entity.Property(e => e.Scheduled).HasColumnName("scheduled");
            });
        }
    }
}