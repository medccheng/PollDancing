﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PollDancingLibrary.Data;

#nullable disable

namespace PollDancingLibrary.Migrations
{
    [DbContext(typeof(CongressDbContext))]
    partial class CongressDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PollDancingLibrary.Models.AddressInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("OfficeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("AddressInformations");

                    b.HasAnnotation("Relational:JsonPropertyName", "addressInformation");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.CosponsoredLegislation", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("LegislationId")
                        .HasColumnType("int");

                    b.HasKey("MemberId", "LegislationId");

                    b.HasIndex("LegislationId");

                    b.ToTable("CosponsoredLegislation");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Depiction", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("Attribution")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "attribution");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasAnnotation("Relational:JsonPropertyName", "imageUrl");

                    b.HasKey("MemberId");

                    b.ToTable("Depictions");

                    b.HasAnnotation("Relational:JsonPropertyName", "depiction");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Legislation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Congress")
                        .HasColumnType("int");

                    b.Property<DateTime?>("IntroducedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Number")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Legislations");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressInformationId")
                        .HasColumnType("int");

                    b.Property<string>("BioguideId")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)")
                        .HasAnnotation("Relational:JsonPropertyName", "bioguideId");

                    b.Property<int?>("District")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "district");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("PartyName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasAnnotation("Relational:JsonPropertyName", "partyName");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasAnnotation("Relational:JsonPropertyName", "state");

                    b.Property<DateTime?>("UpdateDate")
                        .IsRequired()
                        .HasColumnType("datetime2")
                        .HasAnnotation("Relational:JsonPropertyName", "updateDate");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasAnnotation("Relational:JsonPropertyName", "url");

                    b.HasKey("Id");

                    b.HasIndex("AddressInformationId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.SponsoredLegislation", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("LegislationId")
                        .HasColumnType("int");

                    b.HasKey("MemberId", "LegislationId");

                    b.HasIndex("LegislationId");

                    b.ToTable("SponsoredLegislations");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Term", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Congress")
                        .HasMaxLength(50)
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "congress");

                    b.Property<int?>("EndYear")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "endYear");

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<string>("MemberType")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasAnnotation("Relational:JsonPropertyName", "memberType");

                    b.Property<int?>("StartYear")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "startYear");

                    b.Property<string>("StateCode")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasAnnotation("Relational:JsonPropertyName", "stateCode");

                    b.Property<string>("StateName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasAnnotation("Relational:JsonPropertyName", "stateName");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.ToTable("Terms");

                    b.HasAnnotation("Relational:JsonPropertyName", "terms");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.AddressInformation", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.CosponsoredLegislation", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.Legislation", "Legislation")
                        .WithMany("CosponsoredLegislations")
                        .HasForeignKey("LegislationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PollDancingLibrary.Models.Member", "Member")
                        .WithMany("CosponsoredLegislations")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Legislation");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Depiction", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.Member", "Member")
                        .WithOne("Depiction")
                        .HasForeignKey("PollDancingLibrary.Models.Depiction", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Member", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.AddressInformation", "AddressInformation")
                        .WithMany()
                        .HasForeignKey("AddressInformationId");

                    b.Navigation("AddressInformation");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.SponsoredLegislation", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.Legislation", "Legislation")
                        .WithMany("SponsoredLegislations")
                        .HasForeignKey("LegislationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PollDancingLibrary.Models.Member", "Member")
                        .WithMany("SponsoredLegislations")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Legislation");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Term", b =>
                {
                    b.HasOne("PollDancingLibrary.Models.Member", "Member")
                        .WithMany("Terms")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Legislation", b =>
                {
                    b.Navigation("CosponsoredLegislations");

                    b.Navigation("SponsoredLegislations");
                });

            modelBuilder.Entity("PollDancingLibrary.Models.Member", b =>
                {
                    b.Navigation("CosponsoredLegislations");

                    b.Navigation("Depiction");

                    b.Navigation("SponsoredLegislations");

                    b.Navigation("Terms");
                });
#pragma warning restore 612, 618
        }
    }
}
