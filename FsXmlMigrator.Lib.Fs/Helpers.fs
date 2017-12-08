namespace FsXmlMigrator.Lib.Fs

    module Helpers =

        open System.Xml.Linq

        let addElement (xElement : XElement) (elementName : string ) (value : string) = 
            let newXElementToAdd = new XElement(XName.Get elementName, value)
            xElement.Add(newXElementToAdd)
            
        let removeElement (xElement : XElement) (elementName : string ) = 
            let elementToRemove = xElement.Elements(XName.Get elementName)
            elementToRemove.Remove()

        let renameElement (xElement : XElement) (oldElementName : string ) (newElementName : string ) (value : string) = 
            addElement xElement newElementName value
            removeElement xElement oldElementName
