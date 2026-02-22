using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Authorize]
    public class BasketController : ApiController
    {
        private readonly IServiceManager _serviceManager;
        public BasketController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }


        // Get ==>   BaseUrl/api/Basket=id?Basket01
        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBsketAsync(string id)
        {
            var basket = await _serviceManager.BasketService.GetBasketAsync(id);
            return Ok(basket); 
        }
        // Post ==>   BaseUrl/api/Basket
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = await _serviceManager.BasketService.CreateOrUpdateBasketAsync(basketDto);
            return Ok(basket);
        }
        // Delete ==>   BaseUrl/api/Basket/id(Basket01)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            await _serviceManager.BasketService.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
