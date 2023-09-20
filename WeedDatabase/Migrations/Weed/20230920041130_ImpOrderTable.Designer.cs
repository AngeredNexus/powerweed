﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WeedDatabase.Context;

#nullable disable

namespace WeedDatabase.Migrations.Weed
{
    [DbContext(typeof(WeedContext))]
    [Migration("20230920041130_ImpOrderTable")]
    partial class ImpOrderTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.19")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WeedDatabase.Domain.App.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<Guid>("AppUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("app_user_id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("comment");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("DeliveryMan")
                        .HasColumnType("text")
                        .HasColumnName("delivery");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<int>("Status")
                        .HasColumnType("integer")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("PhoneNumber");

                    b.ToTable("orders", "store");
                });

            modelBuilder.Entity("WeedDatabase.Domain.App.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("WeedId")
                        .HasColumnType("uuid")
                        .HasColumnName("weed_id");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .HasDatabaseName("IX_orders_id1");

                    b.HasIndex("OrderId");

                    b.HasIndex("WeedId");

                    b.ToTable("orders", "items");
                });

            modelBuilder.Entity("WeedDatabase.Domain.App.WeedItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("descriptions");

                    b.Property<int>("DiscountStep")
                        .HasColumnType("integer")
                        .HasColumnName("discount_step");

                    b.Property<bool>("HasDiscount")
                        .HasColumnType("boolean")
                        .HasColumnName("has_discount");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("photo_url");

                    b.Property<int>("Price")
                        .HasColumnType("integer")
                        .HasColumnName("price");

                    b.Property<int>("StrainType")
                        .HasColumnType("integer")
                        .HasColumnName("strain");

                    b.Property<decimal>("Thc")
                        .HasColumnType("numeric")
                        .HasColumnName("thc");

                    b.Property<string>("Unique")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("unqiue");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Name");

                    b.HasIndex("Unique");

                    b.ToTable("weed", "store");
                });

            modelBuilder.Entity("WeedDatabase.Domain.Common.SmokiUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("auth_code");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<string>("IdentityHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("identity_hash");

                    b.Property<DateTime>("Modified")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<int>("Source")
                        .HasColumnType("integer")
                        .HasColumnName("messenger_source");

                    b.Property<string>("SourceIdentificator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("source_identificator");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("Role");

                    b.HasIndex("SourceIdentificator");

                    b.ToTable("users", "common");
                });

            modelBuilder.Entity("WeedDatabase.Domain.App.OrderItem", b =>
                {
                    b.HasOne("WeedDatabase.Domain.App.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WeedDatabase.Domain.App.Order", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
