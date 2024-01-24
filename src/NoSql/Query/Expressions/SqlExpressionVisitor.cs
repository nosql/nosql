namespace NoSql.Query.Expressions;

public abstract class SqlExpressionVisitor
{
    protected virtual void Visit(SqlExpression expression)
    {
        if (expression is SqlConstantExpression constantExpression)
        {
            VisitConstant(constantExpression);
        }
        else if (expression is SqlColumnExpression columnExpression)
        {
            VisitColumn(columnExpression);
        }
        else if (expression is SqlJsonExtractExpression jsonExtractExpression)
        {
            VisitJsonExtract(jsonExtractExpression);
        }
        else if (expression is SqlTableExpression tableExpression)
        {
            VisitTable(tableExpression);
        }
        else if(expression is SqlProjectionListExpression projectionListExpression)
        {
            VisitProjectionList(projectionListExpression);
        }
        else if (expression is SqlProjectionExpression projectionExpression)
        {
            VisitProjection(projectionExpression);
        }
        else if (expression is SqlBinaryExpression binaryExpression)
        {
            VisitBinary(binaryExpression);
        }
        else if (expression is SqlUnaryExpression unaryExpression)
        {
            VisitUnary(unaryExpression);
        }
        else if (expression is SqlExistsExpression existExpression)
        {
            VisitExists(existExpression);
        }
        else if (expression is SqlLikeExpression likeExpression)
        {
            VisitLike(likeExpression);
        }
        else if (expression is SqlCastExpression castExpression)
        {
            VisitCast(castExpression);
        }
        else if (expression is SqlInExpression inExpression)
        {
            VisitIn(inExpression);
        }
        else if (expression is SqlSelectExpression selectExpression)
        {
            VisitSelect(selectExpression);
        }
        else if (expression is SqlOrderingExpression orderingExpression)
        {
            VisitOrdering(orderingExpression);
        }
        else if (expression is SqlUpdateExpression updateExpression)
        {
            VisitUpdate(updateExpression);
        }
        else if (expression is SqlDeleteExpression deleteExpression)
        {
            VisitDelete(deleteExpression);
        }
        else if (expression is SqlInsertExpression insertExpression)
        {
            VisitInsert(insertExpression);
        }
        else if (expression is SqlColumnValueSetExpression setExpression)
        {
            VisitColumnValueSet(setExpression);
        }
        else if (expression is SqlJsonSetExpression jsonSetExpression)
        {
            VisitJsonSet(jsonSetExpression);
        }
        else if(expression is SqlJsonArrayLengthExpression arrayLengthExpression)
        {
            VisitJsonArrayLength(arrayLengthExpression);
        }
        else if (expression is SqlJsonArrayEachExpression arrayEachExpression)
        {
            VisitJsonArrayEach(arrayEachExpression);
        }
        else if (expression is SqlJsonArrayEachItemExpression eachItemExpression)
        {
            VisitJsonArrayEachItem(eachItemExpression);
        }
        else if(expression is SqlJsonBuildArrayExpression buildArrayExpression)
        {
            VisitJsonBuildArray(buildArrayExpression);
        }
        else if (expression is SqlJsonMergeExpression patchExpression)
        {
            VisitJsonMerge(patchExpression);
        }
        else if (expression is SqlJsonObjectExpression objectExpression)
        {
            VisitJsonObject(objectExpression);
        }
        else if (expression is SqlFunctionExpression functionExpression)
        {
            VisitFunction(functionExpression);
        }
        else if (expression is SqlFragmentExpression tokenExpression)
        {
            VisitFragment(tokenExpression);
        }
        else if (expression is SqlSubqueryExpression subqueryExpression)
        {
            VisitSubquery(subqueryExpression);
        }
        else
        {
            ThrowHelper.ThrowTranslateException_ExpressionNotSupported(expression);
        }
    }

    protected abstract void VisitJsonMerge(SqlJsonMergeExpression expression);
    protected abstract void VisitJsonBuildArray(SqlJsonBuildArrayExpression expression);
    protected abstract void VisitJsonArrayLength(SqlJsonArrayLengthExpression expression);
    protected abstract void VisitJsonObject(SqlJsonObjectExpression expression);
    protected abstract void VisitJsonSet(SqlJsonSetExpression expression);
    protected abstract void VisitJsonArrayEach(SqlJsonArrayEachExpression expression);
    protected abstract void VisitJsonArrayEachItem(SqlJsonArrayEachItemExpression expression);
    protected abstract void VisitJsonExtract(SqlJsonExtractExpression expression);

    protected abstract void VisitIn(SqlInExpression expression);
    protected abstract void VisitExists(SqlExistsExpression expression);

    protected abstract void VisitSubquery(SqlSubqueryExpression expression);

    protected abstract void VisitLike(SqlLikeExpression expression);

    protected abstract void VisitCast(SqlCastExpression expression);
    protected abstract void VisitBinary(SqlBinaryExpression expression);
    protected abstract void VisitUnary(SqlUnaryExpression expression);
    protected abstract void VisitConstant(SqlConstantExpression expression);
    protected abstract void VisitFragment(SqlFragmentExpression expression);
    protected abstract void VisitFunction(SqlFunctionExpression expression);

    protected abstract void VisitProjectionList(SqlProjectionListExpression expression);
    protected abstract void VisitProjection(SqlProjectionExpression expression);
    protected abstract void VisitColumnValueSet(SqlColumnValueSetExpression expression);
    protected abstract void VisitOrdering(SqlOrderingExpression expression);

    protected abstract void VisitTable(SqlTableExpression expression);
    protected abstract void VisitColumn(SqlColumnExpression expression);


    protected abstract void VisitSelect(SqlSelectExpression expression);
    protected abstract void VisitUpdate(SqlUpdateExpression expression);
    protected abstract void VisitDelete(SqlDeleteExpression expression);
    protected abstract void VisitInsert(SqlInsertExpression expression);
}