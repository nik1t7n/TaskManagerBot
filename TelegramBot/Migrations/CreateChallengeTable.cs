﻿using FluentMigrator;
using Microsoft.Extensions.DependencyInjection;

[Migration(1)] // Укажите уникальный номер миграции
public class CreateChallengeTable : Migration
{
    public override void Up()
    {
        Create.Table("Challenge")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Description").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable()
            .WithColumn("Deadline").AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Challenge");
    }

    //public static IServiceProvider CreateServices()
    //{
    //    return new ServiceCollection()
    //        .AddFluentMigratorCore()
    //        .ConfigureRunner(rb => rb
    //            .AddSqlServer() // Используйте AddSqlServer для SQL Server
    //            .WithGlobalConnectionString("Server=(local);Database=ChallengeManagerBotDb;Trusted_Connection=true;TrustServerCertificate=true;") // Замените на свою строку подключения
    //            .ScanIn(typeof(CreateChallengeTable).Assembly).For.Migrations()) // Указываем FluentMigrator, где искать миграции
    //        .AddLogging(lb => lb.AddFluentMigratorConsole())
    //        .BuildServiceProvider(false);
    //}
}
