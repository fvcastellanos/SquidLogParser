﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SquidLogParser.Data;

namespace SquidLogParser.Migrations
{
    [DbContext(typeof(SquidLogContext))]
    [Migration("20200823032002_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("squid_log")
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SquidLogParser.Data.AccessLogEntry", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                    b.Property<long>("Bytes")
                        .HasColumnName("bytes")
                        .HasColumnType("bigint");

                    b.Property<string>("ClientAddress")
                        .HasColumnName("client_address")
                        .HasColumnType("varchar(200)");

                    b.Property<long>("Duration")
                        .HasColumnName("duration")
                        .HasColumnType("bigint");

                    b.Property<string>("Hash")
                        .HasColumnName("hash")
                        .HasColumnType("varchar(250)");

                    b.Property<int>("Milliseconds")
                        .HasColumnName("milliseconds")
                        .HasColumnType("int");

                    b.Property<string>("Peer")
                        .HasColumnName("peer")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("RequestMethod")
                        .HasColumnName("request_method")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("ResultCode")
                        .HasColumnName("result_code")
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("Time")
                        .HasColumnName("time")
                        .HasColumnType("timestamp");

                    b.Property<string>("Type")
                        .HasColumnName("type")
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Url")
                        .HasColumnName("url")
                        .HasColumnType("varchar(500)");

                    b.Property<string>("User")
                        .HasColumnName("user")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Bytes")
                        .HasName("idx_access_bytes");

                    b.HasIndex("ClientAddress")
                        .HasName("idx_access_log_client_address");

                    b.HasIndex("Time")
                        .HasName("idx_access_log_time");

                    b.HasIndex("Url")
                        .HasName("idx_access_log_url");

                    b.ToTable("access_log_entry");
                });

            modelBuilder.Entity("SquidLogParser.Data.FilteredUrl", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("created")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Url")
                        .HasColumnName("url")
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("Url")
                        .IsUnique()
                        .HasName("uq_filtered_url_url");

                    b.ToTable("filtered_url");
                });

            modelBuilder.Entity("SquidLogParser.Data.LogProcessHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("bigint");

                    b.Property<long>("LinesProcessed")
                        .HasColumnName("lines_processed")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("Processed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("processed")
                        .HasColumnType("timestamp")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("Processed")
                        .HasName("idx_log_process_history_processed");

                    b.ToTable("log_process_history");
                });
#pragma warning restore 612, 618
        }
    }
}
