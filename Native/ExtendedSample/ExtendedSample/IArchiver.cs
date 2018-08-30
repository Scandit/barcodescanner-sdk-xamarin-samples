using System;

namespace ExtendedSample
{
	public interface IArchiver
	{
		void ArchiveText(string filename, string text);
		string UnarchiveText(string filename);
	}
}
