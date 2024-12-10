using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class updateVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[] { "Voucher", new DateTime(2024, 12, 9, 9, 48, 9, 298, DateTimeKind.Local).AddTicks(4786), null, false, true, null, false, "Voucher", 34, null, null, "b-admin/Voucher/Index", null });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Voucher");

            migrationBuilder.UpdateData(
                table: "AboutCates",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 961, DateTimeKind.Local).AddTicks(1948), new DateTime(2024, 8, 27, 15, 9, 50, 961, DateTimeKind.Local).AddTicks(1955) });

            migrationBuilder.UpdateData(
                table: "FAQCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 960, DateTimeKind.Local).AddTicks(2618), new DateTime(2024, 8, 27, 15, 9, 50, 960, DateTimeKind.Local).AddTicks(2626) });

            migrationBuilder.UpdateData(
                table: "FeatureCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 960, DateTimeKind.Local).AddTicks(7317), new DateTime(2024, 8, 27, 15, 9, 50, 960, DateTimeKind.Local).AddTicks(7325) });

            migrationBuilder.UpdateData(
                table: "GalleryCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 959, DateTimeKind.Local).AddTicks(2482), new DateTime(2024, 8, 27, 15, 9, 50, 959, DateTimeKind.Local).AddTicks(2490) });

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 956, DateTimeKind.Local).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 956, DateTimeKind.Local).AddTicks(5783));

            migrationBuilder.UpdateData(
                table: "GroupUsers",
                keyColumn: "Code",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 956, DateTimeKind.Local).AddTicks(5791));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "About",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9659));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Advertisement",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Banner",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9686));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Calendar",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Contact",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9693));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ContactList",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Convenience",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Customer",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "FAQ",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Feature",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "GeneralConfiguration",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Group",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "HomePage",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Log",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "News",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Order",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Permit",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Popup",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Product",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "ProductCate",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9677));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Slide",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Translation",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Trash",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Users",
                column: "CreateDate",
                value: new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "NewsCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 958, DateTimeKind.Local).AddTicks(7667), new DateTime(2024, 8, 27, 15, 9, 50, 958, DateTimeKind.Local).AddTicks(7675) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 952, DateTimeKind.Local).AddTicks(1760), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(2260) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 2,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3724), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3730) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 3,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3739), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3740) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 4,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3744), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3745) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 5,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3749), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3750) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 6,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3765), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3766) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 7,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3770), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3771) });

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Pid",
                keyValue: 8,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3774), new DateTime(2024, 8, 27, 15, 9, 50, 953, DateTimeKind.Local).AddTicks(3775) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 961, DateTimeKind.Local).AddTicks(8702), new DateTime(2024, 8, 27, 15, 9, 50, 961, DateTimeKind.Local).AddTicks(8710) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 2L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(412), new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(418) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 3L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1580), new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1586) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 4L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1593), new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1594) });

            migrationBuilder.UpdateData(
                table: "ProductCates",
                keyColumn: "Pid",
                keyValue: 5L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1598), new DateTime(2024, 8, 27, 15, 9, 50, 962, DateTimeKind.Local).AddTicks(1599) });

            migrationBuilder.UpdateData(
                table: "ProductColors",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 964, DateTimeKind.Local).AddTicks(391), new DateTime(2024, 8, 27, 15, 9, 50, 964, DateTimeKind.Local).AddTicks(400) });

            migrationBuilder.UpdateData(
                table: "ProductOptions",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 963, DateTimeKind.Local).AddTicks(852), new DateTime(2024, 8, 27, 15, 9, 50, 963, DateTimeKind.Local).AddTicks(860) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Pid",
                keyValue: 1,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 956, DateTimeKind.Local).AddTicks(9306), new DateTime(2024, 8, 27, 15, 9, 50, 956, DateTimeKind.Local).AddTicks(9314) });

            migrationBuilder.UpdateData(
                table: "VoucherCates",
                keyColumn: "Pid",
                keyValue: 1L,
                columns: new[] { "CreateDate", "LastLogin" },
                values: new object[] { new DateTime(2024, 8, 27, 15, 9, 50, 959, DateTimeKind.Local).AddTicks(7358), new DateTime(2024, 8, 27, 15, 9, 50, 959, DateTimeKind.Local).AddTicks(7366) });
        }
    }
}
