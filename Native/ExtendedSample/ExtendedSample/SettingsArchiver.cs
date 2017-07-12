using System.Xml.Serialization;
using System.IO;
using Xamarin.Forms;

namespace ExtendedSample
{
    public class SettingsArchiver
    {
        public SettingsArchiver()
        {
            
        }

        public static void ArchiveSettings(Settings settings) 
        {
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
			using (StringWriter textWriter = new StringWriter())
			{
				serializer.Serialize(textWriter, settings);
				var serializedSettings = textWriter.ToString();
                DependencyService.Get<IArchiver>().ArchiveText("settings.xml", serializedSettings);
			}
		}

        public static Settings UnarchiveSettings()
        {
			var serializedSettings = DependencyService.Get<IArchiver>().UnarchiveText("settings.xml");
            if (serializedSettings == null) 
            {
                return new Settings();
            }
			XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (StringReader textReader = new StringReader(serializedSettings))
            {
                return (Settings)serializer.Deserialize(textReader);
            }
		}
    }
}
