using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class updateorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCodeType",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCodeValue",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DiscountCodeDetails",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 398, DateTimeKind.Local).AddTicks(9444), new DateTime(2024, 12, 10, 11, 13, 10, 398, DateTimeKind.Local).AddTicks(9461) });

            migrationBuilder.UpdateData(
                table: "DiscountCodeCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 396, DateTimeKind.Local).AddTicks(9036), new DateTime(2024, 12, 10, 11, 13, 10, 396, DateTimeKind.Local).AddTicks(9044) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 397, DateTimeKind.Local).AddTicks(4010), new DateTime(2024, 12, 10, 11, 13, 10, 397, DateTimeKind.Local).AddTicks(4018) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 398, DateTimeKind.Local).AddTicks(1631), new DateTime(2024, 12, 10, 11, 13, 10, 398, DateTimeKind.Local).AddTicks(1641) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 396, DateTimeKind.Local).AddTicks(4188), new DateTime(2024, 12, 10, 11, 13, 10, 396, DateTimeKind.Local).AddTicks(4196) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(4694));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(6717));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(6727));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(602));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(634));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(632));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(648));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(650));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(652));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(614));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(626));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "DiscountCode",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(625));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(612));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(619));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(653));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(657));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 392, DateTimeKind.Local).AddTicks(8164));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(662));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(618));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(628));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(616));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(661));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(646));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(623));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(621));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(630));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(655));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(664));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 12, 10, 11, 13, 10, 393, DateTimeKind.Local).AddTicks(659));

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 395, DateTimeKind.Local).AddTicks(9178), new DateTime(2024, 12, 10, 11, 13, 10, 395, DateTimeKind.Local).AddTicks(9186) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 389, DateTimeKind.Local).AddTicks(1057), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(3521) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5090), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5098) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5105), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5107) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5110), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5112) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5115), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5116) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5123), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5124) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5127), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5128) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5131), new DateTime(2024, 12, 10, 11, 13, 10, 390, DateTimeKind.Local).AddTicks(5133) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 399, DateTimeKind.Local).AddTicks(8480), new DateTime(2024, 12, 10, 11, 13, 10, 399, DateTimeKind.Local).AddTicks(8493) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(484), new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(492) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1748), new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1754) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1761), new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1763) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1767), new DateTime(2024, 12, 10, 11, 13, 10, 400, DateTimeKind.Local).AddTicks(1768) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 402, DateTimeKind.Local).AddTicks(3143), new DateTime(2024, 12, 10, 11, 13, 10, 402, DateTimeKind.Local).AddTicks(3152) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 401, DateTimeKind.Local).AddTicks(2509), new DateTime(2024, 12, 10, 11, 13, 10, 401, DateTimeKind.Local).AddTicks(2518) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 10, 11, 13, 10, 394, DateTimeKind.Local).AddTicks(342), new DateTime(2024, 12, 10, 11, 13, 10, 394, DateTimeKind.Local).AddTicks(351) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodeType",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodeValue",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DiscountCodeDetails",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(2001), new DateTime(2024, 12, 9, 14, 30, 45, 194, DateTimeKind.Local).AddTicks(2009) });

            migrationBuilder.UpdateData(
                table: "DiscountCodeCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(7306), new DateTime(2024, 12, 9, 14, 30, 45, 192, DateTimeKind.Local).AddTicks(7316) });

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
                keyValue: "DiscountCode",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 14, 30, 45, 188, DateTimeKind.Local).AddTicks(3455));

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
        }
    }
}
