using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TravelExpertsWebApplication.Models
{
    public class CardValidator
    {
        /* Checks if entered credit card number is valid using Luhn's Formula
         * From https://www.codeproject.com/Articles/2782/Credit-Card-Validator-control-for-ASP-NET 
         * Returns true if card number is valid
         * I cleaned it up a little */
        public static bool isValidCreditCard(string cardNumber)
        {
            try
            {
                // Array to contain individual numbers
                ArrayList CheckNumbers = new ArrayList();
                // Get length of card
                int CardLength = cardNumber.Length;

                /* Double the value of alternate digits, starting with the second digit from the right,
                 * i.e. back to front. Loop through starting at the end */
                for (int i = CardLength - 2; i >= 0; i = i - 2)
                {
                    /* Now read the contents at each index, this can then be stored as an array of integers

                     * Double the number returned */
                    CheckNumbers.Add(Int32.Parse(cardNumber[i].ToString()) * 2);
                }

                int CheckSum = 0; // Will hold the total sum of all checksum digits

                // Second stage, add separate digits of all products
                for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++)
                {
                    int _count = 0; // will hold the sum of the digits

                    // Determine if current number has more than one digit
                    if ((int)CheckNumbers[iCount] > 9)
                    {
                        int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
                        // add count to each digit
                        for (int x = 0; x < _numLength; x++)
                        {
                            _count = _count + Int32.Parse(
                                  ((int)CheckNumbers[iCount]).ToString()[x].ToString());
                        }
                    }
                    else
                    {
                        // single digit, just add it by itself
                        _count = (int)CheckNumbers[iCount];
                    }
                    CheckSum = CheckSum + _count; // add sum to the total sum
                }
                /* Stage 3, add the unaffected digits
                 * Add all the digits that we didn't double still starting from the right but this time we'll
                 * start from the rightmost number with alternating digits */
                int OriginalSum = 0;
                for (int y = CardLength - 1; y >= 0; y = y - 2)
                {
                    OriginalSum = OriginalSum + Int32.Parse(cardNumber[y].ToString());
                }

                // Perform the final calculation, if the sum Mod 10 results in 0 then it's valid, otherwise its false.
                return (((OriginalSum + CheckSum) % 10) == 0);
            }
            catch
            {
                return false;
            }
        }

        /* Verifies that the card length is appropriate for the card type
         * Amex is 15 digits; Diners, Master Card, and Visa are 16
         * Returns true if so */
        public static bool isValidLength(CreditCard card)
        {
            // Check if card is Amex
            if (card.CCName == "AMEX")
            {
                // Check card number length
                if (card.CCNumber.Length == 15)
                {
                    return true;
                }
                else // If user says card is Amex but number length is not 15, the number is invalid
                {
                    return false;
                }
            }
            else // card is Diners, MC, or Visa
            {
                if (card.CCNumber.Length == 16)
                {
                    return true;
                }
                else // If user says card is Diners/MC/Visa but length is not 16, number is invalid
                {
                    return false;
                }
            }
        }

        /* Verifies that the card number is appropriate for the card type
         * Amex/Diners start with 37/38 respectively, Visa with 4, and Master Card with 2 or 5
         * Returns true if so */
        public static bool isValidType(CreditCard card)
        {
            if (card.CCName == "AMEX")
            {
                // Check if first 2 digits are 37
                string str = card.CCNumber.Substring(0, 2);
                if (str == "37")
                    return true;
                else
                    return false;
            }
            else if (card.CCName == "Diners")
            {
                // Check if first 2 digits are 38
                string str = card.CCNumber.Substring(0, 2);
                if (str == "38")
                    return true;
                else
                    return false;
            }
            else if (card.CCName == "MC")
            {
                // Check if first digit is 2 or 5
                string str = card.CCNumber.Substring(0, 1);
                if (str == "2" || str == "5")
                    return true;
                else
                    return false;
            }
            else // Visa
            {
                // Check if first digit is 4
                string str = card.CCNumber.Substring(0, 1);
                if (str == "4")
                    return true;
                else
                    return false;
            }
        }

        /* Check if expiry date is valid
         * The jQuery datepicker will point to the first day of the month, but cards
         * usually expire at the end of the month, so we'll use that day to compare
         * Argument must be nullable, like the equivalent field in the model */
        public static bool isValidExpiry(DateTime? expiry)
        {
            DateTime now = DateTime.Now;
            DateTime firstDay = new DateTime(now.Year, now.Month, 1); // First day of current month
            if (expiry < firstDay) // If expiry date is before current month
                return false;
            else
                return true;
        }
    }
}