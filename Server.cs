
using System.IO;
using System.Net.Sockets;
using File = Server.FileServer.File;

namespace Server
{
	public class Server
	{
		private readonly NetworkStream _stream;

		public Server(NetworkStream stream)
		{
			_stream = stream;
		}

		public int ReadMessageInt()
		{
			BinaryReader reader = new BinaryReader(_stream);
			int message = reader.ReadInt32();
			reader.Close();
			return message;
		}
		
		public string ReadMessageString()
		{
			BinaryReader reader = new BinaryReader(_stream);
			string message = reader.ReadString();
			reader.Close();
			return message;
		}

		public void WriteMessage(string message)
		{
			BinaryWriter writer = new BinaryWriter(_stream);
			writer.Write(message);
			writer.Flush();
			writer.Close();
		}

		public void SendFile(string path, File typeFile)
		{
			BinaryWriter writer = new BinaryWriter(_stream);
			writer.Write(typeFile.ConvertFile(path));
			writer.Flush();
			writer.Close();
		}
	}
}