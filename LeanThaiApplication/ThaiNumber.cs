using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public class ThaiNumber : ThaiSymbol
    {
        public ThaiNumber()
        {
        }

        public ThaiNumber(List<string> thaiSymbol, List<string> thaiFonet, string thaiHelpWord, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
        }

        public ThaiNumber(List<string> thaiSymbol, List<string> thaiFonet, string thaiHelpWord, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;
        }
        
    }
}