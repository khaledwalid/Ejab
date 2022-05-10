using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ejab.DAl.Common
{
    public  class SortParameterList : IReadOnlyList<SortParameter>
    {
        public static readonly char Separator = '|';

        private readonly List<SortParameter> _options;

        public SortParameterList()
        {
        }

        public SortParameterList(IEnumerable<SortParameter> sortOptions)
        {
            _options = (sortOptions ?? Enumerable.Empty<SortParameter>()).ToList();
        }

        public int Count
        {
            get { return _options.Count; }
        }

        public SortParameter this[int index]
        {
            get { return _options[index]; }
        }

        public IQueryable Sort(IQueryable source)
        {
            IOrderedQueryable query = null;

            foreach (var parameter in _options)
            {
                query = parameter.Sort(query ?? source);
            }

            return query ?? source;
        }

        public IEnumerable Sort(IEnumerable source)
        {
            return Sort(source.AsQueryable());
        }

        public IEnumerator<SortParameter> GetEnumerator()
        {
            return _options.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join(new string(new[] {Separator}), _options);
        }

       
    }

    public class SortParameter
    {
        private const BindingFlags PublicStaticFlags = BindingFlags.Public | BindingFlags.Static;
        private const BindingFlags PublicInstanceFlags = BindingFlags.Public | BindingFlags.Instance;

        public SortParameter(string propertyName, ListSortDirection direction)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");

            PropertyName = propertyName.Trim();
            Direction = direction;
        }

        public string PropertyName { get; private set; }

        public ListSortDirection Direction { get; private set; }

        public IOrderedQueryable Sort(IQueryable source)
        {
            var type = source.ElementType;
            var property = FindProperty(type);
            var lambdaExpression = CreateExpression(type);

            var visitor = new QueryableExpressionVisitor();
            visitor.Visit(source.Expression);

            var methodName = visitor.IsOrdered ? "ThenBy" : "OrderBy";

            if (Direction == ListSortDirection.Descending)
            {
                methodName += "Descending";
            }

            // TODO: Cache this MethodInfo as a lazy static property
            var method = typeof (Queryable).GetMethods(PublicStaticFlags)
                .Single(x => x.Name == methodName && x.GetParameters().Length == 2)
                .MakeGenericMethod(source.ElementType, property.PropertyType);

            var sortExpression = Expression.Call(null, method, new[] {source.Expression, lambdaExpression});

            return (IOrderedQueryable) source.Provider.CreateQuery(sortExpression);
        }

        private PropertyInfo FindProperty(Type type)
        {
            var properties =
                type.GetProperties(PublicInstanceFlags)
                    .Where(x => string.Equals(PropertyName, x.Name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (properties.Count == 0)
            {
                throw new MissingMemberException(type.FullName, PropertyName);
            }

            return properties[0];
        }

        public Expression CreateExpression(Type type)
        {
            var parameterExpression = Expression.Parameter(type, "x");
            var propertyExpression = Expression.Property(parameterExpression, PropertyName);
            return Expression.Lambda(propertyExpression, parameterExpression);
        }

        public override string ToString()
        {
            return Direction == ListSortDirection.Descending ? "-" + PropertyName : PropertyName;
        }

        public static SortParameter Parse(string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            var name = value.TrimStart('-').Replace("-", "_");
            var direction = value.Length != name.Length ? ListSortDirection.Descending : ListSortDirection.Ascending;
            return new SortParameter(name, direction);
        }

        public static bool TryParse(string value, out SortParameter parameter)
        {
            try
            {
                parameter = Parse(value);
            }
            catch (Exception)
            {
                parameter = null;
                return false;
            }

            return true;
        }

        private class QueryableExpressionVisitor : ExpressionVisitor
        {
            public bool IsOrdered { get; private set; }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (!IsOrdered)
                {
                    var method = node.Method;
                    IsOrdered = method.DeclaringType == typeof (Queryable) &&
                                (method.Name == "OrderBy" || method.Name == "OrderByDescending");
                }

                return base.VisitMethodCall(node);
            }
        }
    }

    public static class QueryableExtentions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, params string[] propertyNames)
        {
            if (propertyNames.Empty())
            {
                return query;
            }

            var list = new List<SortParameter>();
            foreach (var name in propertyNames)
            {
                SortParameter parameter;
                if (SortParameter.TryParse(name, out parameter))
                {
                    list.Add(parameter);
                }
            }

            var sortParameterList = new SortParameterList(list);
            return sortParameterList.Sort(query) as IQueryable<T>;
        }

        public static IQueryable<T> If<T>(this IQueryable<T> query, bool should,
            params Func<IQueryable<T>, IQueryable<T>>[] transforms)
        {
            return should ? transforms.Aggregate(query, (current, transform) => transform.Invoke(current)) : query;
        }

        public static bool Empty<T>(this IEnumerable<T> enumerable)
        {
            return enumerable == null || !enumerable.Any();
        }
    }
}
