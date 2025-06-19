#region Namespace
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
#endregion Namespace

namespace BigMac
{
    public static class cAESEncryption
    {

        #region Main Functions
        /// <summary>
        ///     Function to Use AES-128 Encryption Method
        /// </summary>
        /// <param name="Data">String Data for Encryption</param>
        /// <param name="MKey">Generated 16 Digits Random Numbers for Master Key</param>
        /// <param name="IVKey">Generated 8 Digits Random Numbers for Initialization Vector (IV) Key</param>
        /// <param name="MKeyHex">Convert to Hex-Decimal of Master Key</param>
        /// <param name="IVKeyHex">Convert to Hex-Decimal of Initialization Vector (IV) Key</param>
        /// <param name="EncryptHex">Converted Encrypted Data in Hex-Decimal</param>
        /// <param name="EncryptFinalHex">Final Converted Encrypted Data in Hex-Decimal</param>
        public static string MkeyConstant ="6A70327564413333356F354B5A7E3146";
        public static string VKeyConstant = "23404C414A537536";

        public static string getDecryptedString(string str)
        {
            string dstr = "";
            var MKey = "";
            var IVKey = "";
            var MKeyHex = "";
            var IVKeyHex = "";
            var EncryptHex = "";
            try
            {
                cAESEncryption.AESDecryption(str, out MKey, out IVKey, out MKeyHex, out IVKeyHex, out EncryptHex, out dstr);
            }
            catch (Exception e)
            { dstr = ""; }
            return dstr;
        }

        public static string getEncryptedString(string Data)
        {
            string dstr = "";
            var EncryptHex = "";
            AESEncryption(Data, out EncryptHex, out dstr);
            return dstr;
        }

        public static void AESEncryption(string Data, out string EncryptHex, out string EncryptFinalHex)
        {
            string MKeyHex;
            string IVKeyHex;

           MKeyHex = IVKeyHex = EncryptHex = EncryptFinalHex = string.Empty;
            try
            {
                Rijndael myRijndael = new RijndaelManaged();
                //Generate 16 Digits Random Numbers - For Master Key
                //////MKey = fnRandomNumberGenerator(16);
                //Generate 16 Digits Random Numbers - For IV Key
                //////IVKey = fnRandomNumberGenerator(8);
                //Generate 32 Digits Hex Decimal Numbers - For Master Key
                //////string strMasterKeyHex = ToHexadecimal(StringToByte(cAESEncryption.MkeyConstant));
                string strMasterKeyHex = MkeyConstant;
                //Generate 16 Digits Hex Decimal Numbers - For IV Key
                //////string strInitializationVectorKeyHex = ToHexadecimal(StringToByte(cAESEncryption.VKeyConstant));
                string strInitializationVectorKeyHex = VKeyConstant;

                ///////MKeyHex = strMasterKeyHex;
                ///////IVKeyHex = strInitializationVectorKeyHex;
                MKeyHex = MkeyConstant;
                IVKeyHex = VKeyConstant;

                myRijndael.Key = StringToByte(strMasterKeyHex);              // convert to 32 characters - 256 bits
                myRijndael.IV = StringToByte(strInitializationVectorKeyHex); // 16 characters for IV
                byte[] key = myRijndael.Key;
                byte[] IV = myRijndael.IV;

                ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);

                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                // Write all data to the crypto stream and flush it.
                csEncrypt.Write(StringToByte(Data), 0, StringToByte(Data).Length);
                csEncrypt.FlushFinalBlock();

                // Get the encrypted array of bytes.
                byte[] encrypted = msEncrypt.ToArray();

                //this.txtEncrypt.Text = cHexOperation.ToHexString(encrypted, true); 
                string strEncryptHex = ToHexadecimal(encrypted);
                EncryptHex = strEncryptHex;

                //IV Key(1-8)+M Key(1-8)+M Key(25-32)+Encrypted+M Key(17-24)+IV Key(9-16)+M Key(9-16)
                EncryptFinalHex = strInitializationVectorKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(24, 8) + strEncryptHex + strMasterKeyHex.Substring(16, 8) + strInitializationVectorKeyHex.Substring(8, 8) + strMasterKeyHex.Substring(8, 8);
            }
            catch (Exception ex)
            {

                //EncryptFinalHex = ex.Message.ToString();
                EncryptFinalHex = string.Empty;
            }
        }

        public static void AESEncryption(string Data, out string MKey, out string IVKey, out string MKeyHex, out string IVKeyHex, out string EncryptHex, out string EncryptFinalHex)
        {
            MKey = IVKey = MKeyHex = IVKeyHex = EncryptHex = EncryptFinalHex = string.Empty;
            try
            {
                Rijndael myRijndael = new RijndaelManaged();
                //Generate 16 Digits Random Numbers - For Master Key
                MKey = fnRandomNumberGenerator(16);
                //Generate 16 Digits Random Numbers - For IV Key
                IVKey = fnRandomNumberGenerator(8);
                //Generate 32 Digits Hex Decimal Numbers - For Master Key
                string strMasterKeyHex = ToHexadecimal(StringToByte(MKey));
                //Generate 16 Digits Hex Decimal Numbers - For IV Key
                string strInitializationVectorKeyHex = ToHexadecimal(StringToByte(IVKey));

                MKeyHex = strMasterKeyHex;
                IVKeyHex = strInitializationVectorKeyHex;

                myRijndael.Key = StringToByte(strMasterKeyHex);              // convert to 32 characters - 256 bits
                myRijndael.IV = StringToByte(strInitializationVectorKeyHex); // 16 characters for IV
                byte[] key = myRijndael.Key;
                byte[] IV = myRijndael.IV;

                ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);

                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                // Write all data to the crypto stream and flush it.
                csEncrypt.Write(StringToByte(Data), 0, StringToByte(Data).Length);
                csEncrypt.FlushFinalBlock();

                // Get the encrypted array of bytes.
                byte[] encrypted = msEncrypt.ToArray();

                //this.txtEncrypt.Text = cHexOperation.ToHexString(encrypted, true); 
                string strEncryptHex = ToHexadecimal(encrypted);
                EncryptHex = strEncryptHex;

                //IV Key(1-8)+M Key(1-8)+M Key(25-32)+Encrypted+M Key(17-24)+IV Key(9-16)+M Key(9-16)
                EncryptFinalHex = strInitializationVectorKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(24, 8) + strEncryptHex + strMasterKeyHex.Substring(16, 8) + strInitializationVectorKeyHex.Substring(8, 8) + strMasterKeyHex.Substring(8, 8);
            }
            catch (Exception ex)
            {

                //EncryptFinalHex = ex.Message.ToString();
                EncryptFinalHex = string.Empty;
            }
        }
        /// <summary>
        ///      Function to Use AES-128 Encryption Method
        /// </summary>
        /// <param name="Data">String Data for Encryption</param>
        /// <param name="EncryptFinalHex">Final Converted Encrypted Data in Hex-Decimal</param>
        public static void AESEncryption(string Data, out string EncryptFinalHex)
        {
            EncryptFinalHex = string.Empty;
            try
            {
                Rijndael myRijndael = new RijndaelManaged();
                //Generate 16 Digits Random Numbers - For Master Key
                string MKey = fnRandomNumberGenerator(16);
                //Generate 16 Digits Random Numbers - For IV Key
                string IVKey = fnRandomNumberGenerator(8);
                //Generate 32 Digits Hex Decimal Numbers - For Master Key
                string strMasterKeyHex = ToHexadecimal(StringToByte(MKey));
                //Generate 16 Digits Hex Decimal Numbers - For IV Key
                string strInitializationVectorKeyHex = ToHexadecimal(StringToByte(IVKey));

                myRijndael.Key = StringToByte(strMasterKeyHex);              // convert to 32 characters - 256 bits
                myRijndael.IV = StringToByte(strInitializationVectorKeyHex); // 16 characters for IV
                byte[] key = myRijndael.Key;
                byte[] IV = myRijndael.IV;

                ICryptoTransform encryptor = myRijndael.CreateEncryptor(key, IV);

                MemoryStream msEncrypt = new MemoryStream();
                CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);

                // Write all data to the crypto stream and flush it.
                csEncrypt.Write(StringToByte(Data), 0, StringToByte(Data).Length);
                csEncrypt.FlushFinalBlock();

                // Get the encrypted array of bytes.
                byte[] encrypted = msEncrypt.ToArray();

                //this.txtEncrypt.Text = cHexOperation.ToHexString(encrypted, true); 
                string strEncryptHex = ToHexadecimal(encrypted);

                //IV Key(1-8)+M Key(1-8)+M Key(25-32)+Encrypted+M Key(17-24)+IV Key(9-16)+M Key(9-16)
                EncryptFinalHex = strInitializationVectorKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(24, 8) + strEncryptHex + strMasterKeyHex.Substring(16, 8) + strInitializationVectorKeyHex.Substring(8, 8) + strMasterKeyHex.Substring(8, 8);
            }
            catch (Exception ex)
            {

                EncryptFinalHex = ex.Message.ToString();
            }
        }

        public static void AESDecryption(string EncryptFinalHex, out string MKey, out string IVKey, out string MKeyHex, out string IVKeyHex, out string EncryptHex, out string Data)
        {
            MKey = IVKey = MKeyHex = IVKeyHex = Data = EncryptHex = string.Empty;

            try
            {
                Rijndael myRijndael = new RijndaelManaged();

                //IV Key(1-8)+M Key(1-8)+M Key(25-32)+Encrypted+M Key(17-24)+IV Key(9-16)+M Key(9-16)
                //EncryptFinalHex = strInitializationVectorKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(0, 8) + strMasterKeyHex.Substring(24, 8) + strEncryptHex + strMasterKeyHex.Substring(16, 8) + strInitializationVectorKeyHex.Substring(8, 8) + strMasterKeyHex.Substring(8, 8);

                // Get First 24 Digits from the Data
                string strData1 = EncryptFinalHex.Substring(0, 24);
                EncryptFinalHex = EncryptFinalHex.Substring(24, EncryptFinalHex.Length - 24);
                // Get Last 24 Digits from the Data
                string strData2 = EncryptFinalHex.Substring(EncryptFinalHex.Length - 24, 24);
                // Get Data1(0-8) + Data2(9-16)
                string strInitializationVectorKeyHex = strData1.Substring(0, 8) + strData2.Substring(8, 8);
                // Get Data1(9-16) + Data2(25-32) + Data2(0-8) + Data1(17-24)
                string strMasterKeyHex = strData1.Substring(8, 8) + strData2.Substring(16, 8) + strData2.Substring(0, 8) + strData1.Substring(16, 8);

                //Generate 16 Digits Random Numbers - For Master Key
                MKey = ByteToString(FromHexString(strMasterKeyHex));
                //Generate 16 Digits Random Numbers - For IV Key
                IVKey = ByteToString(FromHexString(strInitializationVectorKeyHex));

                MKeyHex = strMasterKeyHex;
                IVKeyHex = strInitializationVectorKeyHex;


                EncryptFinalHex = EncryptFinalHex.Substring(0, EncryptFinalHex.Length - 24);

                EncryptHex = EncryptFinalHex;

                myRijndael.Key = StringToByte(strMasterKeyHex);              // convert to 32 characters - 256 bits
                myRijndael.IV = StringToByte(strInitializationVectorKeyHex); // 16 characters for IV
                byte[] key = myRijndael.Key;
                byte[] IV = myRijndael.IV;

                ICryptoTransform decryptor = myRijndael.CreateDecryptor(key, IV);

                // Now decrypt the previously encrypted message using the decryptor
                MemoryStream msDecrypt = new MemoryStream(FromHexString(EncryptFinalHex));
                CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                StreamReader reader = new StreamReader(csDecrypt);

                Data = reader.ReadToEnd();
            }
            catch (Exception ex)
            {

                Data = ex.Message.ToString();
                Data = "";
            }
        }
        #endregion Main Functions

        #region Support Functions
        public static string fnRandomNumberGenerator(int length)
        {
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            char[] chars = new char[length];
            string validChars = "abcdefghijklmnopqrstuvwxyzABCEDFGHIJKLMNOPQRSTUVWXYZ1234567890.~!@#$%&*/^|"; //based on your requirment you can take only alphabets or number
            for (int i = 0; i < length; i++)
            {
                byte[] bytes = new byte[1];
                rng.GetBytes(bytes);
                Random rnd = new Random(bytes[0]);
                chars[i] = validChars[rnd.Next(0, 73)];
            }
            return (new string(chars));
        }

        public static byte[] StringToByte(string str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }
        private static byte[] StringToByte(string str, int len)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string ByteToString(byte[] str)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(str);
        }

        public static string ToHexadecimal(byte[] Bytes)
        {
            char[] hexes = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] Result = new char[Bytes.Length << 1];
            int Offset = 0;
            for (int i = 0; i != Bytes.Length; i++)
            {
                Result[Offset++] = hexes[Bytes[i] >> 4];
                Result[Offset++] = hexes[Bytes[i] & 0x0F];
            }
            return new string(Result);
        }

        public static byte[] FromHexString(string src)
        {
            if (String.IsNullOrEmpty(src))
                return null;

            int index = src.Length;
            int sz = index / 2;
            if (sz <= 0)
                return null;

            byte[] rc = new byte[sz];

            while (--sz >= 0)
            {
                char lo = src[--index];
                char hi = src[--index];

                rc[sz] = (byte)(
                    (
                        (hi >= '0' && hi <= '9') ? hi - '0' :
                        (hi >= 'a' && hi <= 'f') ? hi - 'a' + 10 :
                        (hi >= 'A' && hi <= 'F') ? hi - 'A' + 10 :
                        0
                    )
                    << 4 |
                    (
                        (lo >= '0' && lo <= '9') ? lo - '0' :
                        (lo >= 'a' && lo <= 'f') ? lo - 'a' + 10 :
                        (lo >= 'A' && lo <= 'F') ? lo - 'A' + 10 :
                        0
                    )
                );
            }
            return rc;
        }
        #endregion Support Functions

    }
}