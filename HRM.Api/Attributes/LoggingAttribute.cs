
using Microsoft.AspNetCore.Mvc.Filters;
using HRM.Common.Attributes;

namespace HRM.Api.Attributes
{
    public class LoggingAttribute : BaseActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    LogRequest(context);
        //    base.OnActionExecuting(context);
        //}

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            LogResponse(context);
            base.OnResultExecuted(context);
        }
    }
}
