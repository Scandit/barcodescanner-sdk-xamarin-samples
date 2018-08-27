using ExtendedSample.Droid;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(Archiver))]
namespace ExtendedSample.Droid
{
    public class Archiver : IArchiver
    {
        public void ArchiveText(string filename, string text)
        {
            using (var streamWriter = new StreamWriter(GetFilePath(filename), false))
            {
                streamWriter.Write(text);
            }
        }

        public string UnarchiveText(string filename)
        {
            var text = "";
            var filepath = GetFilePath(filename);
            if (File.Exists(filepath))
            {
                using (var streamReader = new StreamReader(filepath))
                {
                    text = streamReader.ReadToEnd();
                }
            }

            return text;
        }

        string GetFilePath(string filename)
        {
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}
