using System.Security.Cryptography;

namespace BoxOwlWeb.Utils {
    public static class Utils {
        public static byte[] GetRandomSalt(int length) {
            var random = new RNGCryptoServiceProvider();
            var salt = new byte[length];
            random.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] SaltHashPassword(byte[] password, byte[] salt) {
            var algorithm = new SHA256CryptoServiceProvider();
            var plainTextWithSaltBytes = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++) {
                plainTextWithSaltBytes[i] = password[i];
            }
            for (int i = 0; i < salt.Length; i++) {
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            }
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}