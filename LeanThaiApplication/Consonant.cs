using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public class Consonant : ThaiBase
    {
        public Consonant()
        {
        }

        public Consonant(List<string> thaiSymbol, List<string> thaiFonetical, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiFonet = thaiFonetical;
            this.EngWords = engWords;
        }

        public Consonant(List<string> thaiSymbol, List<string> thaiFonetical, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiFonet = thaiFonetical;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;
        }
    }
}