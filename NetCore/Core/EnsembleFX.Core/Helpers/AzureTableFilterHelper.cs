
namespace EnsembleFX.Core.Helpers
{
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    public static class AzureTableFilterHelper
    {
        public static string EqualAndFilter(IDictionary<string, string> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = TableQuery.GenerateFilterCondition(item.Key, QueryComparisons.Equal, item.Value);

                }
                else
                {
                    query = query + " and " + TableQuery.GenerateFilterCondition(item.Key, QueryComparisons.Equal, item.Value);

                }
            }

            return query;
        }

        public static string EqualOrFilter(IDictionary<string, string> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = TableQuery.GenerateFilterCondition(item.Key, QueryComparisons.Equal, item.Value);

                }
                else
                {
                    query = query + " or " + TableQuery.GenerateFilterCondition(item.Key, QueryComparisons.Equal, item.Value);

                }
            }

            return query;
        }

        public static string AppendOrFilter(IList<string> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = item;

                }
                else
                {
                    query = query + " or " + item;

                }
            }

            return query;
        }

        public static string AppendAndFilter(IList<string> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = item;

                }
                else
                {
                    query = query + " and " + item;

                }
            }

            return query;
        }

        public static string EqualAndFilter(IDictionary<string, bool> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = TableQuery.GenerateFilterConditionForBool(item.Key, QueryComparisons.Equal, item.Value);

                }
                else
                {
                    query = query + " and " + TableQuery.GenerateFilterConditionForBool(item.Key, QueryComparisons.Equal, item.Value);

                }
            }

            return query;
        }

        public static string EqualOrFilter(IDictionary<string, bool> parameters)
        {
            string query = string.Empty;
            foreach (var item in parameters)
            {
                if (string.IsNullOrEmpty(query))
                {
                    query = TableQuery.GenerateFilterConditionForBool(item.Key, QueryComparisons.Equal, item.Value);

                }
                else
                {
                    query = query + " or " + TableQuery.GenerateFilterConditionForBool(item.Key, QueryComparisons.Equal, item.Value);

                }
            }

            return query;
        }

        public static string StartsWithFilter(string columnName,string value)
        {
            var length = value.Length - 1;
            var lastChar = value[length];

            var nextLastChar = (char)(lastChar + 1);

            var startsWithEndPattern = value.Substring(0, length) + nextLastChar;
            var prefixCondition = TableQuery.CombineFilters(
            TableQuery.GenerateFilterCondition(columnName,
                QueryComparisons.GreaterThanOrEqual,
                value),
            TableOperators.And,
            TableQuery.GenerateFilterCondition(columnName,
                QueryComparisons.LessThan,
                startsWithEndPattern)
            );

            return prefixCondition;
        }
    }
}
