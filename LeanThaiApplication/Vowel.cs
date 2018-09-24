﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public class Vowel : ThaiSymbol
    {

        public Vowel()
        {

        }
        public Vowel(string thaiSymbol, string thaiFonet, string thaiHelpWord, List<String> engWords)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
        }
        public Vowel(string thaiSymbol, string thaiFonet, string thaiHelpWord, string engWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = engWord;
            this.EngDesc = englishDescription;
        }
        public Vowel(string thaiSymbol, string thaiFonet, string thaiHelpWord, List<String> engWords, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;
            this.EngDesc = englishDescription;
        }


    }
}
