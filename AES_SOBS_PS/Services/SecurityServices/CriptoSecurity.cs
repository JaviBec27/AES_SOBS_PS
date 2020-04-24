using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace AES_SOBS_PS.Services.SecurityServices
{
    public static class CriptoSecurity
    {
        /// <summary>
        /// Tipos de Algoritmos para establecer Hash
        /// </summary>
        public enum HashType { MD5, SHA1, SHA256, SHA384, SHA512 }

        /// <summary>
        /// Comparación de cadenas Obtiene el hash de la cadena Original, segun el tipo indicado y lo compara con un hash obtenido de otra fuente.
        /// </summary>
        /// <param name="strOriginal">Cadena Original sin hash</param>
        /// <param name="strHash">Preseunto Hash de strOriginal</param>
        /// <param name="hshType">Tipo de Hash a obtener</param>
        /// <returns></returns>
        public static bool CheckHash(string strOriginal, string strHash, HashType hshType)
        {
            return (GetHash(strOriginal, hshType) == strHash);
        }

        /// <summary>
        /// Obtiene el Hash de una cadena de texto plano
        /// </summary>
        /// <param name="strPlain">Texto del cual se desea conocer el Hash</param>
        /// <param name="hashType">Tipo de Hash que se requiere aplciar</param>
        /// <returns></returns>
        public static string GetHash(string strPlain, HashType hashType)
        {
            var bytes = new UnicodeEncoding().GetBytes(strPlain);
            return GetHash(bytes, hashType);
        }

        /// <summary>
        /// Obtiene el Hash de un arreglo de bytes
        /// </summary>
        /// <param name="bufferObject">Arreglo de bytes de cualquier objeto</param>
        /// <param name="hashType">Tipo de has que se requiere aplicar</param>
        /// <returns></returns>
        public static string GetHash(byte[] bufferObject, HashType hashType)
        {
            byte[] bufferHash = new byte[0];
            var str = "";

            switch (hashType)
            {
                case HashType.MD5:
                    MD5 managedMD5 = new MD5CryptoServiceProvider();
                    bufferHash = managedMD5.ComputeHash(bufferObject);
                    break;
                case HashType.SHA1:
                    var managedSHA1 = new SHA1Managed();
                    bufferHash = managedSHA1.ComputeHash(bufferObject);
                    break;
                case HashType.SHA256:
                    var managedSHA256 = new SHA256Managed();
                    bufferHash = managedSHA256.ComputeHash(bufferObject);
                    break;
                case HashType.SHA384:
                    var managedSHA384 = new SHA384Managed();
                    bufferHash = managedSHA384.ComputeHash(bufferObject);
                    break;
                case HashType.SHA512:
                    var managedSHA512 = new SHA512Managed();
                    bufferHash = managedSHA512.ComputeHash(bufferObject);
                    break;
                default:
                    break;
            }

            foreach (byte num in bufferHash)
            {
                str = str + string.Format("{0:x2}", num);
            }
            return str;
        }

        /// <summary>
        /// Aplicación de un cifrado Avanzado AES para cadenas de texto, Basado en una llave de encripción
        /// </summary>
        /// <param name="clearText"></param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            var clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Aplicación del algoritmo para des-encriptar un texto encriptado con algoritmo Avanzado AES para cadenas de texto, Basado en una llave de encripción
        /// </summary>
        /// <param name="cipherText"></param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            const string encryptionKey = "MAKV2SPBNI99212";
            cipherText = cipherText.Replace(" ", "+");
            var cipherBytes = Convert.FromBase64String(cipherText);
            using (var encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }


    }
}
