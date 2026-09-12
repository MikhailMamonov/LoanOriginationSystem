using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanApplicationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "loan_applications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    RequestedTermMonths = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "loan_products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    BaseInterestRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    MinAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MinTermMonths = table.Column<int>(type: "integer", nullable: false),
                    MaxTermMonths = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    fee_origination_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    fee_early_repayment_percent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    fee_insurance_required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    eligibility_min_age = table.Column<int>(type: "integer", nullable: false),
                    eligibility_max_age = table.Column<int>(type: "integer", nullable: false),
                    eligibility_min_income = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    eligibility_min_work_experience_months = table.Column<int>(type: "integer", nullable: false),
                    eligibility_citizenship_required = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "RU"),
                    eligibility_max_debt_to_income_ratio = table.Column<decimal>(type: "numeric(3,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_loan_products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "application_timeline",
                columns: table => new
                {
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id1 = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FromStatus = table.Column<string>(type: "text", nullable: true),
                    ToStatus = table.Column<string>(type: "text", nullable: false),
                    ChangedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_timeline", x => new { x.ApplicationId, x.Id1 });
                    table.ForeignKey(
                        name: "FK_application_timeline_loan_applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "loan_applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_loan_products_Code",
                table: "loan_products",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_timeline");

            migrationBuilder.DropTable(
                name: "loan_products");

            migrationBuilder.DropTable(
                name: "loan_applications");
        }
    }
}
