namespace Server.FileServer
{
	public class TxtFile : File
	{
		public override string PathFolder { get; } = @"C:\Users\Adilya\RiderProjects\ProjectTCP\Server\TxtFile\";
		public override byte[] ConvertFile(string path)
		{
			return System.IO.File.ReadAllBytes(path);
		}
	}
}