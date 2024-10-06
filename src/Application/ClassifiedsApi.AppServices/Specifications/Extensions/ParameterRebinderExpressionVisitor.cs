using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ClassifiedsApi.AppServices.Specifications.Extensions;

/// <summary>
/// Класс, предназначенный для замены именованных параметров одного выражения на параметры другого.
/// </summary>
public sealed class ParameterRebinderExpressionVisitor : ExpressionVisitor
{
    private readonly IReadOnlyDictionary<ParameterExpression, ParameterExpression> _parametersMap;
    
    private ParameterRebinderExpressionVisitor(IReadOnlyDictionary<ParameterExpression, ParameterExpression> parametersMap)
    {
        _parametersMap = parametersMap;
    }
    
    /// <summary>
    /// Выполняет замену параметров второго выражения на параметры первого.
    /// </summary>
    /// <param name="left">Первое выражение.</param>
    /// <param name="right">Второе выражение.</param>
    /// <typeparam name="TDelegate">Тип делегата.</typeparam>
    /// <returns>Выражение.</returns>
    public static Expression RebindParameters<TDelegate>(Expression<TDelegate> left, Expression<TDelegate> right)
    {
        var rightParameters = right.Parameters;
        var parametersMap = left.Parameters
            .Select((expression, index) => new { LeftParameter = expression, RightParameter = rightParameters[index] })
            .ToDictionary(d => d.RightParameter, d => d.LeftParameter);
        var rebinder = new ParameterRebinderExpressionVisitor(parametersMap);
        return rebinder.Visit(right.Body);
    }
    
    /// <inheritdoc />
    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (_parametersMap.TryGetValue(node, out var replacement))
        {
            node = replacement;
        }
        return base.VisitParameter(node);
    }
}