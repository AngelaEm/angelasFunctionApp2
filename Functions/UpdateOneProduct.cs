using angelasFunctionApp2.DataAccess;
using angelasFunctionApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace angelasFunctionApp2.Functions
{
    public class UpdateOneProduct
    {
        private readonly AppDbContext _context;

        public UpdateOneProduct(AppDbContext context)
        {
            _context = context;
        }

        [Function("UpdateProduct")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{id}")] HttpRequest req, Guid id)
        {
            string expectedKey = Environment.GetEnvironmentVariable("MyFunctionKey");
            string functionKey = req.Headers["x-functions-key"];

            if (functionKey != expectedKey)
            {
                return new UnauthorizedResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedProductData = JsonConvert.DeserializeObject<Products>(requestBody);

            var product = await _context.Products.FindAsync(id);
            if (product is null)
            {
                return new NotFoundResult();
            }

            product.Name = updatedProductData.Name;
            product.Price = updatedProductData.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return new OkObjectResult(product);
        }
    }
}

