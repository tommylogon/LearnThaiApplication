using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public class ThaiNumber : ThaiBase
    {
        public ThaiNumber()
        {
        }

        public ThaiNumber(List<string> thaiSymbol, List<string> thaiFonet, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
        }

        public ThaiNumber(List<string> thaiSymbol, List<string> thaiFonet, string thaiHelpWord, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;
        }
    }
}