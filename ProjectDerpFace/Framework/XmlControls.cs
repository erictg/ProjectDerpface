using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
namespace ProjectDerpface.Framework
{
    public static class XmlControls
    {
        public const int SETTINGS = 0;
        public const int SETTINGS_KEYS = 1;
        public const int GAMETYPES = 2;
        public const int USER_DATA = 3;

        public static void serializeObject<T>(int location, string fileName, T obj)
        {
            XmlSerializer writer = new XmlSerializer(typeof(T));
            string path = getFilePath(location, fileName);
            Debug.WriteLine(path); 
            StreamWriter file = new StreamWriter(path);
            writer.Serialize(file, obj);
            file.Close();
            Debug.WriteLine("serialized");
        }

        public static void deserializeObject<T>(int location, string fileName, ref T obj)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            string path = getFilePath(location, fileName);
            FileStream fs = new FileStream(path, FileMode.Open);
            XmlReader reader = XmlReader.Create(fs);

            obj = (T)serializer.Deserialize(reader);
            fs.Close();            
        }

        private static string getFilePath(int location, string fileName)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Data\Saves\";
            switch (location)
            {
                case SETTINGS:
                    path += @"Settings\" + fileName + @".settings";
                    break;
                case SETTINGS_KEYS:
                    path += @"Settings\" + fileName + @".keys";
                    break;
                case GAMETYPES:
                    path += @"GameTypes\" + fileName + @".gametype";
                    break;
                case USER_DATA:
                    path += @"UserData\" + fileName + @".user";
                    break;
            }

            return path;
        }
    }
}
