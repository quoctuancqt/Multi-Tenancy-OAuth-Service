using System;
using System.Linq;
using OAuthService.Core.Enums;

namespace OAuthService.Core.EntityExtensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TResult> ApplySort<TResult>(
            this IQueryable<TResult> query,
            string sortField,
            OrderByTypeEnum sortType = OrderByTypeEnum.Descending) where TResult : class
        {
            if (string.IsNullOrEmpty(sortField)) return query;

            query = sortType == OrderByTypeEnum.Ascending ? query.OrderBy(sortField.FirstCharToUpper()) : query.OrderByDescending(sortField.FirstCharToUpper());
            return query;
        }

        public static IQueryable<TResult> ApplySort<TResult>(
            this IQueryable<TResult> query,
            SortModel[] sortFields
        ) where TResult : class
        {
            // No sorting needed  
            if (sortFields == null || !sortFields.Any())
            {
                return query;
            }

            var orderedQuery = query.OrderBy(x => true);

            for (var i = 0; i < sortFields.Length; i++)
            {
                var index = i;

                // Bind property to Expression
                var expression = OrderByExtension.GetExpression<TResult>(sortFields[index].SortField);

                // Apply sort expression with sort direction
                if (sortFields[index].OrderByType == OrderByTypeEnum.Ascending)
                {
                    orderedQuery = index == 0
                        ? query.OrderBy(expression)
                        : orderedQuery.ThenBy(expression);
                }
                else
                {
                    orderedQuery = index == 0 ? query.OrderByDescending(expression) : orderedQuery.ThenByDescending(expression);
                }
            }

            query = orderedQuery;
            return query;
        }

        //public static IQueryable<TResult> ApplyLikeSearch<TResult>(
        //    this IQueryable<TResult> query,
        //    string searchKey,
        //    params string[] searchFields)
        //{
        //    if (string.IsNullOrEmpty(searchKey))
        //    {
        //        return query;
        //    }

        //    Expression<Func<TResult, bool>> expression = null;
        //    var parameterExpression = Expression.Parameter(typeof(TResult), "p");

        //    var pattern = Expression.Constant($"%{searchKey}%");
        //    foreach (var searchField in searchFields)
        //    {
        //        Expression<Func<TResult, bool>> lambda;
        //        var nestedProperties = searchField.Split('.');
        //        Expression member = parameterExpression;

        //        foreach (var prop in nestedProperties)
        //        {
        //            member = Expression.PropertyOrField(member, prop);
        //        }

        //        if (member.Type.Name.Equals("String"))
        //        {
        //            var likeMethod = typeof(DbFunctionsExtensions).GetMethod("Like",
        //                new[]
        //                {
        //                    typeof(DbFunctions),
        //                    typeof(string),
        //                    typeof(string)
        //                });
        //            Expression call = Expression.Call(null, likeMethod, Expression.Constant(EF.Functions), member, pattern);

        //            lambda = Expression.Lambda<Func<TResult, bool>>(call, parameterExpression);
        //        }
        //        else
        //        {
        //            lambda = Expression.Lambda<Func<TResult, bool>>(Expression.Equal(member, Expression.Constant(searchKey)), parameterExpression);
        //        }

        //        if (lambda != null)
        //        {
        //            expression = expression == null ? lambda : expression.Or(lambda);
        //        }
        //    }

        //    if (expression == null)
        //    {
        //        return query;
        //    }

        //    query = query.Where(expression);
        //    return query;
        //}

        //public static IQueryable<TResult> ApplyEnumsFilter<TResult, TEnum>(
        //    this IQueryable<TResult> query,
        //    IList<TEnum> statuses,
        //    string filteredField)
        //{
        //    if (!statuses.Any())
        //    {
        //        return query;
        //    }

        //    Expression<Func<TResult, bool>> expression = null;
        //    var parameterExpression = Expression.Parameter(typeof(TResult), "p");

        //    Expression member = parameterExpression;
        //    member = Expression.PropertyOrField(member, filteredField);

        //    foreach (var status in statuses)
        //    {
        //        var lambda = Expression.Lambda<Func<TResult, bool>>(Expression.Equal(member, Expression.Constant(status)), parameterExpression);
        //        if (lambda != null)
        //        {
        //            expression = expression == null ? lambda : expression.Or(lambda);
        //        }
        //    }

        //    if (expression == null)
        //    {
        //        return query;
        //    }

        //    query = query.Where(expression);
        //    return query;
        //}

        //public static IQueryable<TResult> ApplyFilterValues<TResult>(
        //    this IQueryable<TResult> query,
        //    string values,
        //    string filteredField)
        //{
        //    if (string.IsNullOrEmpty(values))
        //    {
        //        return query;
        //    }

        //    Expression<Func<TResult, bool>> expression = null;
        //    var parameterExpression = Expression.Parameter(typeof(TResult), "p");

        //    Expression member = parameterExpression;
        //    member = Expression.PropertyOrField(member, filteredField);

        //    var items = values.Split(",");
        //    foreach (var item in items)
        //    {
        //        var equalMethod = typeof(string).GetMethod("Equals",
        //            new[]
        //            {
        //                typeof(string),
        //                typeof(string)
        //            });

        //        Expression call = Expression.Call(null, equalMethod, Expression.Constant(item), member);
        //        var lambda = Expression.Lambda<Func<TResult, bool>>(call, parameterExpression);

        //        if (lambda != null)
        //        {
        //            expression = expression == null ? lambda : expression.Or(lambda);
        //        }
        //    }

        //    if (expression == null)
        //    {
        //        return query;
        //    }

        //    query = query.Where(expression);
        //    return query;
        //}

        private static string FirstCharToUpper(this string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }
    }
}
