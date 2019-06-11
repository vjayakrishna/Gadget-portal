using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineGadgetStore.Data;
using OnlineGadgetStore.Models;

namespace OnlineGadgetStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Index()
        {
            var carts = await _context.ShoppingCart.Include(s => s.ApplicationUser).Include(s=>s.LineProducts).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            if(user==null)
            {
                carts = carts.Where(c => (String.IsNullOrEmpty(c.ApplicationUserID))).ToList();
            }
            else carts = carts.Where(c => (user.Id.Equals(c.ApplicationUserID))).ToList();
            ShoppingCart cart;
            if (carts.Count == 0)
            {
                cart = new ShoppingCart
                {
                    LineProducts = new List<LineProduct>(),
                    ApplicationUserID = (user == null) ? null : user.Id
                };
                _context.Add(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                cart = carts.ElementAt(0);
            }
            cart.TotalPrice = 0;
            foreach(var item in cart.LineProducts)
            {
                item.Product = await _context.Product.FindAsync(item.ProductID);
                cart.TotalPrice += (item.Product.Price) * (item.Quantity);
            }
            return View(cart);
        }

        public async Task<IActionResult> AddProduct(int Id) 
        {
            var carts = await _context.ShoppingCart.Include(s => s.ApplicationUser).Include(s => s.LineProducts).ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                carts = carts.Where(c => (String.IsNullOrEmpty(c.ApplicationUserID))).ToList();
            }
            else carts = carts.Where(c => (user.Id.Equals(c.ApplicationUserID))).ToList();
            ShoppingCart cart;
            if (carts.Count == 0)
            {
                cart = new ShoppingCart
                {
                    LineProducts = new List<LineProduct>(),
                    ApplicationUserID = (user == null) ? null : user.Id
                };
            }
            else
            {
                cart = carts.ElementAt(0);
            }
            var product = await _context.Product.FindAsync(Id);
            if(cart.LineProducts.Any(l=>l.ProductID==Id))
            {
                var lineproduct = cart.LineProducts.SingleOrDefault(l => l.ProductID == Id);
                lineproduct.Quantity +=1;
            }
            else
            {
                cart.LineProducts.Add(new LineProduct
                {
                    ProductID = Id,
                    Quantity = 1,
                    Price = product.Price
                });
            }
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //return RedirectToAction("Details", "Products", new { Id });
        }
        
        public async Task<IActionResult> RemoveProduct(int Id)
        {
            var lineproduct = await _context.LineProduct.SingleOrDefaultAsync(l => l.ID == Id);
            _context.LineProduct.Remove(lineproduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> CheckOut(int Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart =  _context.ShoppingCart.Include(s=>s.LineProducts).SingleOrDefault(s => s.ID == Id);
            if(String.IsNullOrEmpty(cart.ApplicationUserID))
            {
                var tempcarts = _context.ShoppingCart.Where(s => s.ApplicationUserID == user.Id).ToList();
                foreach(var temp in tempcarts)
                {
                    _context.Remove(temp);
                }
                cart.ApplicationUserID = user.Id;
                _context.Update(cart);
                await _context.SaveChangesAsync();
            }
            _context.Remove(cart);
            await _context.SaveChangesAsync();
            cart.TotalPrice = 0;
            foreach (var item in cart.LineProducts)
            {
                item.Product = await _context.Product.FindAsync(item.ProductID);
                cart.TotalPrice += (item.Product.Price) * (item.Quantity);
            }
            return View(cart);
        }

        private bool ShoppingCartExists(int id)
        {
            return _context.ShoppingCart.Any(e => e.ID == id);
        }
    }
}
