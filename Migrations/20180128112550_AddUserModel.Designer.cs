﻿// <auto-generated />
using api_dating_app.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace apidatingapp.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20180128112550_AddUserModel")]
    partial class AddUserModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("api_dating_app.models.PhotoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<string>("Url");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("api_dating_app.models.UserModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime>("DateOfBirth");

                    b.Property<string>("Gender");

                    b.Property<string>("Interests");

                    b.Property<string>("Introduction");

                    b.Property<string>("KnownAs");

                    b.Property<DateTime>("LastActive");

                    b.Property<string>("LookingFor");

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("api_dating_app.models.ValueModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Values");
                });

            modelBuilder.Entity("api_dating_app.models.PhotoModel", b =>
                {
                    b.HasOne("api_dating_app.models.UserModel", "User")
                        .WithMany("Photos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
