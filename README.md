# XML2JSON

XML2JSON convert XML files to JSON using an XDS file to make a list of single XML elements into a JSON array where necessary.

It uses the JSON.NET library.

## Usage:

	XML2JSON.exe input.xml input.xsd output.json

## Example input.xds:
Assuming that input.xds has a complex element types of genre and book, with maxOccues > 1, that will give as a list of elements of:

    genre
    book
	
## Example XML Input:

	<?xml version="1.0"?>
	<catalog>
	   <book id="bk101">
		  <author>Gambardella, Matthew</author>
		  <title>XML Developer's Guide</title>
		  <genre>Computer</genre>
		  <price>44.95</price>
		  <publish_date>2000-10-01</publish_date>
		  <description>An in-depth look at creating applications 
		  with XML.</description>
	   </book>
	</catalog>
	
## Example Output:

    {
      "catalog": {
        "book": [
          {
            "id": "bk101",
            "author": "Gambardella, Matthew",
            "title": "XML Developer's Guide",
            "genre": [
              {
                "id": 1,
                "text": "Computer"
              }
            ],
            "price": 44.95,
            "publish_date": "2000-10-01",
            "description": "An in-depth look at creating applications \r\n      with XML."
          }
        ]
      }
    }
	
## Note

So, while there is just one "book" element and one "genre" element, because they are included in the elements.txt file, they are 
made in JSON arrays. This is helpful because Newtonsoft cannot use either XSD or JSON Schema files.