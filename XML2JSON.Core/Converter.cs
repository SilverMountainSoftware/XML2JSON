using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace XML2JSON.Core
{
    /// <summary>
    /// Class to handle xml -> json conversion
    /// </summary>
    public class Converter
    {

        /// <summary>
        /// async version of conversion function
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public async Task<string> ConvertToJsonAsync(string xml, List<string> elementsToMakeArrays)
        {
            return await Task<string>.Factory.StartNew(() => ConvertToJson(xml, elementsToMakeArrays));
        }

        /// <summary>
        /// converts xml string to json string
        /// </summary>
        /// <param name="xml">xml data as string</param>
        /// <returns>json data as string</returns>
        public string ConvertToJson(string xml, ICollection<string> elements)
        {
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            //strip comments from xml
            var comments = doc.SelectNodes("//comment()");

            if (comments != null)
            {
                foreach (var node in comments.Cast<XmlNode>())
                {
                    if (node.ParentNode != null)
                        node.ParentNode.RemoveChild(node);
                }
            }

            var rawJsonText = JsonConvert.SerializeXmlNode(doc.DocumentElement, Formatting.Indented);

            //strip the @ and # characters
            var strippedJsonText = Regex.Replace(rawJsonText, "(?<=\")(@)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "(?<=\")(#)(?!.*\":\\s )", string.Empty, RegexOptions.IgnoreCase);

            // unquote numbers and booleans
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"([\\d\\.]+)\\\"", "$1", RegexOptions.IgnoreCase);
            strippedJsonText = Regex.Replace(strippedJsonText, "\\\"(true|false)\\\"", "$1", RegexOptions.IgnoreCase);

            strippedJsonText = FixArrayElements(elements, strippedJsonText);

            return strippedJsonText;
        }

        private JToken MakeElementArray(JToken jToken)
        {
            List<object> list = new List<object>();

            if (jToken is JArray)
            {
                list = jToken.ToObject<List<object>>();
            }
            else if (jToken is JObject)
            {
                list.Add(jToken.ToObject<object>());
            }

            return JToken.FromObject(list);
        }

        private JToken FixOneElement(string element, JToken jTokenMain)
        {
            var tryGood = true;
            try
            {
                JToken jToken = jTokenMain[element];
                if (jToken == null)
                {
                    tryGood = false;
                }
            }
            catch (System.Exception)
            {
                tryGood = false;
            }

            if (tryGood)
            {
                jTokenMain[element] = MakeElementArray(jTokenMain[element]);
            }
            else
            {
                var first = jTokenMain.First;
                do
                {
                    foreach (var child in first.Children().ToList())
                    {
                       var newChild = FixOneElement(element, child);
                       child.Replace(newChild);
                    }

                    first = first.Next;
                } while (first != null);

            }

            return jTokenMain;
        }


        private string FixArrayElements(ICollection<string> elements, string json)
        {
            JToken jTokenMain = JToken.Parse(json);

            foreach (var element in elements)
            {
                jTokenMain = FixOneElement(element, jTokenMain);
            }

            return jTokenMain.ToString();
        }
    }
}
