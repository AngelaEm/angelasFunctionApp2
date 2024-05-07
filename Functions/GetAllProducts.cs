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


        public GetAllProducts(AppDbContext context)
        {
            _context = context;
        }

        [Function("GetAllProducts")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "products")] HttpRequest req)
        {
            string expectedKey = Environment.GetEnvironmentVariable("MyFunctionKey");
            string functionKey = req.Headers["x-functions-key"];


            if (functionKey != expectedKey)
            {
                return new UnauthorizedResult();
            }

            if (req.Method == HttpMethods.Get)
            {
                var product = await _context.Products.ToListAsync();
                return new OkObjectResult(product);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
