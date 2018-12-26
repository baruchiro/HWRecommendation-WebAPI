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
    [Migration("20181226173428_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HWWebApi.Models.Computer", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("motherBoardId");

                    b.Property<long?>("processorId");

                    b.HasKey("Id");

                    b.HasIndex("motherBoardId");

                    b.HasIndex("processorId");

                    b.ToTable("Computers");
                });

            modelBuilder.Entity("HWWebApi.Models.Disk", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ComputerId");

                    b.Property<long>("capacity");

                    b.Property<int>("rpm");

                    b.Property<int>("type");

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

                    b.Property<int>("cores");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("GPUs");
                });

            modelBuilder.Entity("HWWebApi.Models.Memory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Capacity");

                    b.Property<long?>("ComputerId");

                    b.Property<long>("ghz");

                    b.Property<int>("type");

                    b.HasKey("Id");

                    b.HasIndex("ComputerId");

                    b.ToTable("memories");
                });

            modelBuilder.Entity("HWWebApi.Models.MotherBoard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("architacture");

                    b.Property<int>("ddrSockets");

                    b.Property<long>("maxRam");

                    b.Property<int>("sataConnections");

                    b.HasKey("Id");

                    b.ToTable("MotherBoards");
                });

            modelBuilder.Entity("HWWebApi.Models.Processor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("architacture");

                    b.Property<long>("ghz");

                    b.Property<int>("numOfCores");

                    b.HasKey("Id");

                    b.ToTable("Processors");
                });

            modelBuilder.Entity("HWWebApi.Models.Computer", b =>
                {
                    b.HasOne("HWWebApi.Models.MotherBoard", "motherBoard")
                        .WithMany()
                        .HasForeignKey("motherBoardId");

                    b.HasOne("HWWebApi.Models.Processor", "processor")
                        .WithMany()
                        .HasForeignKey("processorId");
                });

            modelBuilder.Entity("HWWebApi.Models.Disk", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("disks")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("HWWebApi.Models.GPU", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("gpus")
                        .HasForeignKey("ComputerId");
                });

            modelBuilder.Entity("HWWebApi.Models.Memory", b =>
                {
                    b.HasOne("HWWebApi.Models.Computer")
                        .WithMany("memories")
                        .HasForeignKey("ComputerId");
                });
#pragma warning restore 612, 618
        }
    }
}
