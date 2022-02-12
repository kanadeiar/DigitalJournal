﻿// <auto-generated />
using System;
using DigitalJournal.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalJournal.Dal.Mssql.Migrations
{
    [DbContext(typeof(DigitalJournalContext))]
    [Migration("20220212191229_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Autoclave1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AutoclaveNumber")
                        .HasColumnType("int");

                    b.Property<int>("AutoclavedCount")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("AutoclavedTime")
                        .HasColumnType("time");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ProductTypeId");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Autoclave1ShiftDatas");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1GeneralShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AutoclaveNumber")
                        .HasColumnType("int");

                    b.Property<int>("Factory1PackProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<double>("Loose1RawValue")
                        .HasColumnType("float");

                    b.Property<double>("Loose2RawValue")
                        .HasColumnType("float");

                    b.Property<double>("Loose3RawValue")
                        .HasColumnType("float");

                    b.Property<int>("PackProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1PackProductTypeId");

                    b.HasIndex("Factory1ProductTypeId");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1GeneralShiftData");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Pack1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ProductTypeId");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Pack1ShiftDatas");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Press1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<double>("Loose1RawValue")
                        .HasColumnType("float");

                    b.Property<double>("Loose2RawValue")
                        .HasColumnType("float");

                    b.Property<double>("Loose3RawValue")
                        .HasColumnType("float");

                    b.Property<int>("ProductCount")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ProductTypeId");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Press1ShiftData");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Factory1ProductTypes");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("IsDelete")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.ToTable("Factory1Shifts");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<double>("Tank1LooseRawValue")
                        .HasColumnType("float");

                    b.Property<double>("Tank2LooseRawValue")
                        .HasColumnType("float");

                    b.Property<double>("Tank3LooseRawValue")
                        .HasColumnType("float");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Warehouse1ShiftData");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse2ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("int");

                    b.Property<int>("Place1ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Place1ProductsCount")
                        .HasColumnType("int");

                    b.Property<int>("Place2ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Place2ProductsCount")
                        .HasColumnType("int");

                    b.Property<int>("Place3ProductTypeId")
                        .HasColumnType("int");

                    b.Property<int>("Place3ProductsCount")
                        .HasColumnType("int");

                    b.Property<int>("ProfileId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("Place1ProductTypeId");

                    b.HasIndex("Place2ProductTypeId");

                    b.HasIndex("Place3ProductTypeId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Warehouse2ShiftData");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Profile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Autoclave1ShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Factory1ProductType")
                        .WithMany()
                        .HasForeignKey("Factory1ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1ProductType");

                    b.Navigation("Factory1Shift");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1GeneralShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Factory1PackProductType")
                        .WithMany()
                        .HasForeignKey("Factory1PackProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Factory1ProductType")
                        .WithMany()
                        .HasForeignKey("Factory1ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1PackProductType");

                    b.Navigation("Factory1ProductType");

                    b.Navigation("Factory1Shift");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Pack1ShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Factory1ProductType")
                        .WithMany()
                        .HasForeignKey("Factory1ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1ProductType");

                    b.Navigation("Factory1Shift");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Press1ShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Factory1ProductType")
                        .WithMany()
                        .HasForeignKey("Factory1ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1ProductType");

                    b.Navigation("Factory1Shift");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse1ShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1Shift");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse2ShiftData", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", "Factory1Shift")
                        .WithMany()
                        .HasForeignKey("Factory1ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Place1ProductType")
                        .WithMany()
                        .HasForeignKey("Place1ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Place2ProductType")
                        .WithMany()
                        .HasForeignKey("Place2ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Factory1.Factory1ProductType", "Place3ProductType")
                        .WithMany()
                        .HasForeignKey("Place3ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DigitalJournal.Domain.Entities.Profile", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Factory1Shift");

                    b.Navigation("Place1ProductType");

                    b.Navigation("Place2ProductType");

                    b.Navigation("Place3ProductType");

                    b.Navigation("Profile");
                });
#pragma warning restore 612, 618
        }
    }
}
