using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;

using UnityEngine;

namespace CPRUnitySystem
{
    public partial class CPRDataSaver
    {
        /// <summary>
        /// Encrypt the character string using AES encryption.
        /// </summary>
        protected static string AesEncrypt(string value, string Password)
        {
            byte[] encryptText;
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;              // BlockSize = 16bytes
                aes.KeySize = 128;                // KeySize = 16bytes
                aes.Mode = CipherMode.CBC;        // CBC mode
                aes.Padding = PaddingMode.PKCS7;    // Padding mode is "PKCS7".

                byte[] text = ConvertToByte(value);

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
            return ConvertToString(encryptText);
        }

        /// <summary>
        /// Decrypts the string using AES encryption.
        /// </summary>
        protected static string AesDecrypt(string value, string Password)
        {
            byte[] decryptText;
            using (AesManaged aes = new AesManaged())
            {
                aes.BlockSize = 128;              // BlockSize = 16bytes
                aes.KeySize = 128;                // KeySize = 16bytes
                aes.Mode = CipherMode.CBC;        // CBC mode
                aes.Padding = PaddingMode.PKCS7;    // Padding mode is "PKCS7".

                byte[] text = ConvertToByte(value);

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
            return ConvertToString(decryptText);
        }

        protected static CPREncryptFunc m_EncryptFunction = AesEncrypt;

        /// <summary>
        /// The encrypt function.
        /// </summary>
        public static CPREncryptFunc EncryptFunction
        {
            get
            {
                return m_EncryptFunction;
            }

            set
            {
                m_EncryptFunction = value;
            }
        }

        public static CPREncryptFunc DefaultEncryptFunction { get { return AesEncrypt; } }

        protected static CPRDecryptFunc m_DecryptFunction = AesDecrypt;

        /// <summary>
        /// The decrypt function.
        /// </summary>
        public static CPRDecryptFunc DecryptFunction
        {
            get
            {
                return m_DecryptFunction;
            }
            set
            {
                m_DecryptFunction = value;
            }
        }

        protected static CPRDecryptFunc DefaultDecryptFunction { get { return AesDecrypt; } }
    }

    public delegate string CPREncryptFunc(string value, string Password);
    public delegate string CPRDecryptFunc(string value, string Password);
}

