namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    public abstract class Repository
    {
        public static string DatabasePath => "..\\..\\..\\Database";

        public void Initialize(string filePath, Action initializationFunction)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            initializationFunction();
        }

        public void Load<T>(string filePath, Action<object> loadResultFunction)
        {
            var repositoryStringFromFile = File.ReadAllText(filePath);
            var serializer = new XmlSerializer(typeof(T));
            using (var fileStream = new StringReader(repositoryStringFromFile))
            {
                var result = serializer.Deserialize(fileStream);
                loadResultFunction(result);
            }
        }

        public void Save<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var serializer = new XmlSerializer(typeof(MigrationHistoryRepository));
            using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this);
            }
        }
    }
}
