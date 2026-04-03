using AdminDashboard.Helper;
using AdminDashboard.Models;
using Domain.Contracs;
using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Presistence.Data.Configrations;
using Services.Specifications;
using Shared;

namespace AdminDashboard.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
          
            // specifications عشان نبعتها ل ال queryparams هنجيب ال
            var queryparams = new ProductSpecificationsParameters();
           
            // specifications و هبعتلها ال products عشان هجيب كل ال specifications هنجيب ال
            var specifications = new ProductWithTypeAndBrandSpecifications(queryparams , true);
            
            // products جبنا كل ال
            var products = await ProductRepo.GetAllAsync(specifications);

            // productViewModel الي product هنحول ال
            var productViewModel = products.Select(product => new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PictureUrl = product.PictureUrl,
                Price = product.Price,
                BrandId = product.BrandId,
                TypeId = product.TypeId,
                Brand = product.ProductBrand,
                Type = product.productType

            });

            return View(productViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            // Valid في حاله انو
            if (ModelState.IsValid)
            {
                //  لو بعت صوره 
                if (model.Image is not null)
                    model.PictureUrl = PictureSettings.UploadFile(model.Image, " products");
                // لو مبعتش صوره
                //else
                //    // adding default images

                // unitofwor; عن طريق Database عشان نعرف نضيفو في Produc الي ProductViewModel هنحول من
                var mappedProduct = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    BrandId = model.BrandId,
                    TypeId = model.TypeId,
                    PictureUrl = model.PictureUrl!
                };
                // database في ال mappedProduct هنروح نضيف ال
                await _unitOfWork.GetRepository<Product, int>().AddAsync(mappedProduct);
                await _unitOfWork.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            // Valid في حاله انو م

            return View(model);
        }
    }
}
