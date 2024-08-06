using API.DTOs;
using API.DTOs.AdminOrder;
using API.DTOs.Product;
using API.DTOs.Shopee;
using AutoMapper;
using Core;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Entities.ShopeeOrder;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDTO>();
                // .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Core.Entities.Identity.Address, AddressDTO>().ReverseMap();
            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();
            CreateMap<AddressDTO, Address>();
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

            //Offline order
            CreateMap<OfflineOrder, OfflineOrderDTO>();
            CreateMap<OfflineOrderDTO, OfflineOrder>();
            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<OfflineOrderSKUDTOs, OfflineOrderSKUs>();
            CreateMap<OfflineOrderSKUs, OfflineOrderSKUDTOs>()
                .ForMember(d => d.ProductSKUId, o => o.MapFrom(s => s.ProductSkuId))
                .ForMember(d => d.SkuDetail, o => o.MapFrom(s => s.ProductSKU))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductSKU.Product.Name));

            CreateMap<ProductDTOs, Product>();
            CreateMap<Product, ProductDTOs>();
            CreateMap<Photo, PhotoDTO>();
            CreateMap<PhotoDTO, Photo>();
            CreateMap<ProductOptionDTO, ProductOptions>();
            CreateMap<ProductOptions, ProductOptionDTO>();
            CreateMap<ProductOptionValueDTO, ProductOptionValues>()
                .ForMember(d => d.ValueName, o => o.MapFrom(s => s.Value));
            CreateMap<ProductOptionValues, ProductOptionValueDTO>();
            CreateMap<ProductSKUDTO, ProductSKUs>();
            CreateMap<ProductSKUs, ProductSKUDTO>();
            CreateMap<ProductSKUValuesDTO, ProductSKUValues>();
            CreateMap<ProductSKUValues, ProductSKUValuesDTO>()
                .ForMember(d => d.OptionValue, o => o.MapFrom(s => s.ProductOptionValue.ValueName))
                .ForMember(d => d.OptionName, o => o.MapFrom(s => s.ProductOptionValue.ProductOption.OptionName));
            CreateMap<ProductSKUs, ProductSKUDetailDTO>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.Name))
                .ForMember(d => d.ProductSKU, o => o.MapFrom(s => s.Product.ProductSKU));

            CreateMap<ShopeeOrderProductDTO, ShopeeProduct>()
                .ForMember(d => d.SKU, o => o.MapFrom(s => s.ProductSKU));
            CreateMap<ShopeeOrderDTO, ShopeeOrder>()
                .ForMember(d => d.OrderDate, o => o.MapFrom(s => DateTime.ParseExact(s.OrderDate, "dd/MM/yyyy H:mm", null)));
            CreateMap<ShopeeOrder, ShopeeOrderDTO>();
            CreateMap<ShopeeProduct, ShopeeOrderProductDTO>()
                .ForMember(d => d.ProductSKU, o => o.MapFrom(s => s.SKU));
        }
    }
}