﻿// <auto-generated />
using System;
using DirectKeyDashboard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DirectKeyDashboard.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("DirectKeyDashboard.Charting.Domain.CustomBarChart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("ApiEndpoint")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("CategoryTokenKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CriterionType")
                        .HasColumnType("int");

                    b.Property<DateTime>("IntervalEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("IntervalStart")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("ProjectionResult")
                        .HasColumnType("int");

                    b.Property<int>("SummaryMethod")
                        .HasColumnType("int");

                    b.Property<string>("ValueTokenKey")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("CustomBarCharts");
                });

            modelBuilder.Entity("DirectKeyDashboard.Charting.Domain.CustomBarChart+CustomBarChartFloatCriterion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CustomBarChartId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Key")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Relation")
                        .HasColumnType("int");

                    b.Property<float>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CustomBarChartId");

                    b.ToTable("BarChartFloatCriteria");
                });

            modelBuilder.Entity("DirectKeyDashboard.Charting.Domain.CustomBarChart+CustomBarChartFloatCriterion", b =>
                {
                    b.HasOne("DirectKeyDashboard.Charting.Domain.CustomBarChart", "CustomBarChart")
                        .WithMany("FloatCriteria")
                        .HasForeignKey("CustomBarChartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}