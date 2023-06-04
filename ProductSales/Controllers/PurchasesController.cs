using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductSales.Models;

namespace ProductSales.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchasesController : ControllerBase
    {
        private readonly ProductCoreContext _context;

        public PurchasesController(ProductCoreContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            return await _context.Purchases.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(int id)
        {
          if (_context.Purchases == null)
          {
              return NotFound();
          }
            var purchase = await _context.Purchases.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }
        /// <summary>
        /// This will get all the purchase of the user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        // GET: api/GetUserPurchases/5
        [HttpGet]
        [Route("/GetUserPurchases/{userId}")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetUserPurchases(string userId)
        {//_context.Purchases.Where(x => x.UserId == new Guid(userId)).Select(x=>x.Product.ProductName)
            var purchasedProducts = await (from products in _context.Products
                                           join purchases in _context.Purchases
                                           on products.ProductId equals purchases.ProductId
                                           where (purchases.UserId == new Guid(userId))
                                           select new Purchase
                                           {
                                               ProductId = purchases.ProductId,
                                               PurchaseQuantity = purchases.PurchaseQuantity,
                                               PurchaseDate=purchases.PurchaseDate,
                                               PurchaseId = purchases.PurchaseId,
                                               Product=products,
                                           }).ToListAsync();

            if (purchasedProducts==null)
            {
                return NotFound();
            }

            return purchasedProducts;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
        {
            if (id != purchase.PurchaseId)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Purchases
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
          if (_context.Purchases == null)
          {
              return Problem("Entity set 'ProductCoreContext.Purchases'  is null.");
          }
            _context.Purchases.Add(purchase);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PurchaseExists(purchase.PurchaseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPurchase", new { id = purchase.PurchaseId }, purchase);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            if (_context.Purchases == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PurchaseExists(int id)
        {
            return (_context.Purchases?.Any(e => e.PurchaseId == id)).GetValueOrDefault();
        }
    }
}
