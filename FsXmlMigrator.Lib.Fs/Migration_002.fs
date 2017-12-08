namespace FsXmlMigrator.Lib.Fs

    open FSharp.Data
    open System.Xml.Linq
    open FsXmlMigrator.Domain.Cs.Repositories
    open Helpers

    module Migration_002 = 

        // old CustomersRepository type declaration based on sample data 
        type CustomersRepository = XmlProvider<"CustomersRepository_001.xml">    

        let migrate () =         

            // load old repository with data using XML type provider
            let oldCustomerRepository = CustomersRepository.Load(CustomersRepository.CustomersRepositoryFilePath)       
            let oldCustomers = oldCustomerRepository.Customers // Strongly typed Customers property!

            // Simple migration - just adding new element 'Address' with no data
            oldCustomers 
            |> Seq.iter (fun c -> (addElement c.XElement "Address" "" ))

            // save the file using new repository definition
            CustomersRepository.Save(oldCustomerRepository.XElement.ToString());       