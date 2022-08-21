
using System;
using System.IO;
using System.Net.Sockets;
using File = Server.FileServer.File;

namespace Server
{
	public class Server
	{
		private readonly BinaryWriter _writer;
		private readonly BinaryReader _reader;

		public Server(BinaryWriter writer, BinaryReader reader)
		{
			_writer = writer;
			_reader = reader;
		}

		public void PrintMessage(string message) => Console.WriteLine(message);

		public int ReadInt()
		{
			int message = _reader.ReadInt32();
			return message;
		}

		public string ReadString()
		{
			string message = _reader.ReadString();
			return message;
		}

		public void WriteMessage(string message)
		{
			_writer.Write(message);
			_writer.Flush();
		}

		public void WriteMessage(int message)
		{
			_writer.Write(message);
			_writer.Flush();
		}

		public void SendFile(string path, File typeFile)
		{
			_writer.Write(typeFile.ConvertFile(path));
			_writer.Flush();
		}
	}
}