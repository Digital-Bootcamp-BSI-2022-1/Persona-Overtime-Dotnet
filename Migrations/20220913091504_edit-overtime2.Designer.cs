﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using dotnet_2.Infrastructure.Data;

#nullable disable

namespace Persona.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220913091504_edit-overtime2")]
    partial class editovertime2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.AuthTokenn", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("expired_at")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("expired_at");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("role");

                    b.Property<string>("token")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("token");

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("user_id");

                    b.HasKey("id");

                    b.ToTable("AuthTokenn");
                });

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.Organization", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<int>("headid")
                        .HasColumnType("integer");

                    b.Property<int>("memberid")
                        .HasColumnType("integer");

                    b.Property<string>("organization_name")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("organization_name");

                    b.HasKey("id");

                    b.HasIndex("headid");

                    b.HasIndex("memberid");

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.Overtime", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<DateOnly>("approved_date")
                        .HasColumnType("date")
                        .HasColumnName("approved_date");

                    b.Property<TimeOnly>("approved_time")
                        .HasColumnType("time")
                        .HasColumnName("approved_time");

                    b.Property<string>("attachment")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("attachment");

                    b.Property<DateOnly>("completed_date")
                        .HasColumnType("date")
                        .HasColumnName("completed_date");

                    b.Property<TimeOnly>("completed_time")
                        .HasColumnType("time")
                        .HasColumnName("completed_time");

                    b.Property<DateOnly>("end_date")
                        .HasColumnType("date")
                        .HasColumnName("end_date");

                    b.Property<TimeOnly>("end_time")
                        .HasColumnType("time")
                        .HasColumnName("end_time");

                    b.Property<int>("is_completed")
                        .HasColumnType("int")
                        .HasColumnName("is_completed");

                    b.Property<string>("remarks")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("remarks");

                    b.Property<DateOnly>("request_date")
                        .HasColumnType("date")
                        .HasColumnName("request_date");

                    b.Property<TimeOnly>("request_time")
                        .HasColumnType("time")
                        .HasColumnName("request_time");

                    b.Property<DateOnly>("start_date")
                        .HasColumnType("date")
                        .HasColumnName("start_date");

                    b.Property<TimeOnly>("start_time")
                        .HasColumnType("time")
                        .HasColumnName("start_time");

                    b.Property<int>("status")
                        .HasColumnType("int")
                        .HasColumnName("status");

                    b.Property<int>("userid")
                        .HasColumnType("integer");

                    b.HasKey("id");

                    b.HasIndex("userid");

                    b.ToTable("Overtime");
                });

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.User", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("id"));

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("email");

                    b.Property<string>("employment_status")
                        .IsRequired()
                        .HasColumnType("varchar(30)")
                        .HasColumnName("employment_status");

                    b.Property<string>("grade")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasColumnName("grade");

                    b.Property<string>("join_date")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("join_date");

                    b.Property<string>("ktp")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("ktp");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("name");

                    b.Property<string>("nik")
                        .IsRequired()
                        .HasColumnType("varchar(8)")
                        .HasColumnName("nik");

                    b.Property<string>("npwp")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("npwp");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("varchar(500)")
                        .HasColumnName("password");

                    b.Property<string>("phone")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnName("phone");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("varchar(200)")
                        .HasColumnName("role");

                    b.HasKey("id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.Organization", b =>
                {
                    b.HasOne("dotnet_2.Infrastructure.Data.Models.User", "head")
                        .WithMany()
                        .HasForeignKey("headid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("dotnet_2.Infrastructure.Data.Models.User", "member")
                        .WithMany()
                        .HasForeignKey("memberid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("head");

                    b.Navigation("member");
                });

            modelBuilder.Entity("dotnet_2.Infrastructure.Data.Models.Overtime", b =>
                {
                    b.HasOne("dotnet_2.Infrastructure.Data.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });
#pragma warning restore 612, 618
        }
    }
}
