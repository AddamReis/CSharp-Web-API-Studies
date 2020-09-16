using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI_Catalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        //executa antes da action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("############################");
            _logger.LogInformation("###---OnActionExecuting---##");
            _logger.LogInformation($"###-DATA: {DateTime.Now}-###");
            _logger.LogInformation("############################");
        }

        //executa depois da action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("############################");
            _logger.LogInformation("###---OnActionExecuted---##");
            _logger.LogInformation($"###-DATA: {DateTime.Now}-###");
            _logger.LogInformation("############################");
        }

        
    }
}
