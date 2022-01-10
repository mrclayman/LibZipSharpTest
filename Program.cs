using System.Text;
using System.Text.Json;
using Xamarin.Tools.Zip;

namespace LibZipSharpTest
{
    public class Program
    {
        private const string InfoJson = "info.json";

        private const string InputFile = "object.zip";

        private const string OutputFile = "outputobject.zip";

        private static readonly JsonSerializerOptions JsonOptions = new ()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static int Main(string[] args)
        {
            try
            {
                byte[] data = GetInputFileData();
                MemoryStream stream = new();
                stream.Write(data);
                stream.Seek(0, SeekOrigin.Begin);

                //FileStream stream = File.Open(InputFile, FileMode.Open);

                Console.WriteLine($"stream.Write: Stream has position: {stream.Position}");

                ZipArchive archive = GetArchive(stream);
                Console.WriteLine($"GetArchive: Stream has position: {stream.Position}");

                // PrintContents(archive, stream);
                // Console.WriteLine($"PrintContents: Stream has position: {stream.Position}");

                ObjectInfo info = GetInfoJsonContent(archive);
                if (info == null || info.Id == Guid.Empty)
                {
                    throw new InvalidOperationException($"Failed to read {InfoJson} from archive");
                }
                Console.WriteLine($"GetInfoJsonContent: Stream has position: {stream.Position}");

                info.Description = "My stupid description";

                RemoveOldInfoJson(archive);
                Console.WriteLine($"RemoveOldInfoJson: Stream has position: {stream.Position}");

                AddNewInfoJson(archive, info);
                Console.WriteLine($"AddNewInfoJson: Stream has position: {stream.Position}");

                archive.Close();
                Console.WriteLine($"archive.Close: Stream has position: {stream.Position}");

                WriteOutputFile(stream);
                Console.WriteLine($"WriteOutputFile: Stream has position: {stream.Position}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An exception has occurred: {ex.Message}");
                Console.WriteLine($"Traceback: {ex.StackTrace}");
                return 1;
            }

            return 0;
        }

        private static byte[] GetInputFileData() =>
            File.ReadAllBytes(InputFile);

        private static ZipArchive GetArchive(Stream stream) =>
            ZipArchive.Open(stream);

        private static ObjectInfo GetInfoJsonContent(ZipArchive archive)
        {
            ZipEntry entry = archive.ReadEntry(InfoJson);
            if (entry == null)
            {
                throw new InvalidOperationException($"No {InfoJson} entry in archive");
            }

            using MemoryStream entryStream = new ();
            entry.Extract(entryStream);

            entryStream.Position = 0;
            using StreamReader reader = new (entryStream);
            string contents = reader.ReadToEnd();

            return JsonSerializer.Deserialize<ObjectInfo>(contents, JsonOptions);
        }

        private static void RemoveOldInfoJson(ZipArchive archive)
        {
            archive.DeleteEntry(InfoJson);
        }

        private static void AddNewInfoJson(ZipArchive archive, ObjectInfo info)
        {
            string contents = JsonSerializer.Serialize(info, JsonOptions);
            archive.AddEntry(InfoJson, contents, Encoding.UTF8);
        }

        private static void WriteOutputFile(MemoryStream stream)
        {
            File.WriteAllBytes(OutputFile, stream.ToArray());
        }

        private static void WriteOutputFile(FileStream stream)
        {
            stream.Flush();
            stream.Close();
        }

        private static void PrintContents(ZipArchive archive, Stream stream)
        {
            foreach (ZipEntry entry in archive)
            {
                Console.WriteLine($"Entry: index: {entry.Index}, full name: {entry.FullName}");
                Console.WriteLine($"Stream has position: {stream.Position}");
            }
        }
    }
}
