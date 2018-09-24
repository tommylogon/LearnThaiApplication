using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public class Consonant : ThaiSymbol
    {
        public Consonant()
        {

        }
        public Consonant(string thaiSymbol, string thaiFonetical, string thaiHelpWord, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWords = engWords;

        }
        public Consonant(string thaiSymbol, string thaiFonetical, string thaiHelpWord, string engWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWord = engWord;
            this.EngDesc = englishDescription;

        }
        public Consonant(string thaiSymbol, string thaiFonetical, string thaiHelpWord, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;

        }
    }
}
