using API.DTOs;
using API.DTOs.AdminOrder;
using API.DTOs.ClientProduct;
using API.DTOs.Product;
using API.DTOs.Revenue;
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
            // CreateMap<Order, OrderToReturnDTO>()
            //     .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
            //     .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price));
            // CreateMap<OrderItem, OrderItemDTO>()
            //     .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
            //     .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
            //     .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.PictureUrl))
            //     .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());
            CreateMap<Order, OrderToReturnDTO>();
            CreateMap<OrderToReturnDTO, Order>();
            CreateMap<OrderItem, OrderItemDTO>();
            CreateMap<OrderItemDTO, OrderItem>();
            CreateMap<ItemOrderedDTO, ProductIemOrdered>();
            CreateMap<ProductIemOrdered, ItemOrderedDTO>();

            CreateMap<ShipToAddressDTO, Address>();
            CreateMap<Address, ShipToAddressDTO>();
            CreateMap<UpdateOrderDTO, Order>();
            CreateMap<Order, UpdateOrderDTO>();

            //Offline order
            CreateMap<OfflineOrder, OfflineOrderDTO>()
                .ForMember(d => d.DistrictId, o => o.MapFrom(s => s.District.Id))
                .ForMember(d => d.ProvinceId, o => o.MapFrom(s => s.Province.Id))
                .ForMember(d => d.WardId, o => o.MapFrom(s => s.Ward.Id));
            CreateMap<OfflineOrderDTO, OfflineOrder>();

            //Website order - Need merge later
            CreateMap<Order, OfflineOrderDTO>();
            CreateMap<OfflineOrderDTO, Order>();

            CreateMap<Customer, CustomerDTO>();
            CreateMap<CustomerDTO, Customer>();
            CreateMap<OfflineOrderSKUDTOs, OfflineOrderSKUs>();
            CreateMap<OfflineOrderStatusDTO, OfflineOrderStatus>();
            CreateMap<OfflineOrderStatus, OfflineOrderStatusDTO>();
            CreateMap<OfflineOrderSKUs, OfflineOrderSKUDTOs>()
                .ForMember(d => d.ProductSKUId, o => o.MapFrom(s => s.ProductSkuId))
                .ForMember(d => d.SkuDetail, o => o.MapFrom(s => s.ProductSKU))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ProductSKU.Product.Name));

            //Customer
            CreateMap<CustomerDTO,Customer>();
            CreateMap<Customer,CustomerDTO>();

            CreateMap<ProductDTOs, Product>();
            CreateMap<Product, ProductDTOs>();
            CreateMap<ProductForClientDTO, Product>();
            CreateMap<Product, ProductForClientDTO>();
            CreateMap<Photo, PhotoDTO>();
            CreateMap<PhotoDTO, Photo>();
            CreateMap<SlideImage, SlideImageDTO>();
            CreateMap<SlideImageDTO, SlideImage>();
            CreateMap<ProductOptionDTO, ProductOptions>();
            CreateMap<ProductOptions, ProductOptionDTO>();
            CreateMap<ProductOptionValueDTO, ProductOptionValues>();
                // .ForMember(d => d.ValueName, o => o.MapFrom(s => s.Value));
            CreateMap<ProductOptionValues, ProductOptionValueDTO>();
            CreateMap<ProductSKUDTO, ProductSKUs>();
            CreateMap<ProductSKUs, ProductSKUDTO>();
            CreateMap<ProductSKUForClientDTO, ProductSKUs>();
            CreateMap<ProductSKUs, ProductSKUForClientDTO>();
            CreateMap<ProductSKUValuesDTO, ProductSKUValues>();
            CreateMap<RevenueSummary, RevenueSummaryDto>();
            CreateMap<RevenueSummaryDto, RevenueSummary>();
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