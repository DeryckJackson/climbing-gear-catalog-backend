﻿// <auto-generated />
using ClimbingGearBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace ClimbingGearBackend.Migrations.ClimbingGear
{
    [DbContext(typeof(ClimbingGearContext))]
    [Migration("20210118221648_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ClimbingGearBackend.Models.Gear", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Brand")
                        .HasColumnType("text");

                    b.Property<int>("DepthMM")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<int>("LengthMM")
                        .HasColumnType("integer");

                    b.Property<bool>("Locking")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Secret")
                        .HasColumnType("text");

                    b.Property<int>("WeightGrams")
                        .HasColumnType("integer");

                    b.Property<int>("WidthMM")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Gear");
                });
#pragma warning restore 612, 618
        }
    }
}