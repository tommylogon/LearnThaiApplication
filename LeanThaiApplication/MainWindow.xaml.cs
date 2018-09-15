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
            /*
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
            new Consonant("ฮ","นกฮูก","nok hoog","Owl"),*/
        };
        static List<Vowel> vowels = new List<Vowel>()
        {/*
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
            new Vowel("ุย","คุย","","")*/
        };


        //<>
        int i = 0;
        int selectedChapter;


        string whatListTLoad = "";
        Random random = new Random();


        public MainWindow()
        {
            InitializeComponent();

            clearFields();

            System.Console.WriteLine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));

            lbl_Counter_Page2.Content = i;

            loadFiles<Word>(words);

            loadFiles<Consonant>(consonants);

            loadFiles<Vowel>(vowels);

            System.Console.WriteLine(consonants.Count());
        }

        private void btn_validate_Click(object sender, RoutedEventArgs e)
        {

            if (rb_Conson_Page2.IsChecked == true)
            {
                if (consonants[i].ThaiFonet == txt_Answear_Page2.Text)
                {
                    txb_Status_Page2.Text = "Correct!";
                }
                else
                {
                    txb_Status_Page2.Text = "Wrong...";
                }
                if (ckb_Helpbox_Page2.IsChecked == true)
                {
                    txb_Description_page2.Text = "(" + consonants[i].ThaiFonet + ") " + consonants[i].EngWord + ": " + consonants[i].EngDesc;
                }

            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                if (vowels[i].ThaiFonet == txt_Answear_Page2.Text)
                {
                    txb_Status_Page2.Text = "Correct!";
                }
                else
                {
                    txb_Status_Page2.Text = "Wrong...";

                    System.Console.WriteLine(vowels[i].ThaiFonet);
                }
                if (ckb_Helpbox_Page2.IsChecked == true)
                {
                    txb_Description_page2.Text = "(" + vowels[i].ThaiFonet + ") " + vowels[i].EngWord + ": " + vowels[i].EngDesc;
                }

            }


            lbl_Counter_Page2.Content = i;
        }

        private void btn_Next_Word_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            
            if (rb_Conson_Page2.IsChecked == true)
            {

                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, true);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {

                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, true);
            }
            else
            {
                MessageBox.Show("please select one or the other");
            }
            lbl_Counter_Page2.Content = i;

        }

        private void btn_Prev_Word_Click(object sender, RoutedEventArgs e)
        {
            clearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {
                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, false);

            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, false);

            }
            else
            {
                MessageBox.Show("please select one or the other");
            }

            lbl_Counter_Page2.Content = i;

        }

        public void clearFields()
        {
            txb_Description_page1.Text = "";
            txb_ThaiScript_Page1.Text = "";
            txb_Status_Page1.Text = "";
            txt_Answear_Page1.Text = "";

            txb_ThaiScript_Page2.Text = "";
            txb_Status_Page2.Text = "";
            txb_Description_page2.Text = "";
            txt_Answear_Page2.Text = "";

            txt_FirstSelectionProperty.Text = "";
            txt_SecondSelectionProperty.Text = "";
            txt_ThirdSelectionProperty.Text = "";
            txt_FourthSelectionProperty.Text = "";

            txt_FifthSelectionProperty.Text = "";

        }

        public void textChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, CheckBox checkBoxDescription, CheckBox checkBoxRandom, bool nextIsForward) where T : new()
        {
            

            Type whatIsT = typeof(T);

            PropertyInfo findThaiScriptProperty;
            PropertyInfo findThaiHelpWordProperty;
            PropertyInfo findThaiFonetProperty;
            PropertyInfo findEngDescProperty;
            PropertyInfo findWordChaptereProperty;

            Object propertylistScript;
            Object propertyFonet;
            Object propertylistWord;
            Object propertyEngDesc;
            Object propertyChapter;

            if (checkBoxRandom.IsChecked == true)
            {

                i = random.Next(0, list.Count());
            }
            else
            {

                if (nextIsForward)
                {
                    i++;
                    if (i > list.Count - 1)
                    {
                        i = 0;
                    }
                }
                else
                {
                    i--;
                    if (i < 0)
                    {
                        i = list.Count - 1;
                    }
                }
            }
            

            findEngDescProperty = list[i].GetType().GetProperty("EngDesc");
            findThaiScriptProperty = list[i].GetType().GetProperty("ThaiScript");
            findThaiFonetProperty = list[i].GetType().GetProperty("ThaiFonet");

            propertyFonet = findThaiFonetProperty.GetValue(list[i]);
            propertylistScript = findThaiScriptProperty.GetValue(list[i]);
            propertyEngDesc = findEngDescProperty.GetValue(list[i]);

            if (list[i].GetType() == typeof(Word))
            {
                findWordChaptereProperty = list[i].GetType().GetProperty("Chapter");
                propertyChapter = findWordChaptereProperty.GetValue(list[i]);

                if ((int)propertyChapter == selectedChapter)
                {
                    textBlockForScript.Text = (String)propertylistScript;
                }
            }
            else
            {
                findThaiHelpWordProperty = list[i].GetType().GetProperty("ThaiHelpWord");
                propertylistWord = findThaiHelpWordProperty.GetValue(list[i]);

                textBlockForScript.Text = propertylistScript + " " + propertylistWord;
            }

            if (checkBoxDescription.IsChecked == true)
            {
                textBlockDescription.Text = propertyFonet + "\r\n" + propertyEngDesc;
            }

        }

        private void rb_Conson_Checked(object sender, RoutedEventArgs e)
        {
            i = 0;
            txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
            lbl_Counter_Page2.Content = i;
        }

        private void rb_Vowel_Checked(object sender, RoutedEventArgs e)
        {
            i = 0;
            txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;
            lbl_Counter_Page2.Content = i;
        }



        private void btn_SubmitNewWord_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false)
            {
                MessageBox.Show("Select a list to load from");
                return;
            }
            else if (rb_Conso_Page3.IsChecked == true)
            {
                SubmitNewWord<Consonant>(consonants);
            }
            else if (rb_Vowel_Page3.IsChecked == true)
            {
                SubmitNewWord<Vowel>(vowels);
            }
            else
            {
                SubmitNewWord<Word>(words);
            }


        }

        public void SubmitNewWord<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);
            Type whatIsNewWord;

            PropertyInfo find_NewWord_ThaiScript_Property;
            PropertyInfo find_NewWord_EnglishWord_Property;



            object newWord;
            object property_ThaiScript_Found_In_Word;
            object property_EngWord_Found_In_Word;





            bool existsInList = false;

            if (whatIsT == typeof(Word))
            {
                int newChapter = System.Convert.ToInt32(txt_FifthSelectionProperty.Text);
                newWord = new Word(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, newChapter);
                whatIsNewWord = typeof(Word);


            }
            else if (whatIsT == typeof(Consonant))
            {
                newWord = new Consonant(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text);
                whatIsNewWord = typeof(Consonant);

            }
            else//(whatIsT == typeof(Vowel))
            {
                newWord = new Vowel(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text);
                whatIsNewWord = typeof(Vowel);

            }

            find_NewWord_ThaiScript_Property = newWord.GetType().GetProperty("ThaiScript");

            find_NewWord_EnglishWord_Property = newWord.GetType().GetProperty("EngDesc");

            property_ThaiScript_Found_In_Word = find_NewWord_ThaiScript_Property.GetValue(newWord);

            property_EngWord_Found_In_Word = find_NewWord_EnglishWord_Property.GetValue(newWord);




            PropertyInfo findThaiScriptProperty;
            PropertyInfo findThaiFonetProperty;
            PropertyInfo findThaiHelpWordProperty;
            PropertyInfo findThaiEngWordProperty;
            PropertyInfo findEngDescrProperty;
            PropertyInfo findChapterProperty;



            Object propertylistScript;
            Object propertylistFonet;
            Object propertylistHelp;
            Object propertylistWord;
            Object propertylistDescr;
            Object propertyChapter;


            foreach (T oldWord in list)
            {
                findThaiScriptProperty = oldWord.GetType().GetProperty("ThaiScript");
                findThaiEngWordProperty = oldWord.GetType().GetProperty("EngDesc");

                propertylistScript = findThaiScriptProperty.GetValue(oldWord);
                propertylistWord = findThaiEngWordProperty.GetValue(oldWord);

                if (property_ThaiScript_Found_In_Word == propertylistScript && (String)property_ThaiScript_Found_In_Word != "" || propertylistWord == property_EngWord_Found_In_Word && (String)property_EngWord_Found_In_Word != "")
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
                if ((String)property_ThaiScript_Found_In_Word != "" || (String)property_EngWord_Found_In_Word != "")
                {
                    list.Add((T)newWord);
                    existsInList = true;
                }


            }



            foreach (T oldWord in list)
            {
                
                findThaiScriptProperty = oldWord.GetType().GetProperty("ThaiScript");
                findThaiFonetProperty = oldWord.GetType().GetProperty("ThaiFonet");
                findThaiHelpWordProperty = oldWord.GetType().GetProperty("ThaiHelpWord");
                findThaiEngWordProperty = oldWord.GetType().GetProperty("EngWord");
                findEngDescrProperty = oldWord.GetType().GetProperty("EngDesc");
                findChapterProperty = oldWord.GetType().GetProperty("Chapter");


                propertylistScript = findThaiScriptProperty.GetValue(oldWord);
                propertylistFonet = findThaiFonetProperty.GetValue(oldWord);
                propertylistWord = findThaiEngWordProperty.GetValue(oldWord);
                propertyChapter = findChapterProperty.GetValue(oldWord);

                if ((String)propertylistScript == txt_FirstSelectionProperty.Text && txt_FirstSelectionProperty.Text != "" || (String)propertylistWord == txt_ThirdSelectionProperty.Text && txt_ThirdSelectionProperty.Text != "")
                {
                    if (null != findThaiScriptProperty && findThaiScriptProperty.CanWrite)
                    {
                        findThaiScriptProperty.SetValue(oldWord, txt_FirstSelectionProperty.Text, null);
                    }

                    if (null != findThaiFonetProperty && findThaiFonetProperty.CanWrite)
                    {
                        findThaiFonetProperty.SetValue(oldWord, txt_SecondSelectionProperty.Text, null);
                    }

                    if (null != findThaiHelpWordProperty && findThaiHelpWordProperty.CanWrite && whatIsT.GetType() == typeof(Consonant) || whatIsT.GetType() == typeof(Vowel))
                    {
                        if (oldWord.GetType() == typeof(Consonant) || oldWord.GetType() == typeof(Vowel))
                        {
                            findThaiHelpWordProperty.SetValue(oldWord, txt_ThirdSelectionProperty.Text, null);
                        }

                    }

                    if (null != findThaiEngWordProperty && findThaiEngWordProperty.CanWrite)
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            findThaiEngWordProperty.SetValue(oldWord, txt_ThirdSelectionProperty.Text, null);
                        }
                        else
                        {
                            findThaiEngWordProperty.SetValue(oldWord, txt_FourthSelectionProperty.Text, null);
                        }

                    }

                    if (null != findChapterProperty && findChapterProperty.CanWrite)
                    {

                        try
                        {
                            findChapterProperty.SetValue(oldWord, System.Convert.ToInt32(txt_FifthSelectionProperty.Text), null);
                        }
                        catch (FormatException)
                        {

                        }
                        catch (OverflowException)
                        {

                        }
                    }
                    if (null != findEngDescrProperty && findEngDescrProperty.CanWrite)
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            findEngDescrProperty.SetValue(oldWord, txt_FourthSelectionProperty.Text, null);
                        }
                        else
                        {
                            findEngDescrProperty.SetValue(oldWord, txt_FifthSelectionProperty.Text, null);
                        }

                    }

                    /*
                    if (null != prop && prop.CanWrite)
                    {
                        prop.SetValue(obj, "Value", null);
                    }*/

                    clearFields();
                    //System.Console.WriteLine(oldWord.ThaiScript + "; " + oldWord.ThaiFonet + "; " + oldWord.EngWord + "; " + oldWord.EngDesc);
                    break;
                }

            }


            saveFiles<T>(list);
        }

        private void btn_LoadList_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false)
            {
                MessageBox.Show("Select a list to load from");
                return;
            }

            lib_LoadedWords.Items.Clear();

            if (whatListTLoad == "Word")
            {
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
            else if (whatListTLoad == "Conso")
            {
                foreach (Consonant conso in consonants)
                {
                    if (conso.ThaiScript == "")
                    {
                        lib_LoadedWords.Items.Add(conso.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.Items.Add(conso.ThaiScript);
                    }
                }
            }
            else if (whatListTLoad == "Vowel")
            {
                foreach (Vowel vowel in vowels)
                {
                    if (vowel.ThaiScript == "")
                    {
                        lib_LoadedWords.Items.Add(vowel.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.Items.Add(vowel.ThaiScript);
                    }
                }
            }

        }

        private void lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {

            string wordToLoad;

            if (lib_LoadedWords.SelectedIndex != -1)
            {
                wordToLoad = lib_LoadedWords.Items[lib_LoadedWords.SelectedIndex].ToString();

                if (whatListTLoad == "Word")
                {
                    foreach (Word word in words)
                    {
                        if (wordToLoad == word.ThaiScript || wordToLoad == word.EngWord)
                        {
                            txt_FirstSelectionProperty.Text = word.ThaiScript;
                            txt_SecondSelectionProperty.Text = word.ThaiFonet;
                            txt_ThirdSelectionProperty.Text = word.EngWord;
                            txt_FourthSelectionProperty.Text = word.EngDesc;
                            txt_FifthSelectionProperty.Text = word.Chapter.ToString();
                            txb_Description_Page4.Text = word.EngDesc;

                        }
                    }
                }
                else if (whatListTLoad == "Conso")
                {
                    foreach (Consonant conso in consonants)
                    {
                        if (wordToLoad == conso.ThaiScript || wordToLoad == conso.EngWord)
                        {
                            txt_FirstSelectionProperty.Text = conso.ThaiScript;
                            txt_SecondSelectionProperty.Text = conso.ThaiFonet;
                            txt_ThirdSelectionProperty.Text = conso.ThaiHelpWord;
                            txt_FourthSelectionProperty.Text = conso.EngWord;
                            txt_FifthSelectionProperty.Text = conso.EngDesc;
                            txb_Description_Page4.Text = conso.EngDesc;
                            break;
                        }
                    }
                }
                else if (whatListTLoad == "Vowel")
                {
                    foreach (Vowel vowel in vowels)
                    {
                        if (wordToLoad == vowel.ThaiScript || wordToLoad == vowel.EngWord)
                        {
                            txt_FirstSelectionProperty.Text = vowel.ThaiScript;
                            txt_SecondSelectionProperty.Text = vowel.ThaiFonet;
                            txt_ThirdSelectionProperty.Text = vowel.ThaiHelpWord;
                            txt_FourthSelectionProperty.Text = vowel.EngWord;
                            txt_FifthSelectionProperty.Text = vowel.EngDesc;
                            txb_Description_Page4.Text = vowel.EngDesc;
                        }
                    }
                }

            }
        }

        public void saveFiles<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            XmlSerialization.WriteToXmlFile<List<T>>("C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/Thai_" + whatIsT.Name + ".xml", list, false);
        }

        public void loadFiles<T>(List<T> list) where T : new()
        {
            /*Type whatIsT = typeof(T);

            PropertyInfo findThaiScriptProperty = list[i].GetType().GetProperty("ThaiScript");
            PropertyInfo findThaiHelpWordProperty = list[i].GetType().GetProperty("ThaiHelpWord");

            Object propertylistScript = findThaiScriptProperty.GetValue(list[i]);
            Object propertylistWord = findThaiHelpWordProperty.GetValue(list[i]);
            
             */
            Type whatIsT = typeof(T);
            PropertyInfo propInFile;
            PropertyInfo propInList;

            Object propertyFromFile;
            Object propertyFromList;

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_" + whatIsT.Name + ".xml");

            List<T> newWordToAdd = new List<T>();

            bool foundInList = false;

            foreach (T wordFoundInFile in wordsFromFIle)
            {
                propInFile = wordFoundInFile.GetType().GetProperty("ThaiScript");

                propertyFromFile = propInFile.GetValue(wordFoundInFile);

                foreach (T WordAlreadyInList in list)
                {
                    propInList = WordAlreadyInList.GetType().GetProperty("ThaiScript");

                    propertyFromList = propInList.GetValue(WordAlreadyInList);

                    if (propertyFromFile.ToString() == propertyFromList.ToString())
                    {
                        System.Console.WriteLine("found word in list");
                        System.Console.WriteLine(propertyFromFile);
                        foundInList = true;
                        break;
                    }
                    else
                    {
                        System.Console.WriteLine("checking next");
                    }
                }
                if (foundInList == false)
                {
                    newWordToAdd.Add(wordFoundInFile);
                }



            }
            list.AddRange(newWordToAdd);


        }

        private void rb_words_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Word";
            clearFields();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";
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

        private void rb_Conso_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Conso";
            clearFields();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";
            lbl_Chapter_Insert.Content = "English Description";

            lib_LoadedWords.Items.Clear();

            foreach (Consonant conso in consonants)
            {
                if (conso.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(conso.EngWord);
                }
                else
                {
                    lib_LoadedWords.Items.Add(conso.ThaiScript);
                }
            }

        }

        private void rb_Vowel_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Vowel";
            clearFields();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";

            lib_LoadedWords.Items.Clear();

            foreach (Vowel vowel in vowels)
            {
                if (vowel.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(vowel.EngWord);
                }
                else
                {
                    lib_LoadedWords.Items.Add(vowel.ThaiScript);
                }
            }

        }
        public int CheckPage1RB()
        {

            if (rb_KeyToUnderstand.IsChecked == true)
            {
                return 0;
            }
            else if (rb_TonalLanguage.IsChecked == true)
            {
                return 1;
            }
            else if (rb_SpecialPron.IsChecked == true)
            {
                return 2;
            }
            else if (rb_Noun.IsChecked == true)
            {
                return 3;
            }
            else if (rb_Numbers.IsChecked == true)
            {
                return 4;
            }
            else if (rb_Time.IsChecked == true)
            {
                return 5;
            }
            else if (rb_Color.IsChecked == true)
            {
                return 6;
            }
            else if (rb_words.IsChecked == true)
            {
                return 7;
            }
            else if (rb_Body.IsChecked == true)
            {
                return 8;
            }
            else
            {
                MessageBox.Show("Select a chapter", "Chapter not selected");
                return 0;
            }


        }

        private void btn_Next_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            selectedChapter = CheckPage1RB();

            clearFields();

            textChanger<Word>(words, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, true);

            lbl_Counter_Page1.Content = i;
        }
        

        private void btn_Prev_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            CheckPage1RB();
            textChanger<Word>(words, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, false);
            lbl_Counter_Page1.Content = i;
        }

        private void btn_validate_Page1_Click(object sender, RoutedEventArgs e)
        {
            CheckPage1RB();
        }

        private void rb_KeyToUnderstand_Checked(object sender, RoutedEventArgs e)
        {
            txb_ThaiScript_Page1.Text = words[i].ThaiScript;
            lbl_Counter_Page1.Content = i;
        }

        private void rb_TonalLanguage_Checked(object sender, RoutedEventArgs e)
        {
            txb_ThaiScript_Page1.Text = words[i].ThaiScript;
            lbl_Counter_Page1.Content = i;
        }
    }







    public abstract class ThaiToEnglish
    {
        string thaiScript;
        string thaiFonet;
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

    public class Word : ThaiToEnglish
    {
        string engDesc;
        int chapter;


        public Word()
        {

        }
        public Word(string thaiWord, string thaiFonet, string engWord, string engDesc)
        {
            this.ThaiScript = thaiWord;

            this.ThaiFonet = thaiFonet;
            this.EngWord = engWord;
            this.engDesc = engDesc;
        }
        public Word(string thaiWord, string thaiFonet, string engWord, string engDesc, int Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = engWord;

            this.engDesc = engDesc;
            this.chapter = Chapter;

        }

        public int Chapter
        {
            get { return chapter; }
            set { chapter = value; }
        }
    }
    public class Consonant : ThaiToEnglish
    {

        string thaiHelpWord;


        public Consonant()
        {

        }
        public Consonant(string thaiSymbol, string thaiHelpWord, string thaiFonetical, string englishWord)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWord = englishWord;

        }
        public Consonant(string thaiSymbol, string thaiHelpWord, string thaiFonetical, string englishWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWord = englishWord;
            this.EngDesc = englishDescription;

        }

        public string ThaiHelpWord
        {
            get { return thaiHelpWord; }
            set { thaiHelpWord = value; }

        }


    }
    public class Vowel : ThaiToEnglish
    {

        string thaiHelpWord;


        public Vowel()
        {

        }
        public Vowel(string thaiSymbol, string thaiHelpWord, string thaiFonet, string englishWord)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
        }
        public Vowel(string thaiSymbol, string thaiHelpWord, string thaiFonet, string englishWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
            this.EngDesc = englishDescription;
        }

        public string ThaiHelpWord
        {
            get { return thaiHelpWord; }
            set { thaiHelpWord = value; }

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
            string message = "We have saved to the location: " + filePath;

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
            MessageBox.Show(message, "Save file Location");
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

