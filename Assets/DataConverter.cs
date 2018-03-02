using System;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace CPRUnitySystem
{
    public partial class CPRDataSaver
    {
        protected static Encoding m_UseEncoding = null;

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

        protected static string ConvertToString(byte[] b)
        {
            if (m_UseEncoding == null)
            {
                return Convert.ToBase64String(b);
            }
            else
            {
                return m_UseEncoding.GetString(b);
            }
        }

        protected static byte[] ConvertToByte(string s)
        {
            if (m_UseEncoding == null)
            {
                return Convert.FromBase64String(s);
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
            return StringConverter(value, typeof(T));
        }

        /// <summary>
        /// Convert a serializable class to a string.
        /// </summary>
        public static string StringConverter(object o, Type t)
        {
            string st = null;
            using (var stream = new MemoryStream())
            {
                XmlSerializer serializer =
                    new XmlSerializer(t);
                serializer.Serialize(stream, o);

                st = ConvertToString(stream.ToArray());
            }
            return st;
        }

        /// <summary>
        /// Convert a string to a serializable class.
        /// </summary>
        public static T ClassConverter<T>(string value) where T : class
        {
            return (T)ClassConverter(value, typeof(T));
        }

        /// <summary>
        /// Convert a string to a serializable class.
        /// </summary>
        public static object ClassConverter(string value, Type t)
        {
            object obj = null;
            using (var stream = new MemoryStream(ConvertToByte(value)))
            {
                XmlSerializer serializer =
                    new XmlSerializer(t);
                obj = serializer.Deserialize(stream);
            }
            return obj;
        }

        protected static SerializeFunc m_SerializeFunction = StringConverter;

        /// <summary>
        /// The serialize function.
        /// </summary>
        public static SerializeFunc SerializeFunction
        {
            get
            {
                return m_SerializeFunction;
            }
            set
            {
                m_SerializeFunction = value;
            }
        }

        protected static DeserializeFunc m_DeserializeFunction = ClassConverter;

        /// <summary>
        /// The deserialize function.
        /// </summary>
        public static DeserializeFunc DeserializeFunction
        {
            get
            {
                return m_DeserializeFunction;
            }
            set
            {
                m_DeserializeFunction = value;
            }
        }
    }

    public delegate string SerializeFunc(object o, Type t);
    public delegate object DeserializeFunc(string s, Type t);
}

