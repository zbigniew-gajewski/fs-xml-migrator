namespace FsXmlMigrator.Lib.Fs

    open FSharp.Data
    open System.Xml.Linq
    open FsXmlMigrator.Domain.Cs.Repositories
    open Helpers

    // Each migration must keep this schema: 'Migration_<next_3_digit_number>'
    module Migration_001 = 

        // old CustomersRepository type declaration based on sample data 
        type CustomersRepository = XmlProvider<"CustomersRepository_000.xml">    

        let migrate () =         

            // load old repository with data using XML type provider
            let actualCustomersRepository = new FsXmlMigrator.Domain.Cs.Repositories.CustomersRepository()
            let oldCustomerRepository = CustomersRepository.Load(actualCustomersRepository.FilePath)       
            let oldCustomers = oldCustomerRepository.Customers // Strongly typed Customers property!

            // Strongly typed access to the old values such as c.Description.
            // Note that there is no 'Description' property in the latest version of Customer class
            // "nameof" does not exists in F# yet, sorry
            oldCustomers 
            |> Seq.iter (fun c -> (renameElement c.XElement "Description" "Desc" c.Description)) // Description is strongly typed!

            // save the file using new repository definition
            actualCustomersRepository.Save(oldCustomerRepository.XElement.ToString());
        