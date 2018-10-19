using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication
{
    public class WordIDTEMP
    {
        public WordIDTEMP()
        {

        }
        public WordIDTEMP(string id)
        {
            this.WordID = id;
        }
        public string WordID { get; set; }



//        endValue = doc.DocumentNode.SelectNodes("//td[@class = 'tz']/a").Count;
//            for (int i = startValue; i<endValue; i++)
//            {
//                string wordID = doc.DocumentNode.SelectNodes("//td[@class = 'tz']/a")[i].Attributes[0].Value;
//        var correctText = doc.DocumentNode.SelectNodes("//td[@class = 'tz']/a")[i].InnerHtml;
//                foreach (Word word in Words)
//                {
//                    if(correctText == word.ThaiScript)
//                    {
//                        WordIDTEMP temp = new WordIDTEMP(wordID);

//        listOfCorrect.Add(temp);
//                        SaveFiles<WordIDTEMP>(listOfCorrect);
//                    }
//}
                
//            }

    }
}
