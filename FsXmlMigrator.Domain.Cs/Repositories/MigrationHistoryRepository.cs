namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using FsXmlMigrator.Domain.Cs.Models;

    public class MigrationHistoryRepository : Repository<MigrationHistoryRepository>
    {
        public List<MigrationHistory> MigrationHistory { get; set; }

        public MigrationHistoryRepository()
        {
            MigrationHistory = new List<MigrationHistory>();
        }

        public override string FilePath =>
            Path.Combine(Path.GetFullPath(DatabasePath), $"{nameof(MigrationHistory)}.xml");

        public override Action InitializationFunction => AddFirstMigration;

        public override Action<object> AfterLoadFunction => ApplyLoadResult;

     
        public void Add(MigrationHistory migrationHistory)
        {
            MigrationHistory.Add(migrationHistory);
        }

        public void AddNextMigration()
        {
            var nextMigration = new MigrationHistory { Id = Guid.NewGuid(), Name = GetNextMigrationName() };
            MigrationHistory.Add(nextMigration);
        }

        private void AddFirstMigration()
        {
            MigrationHistory.Add(new MigrationHistory { Id = Guid.NewGuid(), Name = GetNextMigrationName() });
            Save();
        }

        public string GetNextMigrationName()
        {
            var migrationName = "Migration_";
            var migrationZeroNumber = "000";
            var zeroMigrationName = $"{migrationName}{migrationZeroNumber}";

            if (MigrationHistory.Any())
            {
                var lastMigrationName = MigrationHistory.OrderBy(m => m.Name).LastOrDefault()?.Name ?? zeroMigrationName;
                var stringNumber = lastMigrationName.Substring(10, 3);
                var intNumber = 0;
                var parsingResult = Int32.TryParse(stringNumber, out intNumber);
                if (parsingResult)
                {
                    return $"{migrationName}{(intNumber + 1).ToString(migrationZeroNumber)}";
                }
            }

            return $"{migrationName}{migrationZeroNumber}";

        }

        private void ApplyLoadResult(object loadResult)
        {
            if (loadResult is MigrationHistoryRepository migrationHistoryRepository)
            {
                MigrationHistory = migrationHistoryRepository.MigrationHistory;
            }
        }
    }
}
