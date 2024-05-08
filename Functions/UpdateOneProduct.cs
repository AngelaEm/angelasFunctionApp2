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
        private readonly ILogger<UpdateOneProduct> _logger;

      
        public UpdateOneProduct(AppDbContext context, ILogger<UpdateOneProduct> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Function("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([HttpTrigger(AuthorizationLevel.Function, "put", Route = "products/{id}")] HttpRequest req, Guid id)
        {
            
            var product = await _context.Products.FindAsync(id);
            
            if (product is null)
            {
                return new NotFoundResult();
            }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var newData = JsonConvert.DeserializeObject<Products>(requestBody);

            product.Name = newData.Name;
            product.Price = newData.Price;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated product");
            return new OkObjectResult(product);
        }
    }
}

