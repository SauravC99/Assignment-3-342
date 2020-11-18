using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data;

namespace FileInfo_Collector
{
    public class Program
    {
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            // Get a snapshot of the file system.
            var directoryTree = new DirectoryInfo(path);

            // Enumerate all files recursively including the entire sub-folder hierarchy.
            IEnumerable<FileInfo> fileList = directoryTree.GetFiles("*.*", SearchOption.AllDirectories);

            // Iterate through all the files in all of the directories and use the 
            // generator pattern (yield keyword) to implement the iterator.
            foreach (var file in fileList)
                yield return file.FullName;
        }

        static string FormatByteSize(long byteSize)
        {
            // String list of byte size units after the numerical value.
            // Note: 1kB = 1000B
            string[] byteSizeNames = {"B", "kB", "MB", "GB", "TB", "PB", "EB", "AB"};

            // Used float instead of Decimal because the method only requires 2 siginificant digits.
            // (Decimal has a higher precision than float).
            float numberValue = (float)byteSize;

            // Loop to keep dividing by 1000 while numerical value is >= 1.
            // The index keeps track of which byte size name we will need.
            int index;
            for (index = 0; numberValue / 1000 >= 1; ++index)
                numberValue /= 1000;

            // Parse the result as a string, the 0:n2 part will give 2 significant digits.
            return string.Format("{0:n2}{1}", numberValue, byteSizeNames[index]);
        }

        static XDocument CreateReport(IEnumerable<string> files)
        {
            // Create a HTML document containing a table with three colums:
            // "Type": the file name extension (converted to lower case)
            // "Count": the number of files with this type
            // "Size": the total size of all files with this type
            XElement xmlTable = new XElement("table", 
                new XAttribute("border", "1"),
                new XAttribute("style", "width:100%"),
                new XElement("tr",
                    new XElement("th","Type"),
                    new XElement("th","Count"),
                    new XElement("th","Size")
                )
            );

            // Use the System.IO.FileInfo to get the size of a file with a given path.
            // Sort the table by the byte size value of the "Size" column in descending order.
            // Use FormatByteSize to format the value printed in the "Size" column.
            // Implement this function using LINQ queries: group by and orderby
            var rows = (files.Select(
                        path => new FileInfo(path))
                        ).GroupBy(file => file.Extension
                        ).Select(f => new 
                            {
                                FileNameExt = f.Key.Substring(1).ToLower(),
                                NumOfFiles = f.Count(),
                                TotalSize = f.Sum(file => file.Length)
                            }
                        ).OrderByDescending(f => f.TotalSize);
            
            // Use the System.Xml.Linq.XElement constructor to functionally
            // construct the XML document.
            XElement row1 = xmlTable.Element("tr");
            row1.AddAfterSelf(
                from row in rows
                let size = FormatByteSize(row.TotalSize)
                select new XElement("tr",
                    new XElement("td", row.FileNameExt),
                    new XElement("td", row.NumOfFiles),
                    new XElement("td", size)
                )
            );

            XDocument report = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XDocumentType("html", null, null, null),
                new XElement("html",
                    new XElement("head",
                        new XElement("title", "HTML Report")
                    ),
                    new XElement("body", xmlTable)
                )
            );

            return report;
        }

        public static void Main(string[] args)
        {
            // Take two command line arguments:
            // (1) A path to a folder and
            // (2) a name for a HTML report output file.
            Console.Write("Enter the path of the input folder: "); // <directory path>/<folder name>
            string inputFolderPath = Console.ReadLine();
            Console.Write("Enter the path for the HTML report output file: "); // <directory path>/Report.htm
            string outputReportPath = Console.ReadLine();

            // Call the function that enumerates the files within the folder.
            var folderFiles = EnumerateFilesRecursively(inputFolderPath);
            // Call the function to create the HTML report file.
            var xmlReport = CreateReport(folderFiles);
            // Save the HTML report file to the output path.
            xmlReport.Save(outputReportPath);
        }
    }
}
