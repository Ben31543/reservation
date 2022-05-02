﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reservation.Data;

namespace Reservation.Data.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Reservation.Data.Entities.Admin", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Bank", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankAccount", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("BankId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Owner")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankCard", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("BankId")
                        .HasColumnType("bigint");

                    b.Property<string>("CVV")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("CurrentBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Owner")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ValidThru")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("BankId");

                    b.ToTable("BankCards");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Dish", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("ServiceMemberId")
                        .HasColumnType("bigint");

                    b.Property<byte>("TypeId")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("ServiceMemberId");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("BankCardId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BankCardId")
                        .IsUnique()
                        .HasFilter("[BankCardId] IS NOT NULL");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Payment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Approved")
                        .HasColumnType("bit");

                    b.Property<string>("BankAcountIdTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankCardIdFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("PaymentDatas");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Reserving", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Dishes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsOnlinePayment")
                        .HasColumnType("bit");

                    b.Property<bool>("IsTakeOut")
                        .HasColumnType("bit");

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ReservationDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("ServiceMemberBranchId")
                        .HasColumnType("bigint");

                    b.Property<long>("ServiceMemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("Tables")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ServiceMemberBranchId");

                    b.HasIndex("ServiceMemberId");

                    b.ToTable("Reservings");
                });

            modelBuilder.Entity("Reservation.Data.Entities.ServiceMember", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("AcceptsOnlinePayment")
                        .HasColumnType("bit");

                    b.Property<long?>("BankAccountId")
                        .HasColumnType("bigint");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FacebookUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InstagramUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OrdersCount")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewsCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId")
                        .IsUnique()
                        .HasFilter("[BankAccountId] IS NOT NULL");

                    b.ToTable("ServiceMembers");
                });

            modelBuilder.Entity("Reservation.Data.Entities.ServiceMemberBranch", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CloseTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OpenTime")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ServiceMemberId")
                        .HasColumnType("bigint");

                    b.Property<string>("TablesSchema")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ServiceMemberId");

                    b.ToTable("ServiceMemberBranches");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankAccount", b =>
                {
                    b.HasOne("Reservation.Data.Entities.Bank", "Bank")
                        .WithMany("BankAccounts")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankCard", b =>
                {
                    b.HasOne("Reservation.Data.Entities.Bank", "Bank")
                        .WithMany("BankCards")
                        .HasForeignKey("BankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Dish", b =>
                {
                    b.HasOne("Reservation.Data.Entities.ServiceMember", "ServiceMember")
                        .WithMany()
                        .HasForeignKey("ServiceMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceMember");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Member", b =>
                {
                    b.HasOne("Reservation.Data.Entities.BankCard", "BankCard")
                        .WithOne("Member")
                        .HasForeignKey("Reservation.Data.Entities.Member", "BankCardId");

                    b.Navigation("BankCard");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Reserving", b =>
                {
                    b.HasOne("Reservation.Data.Entities.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Reservation.Data.Entities.ServiceMemberBranch", "ServiceMemberBranch")
                        .WithMany()
                        .HasForeignKey("ServiceMemberBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Reservation.Data.Entities.ServiceMember", "ServiceMember")
                        .WithMany()
                        .HasForeignKey("ServiceMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("ServiceMember");

                    b.Navigation("ServiceMemberBranch");
                });

            modelBuilder.Entity("Reservation.Data.Entities.ServiceMember", b =>
                {
                    b.HasOne("Reservation.Data.Entities.BankAccount", "BankAccount")
                        .WithOne("ServiceMember")
                        .HasForeignKey("Reservation.Data.Entities.ServiceMember", "BankAccountId");

                    b.Navigation("BankAccount");
                });

            modelBuilder.Entity("Reservation.Data.Entities.ServiceMemberBranch", b =>
                {
                    b.HasOne("Reservation.Data.Entities.ServiceMember", "ServiceMember")
                        .WithMany("ServiceMemberBranches")
                        .HasForeignKey("ServiceMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ServiceMember");
                });

            modelBuilder.Entity("Reservation.Data.Entities.Bank", b =>
                {
                    b.Navigation("BankAccounts");

                    b.Navigation("BankCards");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankAccount", b =>
                {
                    b.Navigation("ServiceMember");
                });

            modelBuilder.Entity("Reservation.Data.Entities.BankCard", b =>
                {
                    b.Navigation("Member");
                });

            modelBuilder.Entity("Reservation.Data.Entities.ServiceMember", b =>
                {
                    b.Navigation("ServiceMemberBranches");
                });
#pragma warning restore 612, 618
        }
    }
}
