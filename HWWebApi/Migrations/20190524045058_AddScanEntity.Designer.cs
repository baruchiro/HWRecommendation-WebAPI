﻿// <auto-generated />
using System;
using HWWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HWWebApi.Migrations
{
    [DbContext(typeof(HardwareContext))]
    [Migration("20190524045058_AddScanEntity")]
    partial class AddScanEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HWWebApi.Models.Scan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ComputerId");

                    b.Property<DateTime>("CreationDateTime");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.HasIndex("UserId");

                    b.ToTable("Scans");
                });

            modelBuilder.Entity("HWWebApi.Models.TestString", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.HasKey("Id");

                    b.ToTable("TestStrings");
                });

            modelBuilder.Entity("HWWebApi.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Age");

                    b.Property<int>("Gender");

                    b.Property<string>("Name");

                    b.Property<string>("WorkArea");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HWWebApi.Models.UserChannel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ChannelId");

                    b.Property<string>("UserId");

                    b.Property<long?>("UserId1");

                    b.HasKey("Id");

                    b.HasIndex("UserId1");

                    b.HasIndex("ChannelId", "UserId")
                        .IsUnique()
                        .HasFilter("[ChannelId] IS NOT NULL AND [UserId] IS NOT NULL");

                    b.ToTable("UserChannels");
                });

            modelBuilder.Entity("Models.Computer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("MotherBoardId");

                    b.Property<long?>("ProcessorId");

                    b.HasKey("Id");

                    b.HasIndex("MotherBoardId");

                    b.HasIndex("ProcessorId");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("Models.Disk", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("Capacity");

                    b.Property<long?>("ComputerId");

                    b.Property<string>("Model");

                    b.Property<int?>("Rpm");

                    b.Property<int?>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("Models.Gpu", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ComputerId");

                    b.Property<int?>("Cores");

                    b.Property<string>("Name");

                    b.Property<string>("Processor");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("GPUs");
                });

            modelBuilder.Entity("Models.Memory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BankLabel");

                    b.Property<long>("Capacity");

                    b.Property<long?>("ComputerId");

                    b.Property<string>("DeviceLocator");

                    b.Property<long>("Ghz");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("Memories");
                });

            modelBuilder.Entity("Models.MotherBoard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Architecture");

                    b.Property<int?>("DdrSockets");

                    b.Property<string>("Manufacturer");

                    b.Property<long?>("MaxRam");

                    b.Property<string>("Product");

                    b.Property<int?>("SataConnections");

                    b.HasKey("Id");

                    b.ToTable("MotherBoards");
                });

            modelBuilder.Entity("Models.Processor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Architecture");

                    b.Property<long?>("GHz");

                    b.Property<string>("Name");

                    b.Property<int?>("NumOfCores");

                    b.HasKey("Id");

                    b.ToTable("Processors");
                });

            modelBuilder.Entity("HWWebApi.Models.Scan", b =>
                {
                    b.HasOne("Models.Computer", "Computer")
                        .WithMany()
                        .HasForeignKey("ComputerId");

                    b.HasOne("HWWebApi.Models.User", "User")
                        .WithMany("Scans")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("HWWebApi.Models.UserChannel", b =>
                {
                    b.HasOne("HWWebApi.Models.User")
                        .WithMany("Channels")
                        .HasForeignKey("UserId1");
                });

            modelBuilder.Entity("Models.Computer", b =>
                {
                    b.HasOne("Models.MotherBoard", "MotherBoard")
                        .WithMany()
                        .HasForeignKey("MotherBoardId");

                    b.HasOne("Models.Processor", "Processor")
                        .WithMany()
                        .HasForeignKey("ProcessorId");
                });

            modelBuilder.Entity("Models.Disk", b =>
                {
                    b.HasOne("Models.Computer")
                        .WithMany("Disks")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("Models.Gpu", b =>
                {
                    b.HasOne("Models.Computer")
                        .WithMany("Gpus")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("Models.Memory", b =>
                {
                    b.HasOne("Models.Computer")
                        .WithMany("Memories")
                        .HasForeignKey("ComputerId");
                });
#pragma warning restore 612, 618
        }
    }
}
