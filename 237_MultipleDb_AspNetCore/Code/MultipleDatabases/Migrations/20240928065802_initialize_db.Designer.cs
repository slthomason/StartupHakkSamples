﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MultipleDatabases.Database;

#nullable disable

namespace MultipleDatabases.Migrations
{
    [DbContext(typeof(UsersContext))]
    [Migration("20240928065802_initialize_db")]
    partial class initialize_db
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("MultipleDatabases.Database.Entities.Users", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("customerName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("userId");

                    b.ToTable("users");

                    b.HasData(
                        new
                        {
                            userId = 1,
                            customerName = "Jon Doe",
                            phoneNumber = "+168025600884"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}