using System;
using System.IO;

namespace Server.FileServer
{
	public abstract class File
	{
		public abstract string PathFolder { get; }
		public abstract byte[] ConvertFile(string path);
		
		public string PrintNameFilesInFolder(string folderPath)
		{
			Console.WriteLine("Доступные файлы:");
			string files = null;
			foreach (string path in Directory.GetFiles(folderPath)) {
				string name = path.Substring(path.Length - (path.Length - folderPath.Length));
				Console.WriteLine(name);
				files += name + "\n";
			}
			return files;
		}

		public int LengthFile(string pathFile)
		{
			FileInfo fileInfo = new FileInfo(pathFile);
			return (int) fileInfo.Length;
		}
	}
}