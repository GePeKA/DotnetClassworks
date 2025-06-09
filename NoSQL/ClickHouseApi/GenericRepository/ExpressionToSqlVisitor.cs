using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ClickHouseApi.GenericRepository;

public class ExpressionToSqlVisitor: ExpressionVisitor, IExpressionSqlTranslator
{
    private readonly StringBuilder _sb = new();

    public string Translate(Expression expression)
    {
        Visit(expression);
        return _sb.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sb.Append("(");
        Visit(node.Left);

        _sb.Append(node.NodeType switch
        {
            ExpressionType.AndAlso => " AND ",
            ExpressionType.OrElse => " OR ",
            ExpressionType.Equal => " = ",
            ExpressionType.NotEqual => " != ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            _ => throw new NotSupportedException($"Оператор {node.NodeType} не поддерживается")
        });

        Visit(node.Right);
        _sb.Append(")");
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (TryGetMemberValue(node, out var value))
        {
            AppendValue(value);
            return node;
        }

        // Стандартная обработка членов класса
        _sb.Append(ToSnakeCase(node.Member.Name));
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Type == typeof(string))
        {
            _sb.Append($"'{node.Value}'");
        }
        else if (node.Type.IsEnum)
        {
            _sb.Append((int) node.Value!);
        }
        else
        {
            _sb.Append(node.Value);
        }

        return node;
    }

    protected override Expression VisitUnary(UnaryExpression node)
    {
        if (node.NodeType == ExpressionType.Convert)
        {
            Visit(node.Operand); // убираем лишние конвертации
            return node;
        }

        return base.VisitUnary(node);
    }

    private static string ToSnakeCase(string input)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            var ch = input[i];
            if (char.IsUpper(ch) && i > 0)
                sb.Append('_');
            sb.Append(char.ToLowerInvariant(ch));
        }
        return sb.ToString();
    }

    private bool TryGetMemberValue(MemberExpression memberExpr, out object? value)
    {
        value = null;

        // Обработка захваченных переменных (замыканий)
        if (memberExpr.Expression is ConstantExpression constExpr)
        {
            if (memberExpr.Member is FieldInfo fieldInfo)
            {
                value = fieldInfo.GetValue(constExpr.Value);
                return true;
            }
        }

        return false;
    }

    private void AppendValue(object? value)
    {
        if (value == null)
        {
            _sb.Append("NULL");
            return;
        }

        switch (value)
        {
            case string str:
                _sb.Append($"'{str.Replace("'", "''")}'");
                break;
            case Guid guid:
                _sb.Append($"'{guid}'");
                break;
            case Enum e:
                _sb.Append((int)value);
                break;
            case DateTime dt:
                _sb.Append($"'{dt:yyyy-MM-dd HH:mm:ss}'");
                break;
            case DateOnly date:
                _sb.Append($"'{date:yyyy-MM-dd}'");
                break;
            default:
                _sb.Append(value);
                break;
        }
    }
}
