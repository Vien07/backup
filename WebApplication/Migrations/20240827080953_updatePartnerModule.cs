using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CMS.Migrations
{
    public partial class updatePartnerModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partners",
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
                    table.PrimaryKey("PK_Partners", x => x.Pid);
                });

            migrationBuilder.CreateTable(
                name: "MultiLang_Partners",
                columns: table => new
                {
                    Pid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartnerPid = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LangKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultiLang_Partners", x => x.Pid);
                    table.ForeignKey(
                        name: "FK_MultiLang_Partners_Partners_PartnerPid",
                        column: x => x.PartnerPid,
                        principalTable: "Partners",
                        principalColumn: "Pid",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "Modules",
                columns: new[] { "Code", "CreateDate", "CreateUser", "Deleted", "Enabled", "Icon", "Locked", "ModuleName", "Order", "UpdateDate", "UpdateUser", "Url", "UrlRewrite" },
                values: new object[] { "Partner", new DateTime(2024, 8, 27, 15, 9, 50, 955, DateTimeKind.Local).AddTicks(9672), null, false, true, null, false, "Đối tác (Partner)", 10, null, null, "b-admin/Partner/Index", null });

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

            migrationBuilder.CreateIndex(
                name: "IX_MultiLang_Partners_PartnerPid",
                table: "MultiLang_Partners",
                column: "PartnerPid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MultiLang_Partners");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DeleteData(
                table: "Modules",
                keyColumn: "Code",
                keyValue: "Partner");

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
    }
}
