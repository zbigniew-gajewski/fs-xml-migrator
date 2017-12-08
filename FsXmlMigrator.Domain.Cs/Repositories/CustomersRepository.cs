namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;
    using FsXmlMigrator.Domain.Cs.Models;

    public class CustomersRepository : Repository
    {
        public List<Customer> Customers { get; set; }

        public CustomersRepository()
        {
            Customers = new List<Customer>();
        }

        public static string CustomersRepositoryFilePath =>
            Path.Combine(Path.GetFullPath(DatabasePath), $"{nameof(CustomersRepository)}.xml");


        public void Initialize()
        {
            if (File.Exists(CustomersRepositoryFilePath))
            {
                File.Delete(CustomersRepositoryFilePath);
            }

            Save(InitialDataString);
        }

        public void Load()
        {
            var repositoryStringFromFile = File.ReadAllText(CustomersRepositoryFilePath);
            var serializer = new XmlSerializer(typeof(CustomersRepository));
            using (var fileStream = new StringReader(repositoryStringFromFile))
            {
                if (serializer.Deserialize(fileStream) is CustomersRepository result)
                {
                    Customers = result.Customers;
                }
            }
        }

        public void Add(Customer customer)
        {
            Customers.Add(customer);
        }

        public void Save()
        {
            if (File.Exists(CustomersRepositoryFilePath))
            {
                File.Delete(CustomersRepositoryFilePath);
            }

            var serializer = new XmlSerializer(typeof(CustomersRepository));
            using (var fileStream = new FileStream(CustomersRepositoryFilePath, FileMode.OpenOrCreate))
            {
                serializer.Serialize(fileStream, this);
            }
        }

        public static void Save(string newRepositoryString)
        {
            if (File.Exists(CustomersRepositoryFilePath))
            {
                File.Delete(CustomersRepositoryFilePath);
            }

            File.WriteAllText(CustomersRepositoryFilePath, newRepositoryString);
        }       
     
        private string InitialDataString =>
@"<CustomersRepository xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <Customers>
        <Customer>
            <Id>8f657418-0b32-4da7-a579-5c1f1caeaaa3</Id>
            <Name>Customer1</Name>
            <Description>Customer 1 Description</Description>
        </Customer>
        <Customer>
            <Id>1e5aec35-9e2a-4e45-89c2-eacd4d09d44f</Id>
            <Name>Customer2</Name>
            <Description>Customer 2 Description</Description>
        </Customer>
        <Customer>
            <Id>4f20553c-04fa-4513-81f8-b6dc7f87f778</Id>
            <Name>Customer3</Name>
            <Description>Customer 3 Description</Description>
        </Customer>
    </Customers>
</CustomersRepository>";
    }
}
