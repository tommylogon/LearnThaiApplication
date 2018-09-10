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

using System.Diagnostics;

			

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
		List<Word> words = new List<Word>()
		{
			//page 8 and 9
			new Word("", "", "Sit down", ""),
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

			//page 32 and 33

		};
		List<Consonant> consonants = new List<Consonant>()
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
		List<Vowel> vowels = new List<Vowel>()
		{
			new Vowel("ั","กับ","gap","And"),
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
		

		int i = 0;
		bool isRandom;

		public MainWindow()
        {
            InitializeComponent();
			clearFields();

			txb_ThaiScript_Page2.Text = "";
			lbl_Counter.Content = i;

		}

		private void btn_validate_Click(object sender, RoutedEventArgs e)
		{

			if (rb_Conson.IsChecked == true)
			{
				if (consonants[i].EngPron == txt_Answear.Text)
				{
					txb_Status_Page2.Text = "Correct!";
				}
				else
				{
					txb_Status_Page2.Text = "Wrong...";
				}
				txb_Description_page2.Text = consonants[i].EngPron;
			}
			else if(rb_Vowel.IsChecked == true)
			{
				if (vowels[i].EngPron == txt_Answear.Text)
				{
					txb_Status_Page2.Text = "Correct!";
				}
				else
				{
					txb_Status_Page2.Text = "Wrong...";

					System.Console.WriteLine(vowels[i].EngPron);
				}
				txb_Description_page2.Text = vowels[i].EngPron;
			}


			lbl_Counter.Content = i;
		}

		private void btn_Next_Word_Click(object sender, RoutedEventArgs e)
		{
			clearFields();
			i++;

			if (rb_Conson.IsChecked == true){
				try
				{
					
					if (i >= 0)
					{
						if (i < consonants.Count)
						{

							txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
							
						}
						else
						{
							i = 0;
							txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
							
						}
					}
					else
					{
						i = 0;
						txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
					}

				}
				catch (System.ArgumentOutOfRangeException error)
				{

					System.Console.WriteLine(error.Message);
				}
				
			}
			else if (rb_Vowel.IsChecked == true)
			{
				try
				{
					if (i >= 0)
					{
						if (i < vowels.Count)
						{
							txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
						}
						else
						{
							i = 0;
							txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
						}
					}
					else
					{
						i = 0;
						txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
					}

				}
				catch (System.ArgumentOutOfRangeException error)
				{

					System.Console.WriteLine(error.Message);
				}
				
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
				try
				{
					i--;
					if (i < 0)
					{
						i = consonants.Count - 1;
						txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
					}
					else if (i < consonants.Count)
					{

						txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
					}


				}
				catch (System.ArgumentOutOfRangeException error)
				{

					System.Console.WriteLine(error.Message);
				}
				
			}
			else if(rb_Vowel.IsChecked == true)
			{
				
				try
				{
					i--;
					if (i < 0)
					{
						i = vowels.Count - 1;
						txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
					}
					else if (i < vowels.Count)
					{

						txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
					}


				}
				catch (System.ArgumentOutOfRangeException error)
				{

					System.Console.WriteLine(error.Message);
				}
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
		}

		public void textChanger(string recievedText, TextBlock textBlock)
		{
			textBlock.Text = recievedText;
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
		string thaiHelpWord;
		string engPron;
		string engDesc;



		public Word(string thaiWord, string thaiSpell, string engWord, string engDesc)
		{
			this.thaiScript = thaiWord;
			this.thaiHelpWord = thaiSpell;
			this.thaiHelpWord = engWord;
			this.engDesc = engDesc;
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
		public string EngPron
		{
			get { return engPron; }
			set { engPron = value; }
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
		
		string engPron;
		string engDesc;
		

		public Consonant(string thaiSymbol, string thaiHelpWord, string englishPronounciation, string englishWord)
		{
			this.thaiScript = thaiSymbol;
			this.thaiHelpWord = thaiHelpWord;
			this.engPron = englishPronounciation;
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
		public string EngPron
		{
			get { return engPron; }
			set { engPron = value;}
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
		string engPron;
		string engDesc;

		public Vowel(string thaiSymbol, string thaiHelpWord, string englishPronounciation, string englishDescription)
		{
			this.thaiScript = thaiSymbol;
			this.thaiHelpWord = thaiHelpWord;
			this.engPron = englishPronounciation;
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

		public string EngPron
		{
			get { return engPron; }
			set { engPron = value; }
		}

		public string EngDesc
		{
			get { return engDesc; }
			set { engDesc = value; }
		}
	}

}
