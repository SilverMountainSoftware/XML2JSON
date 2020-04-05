﻿using System.IO;
using XML2JSON.Core;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace XML2JSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().FullName);

            if (args.Length < 2)
            {
                Console.WriteLine("Usage: XML2JSON.exe input.xml output.json [elements.txt]");
                return;
            }

            string inputXml;
            string outputJson;
            var listFile = "";
            var elements = new List<string>();

            inputXml = args[0];
            outputJson = args[1];

            if (args.Length > 2)
            {
                var fileContents = File.ReadAllLines(args[2]);
                elements = new List<string>(fileContents);
            }

            Console.WriteLine("input xml: {0}", inputXml);
            Console.WriteLine("output json: {0}", outputJson);
            Console.WriteLine("list of arrays: {0}", listFile);

            string xml;

            using (var inputFileStream = File.OpenRead(inputXml))
            {
                using (var inputStreamReader = new StreamReader(inputFileStream))
                {
                    xml = inputStreamReader.ReadToEnd();
                }
            }

            Console.WriteLine("Loaded input xml from '{0}'", inputXml);

            var json = Converter.ConvertToJson(xml, elements);

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