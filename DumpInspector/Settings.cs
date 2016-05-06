using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace ProjectRH.DumpInspector {

    public enum SettingsResult {
        Loaded, LoadDefaults, LoadError, Saved, SaveError
    }
    
    [Serializable]
    public class Settings {

        public string LastOpenPath;

        public string LastExportPath;

        [NonSerialized]
        public Exception LastException;

        [NonSerialized]
        private readonly string _filename;


        public Settings() {
            string appDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ProjectRH"
            );
            Directory.CreateDirectory(appDataDir);
            _filename = Path.Combine(
                appDataDir,
                Assembly.GetEntryAssembly().GetName().Name+".settings"
            );
        }

        public void Save() {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(_filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public SettingsResult Load() {
            try {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(_filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                var settings = (Settings)formatter.Deserialize(stream);
                stream.Close();
                LastOpenPath = settings.LastOpenPath;
                LastExportPath = settings.LastExportPath;
                return SettingsResult.Loaded;

            } catch (FileNotFoundException) {
                return SettingsResult.Loaded;
            } catch (Exception e) {
                LastException = e;
                return SettingsResult.LoadError;
            }
         

            
            
        }
    }
}
