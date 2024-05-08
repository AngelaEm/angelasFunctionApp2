using angelasFunctionApp2.DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace angelasFunctionApp2.Functions
{
    public class DeleteProduct
    {
       
        private readonly AppDbContext _context;
        private readonly ILogger<DeleteProduct> _logger;

        public DeleteProduct(AppDbContext context, ILogger<DeleteProduct> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Function("DeleteProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "products/{id}")] HttpRequest req, Guid id)
        {
            string expectedKey = Environment.GetEnvironmentVariable("MyFunctionKey");
            string functionKey = req.Headers["x-functions-key"];

            if (functionKey != expectedKey)
            {
                return new UnauthorizedResult();
            }

            var productToDelete = await _context.Products.FindAsync(id);
            if (productToDelete is null)
            {
                return new NotFoundResult();
            }

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted product");
            return new NoContentResult();


        }
    }
}
