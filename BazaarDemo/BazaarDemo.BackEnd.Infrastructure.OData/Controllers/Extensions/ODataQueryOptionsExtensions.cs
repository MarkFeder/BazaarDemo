using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
//using System.Web.Http.OData.Query;
using System.Web.OData.Query;

public static class ODataQueryOptionsExtensions
    {
        public static bool IsNavigationPropertyExpected<TSource, TKey>(this ODataQueryOptions<TSource> source, Expression<Func<TSource, TKey>> keySelector)
        {
            if (source == null) { throw new ArgumentNullException("source"); }
            if (keySelector == null) { throw new ArgumentNullException("keySelector"); }

            var returnValue = false;
            var propertyName = (keySelector.Body as MemberExpression ?? ((UnaryExpression)keySelector.Body).Operand as MemberExpression).Member.Name;
            var expandProperties = source.SelectExpand == null || string.IsNullOrWhiteSpace(source.SelectExpand.RawExpand) ? new List<string>().ToArray() : source.SelectExpand.RawExpand.Split(',');
            var selectProperties = source.SelectExpand == null || string.IsNullOrWhiteSpace(source.SelectExpand.RawSelect) ? new List<string>().ToArray() : source.SelectExpand.RawSelect.Split(',');

            returnValue = returnValue ^ expandProperties.Contains<string>(propertyName);
            returnValue = returnValue ^ selectProperties.Contains<string>(propertyName);

            return returnValue;
        }

        public static bool IsExpandQuery<TSource>(this ODataQueryOptions<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            var returnValue = false;

            var expandProperties = source.SelectExpand != null && !string.IsNullOrWhiteSpace(source.SelectExpand.RawExpand);
            var selectProperties = source.SelectExpand != null && !string.IsNullOrWhiteSpace(source.SelectExpand.RawSelect);

            returnValue = expandProperties | selectProperties;

            return returnValue;
        }

        public static List<string> GetNavigationProperties<TSource>(this ODataQueryOptions<TSource> source)
        {
            if (source == null) { throw new ArgumentNullException("source"); }

            var returnValue = new List<string>();



            return returnValue;         
        }
    }
