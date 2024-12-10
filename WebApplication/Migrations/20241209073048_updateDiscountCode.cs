using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class updateDiscountCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiLang_Images_Vouchers");

            migrationBuilder.DropTable(
                name: "MultiLang_VoucherCates");

            migrationBuilder.DropTable(
                name: "MultiLang_VoucherDetails");

            migrationBuilder.DropTable(
                name: "VoucherCate_VoucherDetails");

            migrationBuilder.DropTable(
                name: "Images_Vouchers");

            migrationBuilder.DropTable(
                name: "VoucherCates");

            migrationBuilder.DropTable(
                name: "VoucherDetails");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Voucher");

            migrationBuilder.CreateTable(
                name: "DiscountCodeCates",
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
                    table.PrimaryKey("PK_DiscountCodeCates", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeDetails",
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
                    DiscountCodeValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountCodeType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_DiscountCodeCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCodeCatePid = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_DiscountCodeCates", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_DiscountCodeCates_DiscountCodeCates_DiscountCodeCatePid",
                        column: x => x.DiscountCodeCatePid,
                        principalTable: "DiscountCodeCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscountCodeCate_DiscountCodeDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCodeDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    DiscountCodeCatePid = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountCodeCate_DiscountCodeDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_DiscountCodeCate_DiscountCodeDetails_DiscountCodeCates_DiscountCodeCatePid",
                        column: x => x.DiscountCodeCatePid,
                        principalTable: "DiscountCodeCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscountCodeCate_DiscountCodeDetails_DiscountCodeDetails_DiscountCodeDetailPid",
                        column: x => x.DiscountCodeDetailPid,
                        principalTable: "DiscountCodeDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images_DiscountCodes",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCodeDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images_DiscountCodes", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_Images_DiscountCodes_DiscountCodeDetails_DiscountCodeDetailPid",
                        column: x => x.DiscountCodeDetailPid,
                        principalTable: "DiscountCodeDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_DiscountCodeDetails",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountCodeDetailPid = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountCodeCatePid = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_DiscountCodeDetails", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_DiscountCodeDetails_DiscountCodeCates_DiscountCodeCatePid",
                        column: x => x.DiscountCodeCatePid,
                        principalTable: "DiscountCodeCates",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MultiLang_DiscountCodeDetails_DiscountCodeDetails_DiscountCodeDetailPid",
                        column: x => x.DiscountCodeDetailPid,
                        principalTable: "DiscountCodeDetails",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Images_DiscountCodes",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImagesDiscountCodePid = table.Column<long>(type: "bigint", nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Images_DiscountCodes", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Images_DiscountCodes_Images_DiscountCodes_ImagesDiscountCodePid",
                        column: x => x.ImagesDiscountCodePid,
                        principalTable: "Images_DiscountCodes",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(2001), new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(2009) });

            migrationBuilder.InsertData(
                table: "DiscountCodeCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(7306), null, false, true, new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(7316), 0L, 0, null, null, null, true });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 193, DateTimeKind.Local).AddTicks(2320), new DateTime(2024, 12, 9, 14, 30, 45, 193, DateTimeKind.Local).AddTicks(2329) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 193, DateTimeKind.Local).AddTicks(7362), new DateTime(2024, 12, 9, 14, 30, 45, 193, DateTimeKind.Local).AddTicks(7371) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(2223), new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(2235) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(9223));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 189, DateTimeKind.Local).AddTicks(1794));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 189, DateTimeKind.Local).AddTicks(1804));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3363));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3485));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3483));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3491));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3493));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3497));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3411));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3457));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3406));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3499));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3502));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 186, DateTimeKind.Local).AddTicks(6012));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3507));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3414));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3459));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3506));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3489));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3454));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3452));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3461));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3500));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3509));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3504));

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[] { "DiscountCode", new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3455), null, false, true, null, false, "DiscountCode", 34, null, null, "b-admin/DiscountCode/Index", null });

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 191, DateTimeKind.Local).AddTicks(6874), new DateTime(2024, 12, 9, 14, 30, 45, 191, DateTimeKind.Local).AddTicks(6888) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 182, DateTimeKind.Local).AddTicks(7695), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(423) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2103), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2112) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2120), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2121) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2125), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2126) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2130), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2131) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2137), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2138) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2142), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2143) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2146), new DateTime(2024, 12, 9, 14, 30, 45, 184, DateTimeKind.Local).AddTicks(2147) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(8985), new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(8996) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(727), new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(733) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1891), new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1898) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1904), new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1905) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1910), new DateTime(2024, 12, 9, 14, 30, 45, 195, DateTimeKind.Local).AddTicks(1911) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 197, DateTimeKind.Local).AddTicks(1647), new DateTime(2024, 12, 9, 14, 30, 45, 197, DateTimeKind.Local).AddTicks(1655) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 196, DateTimeKind.Local).AddTicks(1699), new DateTime(2024, 12, 9, 14, 30, 45, 196, DateTimeKind.Local).AddTicks(1708) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 189, DateTimeKind.Local).AddTicks(6138), new DateTime(2024, 12, 9, 14, 30, 45, 189, DateTimeKind.Local).AddTicks(6148) });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeCate_DiscountCodeDetails_DiscountCodeCatePid",
                table: "DiscountCodeCate_DiscountCodeDetails",
                column: "DiscountCodeCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodeCate_DiscountCodeDetails_DiscountCodeDetailPid",
                table: "DiscountCodeCate_DiscountCodeDetails",
                column: "DiscountCodeDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_Images_DiscountCodes_DiscountCodeDetailPid",
                table: "Images_DiscountCodes",
                column: "DiscountCodeDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_DiscountCodeCates_DiscountCodeCatePid",
                table: "MultiLang_DiscountCodeCates",
                column: "DiscountCodeCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_DiscountCodeDetails_DiscountCodeCatePid",
                table: "MultiLang_DiscountCodeDetails",
                column: "DiscountCodeCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_DiscountCodeDetails_DiscountCodeDetailPid",
                table: "MultiLang_DiscountCodeDetails",
                column: "DiscountCodeDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_DiscountCodes_ImagesDiscountCodePid",
                table: "MultiLang_Images_DiscountCodes",
                column: "ImagesDiscountCodePid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscountCodeCate_DiscountCodeDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_DiscountCodeCates");

            migrationBuilder.DropTable(
                name: "MultiLang_DiscountCodeDetails");

            migrationBuilder.DropTable(
                name: "MultiLang_Images_DiscountCodes");

            migrationBuilder.DropTable(
                name: "DiscountCodeCates");

            migrationBuilder.DropTable(
                name: "Images_DiscountCodes");

            migrationBuilder.DropTable(
                name: "DiscountCodeDetails");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "DiscountCode");

            migrationBuilder.CreateTable(
                name: "VoucherCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ParentRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    isLocked = table.Column<bool>(type: "bit", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CounterView = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    Enabled = table.Column<bool>(type: "bit", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsHot = table.Column<bool>(type: "bit", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MaxQuantity = table.Column<int>(type: "int", nullable: false),
                    MinTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    PicThumb = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlugTagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TagKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateUser = table.Column<string>(type: "nvarchar(24)", maxLength: 24, nullable: true),
                    UsedQuantity = table.Column<int>(type: "int", nullable: false),
                    VoucherType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherDetails", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_VoucherCates",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: false)
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
                    Images = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<long>(type: "bigint", nullable: false),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false)
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
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TitleSEO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TitleWithoutSign = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: true),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false)
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
                    VoucherCatePid = table.Column<long>(type: "bigint", nullable: false),
                    VoucherDetailPid = table.Column<long>(type: "bigint", nullable: false)
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
                name: "MultiLang_Images_Vouchers",
                columns: table => new
                {
                    Pid = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagesVoucherPid = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 318, DateTimeKind.Local).AddTicks(2907), new DateTime(2024, 12, 9, 13, 40, 36, 318, DateTimeKind.Local).AddTicks(2915) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 317, DateTimeKind.Local).AddTicks(3600), new DateTime(2024, 12, 9, 13, 40, 36, 317, DateTimeKind.Local).AddTicks(3608) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 317, DateTimeKind.Local).AddTicks(8370), new DateTime(2024, 12, 9, 13, 40, 36, 317, DateTimeKind.Local).AddTicks(8378) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(3661), new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(3669) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(4877));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(6905));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(6914));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(871));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(900));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(905));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(906));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(908));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(883));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(895));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(880));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(910));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(913));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 312, DateTimeKind.Local).AddTicks(8328));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(918));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(886));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(884));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(917));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(903));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(891));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(890));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(898));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(911));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(920));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(915));

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[] { "Voucher", new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(893), null, false, true, null, false, "Voucher", 34, null, null, "b-admin/Voucher/Index", null });

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 315, DateTimeKind.Local).AddTicks(8763), new DateTime(2024, 12, 9, 13, 40, 36, 315, DateTimeKind.Local).AddTicks(8771) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 309, DateTimeKind.Local).AddTicks(839), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(3810) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5365), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5372) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5380), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5382) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5385), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5386) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5389), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5390) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5397), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5398) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5401), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5402) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5405), new DateTime(2024, 12, 9, 13, 40, 36, 310, DateTimeKind.Local).AddTicks(5406) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 318, DateTimeKind.Local).AddTicks(9937), new DateTime(2024, 12, 9, 13, 40, 36, 318, DateTimeKind.Local).AddTicks(9945) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(1651), new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(1658) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2748), new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2753) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2758), new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2759) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2763), new DateTime(2024, 12, 9, 13, 40, 36, 319, DateTimeKind.Local).AddTicks(2765) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 321, DateTimeKind.Local).AddTicks(1969), new DateTime(2024, 12, 9, 13, 40, 36, 321, DateTimeKind.Local).AddTicks(1978) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 320, DateTimeKind.Local).AddTicks(2345), new DateTime(2024, 12, 9, 13, 40, 36, 320, DateTimeKind.Local).AddTicks(2354) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 314, DateTimeKind.Local).AddTicks(462), new DateTime(2024, 12, 9, 13, 40, 36, 314, DateTimeKind.Local).AddTicks(470) });

            migrationBuilder.InsertData(
                table: "VoucherCates",
                columns: new[] { "Pid", "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "LastLogin", "Order", "ParentId", "ParentRoute", "UpdateDate", "UpdateUser", "isLocked" },
                values: new object[] { 1L, "/", new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(8533), null, false, true, new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(8541), 0L, 0, null, null, null, true });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Vouchers_VoucherDetailPid",
                table: "Images_Vouchers",
                column: "VoucherDetailPid");

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Images_Vouchers_ImagesVoucherPid",
                table: "MultiLang_Images_Vouchers",
                column: "ImagesVoucherPid");

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
                name: "IX_VoucherCate_VoucherDetails_VoucherCatePid",
                table: "VoucherCate_VoucherDetails",
                column: "VoucherCatePid");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherCate_VoucherDetails_VoucherDetailPid",
                table: "VoucherCate_VoucherDetails",
                column: "VoucherDetailPid");
        }
    }
}
