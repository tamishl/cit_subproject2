using System.Security.Cryptography;
using System.Text;

namespace DataServiceLayer.Services
{
    public class Hashing
    {
        protected const int saltBitsize = 64;
        protected const byte saltBytesize = saltBitsize / 8;
        protected const int hashBitsize = 256;
        protected const int hashBytesize = hashBitsize / 8;

        private HashAlgorithm sha256 = SHA256.Create();
        protected RandomNumberGenerator rand = RandomNumberGenerator.Create();

        // hash(string password)
        // called from Authenticator.register()
        // where salt and hashed password have not been generated,
        // so both are returned for storing in the password database

        public (string hash, string salt) Hash(string password)
        {
            byte[] salt = new byte[saltBytesize];
            rand.GetBytes(salt);
            string saltString = Convert.ToHexString(salt);
            string hash = HashSHA256(password, saltString);
            return (hash, saltString);
        }


        public bool Verify(string loginPassword, string hashedRegisteredPassword, string saltString)
        {
            string hashedLoginPassword = HashSHA256(loginPassword, saltString);
            if (hashedRegisteredPassword == hashedLoginPassword) return true;
            else return false;
        }

        // hashSHA256 is the "workhorse" --- the actual hashing

        public byte[] ComputeIteratedHash(byte[] data, int reps)
        {
            byte[] hashedData = data;
            for (int i = 0; i < reps; i++)
            {
                hashedData = sha256.ComputeHash(hashedData);
            }
            return hashedData;
        }


        private string HashSHA256(string password, string saltString)
        {
            byte[] hashInput = Encoding.UTF8.GetBytes(saltString + password);
            byte[] hashOutput = ComputeIteratedHash(hashInput, 1000000);
            return Convert.ToHexString(hashOutput);
        }
    }
}
