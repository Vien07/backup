using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutCates",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Advertisements",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    DisplayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisements", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Page = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Branchs",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branchs", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: false),
                    NameKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfos",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isMultiLang = table.Column<bool>(type: "bit", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfos", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ContactLists",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLists", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Conveniences",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conveniences", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GoogleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacebookId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(48)", maxLength: 48, nullable: true),
                    ActivationCode = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    CodeExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    MonthOfBirth = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    YearOfBirth = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zalo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "EmailTempateVariables",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTempateVariables", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsPlainText = table.Column<bool>(type: "bit", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "FAQCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "FAQDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "FeatureDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "GalleryCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "GalleryDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "GroupAdminMenus",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAdminMenus", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    View = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "HomePages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackgroundImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePages", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "LogErrors",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Trace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogErrors", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PidDetail = table.Column<int>(type: "int", nullable: false),
                    PidCate = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ModulePreviews",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Obj = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ObjDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    IsEditPicThumb = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModulePreviews", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    UrlRewrite = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "NewsCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "NewsDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "OrderImages",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPid = table.Column<long>(type: "bigint", nullable: false),
                    BackgroundImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderImages", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductCatePid = table.Column<long>(type: "bigint", nullable: false),
                    CustomerPid = table.Column<long>(type: "bigint", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    VAT = table.Column<bool>(type: "bit", nullable: false),
                    IsPayment = table.Column<bool>(type: "bit", nullable: false),
                    ShipFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackgroundPid = table.Column<long>(type: "bigint", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Locked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Popups",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelayTime = table.Column<int>(type: "int", nullable: false),
                    TargetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    DisplayType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popups", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ProductCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsShowHome = table.Column<bool>(type: "bit", nullable: false),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Months = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ProductColors",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsShowHome = table.Column<bool>(type: "bit", nullable: false),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColors", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ProductComments",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    ReplyId = table.Column<long>(type: "bigint", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Like = table.Column<int>(type: "int", nullable: false),
                    Heart = table.Column<int>(type: "int", nullable: false),
                    Share = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductComments", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ProductDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tiki = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Shopee = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Lazada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<long>(type: "bigint", nullable: false),
                    UserAmount = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Cycle = table.Column<int>(type: "int", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "ProductOptions",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    IsShowHome = table.Column<bool>(type: "bit", nullable: false),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOptions", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "PromotionCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "PromotionDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminMenuCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    PermissionPid = table.Column<long>(type: "bigint", nullable: false),
                    Licensed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Slides",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TargetLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slides", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastOnlineTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "VoucherCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "VoucherDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxQuantity = table.Column<int>(type: "int", nullable: false),
                    UsedQuantity = table.Column<int>(type: "int", nullable: false),
                    MinTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VoucherValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "AboutDetails",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutCatePid = table.Column<int>(type: "int", nullable: false),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    ShowTopMenu = table.Column<bool>(type: "bit", nullable: false),
                    ShowFooter = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Default = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_AboutDetails_AboutCates_AboutCatePid",
                        column: x => x.AboutCatePid,
                        principalTable: "AboutCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Advertisements",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbedCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Advertisements", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Advertisements_Advertisements_AdvertisementPid",
                        column: x => x.AdvertisementPid,
                        principalTable: "Advertisements",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Banners",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Banners", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Banners_Banners_BannerPid",
                        column: x => x.BannerPid,
                        principalTable: "Banners",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Branchs",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Branchs", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Branchs_Branchs_BranchPid",
                        column: x => x.BranchPid,
                        principalTable: "Branchs",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Calendars",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Calendars", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Calendars_Calendars_CalendarPid",
                        column: x => x.CalendarPid,
                        principalTable: "Calendars",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Comments",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentPid = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Comments", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Comments_Comments_CommentPid",
                        column: x => x.CommentPid,
                        principalTable: "Comments",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_ContactInfos",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactInfoID = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_ContactInfos", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_ContactInfos_ContactInfos_ContactInfoID",
                        column: x => x.ContactInfoID,
                        principalTable: "ContactInfos",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Conveniences",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConveniencePid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Conveniences", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Conveniences_Conveniences_ConveniencePid",
                        column: x => x.ConveniencePid,
                        principalTable: "Conveniences",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_EmailTemplates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailTemplatePid = table.Column<long>(type: "bigint", nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_EmailTemplates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_EmailTemplates_EmailTemplates_EmailTemplatePid",
                        column: x => x.EmailTemplatePid,
                        principalTable: "EmailTemplates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_FAQCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FAQCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_FAQCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_FAQCates_FAQCates_FAQCatePid",
                        column: x => x.FAQCatePid,
                        principalTable: "FAQCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FAQCate_FAQDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FAQDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    FAQCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQCate_FAQDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_FAQCate_FAQDetails_FAQCates_FAQCatePid",
                        column: x => x.FAQCatePid,
                        principalTable: "FAQCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAQCate_FAQDetails_FAQDetails_FAQDetailPid",
                        column: x => x.FAQDetailPid,
                        principalTable: "FAQDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_FAQs",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FAQDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_FAQs", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_FAQs_FAQDetails_FAQDetailPid",
                        column: x => x.FAQDetailPid,
                        principalTable: "FAQDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_FAQDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FAQDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FAQCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_FAQDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_FAQDetails_FAQCates_FAQCatePid",
                        column: x => x.FAQCatePid,
                        principalTable: "FAQCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_FAQDetails_FAQDetails_FAQDetailPid",
                        column: x => x.FAQDetailPid,
                        principalTable: "FAQDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_FeatureCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_FeatureCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_FeatureCates_FeatureCates_FeatureCatePid",
                        column: x => x.FeatureCatePid,
                        principalTable: "FeatureCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnquireLists",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullNameEnquire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneEnquire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailEnquire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEnquire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    DateEnquire = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecivedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isHidden = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnquireLists", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_EnquireLists_FeatureDetails_ServiceDetailPid",
                        column: x => x.ServiceDetailPid,
                        principalTable: "FeatureDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeatureCate_FeatureDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    FeatureCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeatureCate_FeatureDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_FeatureCate_FeatureDetails_FeatureCates_FeatureCatePid",
                        column: x => x.FeatureCatePid,
                        principalTable: "FeatureCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeatureCate_FeatureDetails_FeatureDetails_FeatureDetailPid",
                        column: x => x.FeatureDetailPid,
                        principalTable: "FeatureDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Features",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Features", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Features_FeatureDetails_FeatureDetailPid",
                        column: x => x.FeatureDetailPid,
                        principalTable: "FeatureDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_FeatureDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeatureDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FeatureCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_FeatureDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_FeatureDetails_FeatureCates_FeatureCatePid",
                        column: x => x.FeatureCatePid,
                        principalTable: "FeatureCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_FeatureDetails_FeatureDetails_FeatureDetailPid",
                        column: x => x.FeatureDetailPid,
                        principalTable: "FeatureDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_GalleryCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_GalleryCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_GalleryCates_GalleryCates_GalleryCatePid",
                        column: x => x.GalleryCatePid,
                        principalTable: "GalleryCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GalleryCate_GalleryDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    GalleryCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryCate_GalleryDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_GalleryCate_GalleryDetails_GalleryCates_GalleryCatePid",
                        column: x => x.GalleryCatePid,
                        principalTable: "GalleryCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GalleryCate_GalleryDetails_GalleryDetails_GalleryDetailPid",
                        column: x => x.GalleryDetailPid,
                        principalTable: "GalleryDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Galleries",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Galleries", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Galleries_GalleryDetails_GalleryDetailPid",
                        column: x => x.GalleryDetailPid,
                        principalTable: "GalleryDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_GalleryDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GalleryDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GalleryCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_GalleryDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_GalleryDetails_GalleryCates_GalleryCatePid",
                        column: x => x.GalleryCatePid,
                        principalTable: "GalleryCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_GalleryDetails_GalleryDetails_GalleryDetailPid",
                        column: x => x.GalleryDetailPid,
                        principalTable: "GalleryDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GroupUserCode = table.Column<int>(type: "int", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    IP = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecoveryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RecoveryString = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Users_GroupUsers_GroupUserCode",
                        column: x => x.GroupUserCode,
                        principalTable: "GroupUsers",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_HomePages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomePagePid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IntroLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_HomePages", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_HomePages_HomePages_HomePagePid",
                        column: x => x.HomePagePid,
                        principalTable: "HomePages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_NewsCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_NewsCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_NewsCates_NewsCates_NewsCatePid",
                        column: x => x.NewsCatePid,
                        principalTable: "NewsCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Newses",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Newses", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Newses_NewsDetails_NewsDetailPid",
                        column: x => x.NewsDetailPid,
                        principalTable: "NewsDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_NewsDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_NewsDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_NewsDetails_NewsCates_NewsCatePid",
                        column: x => x.NewsCatePid,
                        principalTable: "NewsCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_NewsDetails_NewsDetails_NewsDetailPid",
                        column: x => x.NewsDetailPid,
                        principalTable: "NewsDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NewsCate_NewsDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    NewsCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCate_NewsDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_NewsCate_NewsDetails_NewsCates_NewsCatePid",
                        column: x => x.NewsCatePid,
                        principalTable: "NewsCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsCate_NewsDetails_NewsDetails_NewsDetailPid",
                        column: x => x.NewsDetailPid,
                        principalTable: "NewsDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderPid = table.Column<long>(type: "bigint", nullable: false),
                    UID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    OptionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorId = table.Column<long>(type: "bigint", nullable: false),
                    ColorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telegram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Viber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skype = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zalo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QrLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyWebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonalWebsiteLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderPid",
                        column: x => x.OrderPid,
                        principalTable: "Orders",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Advertisement_Pages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdvertisementPid = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Advertisement_Pages", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Advertisement_Pages_Advertisements_AdvertisementPid",
                        column: x => x.AdvertisementPid,
                        principalTable: "Advertisements",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Advertisement_Pages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banner_Pages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerPid = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banner_Pages", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Banner_Pages_Banners_BannerPid",
                        column: x => x.BannerPid,
                        principalTable: "Banners",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Banner_Pages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Pages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PagePid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Pages", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Pages_Pages_PagePid",
                        column: x => x.PagePid,
                        principalTable: "Pages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupPermissons",
                columns: table => new
                {
                    Code = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupUserCode = table.Column<int>(type: "int", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    PermissonCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPermissons", x => x.Code);
                    table.ForeignKey(
                        name: "FK_GroupPermissons_GroupUsers_GroupUserCode",
                        column: x => x.GroupUserCode,
                        principalTable: "GroupUsers",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPermissons_Modules_ModuleCode",
                        column: x => x.ModuleCode,
                        principalTable: "Modules",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPermissons_Permissions_PermissonCode",
                        column: x => x.PermissonCode,
                        principalTable: "Permissions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Popups",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PopupPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmbedCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Popups", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Popups_Popups_PopupPid",
                        column: x => x.PopupPid,
                        principalTable: "Popups",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Popup_Pages",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PopupPid = table.Column<int>(type: "int", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Popup_Pages", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Popup_Pages_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Popup_Pages_Popups_PopupPid",
                        column: x => x.PopupPid,
                        principalTable: "Popups",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_ProductCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_ProductCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_ProductCates_ProductCates_ProductCatePid",
                        column: x => x.ProductCatePid,
                        principalTable: "ProductCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_ProductColors",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductColorPid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_ProductColors", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_ProductColors_ProductColors_ProductColorPid",
                        column: x => x.ProductColorPid,
                        principalTable: "ProductColors",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Products",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Products", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Products_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_ProductDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_ProductDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_ProductDetails_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCate_ProductDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Enable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCate_ProductDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_ProductCate_ProductDetails_ProductCates_ProductCatePid",
                        column: x => x.ProductCatePid,
                        principalTable: "ProductCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCate_ProductDetails_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductColor_ProductDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductColorPid = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSoldOut = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductColor_ProductDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_ProductColor_ProductDetails_ProductColors_ProductColorPid",
                        column: x => x.ProductColorPid,
                        principalTable: "ProductColors",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductColor_ProductDetails_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_ProductOptions",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductOptionPid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_ProductOptions", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_ProductOptions_ProductOptions_ProductOptionPid",
                        column: x => x.ProductOptionPid,
                        principalTable: "ProductOptions",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductOption_ProductDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductOptionPid = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSoldOut = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOption_ProductDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_ProductOption_ProductDetails_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOption_ProductDetails_ProductOptions_ProductOptionPid",
                        column: x => x.ProductOptionPid,
                        principalTable: "ProductOptions",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_PromotionCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_PromotionCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_PromotionCates_PromotionCates_PromotionCatePid",
                        column: x => x.PromotionCatePid,
                        principalTable: "PromotionCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Promotiones",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Promotiones", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Promotiones_PromotionDetails_PromotionDetailPid",
                        column: x => x.PromotionDetailPid,
                        principalTable: "PromotionDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_PromotionDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoProductListId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotionCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_PromotionDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_PromotionDetails_PromotionCates_PromotionCatePid",
                        column: x => x.PromotionCatePid,
                        principalTable: "PromotionCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_PromotionDetails_PromotionDetails_PromotionDetailPid",
                        column: x => x.PromotionDetailPid,
                        principalTable: "PromotionDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Promotion_Products",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionPid = table.Column<long>(type: "bigint", nullable: false),
                    PromotionDetailPid = table.Column<long>(type: "bigint", nullable: true),
                    ProductPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductDetailPid = table.Column<long>(type: "bigint", nullable: true),
                    OptionPid = table.Column<long>(type: "bigint", nullable: false),
                    ProductOptionPid = table.Column<long>(type: "bigint", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion_Products", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Promotion_Products_ProductDetails_ProductDetailPid",
                        column: x => x.ProductDetailPid,
                        principalTable: "ProductDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotion_Products_ProductOptions_ProductOptionPid",
                        column: x => x.ProductOptionPid,
                        principalTable: "ProductOptions",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Promotion_Products_PromotionDetails_PromotionDetailPid",
                        column: x => x.PromotionDetailPid,
                        principalTable: "PromotionDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PromotionCate_PromotionDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    PromotionCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromotionCate_PromotionDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_PromotionCate_PromotionDetails_PromotionCates_PromotionCatePid",
                        column: x => x.PromotionCatePid,
                        principalTable: "PromotionCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PromotionCate_PromotionDetails_PromotionDetails_PromotionDetailPid",
                        column: x => x.PromotionDetailPid,
                        principalTable: "PromotionDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_RecruitmentCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_RecruitmentCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_RecruitmentCates_RecruitmentCates_RecruitmentCatePid",
                        column: x => x.RecruitmentCatePid,
                        principalTable: "RecruitmentCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CV = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecruitmentDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Candidates_RecruitmentDetails_RecruitmentDetailPid",
                        column: x => x.RecruitmentDetailPid,
                        principalTable: "RecruitmentDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Recruitments",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Recruitments", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Recruitments_RecruitmentDetails_RecruitmentDetailPid",
                        column: x => x.RecruitmentDetailPid,
                        principalTable: "RecruitmentDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_RecruitmentDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rank = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Job = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPlace = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_RecruitmentDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_RecruitmentDetails_RecruitmentDetails_RecruitmentDetailPid",
                        column: x => x.RecruitmentDetailPid,
                        principalTable: "RecruitmentDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentCate_RecruitmentDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecruitmentDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    RecruitmentCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentCate_RecruitmentDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_RecruitmentCate_RecruitmentDetails_RecruitmentCates_RecruitmentCatePid",
                        column: x => x.RecruitmentCatePid,
                        principalTable: "RecruitmentCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecruitmentCate_RecruitmentDetails_RecruitmentDetails_RecruitmentDetailPid",
                        column: x => x.RecruitmentDetailPid,
                        principalTable: "RecruitmentDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Slides",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlidePid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Slides", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Slides_Slides_SlidePid",
                        column: x => x.SlidePid,
                        principalTable: "Slides",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_VoucherCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_VoucherCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_VoucherCates_VoucherCates_VoucherCatePid",
                        column: x => x.VoucherCatePid,
                        principalTable: "VoucherCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_Vouchers",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_Vouchers", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_Vouchers_VoucherDetails_VoucherDetailPid",
                        column: x => x.VoucherDetailPid,
                        principalTable: "VoucherDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_VoucherDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_VoucherDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_VoucherDetails_VoucherCates_VoucherCatePid",
                        column: x => x.VoucherCatePid,
                        principalTable: "VoucherCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_VoucherDetails_VoucherDetails_VoucherDetailPid",
                        column: x => x.VoucherDetailPid,
                        principalTable: "VoucherDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherCate_VoucherDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherCate_VoucherDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_VoucherCate_VoucherDetails_VoucherCates_VoucherCatePid",
                        column: x => x.VoucherCatePid,
                        principalTable: "VoucherCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherCate_VoucherDetails_VoucherDetails_VoucherDetailPid",
                        column: x => x.VoucherDetailPid,
                        principalTable: "VoucherDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_AboutDetails",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AboutDetailPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_AboutDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_AboutDetails_AboutDetails_AboutDetailPid",
                        column: x => x.AboutDetailPid,
                        principalTable: "AboutDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_FAQs",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesFAQPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_FAQs", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_FAQs_Images_FAQs_ImagesFAQPid",
                        column: x => x.ImagesFAQPid,
                        principalTable: "Images_FAQs",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Features",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesFeaturePid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Features", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Features_Images_Features_ImagesFeaturePid",
                        column: x => x.ImagesFeaturePid,
                        principalTable: "Images_Features",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Galleries",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesGalleryPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Galleries", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Galleries_Images_Galleries_ImagesGalleryPid",
                        column: x => x.ImagesGalleryPid,
                        principalTable: "Images_Galleries",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCode = table.Column<int>(type: "int", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: false),
                    PermissonCode = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Modules_ModuleCode",
                        column: x => x.ModuleCode,
                        principalTable: "Modules",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissonCode",
                        column: x => x.PermissonCode,
                        principalTable: "Permissions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Users_UserCode",
                        column: x => x.UserCode,
                        principalTable: "Users",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Newses",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesNewsPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Newses", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Newses_Images_Newses_ImagesNewsPid",
                        column: x => x.ImagesNewsPid,
                        principalTable: "Images_Newses",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Products",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesProductPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Products", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Products_Images_Products_ImagesProductPid",
                        column: x => x.ImagesProductPid,
                        principalTable: "Images_Products",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Promotiones",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesPromotionPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Promotiones", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Promotiones_Images_Promotiones_ImagesPromotionPid",
                        column: x => x.ImagesPromotionPid,
                        principalTable: "Images_Promotiones",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Recruitments",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesRecruitmentPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Recruitments", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Recruitments_Images_Recruitments_ImagesRecruitmentPid",
                        column: x => x.ImagesRecruitmentPid,
                        principalTable: "Images_Recruitments",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_Vouchers",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesVoucherPid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_Vouchers", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_Vouchers_Images_Vouchers_ImagesVoucherPid",
                        column: x => x.ImagesVoucherPid,
                        principalTable: "Images_Vouchers",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AboutCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "UpdateDate", "UpdateUser" },
                values: new object[] { 1, "/", new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(6843), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(6851), 0, null, null });

            migrationBuilder.InsertData(
                table: "Configurations",
                columns: new[] { "Key", "Group", "NameKey", "Type", "Value" },
                values: new object[,]
                {
                    { "relate-project-limit", "Other", "RelateProjectLimit", "text", "5" },
                    { "relate-product-limit", "Other", "RelateProductLimit", "text", "5" },
                    { "relate-news-limit", "Other", "RelateNewsLimit", "text", "5" },
                    { "relate-faq-limit", "Other", "RelateFAQLimit", "text", "5" },
                    { "hot-gallery-limit", "Other", "HotGalleryLimit", "text", "5" },
                    { "hot-feature-limit", "Other", "HotFeatureLimit", "text", "5" },
                    { "hot-project-limit", "Other", "HotProjectLimit", "text", "5" },
                    { "hot-product-limit", "Other", "HotProductLimit", "text", "5" },
                    { "hot-news-limit", "Other", "HotNewsLimit", "text", "5" },
                    { "hot-faq-limit", "Other", "HotFAQLimit", "text", "5" },
                    { "facebook-appid", "Infomation", "FacebookAppId", "text", null },
                    { "google-signin-key", "Infomation", "GoogleSignInKey", "text", null },
                    { "reCaptcha-secret-key", "Other", "reCatchaSecretKey", "text", null },
                    { "reCaptcha-site-key", "Other", "reCapchaSiteKey", "text", null },
                    { "relate-feature-limit", "Other", "RelateFeatureLimit", "text", "5" },
                    { "watermark-position", "Images", "WatermarkPosition", "radio", "bottomRight" },
                    { "how-image-list-show", "Images", "HowImageListShow", "radio", "slide" },
                    { "watermark-type", "Images", "WatermarkType", "radio", "image" },
                    { "watermark-opacity", "Images", "WatermarkOpacity", "text", "40" },
                    { "watermark-text", "Images", "WatermarkText", "text", null },
                    { "watermark-image", "Images", "WatermarkImage", "file", null },
                    { "popup-delay-time", "Images", "PopupDelayTime", "text", "2" },
                    { "certificate-image", "Images", "CertificateImage", "file", null },
                    { "bct-image", "Images", "BctImage", "file", null },
                    { "feature-image", "Images", "FeatureImage", "file", null },
                    { "home-image-mobile", "Images", "HomeImageMobile", "file", null },
                    { "home-image", "Images", "HomeImage", "file", null },
                    { "faq-image", "Images", "FAQImage", "file", null },
                    { "default-og-image", "Images", "DefaultOgImage", "file", null },
                    { "favicon", "Images", "Favicon", "file", null },
                    { "position-image-list-show", "Images", "PositionImageListShow", "radio", "top" },
                    { "logo-footer", "Images", "LogoFooter", "file", "bizmac.png" },
                    { "relate-gallery-limit", "Other", "RelateGalleryLimit", "text", "5" },
                    { "page-limit", "Other", "PageLimit", "text", "12" },
                    { "momo-secrect-key", "Infomation", "MomoSecrectKey", "text", "" },
                    { "momo-access-key", "Infomation", "MomoAccessKey", "text", "" },
                    { "momo-partner-code", "Infomation", "MomoPartnerCode", "select", "" },
                    { "momo-api", "Infomation", "MomoApi", "text", "" },
                    { "display-momo", "Infomation", "DisplayMomo", "check", null },
                    { "ibanking-info", "Infomation", "iBankingInfo", "text", "" },
                    { "ibanking-name", "Infomation", "iBankingName", "text", "" }
                });

            migrationBuilder.InsertData(
                table: "Configurations",
                columns: new[] { "Key", "Group", "NameKey", "Type", "Value" },
                values: new object[,]
                {
                    { "display-ibanking", "Infomation", "DisplayiBanking", "check", null },
                    { "tax-code-company", "Infomation", "TaxCodeCompany", "text", "" },
                    { "email-GlobalEmailFooter", "Email", "EmailGlobalEmailFooter", "text", "" },
                    { "email-GlobalEmailHeader", "Email", "EmailGlobalEmailHeader", "text", "" },
                    { "email-SSLTLSEncryption", "Email", "EmailEncryption", "select", "TLS" },
                    { "email-port", "Email", "EmailPort", "text", "587" },
                    { "email-SMTPPassword", "Email", "EmailSMTPPassword", "text", "tjthlgoblzekmpud" },
                    { "date-format", "Other", "DateFormat", "select", "DD/MM/YYYY" },
                    { "email-SMTPUser", "Email", "EmailSMTPUser", "text", "noreply.smtp.web@gmail.com" },
                    { "email-fromEmail", "Email", "EmailFromEmail", "text", "thanh.nc@bizmac.com.vn" },
                    { "email-fromName", "Email", "EmailFromName", "text", "bizmac.com" },
                    { "money-format", "Other", "FormatMoney", "select", "." },
                    { "product-code", "Other", "ProductCode", "text", null },
                    { "seo-config", "Other", "SEOConfig", "check", "off" },
                    { "facebook-login", "Other", "FacebookLogin", "check", null },
                    { "google-login", "Other", "GoogleLogin", "check", null },
                    { "watermark-picThumb-active", "Other", "WatermarkPicThumbActive", "check", null },
                    { "watermark-active", "Other", "WatermarkActive", "check", null },
                    { "page-limit-admin", "Other", "PageLimitAdmin", "text", "12" },
                    { "recaptcha", "Other", "Recaptcha", "check", null },
                    { "maintenance", "Other", "Maintenance", "check", null },
                    { "robots", "Other", "Robots", "check", null },
                    { "page-limit-detail", "Other", "PageLimitDetail", "text", "6" },
                    { "email-SMTPServer", "Email", "EmailSMTPServer", "text", "smtp.gmail.com" },
                    { "logo", "Images", "Logo", "file", "bizmac.png" },
                    { "zalo-oaid", "Infomation", "ZaloOAId", "text", "1" },
                    { "image-upload-max-width", "Images", "ImageMaxWidth", "text", "1366" },
                    { "banner-slide-upload-width", "Images", "BannerSlideWidth", "text", "1920" },
                    { "website-Name", "Infomation", "WebsiteName", "text", null },
                    { "email-Admin", "Infomation", "EmailAdmin", "text", "thanh.nc@bizmac.com.vn" },
                    { "meta-Description", "Infomation", "MetaDescription", "text", null },
                    { "code-Header", "Infomation", "CodeHeader", "text", null },
                    { "code-Body", "Infomation", "CodeBody", "text", null },
                    { "share-facebook", "Infomation", "ShareFacebook", "text", null },
                    { "share-whatsapp", "Infomation", "ShareWhatsApp", "text", null },
                    { "share-skype", "Infomation", "ShareSkype", "text", null },
                    { "share-viber", "Infomation", "ShareViber", "text", null },
                    { "share-twitter", "Infomation", "ShareTwitter", "text", null },
                    { "share-youtube", "Infomation", "ShareYoutube", "text", null },
                    { "meta-Keywords", "Infomation", "MetaKeywords", "text", null },
                    { "share-linkedin", "Infomation", "ShareLinkedin", "text", null },
                    { "share-telegram", "Infomation", "ShareTelegram", "text", null }
                });

            migrationBuilder.InsertData(
                table: "Configurations",
                columns: new[] { "Key", "Group", "NameKey", "Type", "Value" },
                values: new object[,]
                {
                    { "images-upload-prefix", "Images", "ImagePrefix", "text", "bizmac_" },
                    { "root-domain", "Infomation", "RootDomain", "text", null },
                    { "image-upload-min-width", "Images", "ImageMinWidth", "text", "600" },
                    { "link-bct", "Infomation", "LinkBCT", "text", null },
                    { "link-certificate", "Infomation", "LinkCertificate", "text", null },
                    { "share-zalo", "Infomation", "ShareZalo", "text", null },
                    { "link-policy", "Infomation", "LinkPolicy", "text", null },
                    { "share-podcast", "Infomation", "SharePodcast", "text", null },
                    { "share-pinterest", "Infomation", "SharePinterest", "text", null },
                    { "share-instagram", "Infomation", "ShareInstagram", "text", null },
                    { "share-tiktok", "Infomation", "ShareTiktok", "text", null }
                });

            migrationBuilder.InsertData(
                table: "ContactInfos",
                columns: new[] { "Pid", "Code", "Type", "Value", "isMultiLang" },
                values: new object[,]
                {
                    { 6L, "contact-noted", "text", null, true },
                    { 1L, "contact-companyName", "text", null, true },
                    { 2L, "contact-address", "text", null, true },
                    { 3L, "contact-tel", "text", null, false },
                    { 4L, "contact-fax", "text", null, false },
                    { 5L, "contact-hotline", "text", null, false },
                    { 7L, "contact-map", "text", null, false },
                    { 10L, "contact-intro", "text", null, true },
                    { 9L, "contact-time", "text", null, true },
                    { 8L, "contact-email", "text", null, false }
                });

            migrationBuilder.InsertData(
                table: "EmailTempateVariables",
                columns: new[] { "Pid", "Code", "Group", "Name", "Value" },
                values: new object[,]
                {
                    { 83, "{{DatetimeNow}}", "AdminVAT", "DatetimeNow", null },
                    { 84, "{{FromName}}", "AdminVAT", "FromName", null },
                    { 85, "{{InvoiceCode}}", "AdminVAT", "InvoiceCode", null },
                    { 86, "{{InvoiceCode}}", "AdminOrder", "InvoiceCode", null },
                    { 74, "{{Total}}", "AdminOrder", "Total", null },
                    { 87, "{{TemporaryPrice}}", "AdminOrder", "TemporaryPrice", null },
                    { 82, "{{CompanyName}}", "AdminVAT", "CompanyName", null },
                    { 81, "{{Hotline}}", "AdminVAT", "Hotline", null },
                    { 80, "{{Logo}}", "AdminVAT", "Logo", null },
                    { 79, "{{TableProductList}}", "AdminOrder", "TableProductList", null },
                    { 75, "{{Note}}", "AdminOrder", "Note", null },
                    { 73, "{{Deposit}}", "AdminOrder", "Deposit", null },
                    { 60, "{{FullName}}", "AdminOrder", "FullName", null },
                    { 71, "{{PaymentMethod}}", "AdminOrder", "PaymentMethod", null },
                    { 50, "{{DatetimeNow}}", "CustomerOrder", "DatetimeNow", null },
                    { 51, "{{FromName}}", "CustomerOrder", "FromName", null },
                    { 52, "{{FullName}}", "CustomerOrder", "FullName", null },
                    { 53, "{{Email}}", "CustomerOrder", "Email", null },
                    { 54, "{{PhoneNumber}}", "CustomerOrder", "PhoneNumber", null },
                    { 55, "{{Logo}}", "AdminOrder", "Logo", null },
                    { 56, "{{Hotline}}", "AdminOrder", "Hotline", null }
                });

            migrationBuilder.InsertData(
                table: "EmailTempateVariables",
                columns: new[] { "Pid", "Code", "Group", "Name", "Value" },
                values: new object[,]
                {
                    { 57, "{{CompanyName}}", "AdminOrder", "CompanyName", null },
                    { 58, "{{DatetimeNow}}", "AdminOrder", "DatetimeNow", null },
                    { 59, "{{FromName}}", "AdminOrder", "FromName", null },
                    { 88, "{{Logo}}", "EnquireToCustomer", "Logo", null },
                    { 61, "{{Email}}", "AdminOrder", "Email", null },
                    { 62, "{{PhoneNumber}}", "AdminOrder", "PhoneNumber", null },
                    { 63, "{{CompanyAddress}}", "AdminOrder", "Address", null },
                    { 64, "{{FirstName}}", "AdminOrder", "FirstName", null },
                    { 65, "{{LastName}}", "AdminOrder", "LastName", null },
                    { 66, "{{PhoneNumber}}", "AdminOrder", "PhoneNumber", null },
                    { 67, "{{Email}}", "AdminOrder", "Email", null },
                    { 68, "{{CustomerAddress}}", "AdminOrder", "CustomerAddress", null },
                    { 69, "{{State}}", "AdminOrder", "State", null },
                    { 70, "{{IsPayment}}", "AdminOrder", "IsPayment", null },
                    { 72, "{{ShipFee}}", "AdminOrder", "ShipFee", null },
                    { 89, "{{Hotline}}", "EnquireToCustomer", "Hotline", null },
                    { 129, "{{TableProductList}}", "OrderToAdmin", "TableProductList", null },
                    { 91, "{{DatetimeNow}}", "EnquireToCustomer", "DatetimeNow", null },
                    { 110, "{{FullName}}", "OrderToAdmin", "FullName", null },
                    { 109, "{{FromName}}", "OrderToAdmin", "FromName", null },
                    { 108, "{{DatetimeNow}}", "OrderToAdmin", "DatetimeNow", null },
                    { 107, "{{CompanyName}}", "OrderToAdmin", "CompanyName", null },
                    { 106, "{{Hotline}}", "OrderToAdmin", "Hotline", null },
                    { 105, "{{Logo}}", "OrderToAdmin", "Logo", null },
                    { 104, "{{EnquireDate}}", "EnquireToAdmin", "EnquireDate", null },
                    { 103, "{{Content}}", "EnquireToAdmin", "Content", null },
                    { 102, "{{ServiceName}}", "EnquireToAdmin", "ServiceName", null },
                    { 101, "{{PhoneNumber}}", "EnquireToAdmin", "PhoneNumber", null },
                    { 100, "{{Email}}", "EnquireToAdmin", "Email", null },
                    { 99, "{{FullName}}", "EnquireToAdmin", "FullName", null },
                    { 98, "{{FromName}}", "EnquireToAdmin", "FromName", null },
                    { 97, "{{DatetimeNow}}", "EnquireToAdmin", "DatetimeNow", null },
                    { 49, "{{CompanyName}}", "CustomerOrder", "CompanyName", null },
                    { 111, "{{Email}}", "OrderToAdmin", "Email", null },
                    { 112, "{{PhoneNumber}}", "OrderToAdmin", "PhoneNumber", null },
                    { 113, "{{CompanyAddress}}", "OrderToAdmin", "Address", null },
                    { 114, "{{FirstName}}", "OrderToAdmin", "FirstName", null },
                    { 92, "{{FromName}}", "EnquireToCustomer", "FromName", null },
                    { 93, "{{ClientName}}", "EnquireToCustomer", "ClientName", null },
                    { 94, "{{Logo}}", "EnquireToAdmin", "Logo", null },
                    { 95, "{{Hotline}}", "EnquireToAdmin", "Hotline", null },
                    { 96, "{{CompanyName}}", "EnquireToAdmin", "CompanyName", null }
                });

            migrationBuilder.InsertData(
                table: "EmailTempateVariables",
                columns: new[] { "Pid", "Code", "Group", "Name", "Value" },
                values: new object[,]
                {
                    { 125, "{{Note}}", "OrderToAdmin", "Note", null },
                    { 124, "{{Total}}", "OrderToAdmin", "Total", null },
                    { 90, "{{CompanyName}}", "EnquireToCustomer", "CompanyName", null },
                    { 123, "{{Deposit}}", "OrderToAdmin", "Deposit", null },
                    { 121, "{{PaymentMethod}}", "OrderToAdmin", "PaymentMethod", null },
                    { 120, "{{IsPayment}}", "OrderToAdmin", "IsPayment", null },
                    { 119, "{{State}}", "OrderToAdmin", "State", null },
                    { 118, "{{CustomerAddress}}", "OrderToAdmin", "CustomerAddress", null },
                    { 117, "{{Email}}", "OrderToAdmin", "Email", null },
                    { 116, "{{PhoneNumber}}", "OrderToAdmin", "PhoneNumber", null },
                    { 115, "{{LastName}}", "OrderToAdmin", "LastName", null },
                    { 122, "{{ShipFee}}", "OrderToAdmin", "ShipFee", null },
                    { 48, "{{Hotline}}", "CustomerOrder", "Hotline", null },
                    { 11, "{{FromName}}", "ContactToAdmin", "FromName", null },
                    { 46, "{{Content}}", "RecruitToAdmin", "Content", null },
                    { 20, "{{DatetimeNow}}", "ActiveAccount", "DatetimeNow", null },
                    { 19, "{{CompanyName}}", "ActiveAccount", "CompanyName", null },
                    { 18, "{{Hotline}}", "ActiveAccount", "Hotline", null },
                    { 17, "{{Logo}}", "ActiveAccount", "Logo", null },
                    { 15, "{{Title}}", "ContactToAdmin", "Title", null },
                    { 14, "{{PhoneNumber}}", "ContactToAdmin", "PhoneNumber", null },
                    { 13, "{{Email}}", "ContactToAdmin", "Email", null },
                    { 12, "{{FullName}}", "ContactToAdmin", "FullName", null },
                    { 10, "{{DatetimeNow}}", "ContactToAdmin", "DatetimeNow", null },
                    { 9, "{{CompanyName}}", "ContactToAdmin", "CompanyName", null },
                    { 8, "{{Hotline}}", "ContactToAdmin", "Hotline", null },
                    { 7, "{{Logo}}", "ContactToAdmin", "Logo", null },
                    { 6, "{{ClientName}}", "ContactToCustomer", "ClientName", null },
                    { 5, "{{FromName}}", "ContactToCustomer", "FromName", null },
                    { 4, "{{DatetimeNow}}", "ContactToCustomer", "DatetimeNow", null },
                    { 3, "{{CompanyName}}", "ContactToCustomer", "CompanyName", null },
                    { 2, "{{Hotline}}", "ContactToCustomer", "Hotline", null },
                    { 1, "{{Logo}}", "ContactToCustomer", "Logo", null },
                    { 47, "{{Logo}}", "CustomerOrder", "Logo", null },
                    { 21, "{{FromName}}", "ActiveAccount", "FromName", null },
                    { 22, "{{LinkActiveAccount}}", "ActiveAccount", "LinkActiveAccount", null },
                    { 16, "{{Content}}", "ContactToAdmin", "Content", null },
                    { 24, "{{Hotline}}", "ForgotPassword", "Hotline", null },
                    { 45, "{{Title}}", "RecruitToAdmin", "Title", null },
                    { 44, "{{Job}}", "RecruitToAdmin", "Job", null },
                    { 43, "{{PhoneNumber}}", "RecruitToAdmin", "PhoneNumber", null },
                    { 42, "{{Email}}", "RecruitToAdmin", "Email", null }
                });

            migrationBuilder.InsertData(
                table: "EmailTempateVariables",
                columns: new[] { "Pid", "Code", "Group", "Name", "Value" },
                values: new object[,]
                {
                    { 23, "{{Logo}}", "ForgotPassword", "Logo", null },
                    { 39, "{{FromName}}", "RecruitToAdmin", "FromName", null },
                    { 38, "{{DatetimeNow}}", "RecruitToAdmin", "DatetimeNow", null },
                    { 37, "{{CompanyName}}", "RecruitToAdmin", "CompanyName", null },
                    { 36, "{{Hotline}}", "RecruitToAdmin", "Hotline", null },
                    { 35, "{{Logo}}", "RecruitToAdmin", "Logo", null },
                    { 34, "{{ClientName}}", "RecruitToCustomer", "ClientName", null },
                    { 41, "{{FullName}}", "RecruitToAdmin", "FullName", null },
                    { 33, "{{FromName}}", "RecruitToCustomer", "FromName", null },
                    { 32, "{{DatetimeNow}}", "RecruitToCustomer", "DatetimeNow", null },
                    { 31, "{{CompanyName}}", "RecruitToCustomer", "CompanyName", null },
                    { 30, "{{Hotline}}", "RecruitToCustomer", "Hotline", null },
                    { 29, "{{Logo}}", "RecruitToCustomer", "Logo", null },
                    { 28, "{{LinkForgotPassword}}", "ForgotPassword", "LinkForgotPassword", null },
                    { 27, "{{FromName}}", "ForgotPassword", "FromName", null },
                    { 26, "{{DatetimeNow}}", "ForgotPassword", "DatetimeNow", null },
                    { 25, "{{CompanyName}}", "ForgotPassword", "CompanyName", null }
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Pid", "Code", "Enabled", "Group", "IsPlainText", "Order", "Title" },
                values: new object[,]
                {
                    { 3L, "EmailActiveAccount", true, "ActiveAccount", false, 0L, "Email gửi khách hàng kích hoạt tài khoản khi khách hàng đăng ký" },
                    { 4L, "EmailForgotPassword", true, "ForgotPassword", false, 0L, "Email gửi khách hàng khi khách hàng quên mật khẩu" },
                    { 5L, "EmailRecruitToAdmin", false, "RecruitToAdmin", false, 0L, "Email gửi admin khi người dùng ứng tuyển " },
                    { 6L, "EmailRecruitToCustomer", false, "RecruitToCustomer", false, 0L, "Email gửi người dùng khi ứng tuyển" },
                    { 9L, "EmailAdminVAT", true, "AdminVAT", false, 0L, "Email gửi khách hàng VAT" },
                    { 8L, "EmailAdminOrder", true, "AdminOrder", false, 0L, "Email gửi khách hàng khi ấn nút send mail trong admin" },
                    { 12L, "EmailOrderToAdmin", true, "OrderToAdmin", false, 0L, "Email gửi admin khi khách đặt hàng" },
                    { 10L, "EmailEnquireToAdmin", false, "EnquireToAdmin", false, 0L, "Email gửi admin khi đặt lịch hẹn" },
                    { 11L, "EmailEnquireToCustomer", false, "EnquireToCustomer", false, 0L, "Email gửi khách hàng khi đặt lịch hẹn" },
                    { 2L, "EmailContactToCustomer", true, "ContactToCustomer", false, 0L, "Email gửi khách hàng khi liên hệ" },
                    { 7L, "EmailCustomerOrder", true, "CustomerOrder", false, 0L, "Email gửi khách hàng đi ấn nút đặt hàng" },
                    { 1L, "EmailContactToAdmin", true, "ContactToAdmin", false, 0L, "Email gửi admin khi liên hệ" }
                });

            migrationBuilder.InsertData(
                table: "FAQCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(6988), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(6997), 0L, 0, null, null, null, true });

            migrationBuilder.InsertData(
                table: "FeatureCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(1966), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(1975), 0L, 0, null, null, null, true });

            migrationBuilder.InsertData(
                table: "GalleryCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(6912), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(6921), 0L, 0, null, null, null, true });

            migrationBuilder.InsertData(
                table: "GroupUsers",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Name", "Order", "Role", "UpdateDate", "UpdateUser", "View" },
                values: new object[,]
                {
                    { 2, new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(8333), null, false, true, "Admin", 2, "Admin", null, null, true },
                    { 3, new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(8342), null, false, true, "User", 3, "Staff", null, null, true },
                    { 1, new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(6322), null, false, true, "Super Admin", 1, "Super Admin", null, null, false }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[,]
                {
                    { "Users", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1862), null, false, true, null, false, "Nhân viên quản lý (Users)", 63, null, null, "b-admin/Users/Index", null },
                    { "Feature", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1820), null, false, true, null, false, "Tính năng (Feature)", 19, null, null, "b-admin/Feature/", null },
                    { "News", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1818), null, false, true, null, false, "Tin tức (News)", 15, null, null, "b-admin/News/", null },
                    { "Convenience", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1816), null, false, true, null, false, "Tiện ích (Convenience)", 9, null, null, "b-admin/Convenience/Index", null },
                    { "FAQ", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1814), null, false, true, null, false, "FAQ's", 7, null, null, "b-admin/FAQ/", null },
                    { "About", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1804), null, false, true, null, false, "Giới thiệu (About)", 5, null, null, "b-admin/About/", null },
                    { "HomePage", new DateTime(2024, 7, 4, 19, 55, 8, 39, DateTimeKind.Local).AddTicks(8969), null, false, true, null, false, "Trang chủ (Homepage)", 3, null, null, "b-admin/HomePage/", null }
                });

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[,]
                {
                    { "Permit", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1864), null, false, true, null, false, "Phân quyền (Permission)", 65, null, null, "b-admin/Permit/Index", null },
                    { "Trash", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1867), null, false, true, null, false, "Thùng rác (Trash)", 69, null, null, "b-admin/Trash/", null },
                    { "Log", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1866), null, false, true, null, false, "Log", 67, null, null, "b-admin/Log/Index", null },
                    { "Product", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1823), null, false, true, null, false, "Sản phẩm (Product)", 27, null, null, "b-admin/Product/Index", null },
                    { "Order", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1827), null, false, true, null, false, "Đơn hàng", 37, null, null, "b-admin/Order/Index", null },
                    { "Slide", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1829), null, false, true, null, false, "Slide", 38, null, null, "b-admin/Slide/", null },
                    { "Banner", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1831), null, false, true, null, false, "Banner", 41, null, null, "b-admin/Banner/", null },
                    { "Advertisement", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1832), null, false, true, null, false, "Quảng cáo (Advertisement)", 43, null, null, "b-admin/Advertisement/Index", null },
                    { "Popup", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1834), null, false, true, null, false, "Popup", 45, null, null, "b-admin/Popup/Index", null },
                    { "Calendar", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1836), null, false, true, null, false, "Calendar", 47, null, null, "b-admin/Calendar/", null },
                    { "Contact", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1838), null, false, true, null, false, "Liên hệ (Contact)", 49, null, null, "b-admin/Contact/Index", null },
                    { "ContactList", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1840), null, false, true, null, false, "Danh sách liên hệ (Contact list)", 53, null, null, "b-admin/ContactList/Index", null },
                    { "GeneralConfiguration", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1857), null, false, true, null, false, "Cài đặt (Setting)", 57, null, null, "b-admin/GeneralConfiguration/Index", null },
                    { "Translation", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1859), null, false, true, null, false, "Dịch ngôn ngữ", 59, null, null, "b-admin/Translation/Index", null },
                    { "Group", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1860), null, false, true, null, false, "Nhóm quản lý (Group)", 61, null, null, "b-admin/Group/Index", null },
                    { "Customer", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1825), null, false, true, null, false, "Khách hàng (Customer)", 35, null, null, "b-admin/Customer/Index", null },
                    { "ProductCate", new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1822), null, false, true, null, false, "Chu kỳ sản phẩm (ProductCycle)", 25, null, null, "b-admin/ProductCate/Index", null }
                });

            migrationBuilder.InsertData(
                table: "NewsCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(1752), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(1761), 0L, 0, null, null, null, true });

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Pid", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "UpdateDate", "UpdateUser" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 7, 4, 19, 55, 8, 36, DateTimeKind.Local).AddTicks(1760), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(3154), 0, null, null },
                    { 7, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4718), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4719), 0, null, null },
                    { 5, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4705), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4706), 0, null, null },
                    { 4, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4701), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4702), 0, null, null },
                    { 3, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4695), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4697), 0, null, null },
                    { 2, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4679), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4687), 0, null, null },
                    { 8, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4722), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4723), 0, null, null },
                    { 6, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4713), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4715), 0, null, null }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Code", "Locked", "Name" },
                values: new object[,]
                {
                    { "ADD", false, "add" },
                    { "EDIT", false, "edit" },
                    { "DELETE", false, "delete" },
                    { "ONLYSUBADMIN", true, "onlysubadmin" },
                    { "VIEW", false, "view" }
                });

            migrationBuilder.InsertData(
                table: "ProductCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "IsShowHome", "LastLogin", "Months", "Order", "ParentId", "ParentRoute", "PicThumb", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[,]
                {
                    { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(3941), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(3949), 0, 0L, 0, null, null, null, null, true },
                    { 2L, "1", new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(6006), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(6012), 1, 1L, 0, null, null, null, null, false },
                    { 3L, "2", new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7156), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7162), 6, 2L, 0, null, null, null, null, false },
                    { 4L, "3", new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7167), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7169), 9, 3L, 0, null, null, null, null, false },
                    { 5L, "4", new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7173), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7174), 18, 4L, 0, null, null, null, null, false }
                });

            migrationBuilder.InsertData(
                table: "ProductColors",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "IsShowHome", "LastLogin", "Order", "ParentId", "ParentRoute", "PicThumb", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "#000000", new DateTime(2024, 7, 4, 19, 55, 8, 48, DateTimeKind.Local).AddTicks(9370), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 48, DateTimeKind.Local).AddTicks(9383), 1L, 0, null, "default.png", null, null, false });

            migrationBuilder.InsertData(
                table: "ProductOptions",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "IsShowHome", "LastLogin", "Order", "ParentId", "ParentRoute", "PicThumb", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "default", new DateTime(2024, 7, 4, 19, 55, 8, 47, DateTimeKind.Local).AddTicks(7127), null, false, true, false, new DateTime(2024, 7, 4, 19, 55, 8, 47, DateTimeKind.Local).AddTicks(7136), 1L, 0, null, null, null, null, false });

            migrationBuilder.InsertData(
                table: "VoucherCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(1864), null, false, true, new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(1873), 0L, 0, null, null, null, true });

            migrationBuilder.InsertData(
                table: "MultiLang_EmailTemplates",
                columns: new[] { "Pid", "Content", "EmailTemplatePid", "FromName", "LangKey", "Subject" },
                values: new object[,]
                {
                    { 10L, null, 5L, "", "en", "" },
                    { 6L, null, 3L, "", "en", "" },
                    { 7L, "Mật khẩu mới của bạn là: {{NewPassword}}", 4L, "{{FromName}}", "vi", "Quên mật khẩu - {{FromName}}" },
                    { 9L, "{FullName}} {{Email}} {{PhoneNumber}} {{Title}} {{Content}} {{Job}}", 5L, "{{FromName}}", "vi", "Có 1 yêu cầu tuyển dụng - {{FromName}}" },
                    { 11L, "Cảm ơn bạn đã ứng tuyển , chúng tôi sẽ trả lời bạn sớm nhất có thể.", 6L, "{{FromName}}", "vi", "Tuyển dụng - {{FromName}}" },
                    { 12L, null, 6L, "", "en", "" },
                    { 13L, "Cảm ơn bạn đã đặt hàng , chúng tôi sẽ trả lời bạn sớm nhất có thể.", 7L, "{{FromName}}", "vi", "Cảm ơn bạn đã đặt hàng - {{FromName}}" },
                    { 14L, null, 7L, "", "en", "" },
                    { 15L, "Xác nhận đơn hàng , chúng tôi sẽ trả lời bạn sớm nhất có thể.", 8L, "{{FromName}}", "vi", "Xác nhận đơn hàng - {{FromName}}" },
                    { 16L, null, 8L, "", "en", "" },
                    { 17L, "Chúng tôi gửi bạn VAT cho mã đơn hàng {{InvoiceCode}}", 9L, "{{FromName}}", "vi", "Xuất VAT - {{FromName}}" },
                    { 18L, null, 9L, "", "en", "" },
                    { 19L, "<div class=\"tt\"><div class=\"tt\">Xin chào <strong><span class=\"pull - right\"><a class=\"add_merge_field text - primary \" role=\"button\"> {{FromName}},</a></span></strong></div><p>Có <strong>01 </strong>yêu cầu được gửi tới website!</p><ul><li>Tên Khách hàng: {{FullName}}</li><li>Email: {{Email}}</li><li>Số điện thoại: {{PhoneNumber}}</li><li>Dịch vụ: {{ServiceName}}</li><li>Ngày đặt lịch hẹn: {{EnquireDate}}</li><li>Nội dung: {{Content}}</li></ul><hr><div class=\"small text - muted note\">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div></div>", 10L, "{{FromName}}", "vi", "Có 1 khách hàng đặt lịch hẹn" },
                    { 20L, "<div class=\"tt\">Hello<strong><span class=\"pull - right\"><a class=\"add_merge_field text - primary \" role=\"button\"> {{FromName}} </a></span></strong></div><p>There is <strong>01</strong> request sent to the website!</p><ul><li>Customer name: {{FullName}}</li><li>Email: {{Email}}</li><li>Phone number: {{PhoneNumber}}</li><li>Service: {{ServiceName}}</li><li>Appointment date: {{EnquireDate}}</li><li>Content: {{Content}}</li></ul><hr><div class=\"small text - muted note\">* This is an autoresponder email, please do not reply to this message.</div>", 10L, "{{FromName}}", "en", "There is 1 customer who has booked an appointment" },
                    { 21L, "<div class=\"tt\">Xin chào <strong><span class=\"pull - right\"> <a class=\"add_merge_field text - primary \" role=\"button\"> {{ClientName}},</a></span></strong></div><div class=\"tt\">Cảm ơn bạn đã đặt lịch hẹn, chúng tôi sẽ trả lời bạn sớm nhất có thể.</div><hr><div class=\"small text - muted note\">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div>", 11L, "{{FromName}}", "vi", "Cảm ơn bạn đã đặt lịch hẹn" },
                    { 22L, "<div class=\"tt\">Dear<strong><span class=\"pull - right\">&nbsp;<a class=\"add_merge_field text - primary \" style=\"text - decoration: none; \" role=\"button\" data-mce-style=\"text - decoration: none; \">{{ClientName}},</a></span></strong></div><div class=\"tt\">Thank you for your contact, we will reply to you as soon as possible.</div><hr><div class=\"small text-muted note\">* This is an autoresponder email, please do not reply to this message.</div>", 11L, "{{FromName}}", "en", "Thank you for scheduling your appointment!" },
                    { 23L, "<div class=\"e - body\"> <div class=\"tt\">Xin ch&agrave;o <strong><span class=\"pull - right\"><a class=\"add_merge_field text - primary \" role=\"button\"> {{FromName}},</a></span></strong></div> <p>C&oacute; <strong>01 </strong>đơn h&agrave;ng mới!</p> <hr /> <div class=\"mb - 3 fs - 14\"> <div class=\"tt tt - color uppercase border - bottom fs - 14\">Th&ocirc;ng tin đơn h&agrave;ng {{InvoiceCode}} <span class=\"small fw - normal uppercase - none text - body\">({{DatetimeNow}})</span></div> <table class=\"w - 100 mb - 2\"> <tbody> <tr> <td class=\"col - 6 align - top\" style=\"width: 199px; \"> <p><strong>Th&ocirc;ng tin thanh to&aacute;n</strong></p> <p>{{FullName}}</p> <p>{{Email}}</p> <p>{{PhoneNumber}}</p> </td> <td class=\"col - 6 align - top\" style=\"width: 199px; \"> <p><strong>Địa chỉ giao h&agrave;ng</strong></p> <p>{{FullName}}</p> <p>{{CustomerAddress}}</p> <p>{{PhoneNumber}}</p> </td> </tr> </tbody> </table> <p><strong>Phương thức thanh to&aacute;n: </strong>{{PaymentMethod}}</p> <div class=\"tt tt-color uppercase border-bottom fs - 14\">Chi tiết đơn h&agrave;ng</div> <div class=\"tt tt-color uppercase border-bottom fs - 14\">{{TableProductList}}</div> <table class=\"table - invoid fs - 14 mb - 2\" style=\"height: 88px; \"> <tbody> <tr class=\"bg - light fw - bold text - red\" style=\"height: 22px; \"> <td class=\"td p-sm text - right\" style=\"width: 499.344px; height: 22px; \" colspan=\"4\">Tổng gi&aacute; trị đơn h&agrave;ng</td> <td class=\"td p-sm text - right\" style=\"width: 94px; height: 22px; \">{{Total}} đ</td> </tr> </tbody> </table> </div> </div>", 12L, "{{FromName}}", "vi", "Có 01 đơn hàng mới" },
                    { 24L, "<div class=\"e-body\"> <div class=\"tt\">Hello <strong><span class=\"pull-right\"><a class=\"add_merge_field text-primary\" role=\"button\"> {{FromName}},</a></span></strong></div> <p>You have <strong>01 </strong>new order!</p> <hr /> <div class=\"mb-3 fs-14\"> <div class=\"tt tt-color uppercase border-bottom fs-14\">Order Information {{InvoiceCode}} <span class=\"small fw-normal uppercase-none text-body\">({{DatetimeNow}})</span></div> <table class=\"w-100 mb-2\"> <tbody> <tr> <td class=\"col-6 align-top\" style=\"width: 199px; \"> <p><strong>Payment Information</strong></p> <p>{{FullName}}</p> <p>{{Email}}</p> <p>{{PhoneNumber}}</p> </td> <td class=\"col-6 align-top\" style=\"width: 199px; \"> <p><strong>Shipping Address</strong></p> <p>{{FullName}}</p> <p>{{CustomerAddress}}</p> <p>{{PhoneNumber}}</p> </td> </tr> </tbody> </table> <p><strong>Payment Method: </strong>{{PaymentMethod}}</p> <div class=\"tt tt-color uppercase border-bottom fs-14\">Order Details</div> <div class=\"tt tt-color uppercase border-bottom fs-14\">{{TableProductList}}</div> <table class=\"table-invoid fs-14 mb-2\" style=\"height: 88px; \"> <tbody> <tr class=\"bg-light fw-bold text-red\" style=\"height: 22px; \"> <td class=\"td p-sm text-right\" style=\"width: 499.344px; height: 22px; \" colspan=\"4\">Total Order Value</td> <td class=\"td p-sm text-right\" style=\"width: 94px; height: 22px; \">{{Total}} đ</td> </tr> </tbody> </table> </div> </div>", 12L, "{{FromName}}", "en", "New Order Notification" },
                    { 5L, "<a href='{{LinkActiveAccount}}'>Xác nhận tài khoản</a>", 3L, "{{FromName}}", "vi", "Xác nhận tài khoản - {{FromName}}" },
                    { 4L, "<div class=\"tt\">Dear<strong><span class=\"pull - right\">&nbsp;<a class=\"add_merge_field text - primary \" style=\"text - decoration: none; \" role=\"button\" data-mce-style=\"text - decoration: none; \">{{ClientName}},</a></span></strong></div><div class=\"tt\">Thank you for your contact, we will reply to you as soon as possible.</div><hr><div class=\"small text-muted note\">* This is an autoresponder email, please do not reply to this message.</div>", 2L, "{{FromName}}", "en", "Thank you for contacting!" },
                    { 8L, null, 4L, "", "en", "" },
                    { 2L, "<div class=\"tt\">Hello<strong><span class=\"pull - right\"><a class=\"add_merge_field text - primary \" role=\"button\"> {{FromName}} </a></span></strong></div><p>There is <strong>01</strong> request sent to the website!</p><ul><li>Customer name: {{FullName}}</li><li>Email: {{Email}}</li><li>Phone number: {{PhoneNumber}}</li><li>Title: {{Title}}</li><li>Content: {{Content}}</li></ul><hr><div class=\"small text - muted note\">* This is an autoresponder email, please do not reply to this message.</div>", 1L, "{{FromName}}", "en", "There is 1 request for support" },
                    { 1L, "<div class=\"tt\"><div class=\"tt\">Xin chào <strong><span class=\"pull - right\"><a class=\"add_merge_field text - primary \" role=\"button\"> {{FromName}},</a></span></strong></div><p>Có <strong>01 </strong>yêu cầu được gửi tới website!</p><ul><li>Tên Khách hàng: {{FullName}}</li><li>Email: {{Email}}</li><li>Số điện thoại: {{PhoneNumber}}</li><li>Tiêu đề: {{Title}}</li><li>Nội dung: {{Content}}</li></ul><hr><div class=\"small text - muted note\">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div></div>", 1L, "{{FromName}}", "vi", "Có 1 yêu cầu hỗ trợ" },
                    { 3L, "<div class=\"tt\">Xin chào <strong><span class=\"pull - right\"> <a class=\"add_merge_field text - primary \" role=\"button\"> {{ClientName}},</a></span></strong></div><div class=\"tt\">Cảm ơn bạn đã liên hệ, chúng tôi sẽ trả lời bạn sớm nhất có thể.</div><hr><div class=\"small text - muted note\">* Đây là email trả lời tự động, vui lòng không trả lời thư này.</div>", 2L, "{{FromName}}", "vi", "Cảm ơn bạn đã liên hệ" }
                });

            migrationBuilder.InsertData(
                table: "MultiLang_Pages",
                columns: new[] { "Pid", "Description", "LangKey", "PagePid", "Title" },
                values: new object[,]
                {
                    { 1, null, "vi", 1, "Trang chủ" },
                    { 2, null, "en", 1, "Home" },
                    { 3, null, "vi", 2, "Giới thiệu" },
                    { 4, null, "en", 2, "About" },
                    { 5, null, "vi", 3, "Liên hệ" },
                    { 7, null, "vi", 4, "Sản phẩm" },
                    { 8, null, "en", 4, "Product" },
                    { 9, null, "vi", 5, "Khách hàng" },
                    { 6, null, "en", 3, "Contact" },
                    { 11, null, "vi", 6, "Đơn hàng" },
                    { 12, null, "en", 6, "Order" },
                    { 13, null, "vi", 7, "Tính năng" },
                    { 14, null, "en", 7, "Feature" },
                    { 15, null, "vi", 8, "Tin tức" },
                    { 16, null, "en", 8, "News" },
                    { 10, null, "en", 5, "Customer" }
                });

            migrationBuilder.InsertData(
                table: "MultiLang_ProductCates",
                columns: new[] { "Pid", "Description", "LangKey", "Name", "ProductCatePid", "Slug" },
                values: new object[,]
                {
                    { 7L, null, "vi", "18 tháng", 5L, null },
                    { 6L, null, "en", "9 month", 4L, null }
                });

            migrationBuilder.InsertData(
                table: "MultiLang_ProductCates",
                columns: new[] { "Pid", "Description", "LangKey", "Name", "ProductCatePid", "Slug" },
                values: new object[,]
                {
                    { 5L, null, "vi", "9 tháng", 4L, null },
                    { 4L, null, "en", "6 month", 3L, null },
                    { 1L, null, "vi", "1 tháng", 2L, null },
                    { 2L, null, "en", "1 month", 2L, null },
                    { 8L, null, "en", "18 month", 5L, null },
                    { 3L, null, "vi", "6 tháng", 3L, null }
                });

            migrationBuilder.InsertData(
                table: "MultiLang_ProductColors",
                columns: new[] { "Pid", "Description", "LangKey", "Name", "ProductColorPid", "Slug" },
                values: new object[,]
                {
                    { 2L, "", "en", "Default", 1L, "default" },
                    { 1L, "", "vi", "Mặc định", 1L, "default" }
                });

            migrationBuilder.InsertData(
                table: "MultiLang_ProductOptions",
                columns: new[] { "Pid", "Description", "LangKey", "Name", "ProductOptionPid", "Slug" },
                values: new object[,]
                {
                    { 2L, "", "en", "Default", 1L, "default" },
                    { 1L, "", "vi", "Default", 1L, "default" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Pid", "Avatar", "Code", "CreateDate", "CreateUser", "Deleted", "Email", "Enabled", "FirstName", "FullName", "GroupUserCode", "IP", "LastLogin", "LastName", "Order", "Password", "Phone", "RecoveryString", "RecoveryTime", "Salt", "UpdateDate", "UpdateUser" },
                values: new object[] { 1, "", "bizmac", new DateTime(2024, 7, 4, 19, 55, 8, 41, DateTimeKind.Local).AddTicks(2160), null, false, "info@bizmac.com.vn", true, "bizmac", "bizmac ecommerce", 1, null, new DateTime(2024, 7, 4, 19, 55, 8, 41, DateTimeKind.Local).AddTicks(2168), "ecommerce ", 1, "559F52E363C3AA964CBD64481E3E7BDC6E99A9A127BCCD8E010685AC31B2E949", null, null, null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_AboutDetails_AboutCatePid",
                table: "AboutDetails",
                column: "AboutCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_Pages_AdvertisementPid",
                table: "Advertisement_Pages",
                column: "AdvertisementPid");

            migrationBuilder.CreateIndex(
                name: "IX_Advertisement_Pages_PageId",
                table: "Advertisement_Pages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_Pages_BannerPid",
                table: "Banner_Pages",
                column: "BannerPid");

            migrationBuilder.CreateIndex(
                name: "IX_Banner_Pages_PageId",
                table: "Banner_Pages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_RecruitmentDetailPid",
                table: "Candidates",
                column: "RecruitmentDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_EnquireLists_ServiceDetailPid",
                table: "EnquireLists",
                column: "ServiceDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_FAQCate_FAQDetails_FAQCatePid",
                table: "FAQCate_FAQDetails",
                column: "FAQCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_FAQCate_FAQDetails_FAQDetailPid",
                table: "FAQCate_FAQDetails",
                column: "FAQDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCate_FeatureDetails_FeatureCatePid",
                table: "FeatureCate_FeatureDetails",
                column: "FeatureCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_FeatureCate_FeatureDetails_FeatureDetailPid",
                table: "FeatureCate_FeatureDetails",
                column: "FeatureDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryCate_GalleryDetails_GalleryCatePid",
                table: "GalleryCate_GalleryDetails",
                column: "GalleryCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryCate_GalleryDetails_GalleryDetailPid",
                table: "GalleryCate_GalleryDetails",
                column: "GalleryDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissons_GroupUserCode",
                table: "GroupPermissons",
                column: "GroupUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissons_ModuleCode",
                table: "GroupPermissons",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPermissons_PermissonCode",
                table: "GroupPermissons",
                column: "PermissonCode");

            migrationBuilder.CreateIndex(
                name: "IX_Images_FAQs_FAQDetailPid",
                table: "Images_FAQs",
                column: "FAQDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Features_FeatureDetailPid",
                table: "Images_Features",
                column: "FeatureDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Galleries_GalleryDetailPid",
                table: "Images_Galleries",
                column: "GalleryDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Newses_NewsDetailPid",
                table: "Images_Newses",
                column: "NewsDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Products_ProductDetailPid",
                table: "Images_Products",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Promotiones_PromotionDetailPid",
                table: "Images_Promotiones",
                column: "PromotionDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Recruitments_RecruitmentDetailPid",
                table: "Images_Recruitments",
                column: "RecruitmentDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_Vouchers_VoucherDetailPid",
                table: "Images_Vouchers",
                column: "VoucherDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_AboutDetails_AboutDetailPid",
                table: "MultiLang_AboutDetails",
                column: "AboutDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Advertisements_AdvertisementPid",
                table: "MultiLang_Advertisements",
                column: "AdvertisementPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Banners_BannerPid",
                table: "MultiLang_Banners",
                column: "BannerPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Branchs_BranchPid",
                table: "MultiLang_Branchs",
                column: "BranchPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Calendars_CalendarPid",
                table: "MultiLang_Calendars",
                column: "CalendarPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Comments_CommentPid",
                table: "MultiLang_Comments",
                column: "CommentPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_ContactInfos_ContactInfoID",
                table: "MultiLang_ContactInfos",
                column: "ContactInfoID");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Conveniences_ConveniencePid",
                table: "MultiLang_Conveniences",
                column: "ConveniencePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_EmailTemplates_EmailTemplatePid",
                table: "MultiLang_EmailTemplates",
                column: "EmailTemplatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FAQCates_FAQCatePid",
                table: "MultiLang_FAQCates",
                column: "FAQCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FAQDetails_FAQCatePid",
                table: "MultiLang_FAQDetails",
                column: "FAQCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FAQDetails_FAQDetailPid",
                table: "MultiLang_FAQDetails",
                column: "FAQDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FeatureCates_FeatureCatePid",
                table: "MultiLang_FeatureCates",
                column: "FeatureCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FeatureDetails_FeatureCatePid",
                table: "MultiLang_FeatureDetails",
                column: "FeatureCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_FeatureDetails_FeatureDetailPid",
                table: "MultiLang_FeatureDetails",
                column: "FeatureDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_GalleryCates_GalleryCatePid",
                table: "MultiLang_GalleryCates",
                column: "GalleryCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_GalleryDetails_GalleryCatePid",
                table: "MultiLang_GalleryDetails",
                column: "GalleryCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_GalleryDetails_GalleryDetailPid",
                table: "MultiLang_GalleryDetails",
                column: "GalleryDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_HomePages_HomePagePid",
                table: "MultiLang_HomePages",
                column: "HomePagePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_FAQs_ImagesFAQPid",
                table: "MultiLang_Images_FAQs",
                column: "ImagesFAQPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Features_ImagesFeaturePid",
                table: "MultiLang_Images_Features",
                column: "ImagesFeaturePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Galleries_ImagesGalleryPid",
                table: "MultiLang_Images_Galleries",
                column: "ImagesGalleryPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Newses_ImagesNewsPid",
                table: "MultiLang_Images_Newses",
                column: "ImagesNewsPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Products_ImagesProductPid",
                table: "MultiLang_Images_Products",
                column: "ImagesProductPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Promotiones_ImagesPromotionPid",
                table: "MultiLang_Images_Promotiones",
                column: "ImagesPromotionPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Recruitments_ImagesRecruitmentPid",
                table: "MultiLang_Images_Recruitments",
                column: "ImagesRecruitmentPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Vouchers_ImagesVoucherPid",
                table: "MultiLang_Images_Vouchers",
                column: "ImagesVoucherPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_NewsCates_NewsCatePid",
                table: "MultiLang_NewsCates",
                column: "NewsCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_NewsDetails_NewsCatePid",
                table: "MultiLang_NewsDetails",
                column: "NewsCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_NewsDetails_NewsDetailPid",
                table: "MultiLang_NewsDetails",
                column: "NewsDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Pages_PagePid",
                table: "MultiLang_Pages",
                column: "PagePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Popups_PopupPid",
                table: "MultiLang_Popups",
                column: "PopupPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_ProductCates_ProductCatePid",
                table: "MultiLang_ProductCates",
                column: "ProductCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_ProductColors_ProductColorPid",
                table: "MultiLang_ProductColors",
                column: "ProductColorPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_ProductDetails_ProductDetailPid",
                table: "MultiLang_ProductDetails",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_ProductOptions_ProductOptionPid",
                table: "MultiLang_ProductOptions",
                column: "ProductOptionPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_PromotionCates_PromotionCatePid",
                table: "MultiLang_PromotionCates",
                column: "PromotionCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_PromotionDetails_PromotionCatePid",
                table: "MultiLang_PromotionDetails",
                column: "PromotionCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_PromotionDetails_PromotionDetailPid",
                table: "MultiLang_PromotionDetails",
                column: "PromotionDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_RecruitmentCates_RecruitmentCatePid",
                table: "MultiLang_RecruitmentCates",
                column: "RecruitmentCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_RecruitmentDetails_RecruitmentDetailPid",
                table: "MultiLang_RecruitmentDetails",
                column: "RecruitmentDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Slides_SlidePid",
                table: "MultiLang_Slides",
                column: "SlidePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_VoucherCates_VoucherCatePid",
                table: "MultiLang_VoucherCates",
                column: "VoucherCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_VoucherDetails_VoucherCatePid",
                table: "MultiLang_VoucherDetails",
                column: "VoucherCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_VoucherDetails_VoucherDetailPid",
                table: "MultiLang_VoucherDetails",
                column: "VoucherDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCate_NewsDetails_NewsCatePid",
                table: "NewsCate_NewsDetails",
                column: "NewsCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCate_NewsDetails_NewsDetailPid",
                table: "NewsCate_NewsDetails",
                column: "NewsDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderPid",
                table: "OrderDetails",
                column: "OrderPid");

            migrationBuilder.CreateIndex(
                name: "IX_Popup_Pages_PageId",
                table: "Popup_Pages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Popup_Pages_PopupPid",
                table: "Popup_Pages",
                column: "PopupPid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCate_ProductDetails_ProductCatePid",
                table: "ProductCate_ProductDetails",
                column: "ProductCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCate_ProductDetails_ProductDetailPid",
                table: "ProductCate_ProductDetails",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductDetails_ProductColorPid",
                table: "ProductColor_ProductDetails",
                column: "ProductColorPid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductColor_ProductDetails_ProductDetailPid",
                table: "ProductColor_ProductDetails",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOption_ProductDetails_ProductDetailPid",
                table: "ProductOption_ProductDetails",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOption_ProductDetails_ProductOptionPid",
                table: "ProductOption_ProductDetails",
                column: "ProductOptionPid");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_Products_ProductDetailPid",
                table: "Promotion_Products",
                column: "ProductDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_Products_ProductOptionPid",
                table: "Promotion_Products",
                column: "ProductOptionPid");

            migrationBuilder.CreateIndex(
                name: "IX_Promotion_Products_PromotionDetailPid",
                table: "Promotion_Products",
                column: "PromotionDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCate_PromotionDetails_PromotionCatePid",
                table: "PromotionCate_PromotionDetails",
                column: "PromotionCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_PromotionCate_PromotionDetails_PromotionDetailPid",
                table: "PromotionCate_PromotionDetails",
                column: "PromotionDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentCate_RecruitmentDetails_RecruitmentCatePid",
                table: "RecruitmentCate_RecruitmentDetails",
                column: "RecruitmentCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentCate_RecruitmentDetails_RecruitmentDetailPid",
                table: "RecruitmentCate_RecruitmentDetails",
                column: "RecruitmentDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_ModuleCode",
                table: "UserPermissions",
                column: "ModuleCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissonCode",
                table: "UserPermissions",
                column: "PermissonCode");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_UserCode",
                table: "UserPermissions",
                column: "UserCode");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupUserCode",
                table: "Users",
                column: "GroupUserCode");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCate_VoucherDetails_VoucherCatePid",
                table: "VoucherCate_VoucherDetails",
                column: "VoucherCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCate_VoucherDetails_VoucherDetailPid",
                table: "VoucherCate_VoucherDetails",
                column: "VoucherDetailPid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Advertisement_Pages");

            migrationBuilder.DropTable(
                name: "Banner_Pages");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "ContactLists");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "EmailTempateVariables");

            migrationBuilder.DropTable(
                name: "EnquireLists");

            migrationBuilder.DropTable(
                name: "FAQCate_FAQDetails");

            migrationBuilder.DropTable(
                name: "FeatureCate_FeatureDetails");

            migrationBuilder.DropTable(
                name: "GalleryCate_GalleryDetails");

            migrationBuilder.DropTable(
                name: "GroupAdminMenus");

            migrationBuilder.DropTable(
                name: "GroupPermissons");

            migrationBuilder.DropTable(
                name: "LogErrors");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "ModulePreviews");

            migrationBuilder.DropTable(
                name: "MultiLang_AboutDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_Advertisements");

            migrationBuilder.DropTable(
                name: "MultiLang_Banners");

            migrationBuilder.DropTable(
                name: "MultiLang_Branchs");

            migrationBuilder.DropTable(
                name: "MultiLang_Calendars");

            migrationBuilder.DropTable(
                name: "MultiLang_Comments");

            migrationBuilder.DropTable(
                name: "MultiLang_ContactInfos");

            migrationBuilder.DropTable(
                name: "MultiLang_Conveniences");

            migrationBuilder.DropTable(
                name: "MultiLang_EmailTemplates");

            migrationBuilder.DropTable(
                name: "MultiLang_FAQCates");

            migrationBuilder.DropTable(
                name: "MultiLang_FAQDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_FeatureCates");

            migrationBuilder.DropTable(
                name: "MultiLang_FeatureDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_GalleryCates");

            migrationBuilder.DropTable(
                name: "MultiLang_GalleryDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_HomePages");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_FAQs");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Features");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Galleries");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Newses");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Products");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Promotiones");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Recruitments");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_Vouchers");

            migrationBuilder.DropTable(
                name: "MultiLang_NewsCates");

            migrationBuilder.DropTable(
                name: "MultiLang_NewsDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_Pages");

            migrationBuilder.DropTable(
                name: "MultiLang_Popups");

            migrationBuilder.DropTable(
                name: "MultiLang_ProductCates");

            migrationBuilder.DropTable(
                name: "MultiLang_ProductColors");

            migrationBuilder.DropTable(
                name: "MultiLang_ProductDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_ProductOptions");

            migrationBuilder.DropTable(
                name: "MultiLang_PromotionCates");

            migrationBuilder.DropTable(
                name: "MultiLang_PromotionDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_RecruitmentCates");

            migrationBuilder.DropTable(
                name: "MultiLang_RecruitmentDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_Slides");

            migrationBuilder.DropTable(
                name: "MultiLang_VoucherCates");

            migrationBuilder.DropTable(
                name: "MultiLang_VoucherDetails");

            migrationBuilder.DropTable(
                name: "NewsCate_NewsDetails");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "OrderImages");

            migrationBuilder.DropTable(
                name: "Popup_Pages");

            migrationBuilder.DropTable(
                name: "ProductCate_ProductDetails");

            migrationBuilder.DropTable(
                name: "ProductColor_ProductDetails");

            migrationBuilder.DropTable(
                name: "ProductComments");

            migrationBuilder.DropTable(
                name: "ProductOption_ProductDetails");

            migrationBuilder.DropTable(
                name: "Promotion_Products");

            migrationBuilder.DropTable(
                name: "PromotionCate_PromotionDetails");

            migrationBuilder.DropTable(
                name: "RecruitmentCate_RecruitmentDetails");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "VoucherCate_VoucherDetails");

            migrationBuilder.DropTable(
                name: "AboutDetails");

            migrationBuilder.DropTable(
                name: "Advertisements");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Branchs");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "ContactInfos");

            migrationBuilder.DropTable(
                name: "Conveniences");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "FAQCates");

            migrationBuilder.DropTable(
                name: "FeatureCates");

            migrationBuilder.DropTable(
                name: "GalleryCates");

            migrationBuilder.DropTable(
                name: "HomePages");

            migrationBuilder.DropTable(
                name: "Images_FAQs");

            migrationBuilder.DropTable(
                name: "Images_Features");

            migrationBuilder.DropTable(
                name: "Images_Galleries");

            migrationBuilder.DropTable(
                name: "Images_Newses");

            migrationBuilder.DropTable(
                name: "Images_Products");

            migrationBuilder.DropTable(
                name: "Images_Promotiones");

            migrationBuilder.DropTable(
                name: "Images_Recruitments");

            migrationBuilder.DropTable(
                name: "Images_Vouchers");

            migrationBuilder.DropTable(
                name: "Slides");

            migrationBuilder.DropTable(
                name: "NewsCates");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "Popups");

            migrationBuilder.DropTable(
                name: "ProductCates");

            migrationBuilder.DropTable(
                name: "ProductColors");

            migrationBuilder.DropTable(
                name: "ProductOptions");

            migrationBuilder.DropTable(
                name: "PromotionCates");

            migrationBuilder.DropTable(
                name: "RecruitmentCates");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VoucherCates");

            migrationBuilder.DropTable(
                name: "AboutCates");

            migrationBuilder.DropTable(
                name: "FAQDetails");

            migrationBuilder.DropTable(
                name: "FeatureDetails");

            migrationBuilder.DropTable(
                name: "GalleryDetails");

            migrationBuilder.DropTable(
                name: "NewsDetails");

            migrationBuilder.DropTable(
                name: "ProductDetails");

            migrationBuilder.DropTable(
                name: "PromotionDetails");

            migrationBuilder.DropTable(
                name: "RecruitmentDetails");

            migrationBuilder.DropTable(
                name: "VoucherDetails");

            migrationBuilder.DropTable(
                name: "GroupUsers");
        }
    }
}
