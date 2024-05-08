using angelasFunctionApp2.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace angelasFunctionApp2.Functions
{
    public class GetAllProducts
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetAllProducts> _logger;

        public GetAllProducts(AppDbContext context, ILogger<GetAllProducts> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Function("GetProducts")]
        public async Task<IActionResult> GetProducts([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {
           
            if (req.Method == HttpMethods.Get)
            {
                var product = await _context.Products.ToListAsync();

                _logger.LogInformation("Get products");
                return new OkObjectResult(product);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
