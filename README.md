# fs-xml-migrator
XML file migration concept using F# XML type provider.

# Motivation
Sometimes it is enough to store data in a simple XML file by serializing an object instead of using 'professional' database (especially when concurrent access is not required and when the database is not very big). The problem appears, when after changing the model in our code, an XML file (our database) remains 'old' (corresponds to previous object definition). For example we have changed 'Description' property to 'Desc' of the Customer class or we have added 'Address' property to our Customer class. We can't deserialize old XML file using new model, because there is no 'Description' neither 'Address' properties in our current model. In order to make our old XML data working with new model we have to migrate XML file. There are many approaches allowing to achieve this goal, one of which is the usage of FSharp XML Type Provider.

This project shows a very simple concept of migrating existing 'XML database' based on migration concept widely used in EntityFramework Code First (which is based on '__MigrationHistory' table) and FSharp XML Type Provider.

* FsXmlMigration.Domain.Cs - models and repositories (serializers)
* FsXmlMigration.Lib.Fs - migrator which uses FSharp XML Type Provider to read and manipulate 'old' XML data in a strongly typed way
* FsXmlMigration.App.Cs - simple C# app which initializes database and executes migrations

# Prerequisites
- .NET Framework 4.6.1
- Visual Studio 2017 15.4.1 (or above)

# Usage
1. Set breakpoint in Migrator.fs file (line 29)
2. Debug FsXmlMigrationApp.Cs app
3. Observe 'Database\Customers.xml' and 'Database\MigrationHistory.xml' changes each time breakpoint stops the app



