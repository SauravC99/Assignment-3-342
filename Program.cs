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

        public static int Main(string[] args)
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
                Console.WriteLine($"File: {line} size: {FormatByteSize(new FileInfo(line).Length)}"); // test the values returned from EnumerateFilesRecursively
            }
            CreateReport(folderFiles);
            // TODO: call function to write the report

            return 0; // End of program.
        }
    }
}
