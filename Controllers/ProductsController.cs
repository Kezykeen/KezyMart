using Microsoft.AspNetCore.Mvc;
using System.Linq;
using KezyMart.Models;
using KezyMart.Persistence;
using KezyMart.Repositories;
using KezyMart.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KezyMart.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;

        public ProductsController(IUnitOfWork unitOfWork, IProductRepository productRepo,
           ICategoryRepository categoryRepo)
        {
            _unitOfWork = unitOfWork;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        // Browse category
        public ActionResult Browse(string category)
        {
            var categoryModel = _categoryRepo.GetCategory().Include(p=>p.Products)
                .Single(c => c.Name == category);
            return View(categoryModel);
        }

        public ActionResult CategoryList()
        {
            var categories = _categoryRepo.GetCategory().ToList();
            return PartialView("CategoryListPartialView", categories);
        }

        // GET: Products
        public ActionResult Index()
        {
            var product = _productRepo.GetProductWithCategory();

            return User.IsInRole(RoleName.CanManageProducts) ? View("AdminIndex", product.ToList()) : View(product.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = _productRepo.FindProduct(id);
            Category category = _categoryRepo.GetCategory().Single(c => c.Id == product.CategoryId);

            ProductCategoryViewModel viewModel = new ProductCategoryViewModel
            {
                product = product,
                category = category
            };

            if (product == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // GET: Products/Create
        [Authorize(Roles = RoleName.CanManageProducts)]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetCategory(), "Id", "Name");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageProducts)]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Add(product);
                _unitOfWork.Complete();
 
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(_categoryRepo.GetCategory(), "Id", "Name", product.CategoryId);

            return View(product);
        }

        

        // GET: Products/Edit/5
        [Authorize(Roles = RoleName.CanManageProducts)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            Product product = _productRepo.FindProduct(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetCategory(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageProducts)]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Entry(product).State = EntityState.Modified;
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(_categoryRepo.GetCategory(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = RoleName.CanManageProducts)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Product product = _productRepo.FindProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = RoleName.CanManageProducts)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = _productRepo.FindProduct(id);
            _productRepo.Remove(product);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public ActionResult GetProductQuickViewModal(int id)
        {
            var product = _productRepo.FindProduct(id);
            return PartialView("_ProductQuickViewModal", product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
