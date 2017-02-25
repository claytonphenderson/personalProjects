using System;
using System.IO;
namespace ConsoleApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			string[] files = System.IO.Directory.GetFiles("/Users/claytonhenderson/Desktop/");

			//create target file in documents
			string targetDirectory = "/Users/claytonhenderson/Documents/DesktopCopy" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year;
			Directory.CreateDirectory(targetDirectory);

			foreach (string s in files)
			{
				string fileName = Path.GetFileName(s);
				string documentsDestination = Path.Combine(targetDirectory, fileName);
				File.Copy(s, documentsDestination, true);
				//Console.WriteLine("Moved " + fileName + " to documents folder.");
				File.Delete(s);
			}
			//Console.ReadKey();
		}
	}
}
