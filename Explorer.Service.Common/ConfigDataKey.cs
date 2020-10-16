namespace Explorer.Service.Common
{
    public static class ConfigDataKey
    {
        public const string DefaultTokenCode = "ngk.token";

        public const string DefaultTokenSymbol = "NGK";

        public static readonly string[] FilterTokenSymbol = {""};

        public const string LatestBlockCacheKey = "latest_block";

        public const string LatestBlockListCacheKey = "latest_block_list";

        public const string LatestTransactionListCacheKey = "latest_transaction_list";

        public const string LatestProducerListCacheKey = "latest_prodeucer_list";

        public const string TokenAccountsListCacheKey = "token_accounts_list";

        public const int TokenAccountsListCacheExpired = 60;

        public const int TokenAccountsPieRank = 10;

        public const int TpsCalculationPeriod = 1;

        public const string TpsHighestTotal = "tps_highest_total";

        public const int BlockInfoListLatestTotal = 20;

        public const int TransactionListLatestTotal = 10;
    }
}