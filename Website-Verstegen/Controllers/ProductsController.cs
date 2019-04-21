using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Website_Verstegen.Data;
using Website_Verstegen.Models;
using Website_Verstegen.ViewModels;

namespace Website_Verstegen.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DatabaseContext _context;
        private IHostingEnvironment _hostingEnvironment;

        public ProductsController(DatabaseContext context, IHostingEnvironment environment)
        {
            _context = context;
            _hostingEnvironment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ProductOverview(string itemtypes, string sortOrder, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TitleSortParm"] = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";
            ViewData["CategorySortParm"] = sortOrder == "Category" ? "Category_desc" : "Category";
            ViewData["DatumSortParm"] = sortOrder == "Datum" ? "Datum_desc" : "Datum";

            var products = from p in _context.Products select p;
            if (itemtypes != null)
            {
                products = _context.Products.Where(p => p.Category == itemtypes);
            }

            switch (sortOrder)
            {
                case "Title_desc":
                    products = products.OrderByDescending(s => s.Title);
                    break;
                case "Category":
                    products = products.OrderBy(s => s.Category);
                    break;
                case "Category_desc":
                    products = products.OrderByDescending(s => s.Category);
                    break;
                case "Datum":
                    products = products.OrderBy(s => s.Id);
                    break;
                case "Datum_desc":
                    products = products.OrderByDescending(s => s.Id);
                    break;
                default:
                    products = products.OrderBy(s => s.Title);
                    break;
            }

            int pageSize = 6;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ProductOverview(FormCollection form, string sortOrder, int? page)
        {
            var itemValues = form["itemtypes"];

            var products = from p in _context.Products where p.Category == itemValues select p;

            int pageSize = 6;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), page ?? 1, pageSize));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ProductDetails(int? id)
        {
            //Values for the ViewModel
            var product = await _context.Products.FindAsync(id);
            List<ProductDetails> productDetails = _context.ProductDetails.Where(p => p.ProductId == id).ToList();

            Product_ProductDetailViewModel productViewModel = new Product_ProductDetailViewModel { Product = product, AllDetails = productDetails };

            if (id == null) return NotFound();

            return View(productViewModel);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Type,Category,ImageUrl,Description,Contents,DownloadLink")] Product product, IFormFile ImageUrl, IFormCollection form)
        {
            //Makes the local url where the image is located
            string[] paths = { _hostingEnvironment.WebRootPath, "images", product.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            //No Image Uploaded Means Back To The Create Action
            if (ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    if (ModelState.IsValid)
                    {
                        NewProductToDatabase(product, path, form);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    //Actually makes the url from the image to an image
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                        if (ModelState.IsValid)
                        {
                            NewProductToDatabase(product, path, form);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
            }
            return View(product);
        }

        //Writes A New Product to The Database
        private void NewProductToDatabase(Product product, string path, IFormCollection form)
        {
            product.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
            _context.Update(product);
            AddProductDetails(product, form);
        }

        //Adds all the product detail
        private void AddProductDetails(Product product, IFormCollection form)
        {
            //This method gets all the Product Details From The Form And Adds Them To The Right Product
            var product_details = new List<ProductDetails>();
            var count = Convert.ToInt32(form["product_details_counter"]);
            for (int i = 1; i <= count; i++)
            {
                product_details.Add(new ProductDetails { Product = product, ProductId = product.Id, Details = form["product_detail_" + i] });
            }
            product_details.ForEach(dp => _context.ProductDetails.Add(dp));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            //Values for the ViewModel
            var product = await _context.Products.FindAsync(id);
            List<ProductDetails> productDetails = _context.ProductDetails.Where(p => p.ProductId == id).ToList();

            Product_ProductDetailViewModel productViewModel = new Product_ProductDetailViewModel { Product = product, AllDetails = productDetails };

            if (id == null)
            {
                return NotFound();
            }

            if (product == null)
            {
                return NotFound();
            }
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Type,Category,ImageUrl,Description,Contents,DownloadLink")] Product product, IFormFile ImageUrl, IFormCollection form)
        {
            //Gets all the product details from the View
            var formValues = form["product_detail"];

            if (id != product.Id)
            {
                return NotFound();
            }

            //Makes the local url where the image is located
            string[] paths = { _hostingEnvironment.WebRootPath, "images", product.Type + "s".ToLower() };
            var upload = Path.Combine(paths);

            if (ImageUrl != null)
            {
                var path = Path.Combine(upload, ImageUrl.FileName);
                if (System.IO.File.Exists(path))
                {
                    WriteToDatabase(product, path);
                    UpdateProductDetails(formValues, product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Actually makes the url from the image to an image
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                        WriteToDatabase(product, path);
                        UpdateProductDetails(formValues, product);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    UpdateProductDetails(formValues, product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(product);
        }


        //Updates The Product Details That Belong To The Right Product
        private void UpdateProductDetails(string[] newDetails, Product product)
        {
            List<ProductDetails> currentDetails = _context.ProductDetails.Where(p => p.ProductId == product.Id).ToList();
            if (ModelState.IsValid)
            {
                for (int i = 0; i < currentDetails.Count(); i++)
                {
                    currentDetails[i].Details = newDetails[i];
                }
                _context.Update(product);
            }
        }

        //Updates Database With A New Image Path
        private void WriteToDatabase(Product product, string path)
        {
            if (ModelState.IsValid)
            {
                product.ImageUrl = path.Substring(path.LastIndexOf("\\images"));
                _context.Update(product);
            }
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
