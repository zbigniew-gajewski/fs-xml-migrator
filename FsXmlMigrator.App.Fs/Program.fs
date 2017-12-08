open System;
open FsXmlMigrator.Domain.Cs.Repositories;
open FsXmlMigrator.Lib.Fs;   

[<EntryPoint>]
let main argv = 

    // Initialize  databases
    let migrationHistoryRepository = new MigrationHistoryRepository()
    migrationHistoryRepository.Initialize()

    let customersRepository = new CustomersRepository()
    customersRepository.Initialize()

    // Run all migrations from assembly which not exists in 'MigrationHistory.xml' file
    Console.WriteLine("Press any key to finish...")           
    Migrator.migrate()            
    Console.ReadLine() |> ignore

    0 // return an integer exit code
