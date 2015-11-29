using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NumbersToWordsConverter
{
    /// <summary>
    /// 
    /// </summary>
    public static class NumberToWords
    {
        private static readonly Dictionary<int, string> PLdictNbr = new Dictionary<int, string>()
        {
            [0] = "zero",
            [1] = "jeden",
            [2] = "dwa",
            [3] = "trzy",
            [4] = "cztery",
            [5] = "pięć",
            [6] = "sześć",
            [7] = "siedem",
            [8] = "osiem",
            [9] = "dziewięć",
            [10] = "dziesięć",
            [11] = "jedenaście",
            [12] = "dwanaście",
            [13] = "trzynaście",
            [14] = "czternaście",
            [15] = "piętnaście",
            [16] = "szesnaście",
            [17] = "siedemnaście",
            [18] = "osiemmnaście",
            [19] = "dziewięćnaście",
            [20] = "dwadzieścia",
            [30] = "trzydzieści",
            [40] = "czterdzieści",
            [50] = "pięćdziesiąt",
            [60] = "szcześćdziesiąt",
            [70] = "siedemdziesiąt",
            [80] = "osiedziesiąt",
            [90] = "dziewięćdziesiąt",
            [100] = "sto",
            [200] = "dwieście",
            [300] = "trzysta",
            [400] = "czterysta",
            [1000] = "tysiąc",
            [1000000] = "milion",

        };

        private static readonly Dictionary<int, string> PLdictDeclination = new Dictionary<int, string>()
        {
            [100] = "set",
            [1000] = "tysięcy",
            [2000] = "tysiące",
            [1000000] = "milionów",
            [2000000] = "miliony",
        };
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_number"></param>
        /// <returns></returns>
        public static string ToPolish(int _number)
        {
            var z = DivideToHundreds(_number).Select((x, i) => i == 0 ? HundredsToPolish(x) : string.Format($"{HundredsToPolish(x)} {ToPolishDeclination(x, i)}"))
                                             .Reverse();

            return Regex.Replace(string.Join(" ", z), @"\s+", " "); //Remove all unnessesary spaces
        }

        #region auxiliary method for ToPolish method
        private static string ToPolishDeclination(int _number, int magni)
        {

            var nb = _number % 10;
            var mangn = (int)Math.Pow(1000, magni);

            if (_number == 0)
                return string.Empty;

            if (nb == 1)
                return (_number * mangn == mangn ? PLdictNbr[mangn] : PLdictDeclination[mangn]);

            else if (nb > 1 && nb < 5)
                return PLdictDeclination[2 * mangn];

            else
                return PLdictDeclination[mangn];

        }

        private static int[] DivideToHundreds(int _number)
        {
            var nb = _number;
            //Numbers of digits
            var a = (int)Math.Log10(_number);
            //Divide number to hundres groups
            int[] hg = new int[(int)Math.Round(0.5 + (a + 1) / 3, MidpointRounding.AwayFromZero)];

            int j = 0;
            for (int i = 1; i <= a + 1; i = i + 3)
            {
                hg[j] = nb % 1000;
                nb = nb / 1000;
                j++;
            }

            return hg;
        }

        private static string HundredsToPolish(int _number)
        {
            if (_number == 0)
                return string.Empty;

            string hundredRow = string.Empty;
            string tensRow = string.Empty;

            var hr = _number / 100;
            var tr = _number % 100;

            if (hr != 0 && !PLdictNbr.TryGetValue(hr * 100, out hundredRow))
                hundredRow = PLdictNbr[hr] + PLdictDeclination[100];

            if (tr != 0 && !PLdictNbr.TryGetValue(tr, out tensRow))
                tensRow = PLdictNbr[(int)(tr / 10) * 10] + (tr % 10 == 0 ? string.Empty : " " + PLdictNbr[tr % 10]);

            return string.Format($"{hundredRow} {tensRow}");
        }
        #endregion

    }
}
