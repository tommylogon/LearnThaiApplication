using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public class Vowel : ThaiBase
    {
        public Vowel()
        {
        }

        public Vowel(List<string> thaiSymbol, List<string> thaiFonet, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;

            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
        }

        public Vowel(List<string> thaiSymbol, List<string> thaiFonet, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;

            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;
        }
    }
}