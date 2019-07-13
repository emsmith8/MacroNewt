﻿// <auto-generated />
using System;
using MacroNewt.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MacroNewt.Migrations
{
    [DbContext(typeof(MacroNewtContext))]
    [Migration("20190713073348_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MacroNewt.Areas.Identity.Data.DailyCalTotal", b =>
                {
                    b.Property<int>("DailyCalTotalId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CalorieDay");

                    b.Property<string>("Id");

                    b.Property<int>("TotalDailyCalories");

                    b.Property<int>("TotalDailyCarbCalories");

                    b.Property<int>("TotalDailyFatCalories");

                    b.Property<int>("TotalDailyProteinCalories");

                    b.HasKey("DailyCalTotalId");

                    b.HasIndex("Id");

                    b.ToTable("DailyCalTotal");
                });

            modelBuilder.Entity("MacroNewt.Areas.Identity.Data.MacroNewtUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<int>("Age");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DOB");

                    b.Property<int>("DailyTargetCalories");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Gender");

                    b.Property<int>("HeightFeet");

                    b.Property<int>("HeightInches");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfileName");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("MacroNewt.Areas.Identity.Data.UserGoals", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("BaseCalorieTarget");

                    b.Property<int>("CalAdjustment");

                    b.Property<int>("CarbPercent");

                    b.Property<int>("FatPercent");

                    b.Property<string>("OverallGoal");

                    b.Property<string>("Pace");

                    b.Property<int>("ProteinPercent");

                    b.HasKey("Id");

                    b.ToTable("UserGoals");
                });

            modelBuilder.Entity("MacroNewt.Models.Food", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FoodTotalCarb");

                    b.Property<string>("FoodTotalFat");

                    b.Property<string>("FoodTotalProtein");

                    b.Property<int>("MealId");

                    b.Property<string>("Name");

                    b.Property<string>("Ndbno")
                        .IsRequired();

                    b.Property<decimal>("NumberOfServings");

                    b.Property<int>("PortionIndex");

                    b.Property<string>("SelectedPortionLabel");

                    b.Property<string>("SelectedPortionQty");

                    b.Property<string>("Unit");

                    b.Property<int?>("Value")
                        .IsRequired();

                    b.HasKey("FoodId");

                    b.HasIndex("MealId");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("MacroNewt.Models.Meal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Calories");

                    b.Property<int>("CarbCalories");

                    b.Property<int>("FatCalories");

                    b.Property<DateTime>("MealDate");

                    b.Property<string>("MealType");

                    b.Property<int>("ProteinCalories");

                    b.Property<bool>("ReLogged");

                    b.Property<string>("Title");

                    b.Property<string>("UserEmail");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("MacroNewt.Models.Measure", b =>
                {
                    b.Property<int>("MeasureId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DisplayName");

                    b.Property<string>("Eqv");

                    b.Property<string>("Eunit");

                    b.Property<string>("Label");

                    b.Property<int>("NutrientId");

                    b.Property<string>("Qty");

                    b.Property<decimal>("Value");

                    b.HasKey("MeasureId");

                    b.HasIndex("NutrientId");

                    b.ToTable("Measure");
                });

            modelBuilder.Entity("MacroNewt.Models.Nutrient", b =>
                {
                    b.Property<int>("NutrientId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FoodId");

                    b.Property<string>("Group");

                    b.Property<string>("NId");

                    b.Property<string>("Name");

                    b.Property<string>("Unit");

                    b.Property<decimal>("Value");

                    b.HasKey("NutrientId");

                    b.HasIndex("FoodId");

                    b.ToTable("Nutrient");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("MacroNewt.Areas.Identity.Data.DailyCalTotal", b =>
                {
                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser", "MacroNewtUser")
                        .WithMany("DailyCalTotals")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MacroNewt.Areas.Identity.Data.UserGoals", b =>
                {
                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser", "MacroNewtUser")
                        .WithOne("Goals")
                        .HasForeignKey("MacroNewt.Areas.Identity.Data.UserGoals", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MacroNewt.Models.Food", b =>
                {
                    b.HasOne("MacroNewt.Models.Meal", "Meal")
                        .WithMany("FoodComponents")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MacroNewt.Models.Measure", b =>
                {
                    b.HasOne("MacroNewt.Models.Nutrient", "Nutrient")
                        .WithMany("Measures")
                        .HasForeignKey("NutrientId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MacroNewt.Models.Nutrient", b =>
                {
                    b.HasOne("MacroNewt.Models.Food", "Food")
                        .WithMany("Nutrients")
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MacroNewt.Areas.Identity.Data.MacroNewtUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
