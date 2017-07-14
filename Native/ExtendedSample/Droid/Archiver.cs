using System;
using Xamarin.Forms;
using ExtendedSample.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(Archiver))]
namespace ExtendedSample.Droid
{
	public class Archiver : IArchiver
	{
        public Archiver()
        {
        }

		public string UnarchiveText(string filename)
		{
			// TODO
		}

		private string GetFilePath(string filename)
		{
            // TODO
            return "";
		}
    }
}
