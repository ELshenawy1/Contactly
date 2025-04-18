﻿// <auto-generated />
using Contactly.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Contactly.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250407230018_AddConnectionTrackerTable")]
    partial class AddConnectionTrackerTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Contactly.Core.Entities.ConnectionTracker", b =>
                {
                    b.Property<string>("ConnectionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ContactId")
                        .HasColumnType("int");

                    b.HasKey("ConnectionId");

                    b.HasIndex("ContactId")
                        .IsUnique()
                        .HasFilter("[ContactId] IS NOT NULL");

                    b.ToTable("ConnectionTracker");
                });

            modelBuilder.Entity("Contactly.Core.Entities.Contact", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Contactly.Core.Entities.ConnectionTracker", b =>
                {
                    b.HasOne("Contactly.Core.Entities.Contact", "Contact")
                        .WithOne("ConnectionTracker")
                        .HasForeignKey("Contactly.Core.Entities.ConnectionTracker", "ContactId");

                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Contactly.Core.Entities.Contact", b =>
                {
                    b.Navigation("ConnectionTracker")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
