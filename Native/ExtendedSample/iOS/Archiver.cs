using System;
using Xamarin.Forms;
using ExtendedSample.iOS;
using Foundation;

[assembly: Xamarin.Forms.Dependency(typeof(Archiver))]
namespace ExtendedSample.iOS
{
    public class Archiver : IArchiver
    {
        public void ArchiveText(string filename, string text) 
        {
            NSKeyedArchiver.ArchiveRootObjectToFile(NSString.FromObject(text), GetFilePath(filename));
        }

		public string UnarchiveText(string filename)
        {
            var obj = NSKeyedUnarchiver.UnarchiveFile(GetFilePath(filename));
            if (obj == null) 
            {
                return null;
            }
            else 
            {
                return ((NSString)obj).ToString();
            }
        }

        private string GetFilePath(string filename)
        {
            var fileManger = new NSFileManager();
            var url = fileManger.GetUrls(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User)[0];
            return url.Append(filename, false).Path;
        }
    }
}
