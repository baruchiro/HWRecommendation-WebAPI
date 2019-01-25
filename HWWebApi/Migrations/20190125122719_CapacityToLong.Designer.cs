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
    [Migration("20190125122719_CapacityToLong")]
    partial class CapacityToLong
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HWWebApi.Models.Computer", b =>
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

            modelBuilder.Entity("HWWebApi.Models.Disk", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("Capacity");

                    b.Property<long?>("ComputerId");

                    b.Property<int?>("Rpm");

                    b.Property<int?>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("HWWebApi.Models.GPU", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ComputerId");

                    b.Property<int?>("Cores");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("GPUs");
                });

            modelBuilder.Entity("HWWebApi.Models.Memory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Capacity");

                    b.Property<long?>("ComputerId");

                    b.Property<long>("Ghz");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("Memories");
                });

            modelBuilder.Entity("HWWebApi.Models.MotherBoard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Architecture");

                    b.Property<int?>("DdrSockets");

                    b.Property<long?>("MaxRam");

                    b.Property<int?>("SataConnections");

                    b.HasKey("Id");

                    b.ToTable("MotherBoards");
                });

            modelBuilder.Entity("HWWebApi.Models.Processor", b =>
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

            modelBuilder.Entity("HWWebApi.Models.TestString", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Data");

                    b.HasKey("Id");

                    b.ToTable("TestStrings");
                });

            modelBuilder.Entity("HWWebApi.Models.Computer", b =>
                {
                    b.HasOne("HWWebApi.Models.MotherBoard", "MotherBoard")
                        .WithMany()
                        .HasForeignKey("MotherBoardId");

                    b.HasOne("HWWebApi.Models.Processor", "Processor")
                        .WithMany()
                        .HasForeignKey("ProcessorId");
                });

            modelBuilder.Entity("HWWebApi.Models.Disk", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("Disks")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("HWWebApi.Models.GPU", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("GPUs")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("HWWebApi.Models.Memory", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("Memories")
                        .HasForeignKey("ComputerId");
                });
#pragma warning restore 612, 618
        }
    }
}
