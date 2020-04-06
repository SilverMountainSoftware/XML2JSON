using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XML2JSON.Core;
using XML2JSON.Parsing.XsdToObject;

namespace XML2JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            string progName = AppDomain.CurrentDomain.FriendlyName;

            Console.WriteLine(progName);

            if (args.Length < 3)
            {
                Console.WriteLine($"Usage:\n {progName} input.xml input.xsd output.json");
                Environment.Exit(-1);
            }

            Console.WriteLine("Running {0}...", progName);

            string inputXml;
            string inputXsd;
            string outputJson;

            inputXml = args[0];
            inputXsd = args[1];
            outputJson = args[2];

            Console.WriteLine("input xml: {0}", inputXml);
            Console.WriteLine("input of xsd: {0}", inputXsd);
            Console.WriteLine("output json: {0}", outputJson);

            string xml;

            using (var inputFileStream = File.OpenRead(inputXml))
            {
                using (var inputStreamReader = new StreamReader(inputFileStream))
                {
                    xml = inputStreamReader.ReadToEnd();
                }
            }

            var generator = new ClassGenerator();

            using (Stream stream = File.OpenRead(inputXsd))
                generator.Parse(stream);

            var classes = generator.Generate();

            var elements = new HashSet<string>();
            foreach (var item in classes)
            {
                foreach (var element in item.Elements.Where(e => e.IsList))
                {
                    elements.Add(element.XmlName);
                }
            }

            Console.WriteLine("Loaded input xml from '{0}'", inputXml);

            var converter = new Converter();
            var json = converter.ConvertToJson(xml, elements);

            Console.WriteLine("Converted xml to json");

            //save out
            using (var outputFileStream = File.Open(outputJson, FileMode.Create, FileAccess.Write))
            {
                using (var outputStreamWriter = new StreamWriter(outputFileStream))
                {
                    outputStreamWriter.Write(json);
                    outputStreamWriter.Flush();
                    outputStreamWriter.Close();
                }
            }

            Console.WriteLine("Saved output json to '{0}'", outputJson);
        }
    }
}