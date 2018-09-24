using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public abstract class ThaiSymbol : ThaiBase
    {
        string thaiHelpWord;

        public string ThaiHelpWord
        {
            get { return thaiHelpWord; }
            set { thaiHelpWord = value; }

        }
    }
}
