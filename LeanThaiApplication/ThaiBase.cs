using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public abstract class ThaiBase
    {
        string thaiScript;
        string thaiFonet;
        List<String> engWords;
        string engWord;
        string engDesc;


        public string ThaiScript
        {
            get { return thaiScript; }
            set { thaiScript = value; }
        }
        public string ThaiFonet
        {
            get { return thaiFonet; }
            set { thaiFonet = value; }
        }
        public List<String> EngWords
        {
            get { return engWords; }
            set { engWords = value; }
        }
        public string EngWord
        {
            get { return engWord; }
            set { engWord = value; }
        }

        public string EngDesc
        {
            get { return engDesc; }
            set { engDesc = value; }
        }
    }
}
