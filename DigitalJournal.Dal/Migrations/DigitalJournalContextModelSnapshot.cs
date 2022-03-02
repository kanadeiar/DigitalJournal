﻿// <auto-generated />
using System;
using DigitalJournal.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DigitalJournal.Dal.Migrations
{
    [DbContext(typeof(DigitalJournalContext))]
    partial class DigitalJournalContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("DocumentId")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("DocComment");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDirectory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BaseDirectoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("BaseDirectoryId");

                    b.ToTable("DocDirectories");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDocument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("DirectoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Marks")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("DocDocuments");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Autoclave1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AutoclaveNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("AutoclavedCount")
                        .HasColumnType("INTEGER");

                    b.Property<TimeSpan>("AutoclavedTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TimeStart")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("AutoclaveNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1PackProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Loose1RawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Loose2RawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Loose3RawValue")
                        .HasColumnType("REAL");

                    b.Property<int>("PackProductCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Loose1RawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Loose2RawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Loose3RawValue")
                        .HasColumnType("REAL");

                    b.Property<int>("ProductCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<int>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<int>("Units")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Factory1ProductTypes");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Shift", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDelete")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.ToTable("Factory1Shifts");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse1ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Tank1LooseRawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Tank2LooseRawValue")
                        .HasColumnType("REAL");

                    b.Property<double>("Tank3LooseRawValue")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.HasKey("Id");

                    b.HasIndex("Factory1ShiftId");

                    b.HasIndex("ProfileId");

                    b.ToTable("Factory1Warehouse1ShiftData");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Factory1.Factory1Warehouse2ShiftData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Factory1ShiftId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place1ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place1ProductsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place2ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place2ProductsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place3ProductTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Place3ProductsCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProfileId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

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
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Patronymic")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("SurName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("BLOB");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocComment", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Documents.DocDocument", "Document")
                        .WithMany("Comments")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDirectory", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Documents.DocDirectory", "BaseDirectory")
                        .WithMany("Directorys")
                        .HasForeignKey("BaseDirectoryId");

                    b.Navigation("BaseDirectory");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDocument", b =>
                {
                    b.HasOne("DigitalJournal.Domain.Entities.Documents.DocDirectory", "Directory")
                        .WithMany("Documents")
                        .HasForeignKey("DirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Directory");
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

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDirectory", b =>
                {
                    b.Navigation("Directorys");

                    b.Navigation("Documents");
                });

            modelBuilder.Entity("DigitalJournal.Domain.Entities.Documents.DocDocument", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
