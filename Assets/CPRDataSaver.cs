using UnityEngine;

namespace CPRUnitySystem
{
    public partial class CPRDataSaver
    {
        private static LogSecurity m_LogSecurity = LogSecurity.High;

        /// <summary>
        /// What information should be displayed in the log
        /// </summary>
        public static LogSecurity LogSecurity
        {
            get
            {
                return m_LogSecurity;
            }
            set
            {
                m_LogSecurity = value;
            }
        }

        private static void OutputLog(string s, bool security)
        {
            if (LogSecurity == LogSecurity.VeryHigh)
                return;
            if (!security && LogSecurity == LogSecurity.High)
                return;
            Debug.Log("CPRDataSaver : " + s);
        }

        /// <summary>
        /// This function is for using the same function as PlayerPrefs in your own class.
        /// </summary>
        public static void SetPlayerPrefs<T>(string key, T value) where T : class
        {
            PlayerPrefs.SetString(key, m_SerializeFunction(value, typeof(T)));
        }

        /// <summary>
        /// This function is for using the same function as PlayerPrefs in your own class.
        /// </summary>
        public static T GetPlayerPrefs<T>(string key) where T : class
        {
            return (T)m_DeserializeFunction(PlayerPrefs.GetString(key), typeof(T));
        }

        /// <summary>
        /// This function encrypts instances of your own class when writing to PlayerPrefs.
        /// </summary>
        public static void SetPlayerPrefsEncrypt<T>(string key, T value, string password) where T : class
        {
            PlayerPrefs.SetString(key, m_EncryptFunction(m_SerializeFunction(value, typeof(T)), password));
        }

        /// <summary>
        /// This function encrypts instances of your own class when writing to PlayerPrefs.
        /// </summary>
        public static T GetPlayerPrefsEncrypt<T>(string key, string password) where T : class
        {
            return (T)m_DeserializeFunction(m_DecryptFunction(PlayerPrefs.GetString(key), password), typeof(T));
        }

        /// <summary>
        /// This function is for storing instances of your own class in a file.
        /// </summary>
        public static void SetDataFile<T>(string path, T value)
        {
            WriteFile(path, m_SerializeFunction(value, typeof(T)));
        }

        /// <summary>
        /// This function is for reading instances of your own class from the file.
        /// </summary>
        public static T GetDataFile<T>(string path)
        {
            return (T)m_DeserializeFunction(ReadFile(path), typeof(T));
        }
    }

    /// <summary>
    /// What information should be displayed in the log
    /// </summary>
    public enum LogSecurity
    {
        /// <summary>
        /// It does not display any logs.
        /// </summary>
        VeryHigh,
        /// <summary>
        /// It is a normal state.
        /// </summary>
        High,
        /// <summary>
        /// It displays all logs including those related to encryption. 
        /// It is not recommended.
        /// </summary>
        Low
    }
}
