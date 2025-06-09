using System.Linq.Expressions;

namespace ClickHouseApi.GenericRepository;

public interface IExpressionSqlTranslator
{
    string Translate(Expression expression);
}
