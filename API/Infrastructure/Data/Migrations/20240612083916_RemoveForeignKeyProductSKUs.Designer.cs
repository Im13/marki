﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20240612083916_RemoveForeignKeyProductSKUs")]
    partial class RemoveForeignKeyProductSKUs
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Core.Entities.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DivisionType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.DeliveryMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("DeliveryTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ShortName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DeliveryMethods");
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BuyerEmail")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DeliveryMethodId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentIntentId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Subtotal")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("DeliveryMethodId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("TEXT");

                    b.Property<double>("ImportPrice")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductBrandId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductSKU")
                        .HasColumnType("TEXT");

                    b.Property<int>("ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductBrandId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Core.Entities.ProductBrand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductBrands");
                });

            modelBuilder.Entity("Core.Entities.ProductOptionValues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProductOptionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ValueName")
                        .HasColumnType("TEXT");

                    b.Property<int>("ValueTempId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductOptionId");

                    b.ToTable("ProductOptionValues");
                });

            modelBuilder.Entity("Core.Entities.ProductOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("OptionName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductOptions");
                });

            modelBuilder.Entity("Core.Entities.ProductSKUValues", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("OptionValueId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ProductSKUValues");
                });

            modelBuilder.Entity("Core.Entities.ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ProductTypes");
                });

            modelBuilder.Entity("Core.Entities.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodeName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DivisionType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PhoneCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("Core.Entities.ShopeeOrder.ShopeeOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AddressDetails")
                        .HasColumnType("TEXT");

                    b.Property<string>("CustomerName")
                        .HasColumnType("TEXT");

                    b.Property<double>("CustomerShippingFee")
                        .HasColumnType("REAL");

                    b.Property<string>("CustomerUsername")
                        .HasColumnType("TEXT");

                    b.Property<double>("Deposit")
                        .HasColumnType("REAL");

                    b.Property<string>("District")
                        .HasColumnType("TEXT");

                    b.Property<double>("EstimatedShippingFee")
                        .HasColumnType("REAL");

                    b.Property<double>("EstimatedShoppingFeeShopeeDiscount")
                        .HasColumnType("REAL");

                    b.Property<double>("FixedFee")
                        .HasColumnType("REAL");

                    b.Property<string>("Note")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderCompletedDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderPaidDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("TEXT");

                    b.Property<double>("PaymentFee")
                        .HasColumnType("REAL");

                    b.Property<string>("PaymentMethod")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("Province")
                        .HasColumnType("TEXT");

                    b.Property<double>("ReturnOrderFee")
                        .HasColumnType("REAL");

                    b.Property<string>("ReturnStatus")
                        .HasColumnType("TEXT");

                    b.Property<double>("ServiceFee")
                        .HasColumnType("REAL");

                    b.Property<string>("ShipmentCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShippingCompany")
                        .HasColumnType("TEXT");

                    b.Property<double>("ShopComboDiscount")
                        .HasColumnType("REAL");

                    b.Property<double>("ShopVoucher")
                        .HasColumnType("REAL");

                    b.Property<double>("ShopeeCoinReturn")
                        .HasColumnType("REAL");

                    b.Property<double>("ShopeeComboDiscount")
                        .HasColumnType("REAL");

                    b.Property<double>("ShopeeVoucher")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalOrderCustomerPaid")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalOrderValue")
                        .HasColumnType("REAL");

                    b.Property<string>("Ward")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ShopeeOrders");
                });

            modelBuilder.Entity("Core.Entities.ShopeeOrder.ShopeeProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Cost")
                        .HasColumnType("REAL");

                    b.Property<string>("ProductName")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductProperty")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductPropertySKU")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReturnedQuantity")
                        .HasColumnType("TEXT");

                    b.Property<string>("SKU")
                        .HasColumnType("TEXT");

                    b.Property<double>("SalePrice")
                        .HasColumnType("REAL");

                    b.Property<double>("ShopDiscount")
                        .HasColumnType("REAL");

                    b.Property<int?>("ShopeeOrderId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("ShopeeSale")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalSellingPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("TotalShopSale")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ShopeeOrderId");

                    b.ToTable("ShopeeProducts");
                });

            modelBuilder.Entity("Core.Entities.Ward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CodeName")
                        .HasColumnType("TEXT");

                    b.Property<int>("DistrictId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DivisionType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("ShortCodeName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Wards");
                });

            modelBuilder.Entity("Core.ProductSKUs", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("TEXT");

                    b.Property<double>("ImportPrice")
                        .HasColumnType("REAL");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<int?>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SKU")
                        .HasColumnType("TEXT");

                    b.Property<float>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductSKUs");
                });

            modelBuilder.Entity("Core.Entities.District", b =>
                {
                    b.HasOne("Core.Entities.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.Order", b =>
                {
                    b.HasOne("Core.Entities.OrderAggregate.DeliveryMethod", "DeliveryMethod")
                        .WithMany()
                        .HasForeignKey("DeliveryMethodId");

                    b.OwnsOne("Core.Entities.OrderAggregate.Address", "ShipToAddress", b1 =>
                        {
                            b1.Property<int>("OrderId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("CityOrProvinceId")
                                .HasColumnType("INTEGER");

                            b1.Property<int>("DistrictId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("Fullname")
                                .HasColumnType("TEXT");

                            b1.Property<string>("PhoneNumber")
                                .HasColumnType("TEXT");

                            b1.Property<string>("Street")
                                .HasColumnType("TEXT");

                            b1.Property<int>("WardId")
                                .HasColumnType("INTEGER");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("DeliveryMethod");

                    b.Navigation("ShipToAddress")
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.OrderItem", b =>
                {
                    b.HasOne("Core.Entities.OrderAggregate.Order", null)
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("Core.Entities.OrderAggregate.ProductIemOrdered", "ItemOrdered", b1 =>
                        {
                            b1.Property<int>("OrderItemId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("PictureUrl")
                                .HasColumnType("TEXT");

                            b1.Property<int>("ProductItemId")
                                .HasColumnType("INTEGER");

                            b1.Property<string>("ProductName")
                                .HasColumnType("TEXT");

                            b1.HasKey("OrderItemId");

                            b1.ToTable("OrderItems");

                            b1.WithOwner()
                                .HasForeignKey("OrderItemId");
                        });

                    b.Navigation("ItemOrdered");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.HasOne("Core.Entities.ProductBrand", "ProductBrand")
                        .WithMany()
                        .HasForeignKey("ProductBrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.ProductType", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProductBrand");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("Core.Entities.ProductOptionValues", b =>
                {
                    b.HasOne("Core.Entities.ProductOptions", "ProductOption")
                        .WithMany("ProductOptionValues")
                        .HasForeignKey("ProductOptionId");

                    b.Navigation("ProductOption");
                });

            modelBuilder.Entity("Core.Entities.ProductOptions", b =>
                {
                    b.HasOne("Core.Entities.Product", null)
                        .WithMany("ProductOptions")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Core.Entities.ShopeeOrder.ShopeeProduct", b =>
                {
                    b.HasOne("Core.Entities.ShopeeOrder.ShopeeOrder", null)
                        .WithMany("Products")
                        .HasForeignKey("ShopeeOrderId");
                });

            modelBuilder.Entity("Core.Entities.Ward", b =>
                {
                    b.HasOne("Core.Entities.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("District");
                });

            modelBuilder.Entity("Core.ProductSKUs", b =>
                {
                    b.HasOne("Core.Entities.Product", null)
                        .WithMany("ProductSKUs")
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Core.Entities.OrderAggregate.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("Core.Entities.Product", b =>
                {
                    b.Navigation("ProductOptions");

                    b.Navigation("ProductSKUs");
                });

            modelBuilder.Entity("Core.Entities.ProductOptions", b =>
                {
                    b.Navigation("ProductOptionValues");
                });

            modelBuilder.Entity("Core.Entities.ShopeeOrder.ShopeeOrder", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
