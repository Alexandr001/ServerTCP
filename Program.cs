
using System;
using System.Net;
using System.Net.Sockets;
using Server.FileServer;

namespace Server
{
	// Подключается клиент и в отдельном потоке срабатывет метод
		// Поток работает до того момента пока режим работы клиента не равен 0.
	// Получает режим работы программы
	// В зависимости от режима работы отправляет пути к файлам
	// Получает путь к файлу и отправляет данный файл
	
	public class Program
	{
		private const int PORT = 8888;
		
		public static void Main(string[] args)
		{
			TcpListener listener = null;
			try {
				IPAddress localAddr = IPAddress.Parse("127.0.0.1");
				listener = new TcpListener(localAddr, PORT);
				listener.Start();

				while (true) {
					Console.WriteLine("Ожидание подключений... ");

					// получаем входящее подключение
					TcpClient client = listener.AcceptTcpClient();
					Console.WriteLine("Подключен клиент. Выполнение запроса...");

					// получаем сетевой поток для чтения и записи
					NetworkStream stream = client.GetStream();
					
					Server server = new Server(stream);
					int operatingMode = server.ReadMessageInt();
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
					}
					server.WriteMessage(listFiles);
					string pathFile = server.ReadMessageString();
					server.SendFile(pathFile, file);
				}
			} catch (Exception e) {
				Console.WriteLine(e);
				throw;
			} finally {
				listener?.Stop();
			}
		}
	}
}