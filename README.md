# Lab Assignment 3

## Overview
Use C# and LINQ to iterate files, to query, group and order data, and to create an XML document based on that data.


## Implementation
- Create a C# console application.  
This application has two command line arguments: **(1)** A path to a folder and **(2)** a name for a HTML report output file. The application collects all files with the same extension (converted to lower case) and determines for each extension, i.e. file type, the number of files and the total size of all files of this type.

- Implement a class with the following 4 static functions:  

   1. **`static IEnumerable<string> EnumerateFilesRecursively(string path)`**  
   Enumerate all files in a given folder recursively including the entire sub-folder hierarchy. You can use **`System.IO.Directory`**. You could use the generator pattern (**`yield`** keyword) to implement the iterator.  

   2. **`static string FormatByteSize(long byteSize)`**  
   Format a byte size in human readable form. Use the following units: B, kB, MB, GB, TB, PB, EB, and AB where 1kB = 1000B. The numerical value should be greater or equal to 1, less than 1000 and formatted with 2 fixed digits after the decimal point, e.g. **1.30kB**.  

   3. **`static XDocument CreateReport(IEnumerable<string> files)`**  
   Create a HTML document containing a table with three columns: "Type", "Count", and "Size" for the file name extension (converted to lower case), the number of files with this type, and the total size of all files with this type, respectively.  
   You can use **`System.IO.FileInfo`** to get the size of a file with a given path.  
   Sort the table by the byte size value of the "Size" column in descending order.  
   Use the **`FormatByteSize`** function to format the value printed in the "Size" column.  
   Implement this function using LINQ queries making use of **`group by`** and **`orderby`**.  
   Use the **`System.Xml.Linq.XElement`** constructor to functionally construct the XML document.  

   4. **`public static void Main(string[] args)`**  
   Take two command line arguments. The first value is the path of the input folder and the second the path of the HTML report output file. Call the functions above to create the report file.

- **Do not** use lists or arrays to store intermediate data. Instead use iterators to process the sequences of data element without storing it all.


## Testing The Code
Demonstrate that the console application works correctly by creating a report file from some folder on your machine.
