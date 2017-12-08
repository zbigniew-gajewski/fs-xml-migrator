namespace FsXmlMigrator.Lib.Fs

    module Migrator =

        open System
        open System.Reflection
        open FsXmlMigrator.Domain.Cs.Repositories

        let migrate () : unit =
       
            let migrationHistoryRepository = new MigrationHistoryRepository()
            migrationHistoryRepository.Load()
                        
            let migrationsFromAssembly = 
                Assembly.GetExecutingAssembly().GetTypes() 
                |> Seq.filter (fun t -> t.Name.StartsWith("Migration_"))

            let migrationExists (migrationHistoryName : string) = 
                migrationHistoryRepository.MigrationHistory.Exists(fun m -> m.Name = migrationHistoryName) 

            let migrate (migrationFromAssembly : Type) = 
                migrationFromAssembly.GetMethod("migrate").Invoke(null, null) |> ignore
                migrationHistoryRepository.AddNextMigration()
                migrationHistoryRepository.Save()               
            
            let migrateWnenNotExists (migrationFromAssembly : Type) = 
                if not(migrationExists migrationFromAssembly.Name) then migrate migrationFromAssembly

            migrationsFromAssembly
                |> Seq.sortBy (fun t -> t.Name)
                |> Seq.iter (fun m -> migrateWnenNotExists m)

            ()        