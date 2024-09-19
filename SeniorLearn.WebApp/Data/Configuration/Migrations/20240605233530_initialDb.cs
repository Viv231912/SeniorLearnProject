using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SeniorLearn.WebApp.Data.Configuration.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "org");

            migrationBuilder.EnsureSchema(
                name: "timetable");

            migrationBuilder.EnsureSchema(
                name: "finance");

            migrationBuilder.CreateSequence(
                name: "PaymentSequence",
                schema: "org");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organisations",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimetableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "org",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "org",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "org",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "org",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "org",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRolesTypes",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRolesTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRolesTypes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OutstandingFees = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "org",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "org",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Timetables",
                schema: "timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timetables_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "org",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganisationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topics_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalSchema: "org",
                        principalTable: "Organisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Honorary",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Honorary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Honorary_UserRolesTypes_Id",
                        column: x => x.Id,
                        principalSchema: "org",
                        principalTable: "UserRolesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professional",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Discipline = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professional", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professional_UserRolesTypes_Id",
                        column: x => x.Id,
                        principalSchema: "org",
                        principalTable: "UserRolesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleUpdates",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RenewalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserRoleTypeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUpdates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleUpdates_UserRolesTypes_UserRoleTypeId",
                        column: x => x.UserRoleTypeId,
                        principalSchema: "org",
                        principalTable: "UserRolesTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Standard",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standard_UserRolesTypes_Id",
                        column: x => x.Id,
                        principalSchema: "org",
                        principalTable: "UserRolesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsCash",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [org].[PaymentSequence]"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsCash", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsCash_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "org",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsCheque",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [org].[PaymentSequence]"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cleared = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsCheque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsCheque_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "org",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsCreditCard",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [org].[PaymentSequence]"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CardIssuer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AuthorisationNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsCreditCard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsCreditCard_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "org",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentsElectronicFundTransfer",
                schema: "finance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [org].[PaymentSequence]"),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentsElectronicFundTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentsElectronicFundTransfer_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "org",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryPatterns",
                schema: "timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfessionalId = table.Column<int>(type: "int", nullable: false),
                    DeliveryMode = table.Column<int>(type: "int", nullable: false),
                    IsCourse = table.Column<bool>(type: "bit", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    EndStrategyId = table.Column<int>(type: "int", nullable: true),
                    Occurrences = table.Column<int>(type: "int", nullable: true),
                    EndOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sunday = table.Column<bool>(type: "bit", nullable: true),
                    Monday = table.Column<bool>(type: "bit", nullable: true),
                    Tuesday = table.Column<bool>(type: "bit", nullable: true),
                    Wednesday = table.Column<bool>(type: "bit", nullable: true),
                    Thursday = table.Column<bool>(type: "bit", nullable: true),
                    Friday = table.Column<bool>(type: "bit", nullable: true),
                    Saturday = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryPatterns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryPatterns_Professional_ProfessionalId",
                        column: x => x.ProfessionalId,
                        principalSchema: "org",
                        principalTable: "Professional",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                schema: "timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClassDurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    TimetableId = table.Column<int>(type: "int", nullable: false),
                    DeliveryPatternId = table.Column<int>(type: "int", nullable: false),
                    DeliveryMode = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                    table.CheckConstraint("CK_Lessons_StatusId", "StatusId >= 1 AND StatusId <= 5");
                    table.ForeignKey(
                        name: "FK_Lessons_DeliveryPatterns_DeliveryPatternId",
                        column: x => x.DeliveryPatternId,
                        principalSchema: "timetable",
                        principalTable: "DeliveryPatterns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lessons_Timetables_TimetableId",
                        column: x => x.TimetableId,
                        principalSchema: "timetable",
                        principalTable: "Timetables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lessons_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "timetable",
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Enrolments",
                schema: "timetable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrolments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enrolments_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalSchema: "timetable",
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enrolments_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "org",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "org",
                table: "Organisations",
                columns: new[] { "Id", "Name", "TimetableId" },
                values: new object[] { 1, "SeniorLearn", 1 });

            migrationBuilder.InsertData(
                schema: "timetable",
                table: "Timetables",
                columns: new[] { "Id", "OrganisationId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                schema: "timetable",
                table: "Topics",
                columns: new[] { "Id", "Description", "Name", "OrganisationId" },
                values: new object[,]
                {
                    { 1, "Philosophy is the study of fundamental questions related to existence, knowledge, values, reason, and language through critical, analytical, and systematic approaches.", "Philosophy", 1 },
                    { 2, "Mathematics involves the study of quantity, structure, space, and change, forming the foundation for understanding abstract concepts and physical phenomena.", "Mathematics", 1 },
                    { 3, "History examines past events to understand human societies, analyzing documents and artifacts to interpret societal development and change.", "History", 1 },
                    { 4, "Biology is the science of life, studying living organisms, their structure, function, growth, origin, evolution, and distribution.", "Biology", 1 },
                    { 5, "Computer Science focuses on the theory, design, and application of computer systems, covering areas such as algorithms, computation, and data processing.", "Computer Science", 1 },
                    { 6, "Physics studies matter, energy, and the fundamental forces of the universe, seeking to understand the laws governing physical phenomena.", "Physics", 1 },
                    { 7, "Chemistry investigates the properties and behavior of matter, exploring how substances interact with energy and undergo changes.", "Chemistry", 1 },
                    { 8, "Economics analyzes how individuals and societies allocate resources to satisfy needs and wants, examining the consequences of those decisions.", "Economics", 1 },
                    { 9, "English Literature explores written works in the English language, emphasizing literary analysis and interpretation across genres.", "English Literature", 1 },
                    { 10, "Environmental Science studies the interactions between the environment's physical, chemical, and biological components.", "Environmental Science", 1 },
                    { 11, "Art History examines the development of visual arts across cultures and periods, including painting, sculpture, and architecture.", "Art History", 1 },
                    { 12, "Political Science studies government systems, political behavior, and the theoretical and practical aspects of politics.", "Political Science", 1 },
                    { 13, "Psychology is the scientific study of the mind and behavior, exploring how humans perceive, think, feel, and act.", "Psychology", 1 },
                    { 14, "Sociology investigates human social behavior, including the development, structure, and functioning of human society.", "Sociology", 1 },
                    { 15, "Music Theory analyzes the elements of music, including harmony, melody, rhythm, and form, to understand musical composition.", "Music Theory", 1 },
                    { 16, "Philosophy of Science examines the foundations, methods, and implications of science, questioning the nature of scientific knowledge.", "Philosophy of Science", 1 },
                    { 17, "World Religions explores various global belief systems, their teachings, practices, and impacts on societies.", "World Religions", 1 },
                    { 18, "Anthropology studies humans, their ancestors, and related primates, focusing on cultural, social, and physical aspects.", "Anthropology", 1 },
                    { 19, "Linguistics studies language structure, development, and variation, including syntax, semantics, phonetics, and phonology.", "Linguistics", 1 },
                    { 20, "Creative Writing focuses on the art of writing fiction and poetry, emphasizing creativity, narrative techniques, and expression.", "Creative Writing", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "org",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "org",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "org",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "org",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "org",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "org",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "org",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryPatterns_ProfessionalId",
                schema: "timetable",
                table: "DeliveryPatterns",
                column: "ProfessionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_LessonId",
                schema: "timetable",
                table: "Enrolments",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrolments_MemberId",
                schema: "timetable",
                table: "Enrolments",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_DeliveryPatternId",
                schema: "timetable",
                table: "Lessons",
                column: "DeliveryPatternId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Start",
                schema: "timetable",
                table: "Lessons",
                column: "Start");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TimetableId",
                schema: "timetable",
                table: "Lessons",
                column: "TimetableId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_TopicId",
                schema: "timetable",
                table: "Lessons",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_OrganisationId",
                schema: "org",
                table: "Members",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                schema: "org",
                table: "Members",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsCash_MemberId",
                schema: "finance",
                table: "PaymentsCash",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsCheque_MemberId",
                schema: "finance",
                table: "PaymentsCheque",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsCreditCard_MemberId",
                schema: "finance",
                table: "PaymentsCreditCard",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentsElectronicFundTransfer_MemberId",
                schema: "finance",
                table: "PaymentsElectronicFundTransfer",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleUpdates_UserRoleTypeId",
                schema: "org",
                table: "RoleUpdates",
                column: "UserRoleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_OrganisationId",
                schema: "timetable",
                table: "Timetables",
                column: "OrganisationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_OrganisationId",
                schema: "timetable",
                table: "Topics",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRolesTypes_UserId",
                schema: "org",
                table: "UserRolesTypes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "org");

            migrationBuilder.DropTable(
                name: "Enrolments",
                schema: "timetable");

            migrationBuilder.DropTable(
                name: "Honorary",
                schema: "org");

            migrationBuilder.DropTable(
                name: "PaymentsCash",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PaymentsCheque",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PaymentsCreditCard",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "PaymentsElectronicFundTransfer",
                schema: "finance");

            migrationBuilder.DropTable(
                name: "RoleUpdates",
                schema: "org");

            migrationBuilder.DropTable(
                name: "Standard",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "org");

            migrationBuilder.DropTable(
                name: "Lessons",
                schema: "timetable");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "org");

            migrationBuilder.DropTable(
                name: "DeliveryPatterns",
                schema: "timetable");

            migrationBuilder.DropTable(
                name: "Timetables",
                schema: "timetable");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "timetable");

            migrationBuilder.DropTable(
                name: "Professional",
                schema: "org");

            migrationBuilder.DropTable(
                name: "Organisations",
                schema: "org");

            migrationBuilder.DropTable(
                name: "UserRolesTypes",
                schema: "org");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "org");

            migrationBuilder.DropSequence(
                name: "PaymentSequence",
                schema: "org");
        }
    }
}
