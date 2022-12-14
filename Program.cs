
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server.FileServer;
using File = Server.FileServer.File;

namespace Server
{

	public static class Program
	{
		private const int OPERATING_ERROR = 0;
		private const int PORT = 8888;

		private static BinaryWriter _writer;
		private static BinaryReader _reader;
		private static TcpListener _listener;
		private static NetworkStream _stream;
		private static TcpClient _client;
		private static Server _server;
		private static File _file;

		public static void Main(string[] args)
		{
			try {
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				_listener = new TcpListener(localAddr, PORT);
				_listener.Start();

				while (true) {
					Console.WriteLine("Ожидание подключений... ");
					
					_client = _listener.AcceptTcpClient();
					Console.WriteLine("Клиент подключен!");

					_stream = _client.GetStream();
					_writer = new BinaryWriter(_stream);
					_reader = new BinaryReader(_stream);
					
					
					_server = new Server(_writer, _reader);
					int operatingMode = Convert.ToInt32(_server.ReadString());
					if (operatingMode == OPERATING_ERROR) {
						Console.WriteLine("Слиент отключен! ОШИБКА!!!");
						continue;
					}
					_file = File.FileFactory(operatingMode);
					_server.PrintMessage($"Режим работы: {operatingMode}");
					string listFiles = _file.GetNameFilesInFolder(_file.PathFolder);
					_server.WriteMessage(listFiles);
					_server.PrintMessage("Список файлов отправлен!\n" + listFiles);
					string nameFile = _server.ReadString();
					if (nameFile == Convert.ToString(OPERATING_ERROR)) {
						continue;
					}
					string pathFile = _file.PathFolder + nameFile;
					_server.PrintMessage($"Путь к файлу: {pathFile}");
					
					_server.WriteMessage(_file.LengthFile(pathFile));
					_server.PrintMessage("Длинна файла отправлена!");
					
					_server.SendFile(pathFile, _file);
					_server.PrintMessage("Файл отправлен!");
				}
			} catch (Exception e) {
				Console.WriteLine(e);
			} finally {
				_listener?.Stop();
				_stream?.Flush();
				_stream?.Close();
				_reader?.Close();
				_writer?.Close();
				
			}
		}
	}
}