using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EncryptionDecryptionUsingSymmetricKey;
using HelperClasses.Classes;

namespace WebAPI.Helpers
{
    public class DBConnectionStringBuilder
    {
        public static string GetConnectionString(IConfiguration Config, string EntryName = "DBConnectionString")
        {
            string ConnectionString = string.Empty;

            IConfigurationSection Section = Config.GetSection(EntryName);

            if (Section != null)
            {
                ConnectionString = "Data Source=" + Section.GetValue<string>("Data Source");
                ConnectionString += ";Initial Catalog=" + Section.GetValue<string>("Initial Catalog");
                ConnectionString += ";User=" + Section.GetValue<string>("User");

                string Password = Section.GetValue<string>("Password");

                if (Section.GetValue<bool>("IsEncrypted", false))
                    ConnectionString += ";Password=" + AesOperations.DecryptString(GlobalObjects.PasswordKey, Password);
                else
                    ConnectionString += ";Password=" + Password;
            }

            return ConnectionString;
        }
    }
}
