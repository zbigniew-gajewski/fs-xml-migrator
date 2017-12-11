namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public abstract class Repository<T>
    {
        public static string DatabasePath => "..\\..\\..\\Database";

        public abstract string FilePath { get; }

        public abstract Action InitializationFunction { get; }

        public abstract Action<object> AfterLoadFunction { get; }

        public void Initialize()
        {
            DeleteExistingRepository();
            InitializationFunction();
        }

        public void Load()
        {
            var repositoryStringFromFile = File.ReadAllText(FilePath);
            var serializer = new XmlSerializer(typeof(T));
            using (var fileStream = new StringReader(repositoryStringFromFile))
            {
                var result = serializer.Deserialize(fileStream);
                AfterLoadFunction(result);
            }
        }

        public void Save()
        {
            DeleteExistingRepository();

            var serializer = new XmlSerializer(typeof(T));
            using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this);
            }
        }

        public void Save(string newRepositoryString)
        {
            DeleteExistingRepository();
            File.WriteAllText(FilePath, newRepositoryString);
        }

        private void DeleteExistingRepository()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
