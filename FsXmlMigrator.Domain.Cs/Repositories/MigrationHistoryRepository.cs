namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using System.Linq;
    using FsXmlMigrator.Domain.Cs.Models;

    public class MigrationHistoryRepository : Repository
    {
        public List<MigrationHistory> MigrationHistory { get; set; }

        public MigrationHistoryRepository()
        {
            MigrationHistory = new List<MigrationHistory>();
        }

        public static string MigrationHistoryFilePath =>
            Path.Combine(Path.GetFullPath(DatabasePath), $"{nameof(MigrationHistory)}.xml");

        public void Initialize()
        {
            if (File.Exists(MigrationHistoryFilePath))
            {
                File.Delete(MigrationHistoryFilePath);
            }

            MigrationHistory.Add(new MigrationHistory { Id = Guid.NewGuid(), Name = "Migration_000" });
            Save();
        }

        public void Load()
        {
            var repositoryStringFromFile = File.ReadAllText(MigrationHistoryFilePath);
            var serializer = new XmlSerializer(typeof(MigrationHistoryRepository));
            using (var fileStream = new StringReader(repositoryStringFromFile))
            {
                if (serializer.Deserialize(fileStream) is MigrationHistoryRepository result)
                {
                    MigrationHistory = result.MigrationHistory;
                }
            }
        }

        public void Add(MigrationHistory migrationHistory)
        {
            MigrationHistory.Add(migrationHistory);
        }

        public void AddNextMigration()
        {
            var nextMigration = new MigrationHistory { Id = Guid.NewGuid(), Name = GetNextMigrationName() };
            MigrationHistory.Add(nextMigration);
        }

        public void Save()
        {
            if (File.Exists(MigrationHistoryFilePath))
            {
                File.Delete(MigrationHistoryFilePath);
            }

            var serializer = new XmlSerializer(typeof(MigrationHistoryRepository));
            using (var fileStream = new FileStream(MigrationHistoryFilePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this);
            }
        }

        public string GetNextMigrationName()
        {
            var migrationName = "Migration_";
            var migrationZeroNumber = "000";
            var zeroMigrationName = $"{migrationName}{migrationZeroNumber}";

            var lastMigrationName = MigrationHistory.OrderBy(m => m.Name).LastOrDefault()?.Name ?? zeroMigrationName;
            var stringNumber = lastMigrationName.Substring(10, 3);
            var intNumber = 0;
            var parsingResult = Int32.TryParse(stringNumber, out intNumber);
            if (parsingResult)
            {
                return $"{migrationName}{(intNumber + 1).ToString(migrationZeroNumber)}";
            }

            return $"{migrationName}{migrationZeroNumber}";
        }
    }
}
