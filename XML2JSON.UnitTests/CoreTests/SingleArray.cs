using System.Collections.Generic;
using System.Threading.Tasks;
using XML2JSON.Core;
using Xunit;

namespace XML2JSON.UnitTests
{
    public class SingleArray
    {
        private const string xml1 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<!-- catalog comment -->
<catalog>
   <book id = ""bk101"">
      <author> Gambardella, Matthew</author>
      <title>XML Developers Guide</title>
      <genre id=""1"">Computer</genre>
      <price>44.95</price>
      <publish_date>2000-10-01</publish_date>
      <description>An in-depth look at creating applications
      with XML.</description>
   </book>
</catalog>";
        private const string xml2 = @"<?xml version=""1.0"" encoding=""utf-8""?>
<catalog>
   <book id = ""bk101"">
      <author> Gambardella, Matthew</author>
      <title>XML Developers Guide</title>
      <genre id=""1"">Computer</genre>
      <genre id=""2"">Help</genre>
      <price>44.95</price>
      <publish_date>2000-10-01</publish_date>
      <description>An in-depth look at creating applications
      with XML.</description>
   </book>
</catalog>";

        [Fact]
        public void TestGenreNotArray()
        {
            const string expectedJson = @"{
  ""catalog"": {
    ""book"": {
                ""id"": ""bk101"",
      ""author"": "" Gambardella, Matthew"",
      ""title"": ""XML Developers Guide"",
      ""genre"": [
        {
          ""id"": 1,
          ""text"": ""Computer""
        }
      ],
      ""price"": 44.95,
      ""publish_date"": ""2000-10-01"",
      ""description"": ""An in-depth look at creating applications\r\n      with XML.""
    }
  }
}";
            var elements = new List<string>
            {
                "genre"
            };

            var converter = new Converter();
            var json = converter.ConvertToJson(xml1, elements);

            Assert.Equal(expectedJson, json,  ignoreWhiteSpaceDifferences: true);
        }

        [Fact]
        public async Task TestGenreNotArrayAwait()
        {
            const string expectedJson = @"{
  ""catalog"": {
    ""book"": {
                ""id"": ""bk101"",
      ""author"": "" Gambardella, Matthew"",
      ""title"": ""XML Developers Guide"",
      ""genre"": [
        {
          ""id"": 1,
          ""text"": ""Computer""
        }
      ],
      ""price"": 44.95,
      ""publish_date"": ""2000-10-01"",
      ""description"": ""An in-depth look at creating applications\r\n      with XML.""
    }
  }
}";
            var elements = new List<string>
            {
                "genre"
            };

            var converter = new Converter();
            var json = await converter.ConvertToJsonAsync(xml1, elements);

            Assert.Equal(expectedJson, json, ignoreWhiteSpaceDifferences: true);
        }
        [Fact]
        public void TestGenreArray()
        {
            const string expectedJson = @"{
  ""catalog"": {
    ""book"": {
                ""id"": ""bk101"",
      ""author"": "" Gambardella, Matthew"",
      ""title"": ""XML Developers Guide"",
      ""genre"": [
        {
          ""id"": 1,
          ""text"": ""Computer""
        },
        {
          ""id"": 2,
          ""text"": ""Help""
        }
      ],
      ""price"": 44.95,
      ""publish_date"": ""2000-10-01"",
      ""description"": ""An in-depth look at creating applications\r\n      with XML.""
    }
  }
}";
            var elements = new List<string>
            {
                "genre"
            };

            var converter = new Converter();
            var json = converter.ConvertToJson(xml2, elements);

            Assert.Equal(expectedJson, json, ignoreWhiteSpaceDifferences: true);
        }
    }
}
