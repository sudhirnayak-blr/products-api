using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase{
        private readonly ProductContext _context;
        public ProductController(ProductContext context)
        {
            _context = context;
            if(_context.Products.Count()==0){
                _context.Products.Add(new Product { ProductId = 1, ProductName = "Sample", UnitsInStock = 10, UnitPrice = 10 });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<Product>> GetAll(){
            return _context.Products.ToList(); 
        }

        [HttpGet("{id}", Name="GetProduct")]
        public ActionResult<Product> GetDetails(int id){
            //return _context.Products.FirstOrDefault(c => c.ProductId == id);
            var item = _context.Products.Find(id); 
            if(item==null)
                return NotFound();
            return item; 
        } 

        [HttpPost]
        public IActionResult Create(Product item){
            _context.Products.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetProduct", new { id = item.ProductId }, item);
        }
        [HttpPut]
        public IActionResult Update(Product item){
            var product = _context.Products.Find(item.ProductId); 
            if(product==null)
                return NotFound(); 

            product.ProductName = item.ProductName;
            product.UnitPrice = item.UnitPrice;
            product.UnitsInStock = item.UnitsInStock;
            product.Discontinued = item.Discontinued;
  

  
            _context.Products.Update(product);
            _context.SaveChanges();

            return NoContent(); 
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id){
            var item = _context.Products.Find(id); 
            if(item==null)
                return NotFound();
            
             _context.Products.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }
    }
}