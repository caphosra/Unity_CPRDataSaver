using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;

using UnityEngine;

namespace CPRUnitySystem
{
    public static class CPRDataSaver
    {
        /// <summary>
        /// This function is for using the same function as PlayerPrefs in your own class.
        /// </summary>
        public static void SetPlayerPrefs<T>(string key, T value) where T : class
        {
            PlayerPrefs.SetString(key, StringConverter(value));
        }

        /// <summary>
        /// This function is for using the same function as PlayerPrefs in your own class.
        /// </summary>
        public static T GetPlayerPrefs<T>(string key) where T : class
        {
            return ClassConverter<T>(PlayerPrefs.GetString(key));
        }

        /// <summary>
        /// This function encrypts instances of your own class when writing to PlayerPrefs.
        /// </summary>
        public static void SetPlayerPrefsEncrypt<T>(string key, T value, string password) where T : class
        {
            PlayerPrefs.SetString(key, AesEncrypt(StringConverter(value), password));
        }

        /// <summary>
        /// This function encrypts instances of your own class when writing to PlayerPrefs.
        /// </summary>
        public static T GetPlayerPrefsEncrypt<T>(string key, string password) where T : class
        {
            return ClassConverter<T>(AesDecrypt(PlayerPrefs.GetString(key), password));
        }

        private static Encoding m_UseEncoding = null;

        /// <summary>
        /// Encoding to use. If null, use Base64.
        /// </summary>
        public static Encoding UseEncoding
        {
            get
            {
                return m_UseEncoding;
            }

            set
            {
                m_UseEncoding = value;
            }
        }

        private static string convert(byte[] b)
        {
            if(m_UseEncoding == null)
            {
                return System.Convert.ToBase64String(b);
            }
            else
            {
                return m_UseEncoding.GetString(b);
            }
        }

        private static byte[] convert(string s)
        {
            if (m_UseEncoding == null)
            {
                return System.Convert.FromBase64String(s);
            }
            else
            {
                return m_UseEncoding.GetBytes(s);
            }
        }

        /// <summary>
        /// Convert a serializable class to a string.
        /// </summary>
        public static string StringConverter<T>(T value) where T : class
        {
            string st = null;
            using (var stream = new MemoryStream())
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(T));
                serializer.Serialize(stream, value);

                st = convert(stream.ToArray());
            }
            return st;
        }

        /// <summary>
        /// Convert a string to a serializable class.
        /// </summary>
        public static T ClassConverter<T>(string value) where T : class
        {
            T obj = null;
            using (var stream = new MemoryStream(convert(value)))
            {
                XmlSerializer serializer =
                    new XmlSerializer(typeof(T));
                obj = (T)serializer.Deserialize(stream);
            }
            return obj;
        }

        /// <summary>
        /// Encrypt the character string using AES encryption.
        /// </summary>
        private static string AesEncrypt(string value, string Password)
        {
            byte[] encryptText;
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;              // BlockSize = 16bytes
                aes.KeySize = 128;                // KeySize = 16bytes
                aes.Mode = CipherMode.CBC;        // CBC mode
                aes.Padding = PaddingMode.PKCS7;    // Padding mode is "PKCS7".

                byte[] text = convert(value);

                //入力されたパスワードをベースに擬似乱数を新たに生成
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, 16);
                byte[] salt = new byte[16]; // Rfc2898DeriveBytesが内部生成したなソルトを取得
                salt = deriveBytes.Salt;
                // 生成した擬似乱数から16バイト切り出したデータをパスワードにする
                byte[] bufferKey = deriveBytes.GetBytes(16);

                aes.Key = bufferKey;
                // IV ( Initilization Vector ) は、AesManagedにつくらせる
                aes.GenerateIV();

                //Encryption interface.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                encryptText = encryptor.TransformFinalBlock(text, 0, text.Length);

                // salt => iv => encrypttext の順に並び変え
                List<byte> enc = new List<byte>();
                enc.AddRange(salt);
                enc.AddRange(aes.IV);
                enc.AddRange(encryptText);

                encryptText = enc.ToArray();
            }
            return convert(encryptText);
        }

        /// <summary>
        /// Decrypts the string using AES encryption.
        /// </summary>
        private static string AesDecrypt(string value, string Password)
        {
            byte[] decryptText;
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;              // BlockSize = 16bytes
                aes.KeySize = 128;                // KeySize = 16bytes
                aes.Mode = CipherMode.CBC;        // CBC mode
                aes.Padding = PaddingMode.PKCS7;    // Padding mode is "PKCS7".

                byte[] text = convert(value);

                // salt
                byte[] salt = text.Take(16).ToArray();

                // Initilization Vector
                byte[] iv = text.Skip(16).Take(16).ToArray();
                aes.IV = iv;

                text = text.Skip(32).ToArray();

                // ivをsaltにしてパスワードを擬似乱数に変換
                Rfc2898DeriveBytes deriveBytes = new Rfc2898DeriveBytes(Password, salt);
                byte[] bufferKey = deriveBytes.GetBytes(16);    // 16バイトのsaltを切り出してパスワードに変換
                aes.Key = bufferKey;

                //Decryption interface.
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                decryptText = decryptor.TransformFinalBlock(text, 0, text.Length);
            }
            return convert(decryptText);
        }
    }
}
