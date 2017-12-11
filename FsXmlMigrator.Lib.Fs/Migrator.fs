namespace FsXmlMigrator.Lib.Fs

    module Migrator =

        open System
        open System.Reflection
        open FsXmlMigrator.Domain.Cs.Repositories

        let migrate () : unit =
       
            // get migrations from file
            let migrationsFromFile = new MigrationHistoryRepository()
            migrationsFromFile.Load()
                                             
            // helper - checks whether next migration from assembly already exists in the file
            let migrationExistsInRepository (migrationName : string) = 
                migrationsFromFile.MigrationHistory.Exists(fun m -> m.Name = migrationName) 

            // migration execution (e.g. Migration_001.migrate(), then Migration_002.migrate())
            let migrate (migrateMethodInfo : MethodInfo) = 
                migrateMethodInfo.Invoke(null, null) |> ignore
                migrationsFromFile.AddNextMigration()
                migrationsFromFile.Save()               
            
            // helper - checks if migration exists in file - if not, then executes the migration
            let migrateIfNotYetDone (migrationFromAssembly : Type) = 
                if not(migrationExistsInRepository migrationFromAssembly.Name) then 
                    // breakpoint here:
                    migrate (migrationFromAssembly.GetMethod("migrate"))
             
            // get migrations from assembly, check if exists in migration history file and execute
            Assembly.GetExecutingAssembly().GetTypes() 
                |> Seq.filter (fun m -> m.Name.StartsWith("Migration_"))
                |> Seq.sortBy (fun m -> m.Name)
                |> Seq.iter migrateIfNotYetDone

            ()