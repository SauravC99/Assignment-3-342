using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Data;

namespace FileInfo_Collector
{
    public class Program
    {
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            var fileEntries = Directory.EnumerateFileSystemEntries(path);
            
            // Iterate through all the files in the given folder 
            // and the entire sub-folder hierarchy.
            foreach (string fileName in fileEntries)
            {
                // Checks if the current "file" is a sub-folder
                if (Directory.Exists(fileName))
                {
                    // If it is a sub-folder, we will loop through recursively,
                    // returning strings and allowing them to be outputted correctly.
                    foreach (string subFiles in EnumerateFilesRecursively(fileName))
                    {
                        yield return subFiles;
                    }
                }
                else
                {
                    // If it is a normal file, we will simply call yeild return on it.
                    yield return fileName;
                }
            }
        }

        static string FormatByteSize(long byteSize)
        {
            // String list of byte size units after the numerical value.
            // Note: 1kB = 1000B
            string[] byteSizeNames = {"B", "kB", "MB", "GB", "TB", "PB", "EB", "AB"};
            int index = 0;

            // Used float instead of Decimal because the method only requires 2 siginificant digits.
            // (Decimal has a higher precision than float).
            float numberValue = (float)byteSize;

            // Loop to keep dividing by 1000 while numerical value is >= 1.
            // The index keeps track of which byte size name we will need.
            while (numberValue / 1000 >= 1)
            {
                numberValue /= 1000;
                index++;
            }

            // Parse the result as a string, the 0:n2 part will give 2 significant digits.
            return string.Format("{0:n2}{1}", numberValue, byteSizeNames[index]);
        }

        static XDocument CreateReport(IEnumerable<string> files)
        {
            // TODO: Create a HTML document containing a table with three colums:
            // "Type", "Count", and "Size" for the file name extension (converted
            // to lower case),  the number of files with this type, and the total
            // size of all files with this type, respectively.

            // Use the System.IO.FileInfo to get the size of a file with a given path.
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn();
            dc1.DataType = System.Type.GetType("System.String");
            dc1.ColumnName = "Type";
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn();
            dc2.DataType = System.Type.GetType("System.Int32");
            dc2.ColumnName = "Count";
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn();
            dc3.DataType = System.Type.GetType("System.Int64");
            dc3.ColumnName = "Size";
            dt.Columns.Add(dc3);
            foreach (var v in files)
            {
                var f = new FileInfo(v);
                string type = Path.GetExtension(f.ToString());
                bool found = false;
                foreach (DataRow d in dt.Rows)
                {
                    if (d["Type"].Equals(type))
                    {
                        d["Count"] = (int)d["Count"] + 1;
                        d["Size"] = (long)d["Size"] + f.Length;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    DataRow dr = dt.NewRow();
                    dr["Type"] = type;
                    dr["Count"] = 1;
                    dr["Size"] = f.Length;
                    dt.Rows.Add(dr);
                }
            }
            // Sort the table by the byte size value of the "Size" column in descending order.
            dt.DefaultView.Sort = "Size desc";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Console.WriteLine("Type: {0:G}, Count: {1:D}, Size:{2:D}", dt.Rows[i]["Type"], dt.Rows[i]["Count"], dt.Rows[i]["Size"]);
            }


            // Use FormatByteSize to formart the value printed in the "Size" column.

            // Implement this function using LINQ queries: group by and orderby

            // Use the System.Xml.Linq.XElement constructor to functionally
            // construct the XML document.
            return null;
        }

        public static void Main(string[] args)
        {
            // Take two command line arguments:
            // (1) A path to a folder and
            // (2) a name for a HTML report output file.
            Console.Write("Enter a path to a folder: ");
            string inputFolderPath = Console.ReadLine();
            Console.Write("Enter a name for a HTML report output file: ");
            string outputReportPath = Console.ReadLine();

            // Call the function that enumerates the files within the folder.
            var folderFiles = EnumerateFilesRecursively(inputFolderPath);
            foreach (var line in folderFiles)
            {
                // test the values returned from EnumerateFilesRecursively
                Console.WriteLine($"File: {line} size: {FormatByteSize(new FileInfo(line).Length)}");
            }
            CreateReport(folderFiles);
            
            // TODO: call function to write the report
        }
    }
}
