using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/* Author: Robbie Nielsen */

namespace TravelExpertsWebApplication.Models
{
    public static class RandomStringGenerator
    {
        /* Creates a random string of letters and numbers, with specified length.
         * Uses LINQ */
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            // I, O, L, 0 and 1 not available for selection to avoid any ambiguity with fonts
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
