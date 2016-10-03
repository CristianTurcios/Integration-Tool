using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ClassLibrary;

namespace ClassLibrary.Test
{
    [TestFixture]
    class EncryptTest
    {
        // [TestCase("ASDFGHJKLÑ")]
        // [TestCase("LAUREATE INTERNATIONAL UNIVERSITIES")]
        // [TestCase("LNO HONDURAS")]
        [TestCase("ActiveDirectory")]
        public void Encrypt_Encrypt_ReturnDataEncrypted(string data)
        {
            Encrypt encrypt = new Encrypt();
            string dataEncrypted = encrypt.encryptData(data);

            Console.WriteLine(dataEncrypted);
        }

        [TestCase("OsTpUBzUugIKmVIYROVgLwPywjDKQwDgWXrLrt7mW8o+6VnJAxm/dA==")]
        [TestCase("pGlWg2/fsRDRXffk+X7j6w==")]
        [TestCase("hHeKw3V54pvY3aYX8490zg==")]
        public void Encrypt_Decrypt_ReturnDataDecripted(string encriptedData)
        {
            Encrypt encrypt = new Encrypt();
            string decryptedDataResult = encrypt.decryptData(encriptedData);
            // Assert.AreEqual(decryptedDataResult, decryptedData);

            Console.WriteLine(decryptedDataResult);
        }
    }
}
