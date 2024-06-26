﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Recipes.API.DbContexts;

#nullable disable

namespace Recipes.API.Migrations
{
    [DbContext(typeof(RecipeInfoContext))]
    partial class RecipeInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("Recipes.API.Entities.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Measurement")
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("TEXT");

                    b.Property<int>("Qty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RecipeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RecipeId");

                    b.ToTable("Ingredients");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Measurement = "lbs",
                            Name = "Ground Beef",
                            Qty = 2,
                            RecipeId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Egg",
                            Qty = 2,
                            RecipeId = 1
                        },
                        new
                        {
                            Id = 3,
                            Measurement = "cup",
                            Name = "Bread Crumbs",
                            Qty = 1,
                            RecipeId = 1
                        },
                        new
                        {
                            Id = 4,
                            Measurement = "oz",
                            Name = "Lasagna Noodles",
                            Qty = 8,
                            RecipeId = 2
                        },
                        new
                        {
                            Id = 5,
                            Measurement = "oz",
                            Name = "Chorizo",
                            Qty = 8,
                            RecipeId = 2
                        },
                        new
                        {
                            Id = 6,
                            Measurement = "oz",
                            Name = "Ground Beef",
                            Qty = 8,
                            RecipeId = 2
                        },
                        new
                        {
                            Id = 7,
                            Name = "Tortillas",
                            Qty = 12,
                            RecipeId = 3
                        },
                        new
                        {
                            Id = 8,
                            Measurement = "oz",
                            Name = "Enchilada Sauce",
                            Qty = 16,
                            RecipeId = 3
                        },
                        new
                        {
                            Id = 9,
                            Measurement = "oz",
                            Name = "Cheese",
                            Qty = 16,
                            RecipeId = 3
                        },
                        new
                        {
                            Id = 10,
                            Measurement = "oz",
                            Name = "Chicken",
                            Qty = 12,
                            RecipeId = 3
                        });
                });

            modelBuilder.Entity("Recipes.API.Entities.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Difficulty")
                        .HasColumnType("TEXT");

                    b.Property<string>("Directions")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<short?>("Rating")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Difficulty = "Easy",
                            Directions = "Mix bake serve",
                            Name = "Cajun Meatloaf",
                            Rating = (short)5
                        },
                        new
                        {
                            Id = 2,
                            Difficulty = "Easy",
                            Directions = "Mix bake serve",
                            Name = "Mexican Lasagna",
                            Rating = (short)5
                        },
                        new
                        {
                            Id = 3,
                            Difficulty = "Easy",
                            Directions = "Mix Roll bake serve",
                            Name = "Enchilades",
                            Rating = (short)5
                        });
                });

            modelBuilder.Entity("Recipes.API.Entities.Ingredient", b =>
                {
                    b.HasOne("Recipes.API.Entities.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Recipes.API.Entities.Recipe", b =>
                {
                    b.Navigation("Ingredients");
                });
#pragma warning restore 612, 618
        }
    }
}
