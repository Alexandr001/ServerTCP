using System;
using System.IO;

namespace Server.FileServer
{
	public abstract class File
	{
		public abstract string PathFolder { get; }
		public abstract byte[] ConvertFile(string path);
		public static File FileFactory(int mode)
		{
			switch (mode) {
				case (int) Mode.BIN_FILE:
					return new BinaryFile();
				case (int) Mode.TXT_FILE:
					return new TxtFile();
				default:
					throw new Exception("Неправильный режим работы!");
			}
		}
		public string GetNameFilesInFolder(string folderPath)
		{
			string files = null;
			foreach (string path in Directory.GetFiles(folderPath)) {
				string name = path.Substring(path.Length - (path.Length - folderPath.Length));
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