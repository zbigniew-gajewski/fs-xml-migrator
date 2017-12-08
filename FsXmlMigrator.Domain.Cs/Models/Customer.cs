namespace FsXmlMigrator.Domain.Cs.Models
{
    using System;

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Address { get; set; }
    }
}
