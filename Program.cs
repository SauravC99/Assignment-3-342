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

        var fileEntries = Directory.EnumerateFileSystemEntries(path);
        // Use the generator pattern yield to implement the iterator.
        foreach (string fileName in fileEntries)
        {
            // checks if the current "file" is actually another embedded directory
            if (Directory.Exists(fileName))
            {

                // if it is a subdirectory, we will loop through the 
                // recursively returning strings and return those to
                // allow them to be outputted correctly
                foreach (string subFiles in EnumerateFilesRecursively(fileName))
                {
                    yield return subFiles;
                }
            }

            // if it is a normal file, we will simply call yeild return on it
            else
            {
                yield return fileName;
            }
        }
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
        return null;
    }

    public static int Main(string[] args)
    {
        // TODO: Call the functions above to create the report file.

        // Test if the two command line arguments are supplied.
        /*
        if (args.Length == 0)
        {
            Console.WriteLine("Please enter two command line arguments.");
            Console.WriteLine("The first argument should be the path of the input folder and the");
            Console.WriteLine("second argument should be the path of the HTML report output file.");

            return 1;  // Exit program.
        }
        */

        // Take two command line arguments.
        string inputFolderPath = "C:\\Users\\steve\\Desktop\\CECS Courses\\CECS 342";

        // Display the two values entered by the user.
        Console.WriteLine($"Input folder path: {inputFolderPath}");

        // Call the function that enumerates the files within the folder.
        var folderFiles = EnumerateFilesRecursively(inputFolderPath);
        foreach (var line in folderFiles){
            Console.WriteLine($"File: {line} size: {FormatByteSize(new FileInfo(line).Length)}"); // test the values returned from EnumerateFilesRecursively
        }

        // TODO: call function to write the report

        return 0; // End of program.
    }

}