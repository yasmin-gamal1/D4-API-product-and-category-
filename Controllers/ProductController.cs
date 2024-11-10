using APIDay4_Company.DTOs;
using APIDay4_Company.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIDay4_Company.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly CompanyContext _db;

        public ProductController(CompanyContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _db.Products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Amount = p.Amount,
                CategoryId = p.CategoryId
            }).ToListAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            var productDTO = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Amount = product.Amount,
                CategoryId = product.CategoryId
            };
            return Ok(productDTO);
        }

        [HttpGet("price/{price}")]
        public async Task<IActionResult> GetByPrice(decimal price)
        {
            var products = await _db.Products
                .Where(p => p.Price == price)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Description = p.Description,
                    Amount = p.Amount,
                    CategoryId = p.CategoryId
                }).ToListAsync();

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductDTO productDTO)
        {
            var product = new Product
            {
                Name = productDTO.Name,
                Price = productDTO.Price,
                Description = productDTO.Description,
                Amount = productDTO.Amount,
                CategoryId = productDTO.CategoryId
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, productDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, ProductDTO productDTO)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            product.Name = productDTO.Name;
            product.Price = productDTO.Price;
            product.Description = productDTO.Description;
            product.Amount = productDTO.Amount;
            product.CategoryId = productDTO.CategoryId;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
