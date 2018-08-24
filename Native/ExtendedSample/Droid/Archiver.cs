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
            if (System.IO.File.Exists(filename))
            {
                using (var streamReader = new StreamReader(GetFilePath(filename)))
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
