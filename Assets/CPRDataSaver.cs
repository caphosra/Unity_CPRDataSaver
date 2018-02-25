using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;

using UnityEngine;

namespace CPRUnitySystem
{
    public partial class CPRDataSaver
    {
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
    }
}
