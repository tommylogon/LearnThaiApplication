using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public class Word : ThaiBase
    {
        public Word()
        {
        }

        public Word(List<string> thaiWord, List<string> thaiFonet, List<String> engWords, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;

            this.EngDesc = engDesc;
            this.Chapter = Chapter;
        }

        public string Chapter { get; set; }
    }
}