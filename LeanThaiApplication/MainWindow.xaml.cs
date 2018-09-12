using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Reflection;
using Path = System.IO.Path;

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		static List<Word> words = new List<Word>()
		{/*
			//page 8 and 9
			new Word("นั่ง", "nang", "Sit down", "The a is pronounced like bar, car or cut"),
			new Word("", "", "Arm", ""),
			new Word("", "", "Heart", ""),
			new Word("", "", "Black", ""),
			new Word("", "", "Duck", ""),
			new Word("", "", "Beer", ""),
			new Word("", "", "Hit", ""),
			new Word("", "", "Music", ""),
			new Word("", "", "Crow", ""),
			new Word("", "", "Swan", ""),
			new Word("", "", "Tounge", ""),
			new Word("", "", "Table", ""),
			new Word("", "", "Walk", ""),
			new Word("", "", "Screen", ""),
			new Word("", "", "Jail", ""),
			new Word("", "", "Throw", ""),
			new Word("", "", "Ticket", ""),
			new Word("", "", "Hand", ""),
			new Word("", "", "City","")

			//page 32 and 33*/

		};
        static List<Consonant> consonants = new List<Consonant>()
		{
			new Consonant("ก","ไก่","gai","Chicken"),
			new Consonant("ข","ไข่","kai",""),
			new Consonant("ฃ","ขวด","kuad",""),
			new Consonant("ค","ควาย","kwaai",""),
			new Consonant("ฅ","คน","kon",""),
			new Consonant("ฆ","ระฆัง","ra kang",""),
			new Consonant("ง","งู","ngoo",""),
			new Consonant("จ","จาน","jaan",""),
			new Consonant("ฉ","ฉิ่ง","ching",""),
			new Consonant("ช","ช้าง","chaang",""),
			new Consonant("ซ","โซ่","soo",""),
			new Consonant("ฌ","เฌอ","chøø",""),
			new Consonant("ญ","ผู้หญิง","puu ying",""),
			new Consonant("ฎ","ชฎา","cha daa",""),
			new Consonant("ฏ","ปฏัก","ba dag",""),
			new Consonant("ฐ","ฐาน","taan",""),
			new Consonant("ฑ","มณโฑ","montoo",""),
			new Consonant("ฒ","ผู้เฒ่า","puu tow",""),
			new Consonant("ณ","เณร","neen",""),
			new Consonant("ด","เด็ก","deg",""),
			new Consonant("ต","เต่า","tow",""),
			new Consonant("ถ","ถุง","tung",""),
			new Consonant("ท","ทหาร","tahaan",""),
			new Consonant("ธ","ธง","tong",""),
			new Consonant("น","หนู","nuu",""),
			new Consonant("บ","ใบไม้","bai maai",""),
			new Consonant("ป","ปลา","blaa",""),
			new Consonant("ผ","ผึ้ง","pung",""),
			new Consonant("ฝ","ฝา","faa",""),
			new Consonant("พ","พาน","paan",""),
			new Consonant("ฟ","ฟัน","fan",""),
			new Consonant("ภ","สำเภา","sampow",""),
			new Consonant("ม","ม้า","maa",""),
			new Consonant("ย","ยักษ์","yag",""),
			new Consonant("ร","เรือ","rua",""),
			new Consonant("ล","ลิง","ling",""),
			new Consonant("ว","เเหาน","wææn",""),
			new Consonant("ศ","ศาลา","saalaa",""),
			new Consonant("ษ","ฦๅษี","ruu see",""),
			new Consonant("ส","เสือ","sua",""),
			new Consonant("ห","หีบ","heeb",""),
			new Consonant("ฬ","ขุฬา","ju laa",""),
			new Consonant("อ","อ่าง","aang",""),
			new Consonant("ฮ","นกฮูก","nok hoog","Owl"),
		};
        static List<Vowel> vowels = new List<Vowel>()
		{
			new Vowel("ั","กับ","gap","And, Also"),
			new Vowel("-า","บาด","baad",""),
			new Vowel("แ-ะ","แกะ","",""),
			new Vowel("แ","แดง","",""),
			new Vowel("เ-ะ","เตะ","",""),
			new Vowel("เ-","เพลง","",""),
			new Vowel("ิ","คิด","",""),
			new Vowel("ี","อีก","",""),
			new Vowel("โ-ะ","โต๊ะ","",""),
			new Vowel("โ-","โชค","",""),
			new Vowel("เ-าะ","เกาะ","",""),
			new Vowel("อ","นอน","",""),
			new Vowel("เ-อะ","เยอะ","",""),
			new Vowel("เ-อ","เผลอ","",""),
			new Vowel("ุ","จุฬา","",""),
			new Vowel("ู","ลูก","",""),
			new Vowel("ึ","รึ","",""),
			new Vowel("ื","กลืน","",""),
			new Vowel("ไ-","ไม่","",""),
			new Vowel("ใ-","ใส่","",""),
			new Vowel("ำ","ทำ","",""),
			new Vowel("เ-า","เรา","",""),
			new Vowel("เียะ","เดี๊ยะ","",""),
			new Vowel("เีย","เมีย","",""),
			new Vowel("ัว","ตัว","",""),
			new Vowel("ัวะ","ผลัวะ","",""),
			new Vowel("เือะ","เอือะ","",""),
			new Vowel("เือ","เรือ","",""),
			new Vowel("ฦ","ฦาชา","",""),
			new Vowel("ฦๅ","","",""),
			new Vowel("ฤ","ฤทธ์","",""),
			new Vowel("ฤๅ","ฤๅษี","",""),

			new Vowel("-อย","บ่อย","",""),
			new Vowel("เ็ว","เร็ว","",""),
			new Vowel("เียว","เที่ยว","",""),
			new Vowel("เืยว","เหนื่อย","",""),
			new Vowel("ิว","หิว","",""),
			new Vowel("ุย","คุย","","")
		};

        
        
        //<>
        int i = 0;

        bool isRandom;

        

        public MainWindow()
        {
            InitializeComponent();

			clearFields();

            System.Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            lbl_Counter.Content = i;
            loadFiles<Word>(words);
            loadFiles<Consonant>(consonants);
            
        }

		private void btn_validate_Click(object sender, RoutedEventArgs e)
		{

			if (rb_Conson.IsChecked == true)
			{
				if (consonants[i].ThaiFonet == txt_Answear.Text)
				{
					txb_Status_Page2.Text = "Correct!";
				}
				else
				{
					txb_Status_Page2.Text = "Wrong...";
				}
				txb_Description_page2.Text = consonants[i].ThaiFonet;
			}
			else if(rb_Vowel.IsChecked == true)
			{
				if (vowels[i].ThaiFonet == txt_Answear.Text)
				{
					txb_Status_Page2.Text = "Correct!";
				}
				else
				{
					txb_Status_Page2.Text = "Wrong...";

					System.Console.WriteLine(vowels[i].ThaiFonet);
				}
				txb_Description_page2.Text = vowels[i].ThaiFonet;
			}


			lbl_Counter.Content = i;
		}

		private void btn_Next_Word_Click(object sender, RoutedEventArgs e)
		{
			clearFields();
			i++;

			if (rb_Conson.IsChecked == true)
            {
                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, true);
			}
			else if (rb_Vowel.IsChecked == true)
			{
                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, true);
            }
			else
			{
				txb_Information.Text += Environment.NewLine + "please select one or the other";
			}
			lbl_Counter.Content = i;
			

		}

		private void btn_Prev_Word_Click(object sender, RoutedEventArgs e)
		{
			clearFields();
			if (rb_Conson.IsChecked == true)
			{
                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, false);

            }
			else if(rb_Vowel.IsChecked == true)
			{
                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, false);
				
			}
			else
			{
				txb_Information.Text += Environment.NewLine + "please select one or the other";
			}
			lbl_Counter.Content = i;
		}
	
		public void clearFields()
		{
			txb_Status_Page2.Text = "";
			txb_ThaiScript_Page1.Text = "";
			txb_ThaiScript_Page2.Text = "";
			txt_Answear.Text = "";
			txt_Answear_Page1.Text = "";
			txt_NewThaiScript.Text = "";
			txt_NewThaiFonet.Text = "";
			txt_NewEnglish.Text = "";
			txt_NewDescr.Text = "";

		}

		public void textChanger<T>(List<T> list, TextBlock textBlock, bool nextIsForward) where T : new()
        {
            //textBlock.Text = recievedText;
            /*Type whatIsT = typeof(T);T whatKindOfObject
           PropertyInfo propInf = whatIsT.GetProperty("ThaiScript");

           if (whatIsT == typeof(Word))
           {
               */
            if (nextIsForward)
            {
                try
                {

                    if (i >= 0)
                    {
                        if (i < list.Count)
                        {

                            textBlock.Text = list[i].GetType().GetProperty("ThaiScript").ToString();// + " " + list[i].GetType().GetProperty("ThaiHelpWord").ToString();

                        }
                        else
                        {
                            i = 0;
                            textBlock.Text = list[i].GetType().GetProperty("ThaiScript").ToString() + " " + list[i].GetType().GetProperty("ThaiHelpWord").ToString();

                        }
                    }
                    else
                    {
                        i = 0;
                        textBlock.Text = list[i].GetType().GetProperty("ThaiScript").ToString() + " " + list[i].GetType().GetProperty("ThaiHelpWord").ToString();
                    }

                }
                catch (System.ArgumentOutOfRangeException error)
                {

                    System.Console.WriteLine(error.Message);
                }
            }
            else
            {
                try
                {
                    i--;
                    if (i < 0)
                    {
                        i = list.Count - 1;
                        textBlock.Text = list[i].GetType().GetProperty("ThaiScript").ToString() + " " + list[i].GetType().GetProperty("ThaiHelpWord").ToString();
                    }
                    else if (i < consonants.Count)
                    {

                        textBlock.Text = list[i].GetType().GetProperty("ThaiScript").ToString() + " " + list[i].GetType().GetProperty("ThaiHelpWord").ToString();
                    }


                }
                catch (System.ArgumentOutOfRangeException error)
                {

                    System.Console.WriteLine(error.Message);
                }
            }

            
                

            
        }

		private void rb_Conson_Checked(object sender, RoutedEventArgs e)
		{
			i = 0;
			txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
			lbl_Counter.Content = i;
		}

		private void rb_Vowel_Checked(object sender, RoutedEventArgs e)
		{
			i = 0;
			txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
			lbl_Counter.Content = i;
		}

		private void ckb_Randomized_Checked(object sender, RoutedEventArgs e)
		{
			isRandom = true;

		}

		private void btn_SubmitNewWord_Click(object sender, RoutedEventArgs e)
		{
			Word newWord = new Word(txt_NewThaiScript.Text, txt_NewThaiFonet.Text, txt_NewEnglish.Text, txt_NewDescr.Text);

			bool existsInList = false;


			foreach (Word oldWord in words)
			{
				if(newWord.ThaiScript == oldWord.ThaiScript && newWord.ThaiScript != "" || oldWord.EngWord == txt_NewEnglish.Text && newWord.EngWord != "")
				{
					System.Console.WriteLine("exists");
					existsInList = true;
					break;
				}
				else
				{
					System.Console.WriteLine("Does not exist");
					existsInList = false;
					
				}
			}

			if (existsInList == false)
			{
                if(newWord.ThaiScript != "" || newWord.EngWord != "")
                {
                    words.Add(newWord);
                    existsInList = true;
                }
                
				
			}

			foreach (Word oldWord in words)
			{
				System.Console.WriteLine(oldWord.ThaiScript + "; " + oldWord.ThaiFonet + "; " + oldWord.EngWord + "; " + oldWord.EngDesc);
				if (oldWord.ThaiScript == txt_NewThaiScript.Text && txt_NewThaiScript.Text != "" || oldWord.EngWord == txt_NewEnglish.Text && txt_NewEnglish.Text != "")
				{
                    
					oldWord.ThaiScript = txt_NewThaiScript.Text ;
					oldWord.ThaiFonet = txt_NewThaiFonet.Text ;
					oldWord.EngWord = txt_NewEnglish.Text ;
					oldWord.EngDesc = txt_NewDescr.Text ;
					clearFields();
					System.Console.WriteLine(oldWord.ThaiScript + "; " + oldWord.ThaiFonet + "; " + oldWord.EngWord + "; " + oldWord.EngDesc);
					break;
				}
				
			}

			



			XmlSerialization.WriteToXmlFile<List<Word>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_Words.xml", words);

            XmlSerialization.WriteToXmlFile<List<Consonant>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_Consonants.xml", consonants);

            XmlSerialization.WriteToXmlFile<List<Vowel>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_Vowels.xml", vowels);

        }

        public void loadFiles<T>(List<T> list ) where T : new()
        {
            Type whatIsT = typeof(T);
            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_" + whatIsT.Name +".xml");

           

            List<T> newWordToAdd = new List<T>();
            /*Type whatIsT = typeof(T);T whatKindOfObject
            PropertyInfo propInf = whatIsT.GetProperty("ThaiScript");
            
            if (whatIsT == typeof(Word))
            {
                */
            foreach (T wordFoundInFile in wordsFromFIle)
                {
                PropertyInfo propInFile = wordFoundInFile.GetType().GetProperty("ThaiScript");
                foreach (T WordAlreadyInList in list)
                    {
                        PropertyInfo propInList = WordAlreadyInList.GetType().GetProperty("ThaiScript");

                        if (propInList == propInFile)
                        {
                            System.Console.WriteLine("found word in list");
                            break;
                        }
                        else
                        {
                            System.Console.WriteLine("checking next");
                        }
                    }
                    newWordToAdd.Add(wordFoundInFile);

                }
                list.AddRange(newWordToAdd);
            //}

            


            /* List<Word> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<Word>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_Words.xml");
            


            List<Word> newWordToAdd = new List<Word>();

            foreach (Word wordFoundInFile in wordsFromFIle)
            {
                foreach (Word WordAlreadyInList in words)
                {
                    if (WordAlreadyInList.ThaiScript == wordFoundInFile.ThaiScript)
                    {
                        System.Console.WriteLine("found word in list");
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("checking next");
                    }
                }
                newWordToAdd.Add(wordFoundInFile);

            }
            words.AddRange(newWordToAdd);*/
        }

		private void btn_LoadList_Click(object sender, RoutedEventArgs e)
		{

			
			lib_LoadedWords.Items.Clear(); 
			foreach (Word word in words)
			{
				if (word.ThaiScript == "")
				{
					lib_LoadedWords.Items.Add(word.EngWord);
				}
				else
				{
					lib_LoadedWords.Items.Add(word.ThaiScript);
				}
			}
		}

		private void lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
		{
			
			string wordToLoad;
			
			if (lib_LoadedWords.SelectedIndex != -1)
			{
				wordToLoad = lib_LoadedWords.Items[lib_LoadedWords.SelectedIndex].ToString();
				txb_Description_Page4.Text = wordToLoad;

				foreach (Word word in words)
				{
					if (wordToLoad == word.ThaiScript || wordToLoad == word.EngWord)
					{
						txt_NewThaiScript.Text = word.ThaiScript;
						txt_NewThaiFonet.Text = word.ThaiFonet;
						txt_NewEnglish.Text = word.EngWord;
						txt_NewDescr.Text = word.EngDesc;

						txb_Description_Page4.Text = word.EngDesc;
					}
				}
			}
			
			

			
			


		}
	}







	public class ThaiToEnglish
	{
		string thaiScript;
		string thaiHelpWord;
		string thaiWord;
		string engPron;
		string engWord;


		public string ThaiScript
		{
			get { return thaiScript; }
			set { thaiScript = value; }
		}
		public string ThaiHelpWord
		{
			get { return thaiHelpWord; }
			set { thaiHelpWord = value; }

		}
		public string ThaiWord
		{
			get { return thaiWord; }
			set { thaiWord = value; }
		}
		public string EngPron
		{
			get { return engPron; }
			set { engPron = value; }
		}
		public string EngWord
		{
			get { return engWord; }
			set { engWord = value; }
		}
	}
	public class Word
	{
		string thaiScript;
		string thaiFonet;
		string engWord;
		string engDesc;


		public Word()
		{

		}
		public Word(string thaiWord, string thaiFonet, string engWord, string engDesc)
		{
			this.thaiScript = thaiWord;
			this.thaiFonet = thaiFonet;
			this.engWord = engWord;
			this.engDesc = engDesc;
		}
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
	public class Consonant
	{
		string thaiScript;
		string thaiHelpWord;
		
		string thaiFonet;
		string engDesc;
		
        public Consonant()
        {

        }
		public Consonant(string thaiSymbol, string thaiHelpWord, string thaiFonetical, string englishWord)
		{
			this.thaiScript = thaiSymbol;
			this.thaiHelpWord = thaiHelpWord;
			this.thaiFonet = thaiFonetical;
			this.engDesc = englishWord;

		}
		public string ThaiScript
		{
			get { return thaiScript;  }
			set { thaiScript = value; }
		}
		public string ThaiHelpWord
		{
			get { return thaiHelpWord; }
			set { thaiHelpWord = value;}

		}
		public string ThaiFonet
		{
			get { return thaiFonet; }
			set { thaiFonet = value;}
		}
		public string EngDesc
		{
			get { return engDesc; }
			set { engDesc = value; }
		}

	}
	public class Vowel
	{
		string thaiScript;
		string thaiHelpWord;	
		string thaiFonet;
		string engDesc;

        public Vowel()
        {

        }
		public Vowel(string thaiSymbol, string thaiHelpWord, string thaiFonet, string englishDescription)
		{
			this.thaiScript = thaiSymbol;
			this.thaiHelpWord = thaiHelpWord;
			this.thaiFonet = thaiFonet;
			this.engDesc = englishDescription;
		}
		public string ThaiScript
		{
			get { return thaiScript; }
			set { thaiScript = value; }
		}
		
		public string ThaiHelpWord
		{
			get { return thaiHelpWord; }
			set { thaiHelpWord = value; }

		}

		public string ThaiFonet
		{
			get { return thaiFonet; }
			set { thaiFonet = value; }
		}

		public string EngDesc
		{
			get { return engDesc; }
			set { engDesc = value; }
		}
	}

    //http://blog.danskingdom.com/saving-and-loading-a-c-objects-data-to-an-xml-json-or-binary-file/

    public static class XmlSerialization
	{
		/// <summary>
		/// Writes the given object instance to an XML file.
		/// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
		/// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
		/// <para>Object type must have a parameterless constructor.</para>
		/// </summary>
		/// <typeparam name="T">The type of object being written to the file.</typeparam>
		/// <param name="filePath">The file path to write the object instance to.</param>
		/// <param name="objectToWrite">The object instance to write to the file.</param>
		/// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
		public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
		{
			TextWriter writer = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				writer = new StreamWriter(filePath, append);
				serializer.Serialize(writer, objectToWrite);
			}
			finally
			{
				if (writer != null)
					writer.Close();
			}
		}

		/// <summary>
		/// Reads an object instance from an XML file.
		/// <para>Object type must have a parameterless constructor.</para>
		/// </summary>
		/// <typeparam name="T">The type of object to read from the file.</typeparam>
		/// <param name="filePath">The file path to read the object instance from.</param>
		/// <returns>Returns a new instance of the object read from the XML file.</returns>
		public static T ReadFromXmlFile<T>(string filePath) where T : new()
		{
			TextReader reader = null;
			try
			{
				var serializer = new XmlSerializer(typeof(T));
				reader = new StreamReader(filePath);
				return (T)serializer.Deserialize(reader);
			}
			finally
			{
				if (reader != null)
					reader.Close();
			}
		}
	}
}
