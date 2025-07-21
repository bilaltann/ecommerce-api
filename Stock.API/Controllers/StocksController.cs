using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly IMongoCollection<Stock.API.Models.Entities.Stock> _stocksCollection;

        public StocksController(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("StockAPIDB");
            _stocksCollection = database.GetCollection<Stock.API.Models.Entities.Stock>("stock");
        }

        [HttpGet]
        public async Task<IActionResult> GetStocks()
        {var stocks = await _stocksCollection.Find(
            _ => true).ToListAsync();
            return Ok(stocks);
        }
    }
}
