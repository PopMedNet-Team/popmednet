﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PopMedNet.DMCS.Data.Model;

namespace PopMedNet.DMCS.Data.Migrations.Model
{
    [DbContext(typeof(ModelContext))]
    [Migration("20210730145647_AddDocumentStateToDocuments")]
    partial class AddDocumentStateToDocuments
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.AuthenticationLog", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Details")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("IPAddress")
                        .HasColumnType("nvarchar(40)")
                        .HasMaxLength(40);

                    b.Property<bool>("Success")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("UserID");

                    b.ToTable("AuthenticationLogs");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.DataMart", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Acronym")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Adapter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("AdapterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AutoProcess")
                        .HasColumnType("int");

                    b.Property<int>("CacheDays")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EnableExplictCacheRemoval")
                        .HasColumnType("bit");

                    b.Property<bool>("EncryptCache")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<byte[]>("PmnTimestamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("ID");

                    b.ToTable("Datamarts");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Document", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ContentCreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ContentModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("DocumentState")
                        .HasColumnType("int");

                    b.Property<Guid>("ItemID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Kind")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<long>("Length")
                        .HasColumnType("bigint");

                    b.Property<string>("MimeType")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<byte[]>("PmnTimestamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("RevisionSetID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("UploadedByID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50)
                        .HasDefaultValue("1.0.0.0");

                    b.HasKey("ID");

                    b.HasIndex("ID")
                        .HasAnnotation("SqlServer:Include", new[] { "RevisionSetID", "Version" });

                    b.HasIndex("UploadedByID");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Log", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("DateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ResponseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ResponseID");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Request", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Activity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActivityDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ActivityProject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdditionalInstructions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Identifier")
                        .HasColumnType("bigint");

                    b.Property<string>("MSRequestID")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhiDisclosureLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PmnTimestamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Project")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PurposeOfUse")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReportAggregationLevel")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestorCenter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceActivity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceActivityProject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceTaskOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubmittedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SubmittedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("TaskOrder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkPlanType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.RequestDataMart", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DataMartID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("ModelID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ModelText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PmnTimestamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.Property<string>("RejectReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RequestID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("RoutingType")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("DataMartID");

                    b.HasIndex("RequestID");

                    b.ToTable("RequestDataMarts");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.RequestDocument", b =>
                {
                    b.Property<Guid>("ResponseID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RevisionSetID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.HasKey("ResponseID", "RevisionSetID");

                    b.ToTable("RequestDocuments");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Response", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<byte[]>("PmnTimestamp")
                        .IsConcurrencyToken()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("RequestDataMartID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("RespondedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("ResponseTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SubmitMessage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("RequestDataMartID");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.User", b =>
                {
                    b.Property<Guid>("ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(400)")
                        .HasMaxLength(400);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.UserDataMart", b =>
                {
                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DataMartID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserID", "DataMartID");

                    b.HasIndex("DataMartID");

                    b.ToTable("UserDataMarts");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.AuthenticationLog", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.User", "User")
                        .WithMany("AuthenticationLogs")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Document", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.User", "UploadedBy")
                        .WithMany()
                        .HasForeignKey("UploadedByID")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Log", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.Response", "Response")
                        .WithMany("Logs")
                        .HasForeignKey("ResponseID");
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.RequestDataMart", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.DataMart", "DataMart")
                        .WithMany("Requests")
                        .HasForeignKey("DataMartID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PopMedNet.DMCS.Data.Model.Request", "Request")
                        .WithMany("Routes")
                        .HasForeignKey("RequestID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.RequestDocument", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.Response", "Response")
                        .WithMany("Documents")
                        .HasForeignKey("ResponseID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.Response", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.RequestDataMart", "RequestDataMart")
                        .WithMany("Responses")
                        .HasForeignKey("RequestDataMartID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("PopMedNet.DMCS.Data.Model.UserDataMart", b =>
                {
                    b.HasOne("PopMedNet.DMCS.Data.Model.DataMart", "DataMart")
                        .WithMany("Users")
                        .HasForeignKey("DataMartID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PopMedNet.DMCS.Data.Model.User", "User")
                        .WithMany("DataMarts")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
