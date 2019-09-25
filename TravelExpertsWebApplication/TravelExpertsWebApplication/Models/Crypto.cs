using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

/* Author: Robbie Nielsen */

namespace TravelExpertsWebApplication.Models
{
    // For password hashing
    public static class Crypto
    {
        public static string CreateSalt(int size)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            rng.GetBytes(buff);
            return Convert.ToBase64String(buff);
        }

        // hash function with salt: yields different results for the same password
        public static string Hash(string value, string salt)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                // need  to convert the string to an array of bytes first; then apply MD5 has algorithm
                UTF8Encoding utf8 = new UTF8Encoding(); // Unicode standard
                byte[] data = md5.ComputeHash(utf8.GetBytes(value + salt)); // hash password + salt
                return Convert.ToBase64String(data);
            }
            // in the database, store username, salt, and hash of the password plus salt
            // when user enters username and password, add salt to the password,
            // hash and and compare with stored hash for this user
        }

        public static string HashNoSalt(string value)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                // need  to convert the string to an array of bytes first; then apply MD5 has algorithm
                UTF8Encoding utf8 = new UTF8Encoding(); // Unicode standard
                byte[] data = md5.ComputeHash(utf8.GetBytes(value)); // hash password + salt
                return Convert.ToBase64String(data);
            }
            // in the database, store username, salt, and hash of the password plus salt
            // when user enters username and password, add salt to the password,
            // hash and and compare with stored hash for this user
        }
    }
}
