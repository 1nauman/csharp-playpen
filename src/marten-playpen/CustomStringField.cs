using System.Linq.Expressions;
using System.Reflection;
using Marten;
using Marten.Linq.Fields;
using Marten.Linq.Filters;
using Weasel.Postgresql.SqlGeneration;

namespace marten_playpen;

public class CustomStringField : StringField
{
    public CustomStringField(string dataLocator, Casing casing, MemberInfo[] members) : base(dataLocator, casing,
        members)
    {
        // Field type is string and overriding default behavior of casting to jsonb with direct selection
        JSONBLocator = RawLocator;
    }

    public override ISqlFragment CreateComparison(string op, ConstantExpression constantExpression,
        Expression memberExpression)
    {
        var expression = constantExpression;
        if (constantExpression.Type.IsAssignableTo(typeof(IEnumValue)) &&
            constantExpression.Value is IEnumValue enumerationValue)
        {
            expression = Expression.Constant(enumerationValue.Value);
        }

        if (constantExpression.Value is null)
        {
            return new IsNullFilter(this);
        }

        return new ComparisonFilter(this, new CommandParameter(expression), op);
    }
}