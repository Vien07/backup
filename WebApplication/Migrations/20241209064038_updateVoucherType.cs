using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class updateVoucherType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoucherType",
                table: "VoucherDetails",
                type: "nvarchar(max)",
                nullable: true);

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

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Voucher",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 13, 40, 36, 313, DateTimeKind.Local).AddTicks(893));

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

            migrationBuilder.UpdateData(
                table: "VoucherCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(8533), new DateTime(2024, 12, 9, 13, 40, 36, 316, DateTimeKind.Local).AddTicks(8541) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VoucherType",
                table: "VoucherDetails");

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 303, DateTimeKind.Local).AddTicks(9816), new DateTime(2024, 12, 9, 9, 48, 9, 303, DateTimeKind.Local).AddTicks(9824) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 302, DateTimeKind.Local).AddTicks(9717), new DateTime(2024, 12, 9, 9, 48, 9, 302, DateTimeKind.Local).AddTicks(9725) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 303, DateTimeKind.Local).AddTicks(4878), new DateTime(2024, 12, 9, 9, 48, 9, 303, DateTimeKind.Local).AddTicks(4885) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 301, DateTimeKind.Local).AddTicks(9440), new DateTime(2024, 12, 9, 9, 48, 9, 301, DateTimeKind.Local).AddTicks(9448) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(9043));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 299, DateTimeKind.Local).AddTicks(1292));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 299, DateTimeKind.Local).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4746));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4795));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4793));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4799));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4802));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4788));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4781));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4807));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4812));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4780));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4785));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4792));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Voucher",
                column: "CreateDate",
                value: new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4786));

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 301, DateTimeKind.Local).AddTicks(4421), new DateTime(2024, 12, 9, 9, 48, 9, 301, DateTimeKind.Local).AddTicks(4429) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 293, DateTimeKind.Local).AddTicks(9447), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(2929) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4442), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4451) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4458), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4460) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4464), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4465) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4468), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4469) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4474), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4476) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4478), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4479) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4482), new DateTime(2024, 12, 9, 9, 48, 9, 295, DateTimeKind.Local).AddTicks(4483) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(6830), new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(6838) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(8590), new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(8597) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9810), new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9816) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9821), new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9822) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9826), new DateTime(2024, 12, 9, 9, 48, 9, 304, DateTimeKind.Local).AddTicks(9828) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 306, DateTimeKind.Local).AddTicks(8939), new DateTime(2024, 12, 9, 9, 48, 9, 306, DateTimeKind.Local).AddTicks(8947) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 305, DateTimeKind.Local).AddTicks(9292), new DateTime(2024, 12, 9, 9, 48, 9, 305, DateTimeKind.Local).AddTicks(9300) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 299, DateTimeKind.Local).AddTicks(4870), new DateTime(2024, 12, 9, 9, 48, 9, 299, DateTimeKind.Local).AddTicks(4879) });

            migrationBuilder.UpdateData(
                table: "VoucherCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 48, 9, 302, DateTimeKind.Local).AddTicks(4652), new DateTime(2024, 12, 9, 9, 48, 9, 302, DateTimeKind.Local).AddTicks(4660) });
        }
    }
}
