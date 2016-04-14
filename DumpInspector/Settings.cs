using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Reflection;

namespace ProjectRH.DumpInspector {

    public enum SettingsResult {
        LoadOK, LoadDefaults, LoadError, SaveOK, SaveError
    }
    
    [Serializable]
    public class Settings {

        public string LastOpenPath;

        public string LastExportPath;

        [NonSerialized]
        public Exception LastException;

        [NonSerialized]
        private string Filename;


        public Settings() {
            string appDataDir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "ProjectRH"
            );
            Directory.CreateDirectory(appDataDir);
            this.Filename = Path.Combine(
                appDataDir,
                Assembly.GetEntryAssembly().GetName().Name+".settings"
            );
        }

        public void Save() {
            IFormatter formatter;
            Stream stream;
            formatter = new BinaryFormatter();
            stream = new FileStream(Filename, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, this);
            stream.Close();
        }

        public SettingsResult Load() {
            Settings settings;
            IFormatter formatter;
            Stream stream;
            try {
                formatter = new BinaryFormatter();
                stream = new FileStream(Filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                settings = (Settings)formatter.Deserialize(stream);
                stream.Close();
                this.LastOpenPath = settings.LastOpenPath;
                this.LastExportPath = settings.LastExportPath;
                return SettingsResult.LoadOK;

            } catch (FileNotFoundException) {
                return SettingsResult.LoadOK;
            } catch (Exception e) {
                LastException = e;
                return SettingsResult.LoadError;
            }
         

            
            
        }
    }
}
