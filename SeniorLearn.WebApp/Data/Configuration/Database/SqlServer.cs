using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.EntityFrameworkCore.Update;
using SeniorLearn.WebApp.Data.Configuration.Database;

namespace SeniorLearn.WebApp.Data.Configuration.Migrations;



[DbContext(typeof(ApplicationDbContext))]
[Migration("99999999999999_Last1")]
public class Last1 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        Task.Run(() => migrationBuilder.RunSqlScripts()).GetAwaiter().GetResult();
        migrationBuilder.DeleteData(HistoryRepository.DefaultTableName, nameof(HistoryRow.MigrationId), "string", "99999999999999_Last2", null);
        //migrationBuilder.Sql($"DELETE FROM {HistoryRepository.DefaultTableName} WHERE {nameof(HistoryRow.MigrationId)}='99999999999999_Last2'");  // or this way
    }
}

[DbContext(typeof(ApplicationDbContext))]
[Migration("99999999999999_Last2")]
public class Last2 : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        Task.Run(() => migrationBuilder.RunSqlScripts()).GetAwaiter().GetResult();
        migrationBuilder.DeleteData(HistoryRepository.DefaultTableName, nameof(HistoryRow.MigrationId), "string", "99999999999999_Last1", null);
        //migrationBuilder.Sql($"DELETE FROM {HistoryRepository.DefaultTableName} WHERE {nameof(HistoryRow.MigrationId)}='99999999999999_Last1'");  // or this way
    }
}



public class SqlFileOperation : MigrationOperation
{
    public string File { get; }
    public SqlFileOperation(string file)
    {
        File = file;
    }

    public static OperationBuilder<SqlFileOperation> ScriptFile(MigrationBuilder mb, string file)
    {
        var operation = new SqlFileOperation(file); 
        mb.Operations.Add(operation);   
        return new OperationBuilder<SqlFileOperation>(operation);   
    }
}

public class MyMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
{
    public MyMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, 
        ICommandBatchPreparer commandBatchPreparer) : base(dependencies, commandBatchPreparer)
    {
        
    }

    protected override void Generate(MigrationOperation operation, IModel? model, MigrationCommandListBuilder builder)
    {
        if (operation is SqlFileOperation sqlFileOperation)
        {
            Generate(sqlFileOperation, builder);
        }
        else
        {
            base.Generate(operation, model, builder);
        }
    }

    private void Generate(SqlFileOperation operation, MigrationCommandListBuilder builder)
    {
        var sqlHelper = Dependencies.SqlGenerationHelper;
        var stringMapping = Dependencies.TypeMappingSource.FindMapping(typeof(string));
        string script = File.ReadAllText(@$"{Environment.CurrentDirectory}\Data\Configuration\Database\Scripts\{operation.File}");
        builder.AppendLine(script).EndCommand();

    }
}

