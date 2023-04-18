﻿// <auto-generated />
using System;
using Builders.Bills.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Builders.Bills.Database.Migrations
{
    [DbContext(typeof(BillsDbContext))]
    partial class BillsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Builders.Bills.Database.Models.Bill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<string>("DueDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FineAmountCalculated")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("FineRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestAmountCalculated")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OriginalAmount")
                        .HasColumnType("TEXT");

                    b.Property<string>("PaymentDate")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("RequestDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bills");
                });
#pragma warning restore 612, 618
        }
    }
}
