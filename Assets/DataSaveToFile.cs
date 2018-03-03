using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace CPRUnitySystem
{
    public partial class CPRDataSaver
    {
        private static Encoding m_FileEncoding = Encoding.ASCII;

        public static Encoding FileEncoding
        {
            get
            {
                return m_FileEncoding;
            }
            set
            {
                m_FileEncoding = value;
            }
        }

        private static void WriteFile(string path, string data)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, m_FileEncoding))
                {
                    sw.Write(data);
                }
            }
            catch(Exception e)
            {
                Debug.LogException(e);
            }
        }

        private static string ReadFile(string path)
        {
            FileInfo fi = new FileInfo(path);
            string str = "";

            try
            {
                using (StreamReader sr = new StreamReader(fi.OpenRead(), m_FileEncoding))
                {
                    str = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return str;
        }
    }
}
