using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public class Word : ThaiBase
    {
        string engDesc;
        string chapter;

        public Word()
        {

        }
        public Word(string thaiWord, string thaiFonet, List<String> engWords, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engWords;

            this.engDesc = engDesc;
            this.chapter = Chapter;

        }
        public Word(string thaiWord, string thaiFonet, string engword, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWords = engword.Split(';', ',').ToList<String>();
            this.engDesc = engDesc;
            this.chapter = Chapter;

        }

        public string Chapter
        {
            get { return chapter; }
            set { chapter = value; }
        }
    }
}
