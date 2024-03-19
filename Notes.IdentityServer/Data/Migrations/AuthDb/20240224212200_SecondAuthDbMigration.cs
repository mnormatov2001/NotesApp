using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notes.IdentityServer.Data.Migrations.AuthDb;

/// <inheritdoc />
public partial class SecondAuthDbMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "character varying(128)",
            maxLength: 128,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            table: "Users",
            type: "character varying(128)",
            maxLength: 128,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "LastName",
            table: "Users",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "FirstName",
            table: "Users",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "character varying(128)",
            oldMaxLength: 128,
            oldNullable: true);
    }
}