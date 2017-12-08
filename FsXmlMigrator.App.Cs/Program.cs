namespace FsXmlMigrator.App.Cs
{
    using System;
    using FsXmlMigrator.Domain.Cs.Repositories;
    using FsXmlMigrator.Lib.Fs;
    
    public class Program
    {
        static void Main(string[] args)
        {
            // Initialize  databases
            var migrationHistoryRepository = new MigrationHistoryRepository();
            migrationHistoryRepository.Initialize();

            var customersRepository = new CustomersRepository();
            customersRepository.Initialize();

            // Run all migrations from assembly which not exists in 'MigrationHistory.xml' file
            Console.WriteLine("Press any key to finish...");           
            Migrator.migrate();            
            Console.ReadLine();
        }
    }
}
