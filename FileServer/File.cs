using System;
using System.IO;

namespace Server.FileServer
{
	public abstract class File
	{
		public abstract string PathFolder { get; }
		public abstract byte[] ConvertFile(string name);
		
		public string PrintNameFilesInFolder(string folderPath)
		{
			Console.WriteLine("Доступные файлы:");
			string files = null;
			foreach (string path in Directory.GetFiles(folderPath)) {
				string name = path.Substring(path.Length - (path.Length - folderPath.Length) + 1);
				Console.WriteLine(name);
				files += name + "\n";
			}
			return files;
		}
	}
}