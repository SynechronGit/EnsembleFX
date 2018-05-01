using System;
using System.Collections.Generic;
using System.Globalization;

namespace EnsembleFX.Filters
{
    public class FilteringClause
    {
        #region Public Methods

        public static string BuildWhereClause<T>(int index, string logic,
          GridFilter filter, List<object> parameters)
        {
            var entityType = (typeof(T));
            string[] t = filter.Field.Split('.');
            var property = entityType.GetProperty(t[t.Length - 1]);


            switch (filter.Operator.ToLower())
            {
                case "eq":
                case "neq":
                case "gte":
                case "gt":
                case "lte":
                case "lt":
                    if (property != null)
                    {
                        if (typeof(DateTime).IsAssignableFrom(property.PropertyType)
                            || typeof(Nullable<DateTime>).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(DateTime.Parse(filter.Value, CultureInfo.CreateSpecificCulture("en-us")));
                            return string.Format("{0}{1}@{2}",
                                filter.Field,
                                ToLinqOperator(filter.Operator),
                                index);
                        }

                        if (typeof(Boolean?).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(Boolean.Parse(filter.Value));
                            return string.Format("{0}{1}@{2}",
                                filter.Field,
                                ToLinqOperator(filter.Operator),
                                index);
                        }
                        if (typeof(int).IsAssignableFrom(property.PropertyType)
                            || typeof(Nullable<int>).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(int.Parse(filter.Value));
                            return string.Format("{0}{1}@{2}",
                                filter.Field,
                                ToLinqOperator(filter.Operator),
                                index);
                        }
                        if (typeof(double).IsAssignableFrom(property.PropertyType)
                            || typeof(Nullable<double>).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(double.Parse(filter.Value));
                            return string.Format("{0}{1}@{2}",
                                filter.Field,
                                ToLinqOperator(filter.Operator),
                                index);
                        }
                        if (typeof(decimal).IsAssignableFrom(property.PropertyType)
                            || typeof(Nullable<decimal>).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(decimal.Parse(filter.Value));
                            return string.Format("{0}{1}@{2}",
                                filter.Field,
                                ToLinqOperator(filter.Operator),
                                index);
                        }
                        if (typeof(Guid).IsAssignableFrom(property.PropertyType))
                        {
                            parameters.Add(Guid.Parse(filter.Value));
                            return string.Format("{0}.Equals(@{1})",
                                filter.Field,
                                index);
                        }
                    }
                    parameters.Add(filter.Value);
                    return string.Format("{0}{1}@{2}",
                        filter.Field,
                        ToLinqOperator(filter.Operator),
                        index);
                case "startswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0} != null && {0}.StartsWith(" + "@{1})",
                        filter.Field,
                        index);
                case "endswith":
                    parameters.Add(filter.Value);
                    return string.Format("{0} != null && {0}.EndsWith(" + "@{1})",
                        filter.Field,
                        index);
                case "contains":
                    parameters.Add(filter.Value);
                    return string.Format("{0} != null && {0}.Contains(" + "@{1})",
                        filter.Field,
                        index);
                default:
                    throw new ArgumentException(
                        "This operator is not yet supported for this Grid",
                        filter.Operator);
            }
        }

        public static string ToLinqOperator(string @operator)
        {
            switch (@operator.ToLower())
            {
                case "eq": return " == ";
                case "neq": return " != ";
                case "gte": return " >= ";
                case "gt": return " > ";
                case "lte": return " <= ";
                case "lt": return " < ";
                case "or": return " || ";
                case "and": return " && ";
                default: return null;
            }
        }
        #endregion
    }
}
