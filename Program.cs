using System;
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
public class Program {
	static IEnumerable<string> EnumerateFilesRecursively(string path) {

	}

	static string FormatByteSize(long byteSize) {
		// String list of ends to the size
		string[] ends = { "Bytes", "kB", "mB", "gB", "tB", "pB" };
		int counter = 0;
		// Used float instead of Decimal here cause method only returns 2 siginificant digits
		// (Decimal has a higher precision than float)
		float number = (float)byteSize;

		// Loop keep dividing by 1000 until you can't
		// counter keeps track of which end we are at
		while (number / 1000 >= 1) {
			number /= 1000;
			counter++;
		}
		// Put it together and return, the 0:n2 part will give 2 significant digits
		return string.Format("{0:n2}{1}", number, ends[counter]);
	}

	static XDocument CreateReport(IEnumerable<string> files) {

	}

	public static void Main(string[] args) {

	}

}