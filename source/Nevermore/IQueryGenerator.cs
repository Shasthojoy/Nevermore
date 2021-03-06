using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Server;
using Nevermore.Joins;

namespace Nevermore
{
    public interface IQueryGenerator
    {
        CommandParameters QueryParameters { get; }
        IList<IJoin> Joins { get; }
        string ViewOrTableName { get; }

        string ToString();

        void UseTable(string tableName);
        void UseView(string viewName);
        void UseHint(string hintClause);
        
        string CountQuery();
        string TopQuery(int top = 1);
        string SelectQuery(bool orderBy = true);
        string PaginateQuery(int skip, int take);
        string DeleteQuery();

        void AddOrder(string fieldName, bool descending);
        void AddWhere(WhereParameter whereParams);
        void AddWhere(string whereClause);
        void WhereBetween(string fieldName, object startValue, object endValue);
        void WhereBetweenOrEqual(string fieldName, object startValue, object endValue);
        void WhereEquals(string fieldName, object value);
        void WhereNotEquals(string fieldName, object value);
        void WhereIn(string fieldName, object values);
        void WhereStartsWith(string fieldName, object value);
        void WhereEndsWith(string fieldName, object value);
        void WhereGreaterThan(string fieldName, object value);
        void WhereGreaterThanOrEqual(string fieldName, object value);
        void WhereLessThan(string fieldName, object value);
        void WhereLessThanOrEqual(string fieldName, object value);
        void WhereContains(string fieldName, object value);
        void WhereContainsAll(string fieldName, IEnumerable<object> values);
        void WhereContainsAny(string fieldName, IEnumerable<object> values);
        void OpenSubClause();
        void CloseSubClause();
        void AndAlso();
        void OrElse();
        void AddParameter(string fieldName, object value);
        void AddJoin(IJoin join);
    }
}