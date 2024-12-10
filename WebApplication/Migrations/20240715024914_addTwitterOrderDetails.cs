using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class addTwitterOrderDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 298, DateTimeKind.Local).AddTicks(3658), new DateTime(2024, 7, 15, 9, 49, 12, 298, DateTimeKind.Local).AddTicks(3665) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 297, DateTimeKind.Local).AddTicks(3959), new DateTime(2024, 7, 15, 9, 49, 12, 297, DateTimeKind.Local).AddTicks(3967) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 297, DateTimeKind.Local).AddTicks(8918), new DateTime(2024, 7, 15, 9, 49, 12, 297, DateTimeKind.Local).AddTicks(8926) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 296, DateTimeKind.Local).AddTicks(4061), new DateTime(2024, 7, 15, 9, 49, 12, 296, DateTimeKind.Local).AddTicks(4070) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(4979));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(7176));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1091));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1119));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1117));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1122));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1124));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1125));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1103));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1100));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1127));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1131));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 292, DateTimeKind.Local).AddTicks(8699));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1105));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1114));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1134));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1120));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1110));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1108));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1115));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1129));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1148));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 7, 15, 9, 49, 12, 293, DateTimeKind.Local).AddTicks(1132));

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 295, DateTimeKind.Local).AddTicks(9176), new DateTime(2024, 7, 15, 9, 49, 12, 295, DateTimeKind.Local).AddTicks(9184) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 289, DateTimeKind.Local).AddTicks(4371), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(4968) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6457), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6464) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6472), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6474) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6477), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6478) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6481), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6483) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6489), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6490) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6493), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6494) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6498), new DateTime(2024, 7, 15, 9, 49, 12, 290, DateTimeKind.Local).AddTicks(6499) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(413), new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(421) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(3157), new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(3164) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4272), new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4278) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4284), new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4285) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4289), new DateTime(2024, 7, 15, 9, 49, 12, 299, DateTimeKind.Local).AddTicks(4290) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 301, DateTimeKind.Local).AddTicks(3413), new DateTime(2024, 7, 15, 9, 49, 12, 301, DateTimeKind.Local).AddTicks(3421) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 300, DateTimeKind.Local).AddTicks(3716), new DateTime(2024, 7, 15, 9, 49, 12, 300, DateTimeKind.Local).AddTicks(3725) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 294, DateTimeKind.Local).AddTicks(775), new DateTime(2024, 7, 15, 9, 49, 12, 294, DateTimeKind.Local).AddTicks(783) });

            migrationBuilder.UpdateData(
                table: "VoucherCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 15, 9, 49, 12, 296, DateTimeKind.Local).AddTicks(9116), new DateTime(2024, 7, 15, 9, 49, 12, 296, DateTimeKind.Local).AddTicks(9124) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "OrderDetails");

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(6843), new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(6851) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(6988), new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(6997) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(1966), new DateTime(2024, 7, 4, 19, 55, 8, 45, DateTimeKind.Local).AddTicks(1975) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(6912), new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(6921) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(8333));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(8342));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1804));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1831));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1836));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1838));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1816));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1825));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1820));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1857));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1860));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 39, DateTimeKind.Local).AddTicks(8969));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1866));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1818));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1864));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1834));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1823));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1859));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1867));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 7, 4, 19, 55, 8, 40, DateTimeKind.Local).AddTicks(1862));

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(1752), new DateTime(2024, 7, 4, 19, 55, 8, 43, DateTimeKind.Local).AddTicks(1761) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 36, DateTimeKind.Local).AddTicks(1760), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(3154) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4679), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4687) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4695), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4697) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4701), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4702) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4705), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4706) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4713), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4715) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4718), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4719) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4722), new DateTime(2024, 7, 4, 19, 55, 8, 37, DateTimeKind.Local).AddTicks(4723) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(3941), new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(3949) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(6006), new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(6012) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7156), new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7162) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7167), new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7169) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7173), new DateTime(2024, 7, 4, 19, 55, 8, 46, DateTimeKind.Local).AddTicks(7174) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 48, DateTimeKind.Local).AddTicks(9370), new DateTime(2024, 7, 4, 19, 55, 8, 48, DateTimeKind.Local).AddTicks(9383) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 47, DateTimeKind.Local).AddTicks(7127), new DateTime(2024, 7, 4, 19, 55, 8, 47, DateTimeKind.Local).AddTicks(7136) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 41, DateTimeKind.Local).AddTicks(2160), new DateTime(2024, 7, 4, 19, 55, 8, 41, DateTimeKind.Local).AddTicks(2168) });

            migrationBuilder.UpdateData(
                table: "VoucherCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(1864), new DateTime(2024, 7, 4, 19, 55, 8, 44, DateTimeKind.Local).AddTicks(1873) });
        }
    }
}
