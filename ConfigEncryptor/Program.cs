using System;
using System.IO;
using System.Security.Cryptography;

namespace Hackney.ConfigEncryptor
{
    public enum UseWhichFileEnum
    {
        Server,
        Local
    }


    class Program
    {

        public const int ExitNoUserPassword = -1;
        public const int ExitError = -2;
        public const int ExitCryptographicError = -3;

        static void Main(string[] args)
        {
            try
            {
                string password = Environment.GetEnvironmentVariable("DevEncryptPassword");
                if (string.IsNullOrEmpty(password))
                {
                    Console.WriteLine("Please ensure system user variable 'DevEnCryptPassword' exists and is set to a minimum of 10 characters.");
                    Environment.ExitCode = -1;
                    return;
                }

                if (args[0].ToUpper() == "/E:")
                    FileEncrypt(args[1], args[2], password);
                if (args[0].ToUpper() == "/D:")
                {
                    FileDecrypt(args[1], args[2], password);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.ExitCode = -2;
            }
        }
        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[32];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }

            return data;
        }
        
        private static void FileEncrypt(string inputFile, string outputFile, string password)
        {

            byte[] salt = GenerateRandomSalt();

            FileStream fsCrypt = new FileStream(outputFile, FileMode.Create);
            
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Padding = PaddingMode.PKCS7;

            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Mode = CipherMode.CFB;

            fsCrypt.Write(salt, 0, salt.Length);

            CryptoStream cs = new CryptoStream(fsCrypt, AES.CreateEncryptor(), CryptoStreamMode.Write);

            FileStream fsIn = new FileStream(inputFile, FileMode.Open);
            
            byte[] buffer = new byte[1048576];
            int read;

            try
            {
                while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                {
                    cs.Write(buffer, 0, read);
                }

                // Close up
                fsIn.Close();
            }
            catch (Exception ex)
            {
                Environment.ExitCode = -2;
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                cs.Close();
                fsCrypt.Close();
            }
        }
        
        public static void FileDecrypt(string encryptedFile, string decryptedFile, string password)
        {
            byte[] passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            byte[] salt = new byte[32];

            FileStream filestreamEncrypted = new FileStream(encryptedFile, FileMode.Open);
            filestreamEncrypted.Read(salt, 0, salt.Length);

            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 50000);
            AES.Key = key.GetBytes(AES.KeySize / 8);
            AES.IV = key.GetBytes(AES.BlockSize / 8);
            AES.Padding = PaddingMode.PKCS7;
            AES.Mode = CipherMode.CFB;
            
            CryptoStream cs = new CryptoStream(filestreamEncrypted, AES.CreateDecryptor(), CryptoStreamMode.Read);
            MemoryStream msOut = new MemoryStream();

            byte[] buffer = new byte[1048576];

            try
            {
                int bytesRead;
                while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    msOut.Write(buffer, 0, bytesRead);
                }

                msOut.Position = 0;
                StreamReader sr = new StreamReader(msOut);
                string decrypted = sr.ReadToEnd();

                //now compare the decrypted string with the already decrypted local file
                FileStream fs = new FileStream(decryptedFile, FileMode.OpenOrCreate);
                StreamReader srConfig = new StreamReader(fs);
                string config = srConfig.ReadToEnd();
                msOut.Close();
                fs.Close();

                filestreamEncrypted.Close();
                if (config != decrypted)
                {
                    FormCompare compare = new FormCompare();
                    compare.LocalFileContent = config;
                    compare.ServerFileContent = decrypted;
                    compare.LocalFileName = decryptedFile;
                    compare.ServerFileName = encryptedFile;
                    
                    bool runningInTeamcity = (!Environment.UserInteractive);

                    if (!runningInTeamcity)
                    {
                        try
                        {
                            compare.HighlightCompare();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        
                        compare.ShowDialog();
                    }

                    if (compare.UseWhichFile == UseWhichFileEnum.Server || runningInTeamcity )
                    {
                        //replace the local file with the one we just decrypted
                        File.WriteAllText(decryptedFile,decrypted);
                    }
                    else
                    {
                        //encrypt the local and replace the .encrypted one
                        FileEncrypt(decryptedFile, encryptedFile, password);
                    }
                }

            }
            catch (CryptographicException ex_CryptographicException)
            {
                Console.WriteLine("CryptographicException error: " + ex_CryptographicException.Message);
                Environment.ExitCode = -3;
            }
            catch (Exception ex)
            {
                Environment.ExitCode = -2;
                Console.WriteLine("Error: " + ex.Message);
            }

            try
            {
                cs.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error closing CryptoStream: " + ex.Message);
                Environment.ExitCode = ExitCryptographicError;
            }
            finally
            {
                msOut.Close();
                filestreamEncrypted.Close();
            }
        }
    }
}
