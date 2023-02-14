using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace CaptureVideo
{
    internal class Config
    {
        public static string mainPath = Environment.GetEnvironmentVariable("appdata") + "\\CaptureVideo";
        private string filePath = mainPath + "//config.ini";
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public Config()
        {
            if (!Directory.Exists(mainPath))
            {
                Directory.CreateDirectory(mainPath);
            }
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }
        }

        public string Read(string category, string key, string def)
        {
            StringBuilder sb = new StringBuilder();

            GetPrivateProfileString(category, key, def, sb, 32767, filePath);

            return sb.ToString();
        }

        public void Write(string category, string key, string value)
        {
            WritePrivateProfileString(category, key, value, filePath);
        }

        public void CopyResource(string fileName, byte[] data)
        {
            File.WriteAllBytes(ResourcePath(fileName), data);
        }

        public string ResourcePath(string fileName)
        {
            return mainPath + fileName;
        }
    }
}
