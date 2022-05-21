using CUFETestTask.API.Data.DBContext;
using CUFETestTask.API.Data.Models;
using System.Security.Cryptography;
using System.Text;

namespace CUFETestTask.API.Services
{
    public class UserService : IUserService
    {
        readonly UserDbContext _userDbContext;
        public UserService(UserDbContext context)
        {
            _userDbContext = context;
        }
        public async Task AddNewUser(UserModel user)
        {
            user.Address = EncryptText(user.Address, user.Password);
            user.BirthDate = EncryptText(user.BirthDate, user.Password);
            user.Occupation = EncryptText(user.Occupation, user.Password);
            user.Password = EncryptPassword(user.Password);

            _userDbContext.Users.Add(user);

            _userDbContext.SaveChanges();

        }

        public async Task<UserModel?> GetUserData(UserModel searchUser)
        {
            UserModel validateUser = _userDbContext.Users.Where(u => u.UserName == searchUser.UserName).FirstOrDefault();
            if (validateUser == null)
            {
                return null;
            }
            else
            {
                if (validateUser.Password == EncryptPassword(searchUser.Password))
                {
                    validateUser.Address = DecryptText(validateUser.Address, searchUser.Password);
                    validateUser.Occupation = DecryptText(validateUser.Occupation, searchUser.Password);
                    validateUser.BirthDate = DecryptText(validateUser.BirthDate, searchUser.Password);
                    return validateUser;
                }
                else
                {
                    return null;
                }
            }
        }
        public async Task<bool> ResetPassword(UserModel searchUser, string newPassword)
        {
            UserModel validateUser = _userDbContext.Users.Where(u => u.UserName == searchUser.UserName).FirstOrDefault();
            if (validateUser == null)
            {
                return false;
            }
            else
            {
                if (validateUser.Password == EncryptPassword(searchUser.Password))
                {
                    validateUser.Address = DecryptText(validateUser.Address, searchUser.Password);
                    validateUser.Occupation = DecryptText(validateUser.Occupation, searchUser.Password);
                    validateUser.BirthDate = DecryptText(validateUser.BirthDate, searchUser.Password);

                    validateUser.Address = EncryptText(validateUser.Address, newPassword);
                    validateUser.BirthDate = EncryptText(validateUser.BirthDate, newPassword);
                    validateUser.Occupation = EncryptText(validateUser.Occupation, newPassword);
                    validateUser.Password = EncryptPassword(newPassword);

                    _userDbContext.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string EncryptPassword(string password)
        {
            var stringToHash = Encoding.UTF8.GetBytes(password);
            using (var alg = SHA512.Create())
            {
                string hex = "";

                var hashValue = alg.ComputeHash(stringToHash);
                foreach (byte x in hashValue)
                {
                    hex += String.Format("{0:x2}", x);
                }
                return hex;
            }
        }
  
        private static string EncryptText(string text, string password)
        {
            byte[] iV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            int BlockSize = 128;
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.BlockSize = BlockSize;
            crypt.IV = iV;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
        private static string DecryptText(string text, string password)
        {
            byte[] iV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
            byte[] bytes = Convert.FromBase64String(text);
            SymmetricAlgorithm crypt = Aes.Create();
            HashAlgorithm hash = MD5.Create();
            crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(password));
            crypt.IV = iV;
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                using (CryptoStream cryptoStream =
                   new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[bytes.Length];
                    cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                  
                    return Encoding.Unicode.GetString(decryptedBytes).Replace("\u0000",String.Empty);
                }
            }
        }
    }
}
