using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VolunteerCenterMVCProject.Data.Migrations
{
	public partial class Init : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "FirstName",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<string>(
				name: "LastName",
				table: "AspNetUsers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "");

			migrationBuilder.CreateTable(
				name: "Categories",
				columns: table => new
				{
					CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Categories", x => x.CategoryId);
				});

			migrationBuilder.CreateTable(
				name: "Locations",
				columns: table => new
				{
					LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
					City = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Locations", x => x.LocationId);
				});

			migrationBuilder.CreateTable(
				name: "Events",
				columns: table => new
				{
					EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
					LocationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Budget = table.Column<double>(type: "float", nullable: false),
					CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					CreatedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Events", x => x.EventId);
					table.ForeignKey(
						name: "FK_Events_AspNetUsers_CreatedBy",
						column: x => x.CreatedBy,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Events_Categories_CategoryId",
						column: x => x.CategoryId,
						principalTable: "Categories",
						principalColumn: "CategoryId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Events_Locations_LocationId",
						column: x => x.LocationId,
						principalTable: "Locations",
						principalColumn: "LocationId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Signups",
				columns: table => new
				{
					SignupId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					VolunteerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
					SignupDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Signups", x => x.SignupId);
					table.ForeignKey(
						name: "FK_Signups_AspNetUsers_VolunteerId",
						column: x => x.VolunteerId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Signups_Events_EventId",
						column: x => x.EventId,
						principalTable: "Events",
						principalColumn: "EventId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "statusHistories",
				columns: table => new
				{
					StatusHistoryId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					EventId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ChangedBy = table.Column<string>(type: "nvarchar(450)", nullable: false),
					NewStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
					ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_statusHistories", x => x.StatusHistoryId);
					table.ForeignKey(
						name: "FK_statusHistories_AspNetUsers_ChangedBy",
						column: x => x.ChangedBy,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_statusHistories_Events_EventId",
						column: x => x.EventId,
						principalTable: "Events",
						principalColumn: "EventId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Events_CategoryId",
				table: "Events",
				column: "CategoryId");

			migrationBuilder.CreateIndex(
				name: "IX_Events_CreatedBy",
				table: "Events",
				column: "CreatedBy");

			migrationBuilder.CreateIndex(
				name: "IX_Events_LocationId",
				table: "Events",
				column: "LocationId");

			migrationBuilder.CreateIndex(
				name: "IX_Signups_EventId",
				table: "Signups",
				column: "EventId");

			migrationBuilder.CreateIndex(
				name: "IX_Signups_VolunteerId",
				table: "Signups",
				column: "VolunteerId");

			migrationBuilder.CreateIndex(
				name: "IX_statusHistories_ChangedBy",
				table: "statusHistories",
				column: "ChangedBy");

			migrationBuilder.CreateIndex(
				name: "IX_statusHistories_EventId",
				table: "statusHistories",
				column: "EventId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Signups");

			migrationBuilder.DropTable(
				name: "statusHistories");

			migrationBuilder.DropTable(
				name: "Events");

			migrationBuilder.DropTable(
				name: "Categories");

			migrationBuilder.DropTable(
				name: "Locations");

			migrationBuilder.DropColumn(
				name: "FirstName",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "LastName",
				table: "AspNetUsers");
		}
	}
}