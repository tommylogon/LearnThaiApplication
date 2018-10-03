using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnThaiApplication
{
    public class Word : ThaiBase
    {
        public Word()
        {
        }

        public Word(string thaiWord, string thaiFonet, List<String> engWords, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;

            this.EngDesc = engDesc;
            this.Chapter = Chapter;
        }

        public Word(string thaiWord, string thaiFonet, string engword, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engword.Split(';', ',').ToList<String>();
            this.EngDesc = engDesc;
            this.Chapter = Chapter;
        }

        public string Chapter { get; set; }
    }
}