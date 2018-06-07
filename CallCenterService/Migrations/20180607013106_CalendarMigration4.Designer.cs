using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CallCenterService.Models;

namespace CallCenterService.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20180607013106_CalendarMigration4")]
    partial class CalendarMigration4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CallCenterService.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ApartmentNumber");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("PostCode")
                        .IsRequired();

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Specialization");

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("StreetNumber")
                        .IsRequired();

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<int?>("WorkTimeId");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.HasIndex("WorkTimeId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("CallCenterService.Models.CalendarEvent", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("End");

                    b.Property<bool>("IsFullDay");

                    b.Property<string>("ResourceId");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("ThemeColor");

                    b.HasKey("EventId");

                    b.ToTable("CalendarEvents");
                });

            modelBuilder.Entity("CallCenterService.Models.Client", b =>
                {
                    b.Property<int>("ClientId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApartmentNumber");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("PostCode")
                        .IsRequired();

                    b.Property<string>("SecondName")
                        .IsRequired();

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("StreetNumber")
                        .IsRequired();

                    b.HasKey("ClientId");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CallCenterService.Models.EventHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Operation")
                        .IsRequired();

                    b.Property<string>("Table")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("EventHistory");
                });

            modelBuilder.Entity("CallCenterService.Models.Fault", b =>
                {
                    b.Property<int>("FaultId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ApplicationDate");

                    b.Property<string>("ClientDescription");

                    b.Property<int?>("ProductID");

                    b.Property<string>("Status");

                    b.HasKey("FaultId");

                    b.HasIndex("ProductID");

                    b.ToTable("Faults");
                });

            modelBuilder.Entity("CallCenterService.Models.Product", b =>
                {
                    b.Property<int>("ProductID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int?>("TypeId");

                    b.HasKey("ProductID");

                    b.HasIndex("ClientId");

                    b.HasIndex("TypeId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("CallCenterService.Models.Repair", b =>
                {
                    b.Property<int>("RepairId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("CalendarEventEventId");

                    b.Property<DateTime?>("Date");

                    b.Property<string>("Description");

                    b.Property<int>("FaultId");

                    b.Property<decimal>("PartsPrice");

                    b.Property<decimal>("Price");

                    b.Property<string>("ServicerId");

                    b.HasKey("RepairId");

                    b.HasIndex("CalendarEventEventId");

                    b.HasIndex("FaultId");

                    b.ToTable("Repairs");
                });

            modelBuilder.Entity("CallCenterService.Models.ServicerSpecializations", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ServicerId")
                        .IsRequired();

                    b.Property<int>("SpecId");

                    b.HasKey("Id");

                    b.HasIndex("SpecId");

                    b.ToTable("ServicerSpecializations");
                });

            modelBuilder.Entity("CallCenterService.Models.Specialization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Specialization");
                });

            modelBuilder.Entity("CallCenterService.Models.WorkTime", b =>
                {
                    b.Property<int>("WorkTimeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FridayEnd");

                    b.Property<string>("FridayStart");

                    b.Property<string>("MondayEnd");

                    b.Property<string>("MondayStart");

                    b.Property<string>("SaturdayEnd");

                    b.Property<string>("SaturdayStart");

                    b.Property<string>("ServicerId");

                    b.Property<string>("SundayEnd");

                    b.Property<string>("SundayStart");

                    b.Property<string>("ThursdayEnd");

                    b.Property<string>("ThursdayStart");

                    b.Property<string>("TuesdayEnd");

                    b.Property<string>("TuesdayStart");

                    b.Property<string>("WednesdayEnd");

                    b.Property<string>("WednesdayStart");

                    b.HasKey("WorkTimeId");

                    b.ToTable("WorkTime");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CallCenterService.Models.ApplicationUser", b =>
                {
                    b.HasOne("CallCenterService.Models.WorkTime", "WorkTime")
                        .WithMany()
                        .HasForeignKey("WorkTimeId");
                });

            modelBuilder.Entity("CallCenterService.Models.Fault", b =>
                {
                    b.HasOne("CallCenterService.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("CallCenterService.Models.Product", b =>
                {
                    b.HasOne("CallCenterService.Models.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CallCenterService.Models.Specialization", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId");
                });

            modelBuilder.Entity("CallCenterService.Models.Repair", b =>
                {
                    b.HasOne("CallCenterService.Models.CalendarEvent", "CalendarEvent")
                        .WithMany()
                        .HasForeignKey("CalendarEventEventId");

                    b.HasOne("CallCenterService.Models.Fault", "Fault")
                        .WithMany()
                        .HasForeignKey("FaultId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CallCenterService.Models.ServicerSpecializations", b =>
                {
                    b.HasOne("CallCenterService.Models.Specialization", "Spec")
                        .WithMany()
                        .HasForeignKey("SpecId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CallCenterService.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CallCenterService.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CallCenterService.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
