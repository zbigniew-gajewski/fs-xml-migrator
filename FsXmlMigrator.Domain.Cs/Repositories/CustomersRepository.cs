namespace FsXmlMigrator.Domain.Cs.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FsXmlMigrator.Domain.Cs.Models;

    public class CustomersRepository : Repository<CustomersRepository>
    {
        public List<Customer> Customers { get; set; }

        public CustomersRepository()
        {
            Customers = new List<Customer>();
        }

        public override string FilePath =>
            Path.Combine(Path.GetFullPath(DatabasePath), $"{nameof(Customers)}.xml");

        public override Action InitializationFunction => () => Save(InitialDataString);

        public override Action<object> AfterLoadFunction => ApplyLoadResult;

        public void Add(Customer customer)
        {
            Customers.Add(customer);
        }

        private void ApplyLoadResult(object loadResult)
        {
            if (loadResult is CustomersRepository customerRepository)
            {
                Customers = customerRepository.Customers;
            }
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
