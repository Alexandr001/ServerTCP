
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server.FileServer;
using File = Server.FileServer.File;

namespace Server
{
	// Подключается клиент и в отдельном потоке срабатывет метод
		// Поток работает до того момента пока режим работы клиента не равен 0.
	// Получает режим работы программы
	// В зависимости от режима работы отправляет пути к файлам
	// Получает путь к файлу и отправляет данный файл
	
	public class Program
	{
		private static BinaryWriter _writer;
		private static BinaryReader _reader;
		private static TcpListener _listener;
		private static NetworkStream _stream;
		private static TcpClient _client;
		private const int PORT = 8888;
		
		public static void Main(string[] args)
		{
			try {
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				_listener = new TcpListener(localAddr, PORT);
				_listener.Start();

				while (true) {
					Console.WriteLine("Ожидание подключений... ");

					// получаем входящее подключение
					_client = _listener.AcceptTcpClient();
					Console.WriteLine("Подключен клиент. Выполнение запроса...");

					// получаем сетевой поток для чтения и записи
					_stream = _client.GetStream();
					_writer = new BinaryWriter(_stream);
					_reader = new BinaryReader(_stream);
					
					
					Server server = new Server(_writer, _reader);
					int operatingMode = server.ReadMessageInt(); // Считывание режима работы
					string listFiles = null;
					File file = null;
					switch (operatingMode) {
						case 1:
							file = new TxtFile();
							listFiles = file.PrintNameFilesInFolder(file.PathFolder);
							break;
						case 2:
							file = new BinaryFile();
							listFiles = file.PrintNameFilesInFolder(file.PathFolder);
							break;
						default:
							throw new Exception("Нет такого режима работы!");
					}
					server.WriteMessage(listFiles); // Отправка списска файлов
					string pathFile = server.ReadMessageString(); // Чтение пути файла
					server.WriteMessage(file.LengthFile(pathFile)); // Отправка длинны файла
					server.SendFile(pathFile, file); // Отправка файла
				}
			} catch (Exception e) {
				Console.WriteLine(e);
				throw;
			} finally {
				_listener?.Stop();
				_stream.Flush();
				_stream.Close();
				_reader.Close();
				_writer.Close();
				
			}
		}
	}
}