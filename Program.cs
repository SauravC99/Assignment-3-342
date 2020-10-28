﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Xml.Linq;

/// <summary>
/// Summary description for Class1
/// </summary>
public class Program
{
    static IEnumerable<string> EnumerateFilesRecursively(string path)
    {
        // TODO: Enumerate all files in a given folder recursively including
        // the entire sub-folder hierarchi.

        // Use System.IO.Directory

        // Use the generator pattern yield to implement the iterator.
    }

    static string FormatByteSize(long byteSize)
    {
        // String list of ends to the size
        string[] ends = { "Bytes", "kB", "mB", "gB", "tB", "pB" };
        int counter = 0;
        // Used float instead of Decimal here cause method only returns 2 siginificant digits
        // (Decimal has a higher precision than float)
        float number = (float)byteSize;

        // Loop keep dividing by 1000 until you can't
        // counter keeps track of which end we are at
        while (number / 1000 >= 1)
        {
            number /= 1000;
            counter++;
        }
        // Put it together and return, the 0:n2 part will give 2 significant digits
        return string.Format("{0:n2}{1}", number, ends[counter]);
    }

    static XDocument CreateReport(IEnumerable<string> files)
    {
        // TODO: Create a HTML document containing a table with three colums:
        // "Type", "Count", and "Size" for the file name extension (converted
        // to lower case),  the number of files with this type, and the total
        // size of all files with this type, respectively.

        // Use the System.IO.FileInfo to get the size of a file with a given path.

        // Sort the table by the byte size value of the "Size" column in descending order.

        // Use FormatByteSize to formart the value printed in the "Size" column.

        // Implement this function using LINQ queries: group by and orderby

        // Use the System.Xml.Linq.XElement constructor to functionally
        // construct the XML document.
    }

    public static void Main(string[] args)
    {
        // TODO: Take two command line arguments. The first value is the path
        // of the input folder and the second the path of the HTML report
        // output file. Call the functions above to create the report file.
    }

}