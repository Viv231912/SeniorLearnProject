using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Migrations;
using SeniorLearn.WebApp.Data.Configuration.Migrations;

namespace SeniorLearn.WebApp.Data.Configuration.Database
{
    public static class MigrationBuilderSqlExtensions
    {

        public static void RunSqlScripts(this MigrationBuilder mb)
        {
            SqlFileOperation.ScriptFile(mb, "CreateScheduledLessonsView.sql");
        }
    }
}
