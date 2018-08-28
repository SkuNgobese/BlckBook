using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class IdentityInfo
    {
        public IdentityInfo(string identityNumber)
        {
            this.Initialize(identityNumber);
        }

        public string IdentityNumber { get; private set; }

        public DateTime BirthDate { get; private set; }

        public String Gender { get; private set; }

        public int Age { get; private set; }

        public string AgeToLongString { get; private set; }

        public bool IsSouthAfrican { get; private set; }

        public bool IsValid { get; private set; }

        private void Initialize(string identityNumber)
        {
            this.IdentityNumber = (identityNumber ?? string.Empty).Replace(" ", "");
            if (this.IdentityNumber.Length == 13)
            {
                var digits = new int[13];
                for (int i = 0; i < 13; i++)
                {
                    digits[i] = int.Parse(this.IdentityNumber.Substring(i, 1));
                }
                int control1 = digits.Where((v, i) => i % 2 == 0 && i < 12).Sum();
                string second = string.Empty;
                digits.Where((v, i) => i % 2 != 0 && i < 12).ToList().ForEach(v =>
                      second += v.ToString());
                var string2 = (int.Parse(second) * 2).ToString();
                int control2 = 0;
                for (int i = 0; i < string2.Length; i++)
                {
                    control2 += int.Parse(string2.Substring(i, 1));
                }
                var control = (10 - ((control1 + control2) % 10))% 10;
                if (digits[12] == control)
                {
                    this.BirthDate = DateTime.ParseExact(this.IdentityNumber
                        .Substring(0, 6), "yyMMdd", null);
                    this.Gender = digits[6] < 5 ? "Female" : "Male";
                    this.IsSouthAfrican = digits[10] == 0;
                    this.Age = CalculateAge(BirthDate);
                    this.AgeToLongString = CalculateAgeToLongString(BirthDate);
                    this.IsValid = true;
                }
            }
        }

        private int CalculateAge(DateTime birthDay)
        {
            DateTime today = DateTime.Today;
            int age = age = new DateTime(today.Subtract(birthDay).Ticks).Year - 1;

            return age;
        }
        private string CalculateAgeToLongString(DateTime birthDay)
        {
            TimeSpan difference = DateTime.Now.Subtract(birthDay);
            DateTime currentAge = DateTime.MinValue + difference;
            int years = currentAge.Year - 1;
            int months = currentAge.Month - 1;
            int days = currentAge.Day - 1;

            return String.Format("{0} years, {1} months and {2} days.", years, months, days);
        }
    }
}