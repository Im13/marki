using API.DTOs;
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
            CreateMap<Product, ProductToReturnDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());
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

            CreateMap<ProductDTOs, Product>();
            CreateMap<Product, ProductDTOs>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name));
            CreateMap<ProductOptionDTO, ProductOptions>();
            CreateMap<ProductOptions, ProductOptionDTO>();
            CreateMap<ProductOptionValueDTO, ProductOptionValues>();
            CreateMap<ProductOptionValues, ProductOptionValueDTO>();
            CreateMap<ProductSKUDTO, ProductSKUs>();
            CreateMap<ProductSKUs, ProductSKUDTO>();
            CreateMap<ProductSKUValuesDTO, ProductSKUValues>();
            CreateMap<ProductSKUValues, ProductSKUValuesDTO>()
                .ForMember(d => d.OptionValue, o => o.MapFrom(s => s.ProductOptionValue.ValueName))
                .ForMember(d => d.OptionName, o => o.MapFrom(s => s.ProductOptionValue.ProductOption.OptionName));

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