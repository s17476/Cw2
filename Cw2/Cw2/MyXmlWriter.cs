using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Cw2
{
    class MyXmlWriter : IFileWriter
    {

        public void write(Uczelnia uczelnia, string outputData)
        {
            var outputFile = new StreamWriter(outputData);
            var serializer = new XmlSerializer(typeof(Uczelnia), new XmlRootAttribute("uczelnia"));
            XmlSerializerNamespaces nsSerializer = new XmlSerializerNamespaces();
            nsSerializer.Add(string.Empty, string.Empty);

            //konfiguracja serializacji
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.NewLineOnAttributes = false;
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.Encoding = System.Text.Encoding.UTF8;

            XmlWriter writer = XmlWriter.Create(outputFile, settings);

            //serializacja
            serializer.Serialize(writer, uczelnia, nsSerializer);
        }
    }
}
