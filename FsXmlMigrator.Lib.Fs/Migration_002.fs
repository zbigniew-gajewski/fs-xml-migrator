namespace FsXmlMigrator.Lib.Fs

    open FSharp.Data
    open System.Xml.Linq
    open Helpers
    open FsXmlMigrator.Domain.Cs.Repositories

    // Each migration must keep this schema: 'Migration_<next_3_digit_number>'
    module Migration_002 = 

        // old CustomersRepository type declaration based on sample data 
        type CustomersRepository = XmlProvider<"CustomersRepository_001.xml">    

        let migrate () =         

            // load old repository with data using XML type provider
            let actualCustomersRepository = new FsXmlMigrator.Domain.Cs.Repositories.CustomersRepository()
            let oldCustomerRepository = CustomersRepository.Load(actualCustomersRepository.FilePath)       
            let oldCustomers = oldCustomerRepository.Customers // Strongly typed Customers property!

            // Simple migration - just adding new element 'Address' with no data
            oldCustomers 
            |> Seq.iter (fun c -> (addElement c.XElement "Address" "" ))

            // save the file using new repository definition
            actualCustomersRepository.Save(oldCustomerRepository.XElement.ToString());       