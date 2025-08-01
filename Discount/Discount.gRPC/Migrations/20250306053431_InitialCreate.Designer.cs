﻿// <auto-generated />
using Discount.Grpc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Discount.gRPC.Migrations
{
    [DbContext(typeof(DiscountContext))]
    [Migration("20250306053431_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("Discount.gRPC.Models.Coupon", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            Id = "018c1496-d399-4010-a5b2-1c54070e7d44",
                            Amount = 150,
                            Description = "IPhone Discount",
                            ProductId = "3332779a-e455-42ba-ab0f-a0b0014bd5b0"
                        },
                        new
                        {
                            Id = "67dc335b-f900-4c4a-af4c-2ca32214d8a4",
                            Amount = 100,
                            Description = "Samsung Discount",
                            ProductId = "ed1a486a-bda9-4e90-b5f3-97911c2b26d0"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
