﻿using LotteryServices.Interfaces;
using System.Security.Cryptography;

namespace LotteryServices.Services
{
    public class ServiceEncriptado : IEncriptado
    {

        private static readonly byte[] Key = HexStringToByteArray("5b05196eb7aa7f487ad6de85828e1b09");
        private static readonly byte[] IV = HexStringToByteArray("00000000000000000000000000000000");

        public ServiceEncriptado()
        {
        }

        public string Decrypt(string cipherText)
        {
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;

                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
                // Maneja cualquier excepción de desencriptación aquí
                throw new InvalidOperationException("Error al desencriptar el texto.", ex);
            }
        }

        public string Encrypt(string text)
        {
            throw new NotImplementedException();
        }

        private static byte[] HexStringToByteArray(string hex)
        {
            int length = hex.Length;
            byte[] bytes = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

    }
}