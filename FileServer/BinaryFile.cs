
namespace Server.FileServer
{
	public class BinaryFile : File
	{
		public override string PathFolder { get; } = @"C:\Users\Adilya\RiderProjects\ProjectTCP\Server\BinaryFile\";
		public override byte[] ConvertFile(string name)
		{
			return System.IO.File.ReadAllBytes(PathFolder + name);
		}
	}
}