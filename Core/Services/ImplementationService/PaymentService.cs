using AutoMapper;
using Domain.Contracs;
using Domain.Entities.BasketModule;
using Domain.Entities.OrderModule;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Services.Abstractions.Contracts;
using Shared.DTOs.BasketModule;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = Domain.Entities.ProductModule.Product;
namespace Services.ImplementationService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;

        public PaymentService( IConfiguration configuration , IUnitOfWork unitOfWork , IMapper mapper , IBasketRepository basketRepository)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
           _mapper = mapper;
            _basketRepository = basketRepository;
        }

        /// <summary>
        ///  Refactor قبل ال Function دي ال
        /// دي مشروخ فيها حصل اي بالتفاصيل الممل
        /// </summary>

        //public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        //{
        //    //0] Instal Package ==> Stripe.Net

        //    //1] Set Up Key [ Secret Key ] 
        //    // اللي بيكلمك او مين الاكونت اللي بيكلمك Api كدا عرفتو من ال
        //    StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

        //    //2] Get basket [by basketId]
        //    var basket = await _basketRepository.GetBasketAsync(basketId)
        //                       ?? throw new BasketNotFoundException(basketId);

        //    //3] Validate items Price ==> [basket.item.price =  product.Price ] Product ==> From Database
        //    foreach(var item in basket.BasketItems)
        //    {
        //        // client  مش اللي مبعوت في السله او اللي باعتو ال database عشان اجيب السعر الحقيقي بتاعو من ال product انا جبت ال
        //        var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
        //                     ?? throw new ProductNotFoundException(item.Id);

        //        // اللي مبعوت في السله Price مش تاخد ال DB من ال Price كدا بعرفو انو يجيب ال
        //        item.Price = product.Price;
        //    }

        //    //4] Validate Shipping Price ==> Get DeliveryMethod [DeliveryMethodId] ==> ShippingPrice = DeliveryMethod.price

        //    // ShippingPrice مش هعمل فحص بقا ع ال Value عشان هيه لو مفهاش  Exception هرجعلو Value لو مفهاش  DeliveryMethofId دي بتعمل فحص بتشوف ال
        //    if (!basket.DeliveryMethoId.HasValue) throw new Exception(" No Delivery Method Selected");

        //    // هروح بقا اجيبها DeliveryMethoId لو هو مختار 
        //    var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
        //          .GetByIdAsync(basket.DeliveryMethoId.Value) 
        //          ?? throw new DeliveryMethodNotFountExceptions(basket.DeliveryMethoId.Value);

        //    // --- DB لانها جايه من ال DeliveryMethod.price ان هو ياخدها من ال shippingPrice ل القيمه بتاع set هروح بقا اعمل
        //    basket.ShippingPrice = deliveryMethod.Price;

        //    //5] total ==> [subtotal + Shippingprice] 
        //    //              subtotal ==> (long) ([basket.items.Quantity * basket.items.price] + ShippingPric ) *100
        //    // *100 الناتج اللي هيطلع هنضربو ف cent و عشان نحولو ل cent ميكونش علش شكل دولار لا عاوز علي شكل اصغر عمله ل الدولار اللي هيه ال total هو عاوز ال
        //    // long ل ال casting و اعملها total ف هروح اجيب ال long بيقبل العمله علي شكل  Stripe و ال

        //    var total = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;

        //    //6] Create Or Update PaymentIntentId

        //    // براحتي Create  و  Update عشان اعمل Stripe بتاع ال service دي بتوصلني ل ال         
        //    var stripeService = new PaymentIntentService();
        //    //
        //    //Create فاضيه يبقي كدا هو رايح يعمل basket.paymentItntenId لو لقيت ال
        //    if(string.IsNullOrEmpty(basket.PaymentIntentId))
        //    {
        //        // Create
        //        var options = new PaymentIntentCreateOptions
        //        {
        //            Amount = total, // Total = subtotal + ShippingPrice
        //            Currency = "USD", // doller العمله
        //            PaymentMethodTypes = ["card"],   // طريقه الدفع ب الفيزا

        //        };
        //        //  PaymentIntentId ل ال Create كدا احنا عملنا
        //        // (PaymentIntentId , ClientSecret)كلها اللي راجعه و دي اللي فيها المعلومتين اللي عاوزهم الفرونت الل يهما paymentIntet دي كدا ال
        //        var paymentIntet = await stripeService.CreateAsync(options);

        //        basket.PaymentIntentId = paymentIntet.Id; // front دي اول معلومه محتاجه ترجع ل ال
        //        basket.ClientSecret = paymentIntet.ClientSecret;  // front دي تاني معلومه محتاج ارجعها ل ال
        //    }
        //    // Update مش فاضيه ف هروح اعمل PaymentIntentId كدا هيه فيها
        //    else
        //    {
        //        // و هخلي السعر يساوي السعر بتاعي  Update هعمل
        //        var options = new PaymentIntentUpdateOptions()
        //        {
        //              Amount = total,
        //        };
        //        // Update اللي هيعمل عليها basket.PaymentIntentId بعتلو هنا ال
        //        await stripeService.UpdateAsync(basket.PaymentIntentId, options);
        //    }

        //    //7] Save Changes [Update] Basket
        //    // ---> عشان التغيرات اللي حصلت دي basket ل ال CreateOrUpdate هيعمل
        //    await _basketRepository.CreateOrUpdateBasketAsync(basket);

        //    //8] Map To BasketDto == return
        //    return _mapper.Map<BasketDto>(basket);

        //}

       
        
        /// <summary>
        ///  Refactor بعد ال Function دي ال
        /// </summary>

        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            //0] Instal Package ==> Stripe.Net

            //1] Set Up Key [ Secret Key ] 
            // اللي بيكلمك او مين الاكونت اللي بيكلمك Api كدا عرفتو من ال
            StripeConfiguration.ApiKey = _configuration.GetSection("StripeSettings")["SecretKey"];

            //2] Get basket [by basketId]
           var basket = await GetBasketAsybc(basketId);

            //3] Validate items Price ==> [basket.item.price =  product.Price ] Product ==> From Database
            //4] Validate Shipping Price ==> Get DeliveryMethod [DeliveryMethodId] ==> ShippingPrice = DeliveryMethod.price

            //  DeliveryMethoId و basket.BasketItems.price علي ال Validate دي بتعمل
            await ValidateBasketAsync(basket);

            //5] total ==> [subtotal + Shippingprice] 
            //              subtotal ==> (long) ([basket.items.Quantity * basket.items.price] + ShippingPric ) *100
            // *100 الناتج اللي هيطلع هنضربو ف cent و عشان نحولو ل cent ميكونش علش شكل دولار لا عاوز علي شكل اصغر عمله ل الدولار اللي هيه ال total هو عاوز ال
            // long ل ال casting و اعملها total ف هروح اجيب ال long بيقبل العمله علي شكل  Stripe و ال
            var total = CalculateTotalAsync(basket);
         
            
            //6] Create Or Update PaymentIntentId
            await UpdateOrCreatePaymentIntentAsync(basket, total);
           
            //7] Save Changes [Update] Basket
            // ---> عشان التغيرات اللي حصلت دي basket ل ال CreateOrUpdate هيعمل
            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            //8] Map To BasketDto == return
            return _mapper.Map<BasketDto>(basket);

        }

        // Create Or Update PaymentIntent
        private async Task UpdateOrCreatePaymentIntentAsync(CustomerBasket basket, long total)
        {
           /// براحتي Create  و Update عشان اعمل Stripe بتاع ال service دي بتوصلني ل ال
                var stripeService = new PaymentIntentService();
            //
            //Create فاضيه يبقي كدا هو رايح يعمل basket.paymentItntenId لو لقيت ال
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions
                {
                    Amount = total, // Total = subtotal + ShippingPrice
                    Currency = "USD", // doller العمله
                    PaymentMethodTypes = ["card"],   // طريقه الدفع ب الفيزا

                };
                //  PaymentIntentId ل ال Create كدا احنا عملنا
                // (PaymentIntentId , ClientSecret)كلها اللي راجعه و دي اللي فيها المعلومتين اللي عاوزهم الفرونت الل يهما paymentIntet دي كدا ال
                var paymentIntet = await stripeService.CreateAsync(options);

                basket.PaymentIntentId = paymentIntet.Id; // front دي اول معلومه محتاجه ترجع ل ال
                basket.ClientSecret = paymentIntet.ClientSecret;  // front دي تاني معلومه محتاج ارجعها ل ال
            }
            // Update مش فاضيه ف هروح اعمل PaymentIntentId كدا هيه فيها
            else
            {
                // و هخلي السعر يساوي السعر بتاعي  Update هعمل
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = total,
                };
                // Update اللي هيعمل عليها basket.PaymentIntentId بعتلو هنا ال
                await stripeService.UpdateAsync(basket.PaymentIntentId, options);
            }
        }

        // Calculate Total 
        private long CalculateTotalAsync(CustomerBasket basket)
        {
             var total = (long)(basket.BasketItems.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;
             return total;
        }

        /// <summary>
        /// DeliveryMethoId  و basket.BasketItems.price علي ال Validate دي بتعمل
        /// </summary>
        private async Task ValidateBasketAsync(CustomerBasket basket)
        {
            foreach (var item in basket.BasketItems)
            {
                // client  مش اللي مبعوت في السله او اللي باعتو ال database عشان اجيب السعر الحقيقي بتاعو من ال product انا جبت ال
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id)
                             ?? throw new ProductNotFoundException(item.Id);

                // اللي مبعوت في السله Price مش تاخد ال DB من ال Price كدا بعرفو انو يجيب ال
                item.Price = product.Price;
            }

            // ShippingPrice مش هعمل فحص بقا ع ال Value عشان هيه لو مفهاش  Exception هرجعلو Value لو مفهاش  DeliveryMethofId دي بتعمل فحص بتشوف ال
            if (!basket.DeliveryMethoId.HasValue) throw new Exception(" No Delivery Method Selected");

            // هروح بقا اجيبها DeliveryMethoId لو هو مختار 
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>()
                  .GetByIdAsync(basket.DeliveryMethoId.Value)
                  ?? throw new DeliveryMethodNotFountExceptions(basket.DeliveryMethoId.Value);

            // --- DB لانها جايه من ال DeliveryMethod.price ان هو ياخدها من ال shippingPrice ل القيمه بتاع set هروح بقا اعمل
            basket.ShippingPrice = deliveryMethod.Price;
        }

        // Get Basket
        private async Task<CustomerBasket> GetBasketAsybc(string basketId)
        {
            return await _basketRepository.GetBasketAsync(basketId)
                              ?? throw new BasketNotFoundException(basketId);
        }

    }
}
