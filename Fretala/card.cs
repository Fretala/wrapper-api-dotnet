using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fretala
{
    public class Card
    {
        private string name;
        private string number;
        private string expDate;
        private string cvv;
        private string token;
        private string cardBrand;
        private string lastDigits;

        public Card(string name, string number, string expDate, string cvv)
        {
            this.name = name;
            this.number = number;
            this.expDate = expDate;
            this.cvv = cvv;
        }

        public Card(string token, string cardBrand, string lastDigits)
        {
            this.token = token;
            this.cardBrand = cardBrand;
            this.lastDigits = lastDigits;
        }
    }
}