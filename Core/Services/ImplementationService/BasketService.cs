using AutoMapper;
using Domain.Contracs;
using Domain.Entities.BasketModule;
using Domain.Exceptions;
using Services.Abstractions.Contracts;
using Shared.DTOs.BasketModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ImplementationService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketService(IBasketRepository basketRepository , IMapper mapper)
        {
            _basketRepository = basketRepository;
           _mapper = mapper;
        }
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDTO)
        {
            var basket = _mapper.Map<BasketDto , CustomerBasket>(basketDTO);
            var createdOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket);
            if (createdOrUpdatedBasket is null)
                throw new Exception("Can Not Create Or Update The Basket");
            else
                return _mapper.Map< CustomerBasket , BasketDto>(createdOrUpdatedBasket);
        }

        public async Task<bool> DeleteBasketAsync(string id)
            => await _basketRepository.DeleteBasketAsync(id);

        public async Task<BasketDto> GetBasketAsync(string id)
        {
           var basket = await _basketRepository.GetBasketByIdAsync(id);
            if (basket is null)
                throw new BasketNotFoundException(id);
            else
                return _mapper.Map<CustomerBasket , BasketDto>(basket);
        }
    }
}
