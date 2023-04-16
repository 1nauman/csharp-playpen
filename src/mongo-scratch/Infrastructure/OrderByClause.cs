using System.Linq.Expressions;

namespace mongo_scratch.Infrastructure;

public class OrderByClause<TView, TProperty>
{
    public OrderByClause(Expression<Func<TView, TProperty>> propertySelect, bool isDescending = false)
    {
        IsDescending = isDescending;
        PropertySelect = propertySelect ?? throw new ArgumentNullException(nameof(propertySelect));
    }

    public bool IsDescending { get; }

    public Expression<Func<TView, TProperty>> PropertySelect { get; }
}

public class OrderByClause<TView> : OrderByClause<TView, object>
{
    public OrderByClause(Expression<Func<TView, object>> propertySelect, bool isDescending = false)
        : base(propertySelect, isDescending)
    {
    }
}