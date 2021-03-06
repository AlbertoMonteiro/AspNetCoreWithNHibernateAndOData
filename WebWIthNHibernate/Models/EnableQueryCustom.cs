﻿using System;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebWIthNHibernate.Models
{
    public sealed class EnableQueryCustomAttribute : EnableQueryAttribute
    {
        private static readonly Type PageResultType = typeof(PaginatedResult<>);

        public override void ValidateQuery(HttpRequest request, ODataQueryOptions queryOptions)
        {
            base.ValidateQuery(request, queryOptions);
            request.HttpContext.Items.Add(nameof(ODataQueryOptions), queryOptions);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
                return;
            base.OnActionExecuted(context);
            var oDataFeature = context.HttpContext.ODataFeature();
            if (oDataFeature.TotalCount.HasValue && context.Result is ObjectResult obj && obj.Value is IQueryable queryable)
                context.Result = new ObjectResult(CreateInstance(queryable, oDataFeature.TotalCount)) { StatusCode = 200 };
        }

        private static object CreateInstance(IQueryable query, long? totalCount) 
            => Activator.CreateInstance(PageResultType.MakeGenericType(query.ElementType), query, totalCount);
    }
}
