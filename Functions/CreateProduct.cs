using angelasFunctionApp2.DataAccess;
using angelasFunctionApp2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace angelasFunctionApp2.Functions
{
    public class CreateProduct
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CreateProduct> _logger;

        public CreateProduct(AppDbContext context, ILogger<CreateProduct> logger)
        {
            _context = context;
            _logger = logger;
        }

        [Function("CreateOneProduct")]
        public async Task<IActionResult> CreateOneProduct([HttpTrigger(AuthorizationLevel.Function, "post", Route = "products")] HttpRequest req)
        {
            

            if (req.Method == HttpMethods.Post)
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var product = JsonConvert.DeserializeObject<Products>(requestBody);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Create product");
                return new CreatedResult("/products", product);

            }
            else
            {

                return new BadRequestResult();
            }
        }
    }
}
