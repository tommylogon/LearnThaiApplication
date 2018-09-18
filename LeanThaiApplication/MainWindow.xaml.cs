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
        static List<ThaiNumber> numbers = new List<ThaiNumber>();

        static List<Word> DisplayList = new List<Word>();
        //<>

        int i = 0;

        string languageFilePath = "C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/";


        string selectedChapter;

        string whatListTLoad;

        Random random = new Random();

        
        PropertyInfo foundThaiScriptProperty;
        PropertyInfo foundThaiHelpWordProperty;
        PropertyInfo foundThaiFonetProperty;
        PropertyInfo foundEngWordProperty;
        PropertyInfo foundEngDescProperty;
        PropertyInfo foundWordChapterProperty;

        Object propertyScript;
        Object propertyFonet;
        Object propertyHelpWord;
        Object propertyEngWord;
        Object propertyEngDesc;
        Object propertyChapter;
        
        public MainWindow()
        {
            InitializeComponent();

            clearFields();


            lbl_Counter_Page2.Content = i;
            lbl_Counter_Page1.Content = i;
            txb_FilePath_Settings.Text = languageFilePath;
            txt_NewSavePath_Settings.Text = languageFilePath;
            loadFiles<Word>(words);

            loadFiles<Consonant>(consonants);

            loadFiles<Vowel>(vowels);

            loadFiles<ThaiNumber>(numbers);


            CheckList();
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

        public void textChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, CheckBox checkBoxDescription, CheckBox checkBoxRandom, int nextValueToAdd) where T : new()
        {

            Type whatIsT = typeof(T);
            
            if (checkBoxRandom.IsChecked == true)
            {
                i = random.Next(0, list.Count());
            }
            else
            {
                if (nextValueToAdd > 0)
                {
                    i++;
                    if (i > list.Count - 1)
                    {
                        i = 0;
                    }
                }
                else if(nextValueToAdd < 0)
                {
                    i--;
                    if (i < 0)
                    {
                        i = list.Count - 1;
                    }
                }
                else
                {
                    textBlockForScript.Text = (String) propertyScript;
                }
            }

            foundEngDescProperty = list[i].GetType().GetProperty("EngDesc");
            foundThaiScriptProperty = list[i].GetType().GetProperty("ThaiScript");
            foundThaiFonetProperty = list[i].GetType().GetProperty("ThaiFonet");

            propertyFonet = foundThaiFonetProperty.GetValue(list[i]);
            propertyScript = foundThaiScriptProperty.GetValue(list[i]);
            propertyEngDesc = foundEngDescProperty.GetValue(list[i]);

            if (list[i].GetType() == typeof(Word))
            {
                foundWordChapterProperty = list[i].GetType().GetProperty("Chapter");

                propertyChapter = foundWordChapterProperty.GetValue(list[i]);

                if ((string) propertyChapter == selectedChapter)
                {
                    textBlockForScript.Text = (String)propertyScript;
                }
                else
                {
                    MessageBox.Show("There are no content with chapter" + selectedChapter + " available right now.");
                    return;
                }
            }
            else
            {
                foundThaiHelpWordProperty = list[i].GetType().GetProperty("ThaiHelpWord");
                propertyEngWord = foundThaiHelpWordProperty.GetValue(list[i]);

                textBlockForScript.Text = propertyScript + " " + propertyEngWord;
            }

            if (checkBoxDescription.IsChecked == true)
            {
                textBlockDescription.Text = propertyFonet + "\r\n" + propertyEngDesc;
            }

        }

        public void SubmitNewWord<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);
            

            PropertyInfo foundNewWordThaiScriptProperty;
            PropertyInfo foundNewWordEngWordProperty;
            
            object newWord;

            object property_ThaiScript_Found_In_Word;
            object property_EngWord_Found_In_Word;
            
            bool existsInList = false;

            if(txt_FirstSelectionProperty.Text != "" && txt_SecondSelectionProperty.Text != "" && txt_ThirdSelectionProperty.Text != "" && txt_FourthSelectionProperty.Text != "")
            {
                if (whatIsT == typeof(Word))
                {
                    
                    newWord = new Word(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                    


                }
                else if (whatIsT == typeof(Consonant))
                {
                    newWord = new Consonant(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                    

                }
                else if (whatIsT == typeof(Vowel))
                {
                    newWord = new Vowel(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                    

                }
                else
                {
                    newWord = new ThaiNumber(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                    
                }

                foundNewWordThaiScriptProperty = newWord.GetType().GetProperty("ThaiScript");
                foundNewWordEngWordProperty = newWord.GetType().GetProperty("EngWord");

                property_ThaiScript_Found_In_Word = foundNewWordThaiScriptProperty.GetValue(newWord);
                property_EngWord_Found_In_Word = foundNewWordEngWordProperty.GetValue(newWord);


                //try and combine both foreach somehow
                foreach (T oldWord in list)
                {
                    foundThaiScriptProperty = oldWord.GetType().GetProperty("ThaiScript");
                    foundEngWordProperty = oldWord.GetType().GetProperty("EngWord");

                    propertyScript = foundThaiScriptProperty.GetValue(oldWord);
                    propertyEngWord = foundEngWordProperty.GetValue(oldWord);

                    if (property_ThaiScript_Found_In_Word == propertyScript && (String)property_ThaiScript_Found_In_Word != "" || propertyEngWord == property_EngWord_Found_In_Word && (String)property_EngWord_Found_In_Word != "")
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

                    foundThaiScriptProperty = oldWord.GetType().GetProperty("ThaiScript");
                    foundThaiFonetProperty = oldWord.GetType().GetProperty("ThaiFonet");
                    foundThaiHelpWordProperty = oldWord.GetType().GetProperty("ThaiHelpWord");
                    foundEngWordProperty = oldWord.GetType().GetProperty("EngWord");
                    foundEngDescProperty = oldWord.GetType().GetProperty("EngDesc");
                    foundWordChapterProperty = oldWord.GetType().GetProperty("Chapter");

                    propertyScript = foundThaiScriptProperty.GetValue(oldWord);
                    propertyEngWord = foundEngWordProperty.GetValue(oldWord);
                    propertyChapter = foundWordChapterProperty.GetValue(oldWord);

                    if ((String)propertyScript == txt_FirstSelectionProperty.Text && txt_FirstSelectionProperty.Text != "" || (String)propertyEngWord == txt_ThirdSelectionProperty.Text && txt_ThirdSelectionProperty.Text != "")
                    {
                        if(oldWord.GetType() == typeof(Word))
                        {
                            if ((String)propertyChapter == txt_FifthSelectionProperty.Text && txt_FifthSelectionProperty.Text != "")
                            {
                                setNewValuesToOldWord(oldWord, whatIsT);
                                break;
                            }
                            
                        }
                        else
                        {
                            setNewValuesToOldWord(oldWord, whatIsT);
                            break;
                        }
                        

                        

                        /*
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(obj, "Value", null);
                        }*/

                        
                        
                    }

                }

                clearFields();
                saveFiles<T>(list);
            }
            

            
        }

        public void setNewValuesToOldWord(Object oldWord, Type whatIsT)
        {
            if (null != foundThaiScriptProperty && foundThaiScriptProperty.CanWrite)
            {
                foundThaiScriptProperty.SetValue(oldWord, txt_FirstSelectionProperty.Text, null);
            }

            if (null != foundThaiFonetProperty && foundThaiFonetProperty.CanWrite)
            {
                foundThaiFonetProperty.SetValue(oldWord, txt_SecondSelectionProperty.Text, null);
            }

            if (null != foundThaiHelpWordProperty && foundThaiHelpWordProperty.CanWrite && whatIsT.GetType() == typeof(Consonant) || whatIsT.GetType() == typeof(Vowel))
            {
                if (oldWord.GetType() == typeof(Consonant) || oldWord.GetType() == typeof(Vowel))
                {
                    foundThaiHelpWordProperty.SetValue(oldWord, txt_ThirdSelectionProperty.Text, null);
                }

            }

            if (null != foundEngWordProperty && foundEngWordProperty.CanWrite)
            {
                if (oldWord.GetType() == typeof(Word))
                {
                    foundEngWordProperty.SetValue(oldWord, txt_ThirdSelectionProperty.Text, null);
                }
                else
                {
                    foundEngWordProperty.SetValue(oldWord, txt_FourthSelectionProperty.Text, null);
                }

            }

            if (null != foundWordChapterProperty && foundWordChapterProperty.CanWrite)
            {

                try
                {
                    foundWordChapterProperty.SetValue(oldWord, txt_FifthSelectionProperty.Text, null);
                }
                catch (FormatException)
                {

                }
                catch (OverflowException)
                {

                }
            }
            if (null != foundEngDescProperty && foundEngDescProperty.CanWrite)
            {
                if (oldWord.GetType() == typeof(Word))
                {
                    foundEngDescProperty.SetValue(oldWord, txt_FourthSelectionProperty.Text, null);
                }
                else
                {
                    foundEngDescProperty.SetValue(oldWord, txt_FifthSelectionProperty.Text, null);
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

            Type whatIsT = typeof(T);

            PropertyInfo ScriptInFile = null;
            PropertyInfo ChapterFromFile = null;

            PropertyInfo ScriptInList = null;
            PropertyInfo ChapterFromList = null;

            Object propertyFromFile = null;
            Object propertyChapterFromFile = null;

            Object propertyChapterFromList = null;
            Object propertyFromList = null;

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>("C:/Users/tommy/source/repos/LearnThaiApplication/Language_Files/Thai_" + whatIsT.Name + ".xml");

            List<T> newWordToAdd = new List<T>();

            bool foundInList = false;

            //check if words found in file already have been loaded.
            foreach (T wordFoundInFile in wordsFromFIle)
            {
                ScriptInFile = wordFoundInFile.GetType().GetProperty("ThaiScript");
                ChapterFromFile = wordFoundInFile.GetType().GetProperty("Chapter");

                propertyFromFile = ScriptInFile.GetValue(wordFoundInFile);

                if (null != ChapterFromFile && ChapterFromFile.CanWrite)
                {
                    propertyChapterFromFile = ChapterFromFile.GetValue(wordFoundInFile);
                }

                foreach (T WordAlreadyInList in list)
                {
                    ScriptInList = WordAlreadyInList.GetType().GetProperty("ThaiScript");
                    ChapterFromList = WordAlreadyInList.GetType().GetProperty("Chapter");

                    propertyFromList = ScriptInList.GetValue(WordAlreadyInList);

                    if (null != ChapterFromList && ChapterFromList.CanWrite)
                    {
                        propertyChapterFromList = ChapterFromList.GetValue(WordAlreadyInList);
                    }

                   if (propertyFromFile.ToString() == propertyFromList.ToString())
                    {
                        if(WordAlreadyInList.GetType() == typeof(Word))
                        {
                            if(propertyChapterFromList.ToString() == propertyChapterFromFile.ToString())
                            {
                                System.Console.WriteLine("found word in list");
                                System.Console.WriteLine(propertyFromFile);
                                foundInList = true;
                                break;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("found word in list");
                            System.Console.WriteLine(propertyFromFile);
                            foundInList = true;
                            break;
                        }

                        
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

        public void validateAnswear<T>(List<T> list, TextBox textboxAnswear, TextBlock textBlockStatus, TextBlock textBlockDesc, CheckBox checkBoxDesc)
        {
            
           

            foundThaiFonetProperty = list[i].GetType().GetProperty("ThaiFonet");
            foundEngWordProperty = list[i].GetType().GetProperty("EngWord");
            foundEngDescProperty = list[i].GetType().GetProperty("EngDesc");

            propertyFonet = foundThaiFonetProperty.GetValue(list[i]);
            propertyEngWord = foundEngWordProperty.GetValue(list[i]);
            propertyEngDesc = foundEngDescProperty.GetValue(list[i]);

            
            if ((String) propertyFonet == textboxAnswear.Text)
            {
                textBlockStatus.Text = "Correct!";
            }
            else
            {
                textBlockStatus.Text = "Wrong...";
            }
            if (checkBoxDesc.IsChecked == true)
            {
                textBlockDesc.Text = "(" + propertyFonet + ") \r\n" + propertyEngWord + ": " + propertyEngDesc;
            }
                
            
            lbl_Counter_Page2.Content = i;
        }

        public void SelectionChanged<T>(List<T> list)
        {
            Object wordToLoad;

            Type whatIsT = typeof(T);

            if (lib_LoadedWords.SelectedIndex != -1)
            {
                wordToLoad = lib_LoadedWords.SelectedItem;

                PropertyInfo wordToLoadThaiScript = wordToLoad.GetType().GetProperty("ThaiScript");
                PropertyInfo wordToLoadThaiFonetProperty = wordToLoad.GetType().GetProperty("ThaiFonet");
                PropertyInfo wordToLoadThaiHelpWordProperty = wordToLoad.GetType().GetProperty("ThaiHelpWord");
                PropertyInfo wordToLoadEngWordProperty = wordToLoad.GetType().GetProperty("EngWord");
                PropertyInfo wordToLoadDescProperty = wordToLoad.GetType().GetProperty("EngDesc");
                PropertyInfo wordToLoadWordChapterProperty = wordToLoad.GetType().GetProperty("Chapter");

                Object wordToLoadScript = null;
                Object wordToLoadFonet = null;
                Object wordToLoadHelpWord = null;
                Object wordToLoadEngWord = null;
                Object wordToLoadEngDesc = null;
                Object wordToLoadChapter = null;

                if (null != wordToLoadThaiScript && wordToLoadThaiScript.CanWrite)
                {
                    wordToLoadScript = wordToLoadThaiScript.GetValue(wordToLoad);
                }
                if (null != wordToLoadThaiFonetProperty && wordToLoadThaiFonetProperty.CanWrite)
                {
                    wordToLoadFonet = wordToLoadThaiFonetProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadThaiHelpWordProperty && wordToLoadThaiHelpWordProperty.CanWrite)
                {
                    wordToLoadHelpWord = wordToLoadThaiHelpWordProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadEngWordProperty && wordToLoadEngWordProperty.CanWrite)
                {
                    wordToLoadEngWord = wordToLoadEngWordProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadDescProperty && wordToLoadDescProperty.CanWrite)
                {
                    wordToLoadEngDesc = wordToLoadDescProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadWordChapterProperty && wordToLoadWordChapterProperty.CanWrite)
                {
                    wordToLoadChapter = wordToLoadWordChapterProperty.GetValue(wordToLoad);
                }


                foreach (T wordFromList in list)
                {
                    foundThaiScriptProperty = wordFromList.GetType().GetProperty("ThaiScript");
                    foundThaiFonetProperty = wordFromList.GetType().GetProperty("ThaiFonet");
                    foundThaiHelpWordProperty = wordFromList.GetType().GetProperty("ThaiHelpWord");
                    foundEngWordProperty = wordFromList.GetType().GetProperty("EngWord");
                    foundEngDescProperty = wordFromList.GetType().GetProperty("EngDesc");
                    foundWordChapterProperty = wordFromList.GetType().GetProperty("Chapter");

                    



                    if (null != foundThaiScriptProperty && foundThaiScriptProperty.CanWrite)
                    {
                        propertyScript = foundThaiScriptProperty.GetValue(wordFromList);
                    }
                    if (null != foundThaiFonetProperty && foundThaiFonetProperty.CanWrite)
                    {
                        propertyFonet = foundThaiFonetProperty.GetValue(wordFromList);
                    }
                    if (null != foundThaiHelpWordProperty && foundThaiHelpWordProperty.CanWrite)
                    {
                        propertyHelpWord = foundThaiHelpWordProperty.GetValue(wordFromList);
                    }
                    if (null != foundEngWordProperty && foundEngWordProperty.CanWrite)
                    {
                        propertyEngWord = foundEngWordProperty.GetValue(wordFromList);
                    }
                    if (null != foundEngDescProperty && foundEngDescProperty.CanWrite)
                    {
                        propertyEngDesc = foundEngDescProperty.GetValue(wordFromList);
                    }
                    if (null != foundWordChapterProperty && foundWordChapterProperty.CanWrite)
                    {
                        propertyChapter = foundWordChapterProperty.GetValue(wordFromList);
                    }

                    if (wordFromList.GetType() == typeof(Word) && (String)wordToLoadChapter == (String)propertyChapter)
                    {
                        if ((String)wordToLoadScript == (String)propertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)propertyScript;
                            txt_SecondSelectionProperty.Text = (String)propertyFonet;
                            txt_ThirdSelectionProperty.Text = (String)propertyEngWord;
                            txt_FourthSelectionProperty.Text = (String)propertyEngDesc;
                            txt_FifthSelectionProperty.Text = (String)propertyChapter;
                            txb_Description_Page4.Text = (String)propertyEngDesc;
                            break;
                        }
                    }
                    else if(wordFromList.GetType() == typeof(Consonant) || wordFromList.GetType() == typeof(Vowel) || wordFromList.GetType() == typeof(ThaiNumber))
                    {
                        if ((String)wordToLoadScript == (String)propertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)propertyScript;
                            txt_SecondSelectionProperty.Text = (String)propertyFonet;
                            txt_ThirdSelectionProperty.Text = (String)propertyHelpWord;
                            txt_FourthSelectionProperty.Text = (String)propertyEngWord;
                            txt_FifthSelectionProperty.Text = (String)propertyEngDesc;
                            txb_Description_Page4.Text = (String)propertyEngDesc;
                            break;
                        }
                            
                    }                   
                }
            }
        }

        public void findWordWithChapter()
        {
            DisplayList.Clear();

            foreach (Word word in words)
            {
                if(word.Chapter == selectedChapter)
                {

                    DisplayList.Add(word);
                }
                
            }
            
        }

        public void CheckList()
        {
            foreach(Word word in words)
            {
                System.Console.WriteLine(word.ThaiScript + " " + word.ThaiFonet + " " + word.EngWord + " " + word.EngDesc + " " + word.Chapter);
            }
        }

        private void btn_validate_Page2_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conson_Page2.IsChecked == true)
            {
                validateAnswear<Consonant>(consonants,txt_Answear_Page2,txb_Status_Page2,txb_Description_page2, ckb_Helpbox_Page2);

            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                validateAnswear<Vowel>(vowels, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else if (rb_Closing_Page2.IsChecked == true)
            {
                //TODO
            }
            else if(rb_NumberSymbol_Page2.IsChecked == true)
            {
                validateAnswear<ThaiNumber>(numbers, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else
            {
                MessageBox.Show("Please select a category.");
            }
            
        }

        private void btn_Next_Word_Click(object sender, RoutedEventArgs e)
        {
            clearFields();
            
            if (rb_Conson_Page2.IsChecked == true)
            {

                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {

                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                textChanger<ThaiNumber>(numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
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
                textChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);

            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                textChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);

            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                textChanger<ThaiNumber>(numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else
            {
                MessageBox.Show("please select one or the other");
            }

            lbl_Counter_Page2.Content = i;

        }
        
        private void rb_Conson_Checked(object sender, RoutedEventArgs e)
        {
            i = 0;
            txb_ThaiScript_Page2.Text = consonants[i].ThaiScript + " " + consonants[i].ThaiHelpWord;
            lbl_Counter_Page2.Content = i;
            txb_Information_Page2.Text = "To property pronounce a Thai Consonant you add the sound from อ (o).";
        }

        private void rb_Vowel_Checked(object sender, RoutedEventArgs e)
        {
            i = 0;
            txb_ThaiScript_Page2.Text = vowels[i].ThaiScript + " " + vowels[i].ThaiHelpWord;

            lbl_Counter_Page2.Content = i;

            txb_Information_Page2.Text = "When you are reading a vowel, you almost always pronaounce the consonant first, then the surounding vowel.";
        }

        private void rb_NumberSymbol_Page2_Checked(object sender, RoutedEventArgs e)
        {
            i = 0;
            txb_ThaiScript_Page2.Text = numbers[i].ThaiScript + " " + numbers[i].ThaiHelpWord;

            lbl_Counter_Page2.Content = i;

            txb_Information_Page2.Text = "";
        }

        private void btn_SubmitNewWord_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false && rb_ThaiNumber_Page3.IsChecked ==false)
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
            else if(rb_words_Page3.IsChecked == true)
            {
                SubmitNewWord<Word>(words);
            }
            else
            {
                SubmitNewWord<ThaiNumber>(numbers);
            }


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
            else if(whatListTLoad == "ThaiNumber")
            {
                foreach (ThaiNumber number in numbers)
                {
                    if (number.ThaiScript == "")
                    {
                        lib_LoadedWords.Items.Add(number.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.Items.Add(number.ThaiScript);
                    }
                }
            }

        }

        private void lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (rb_words_Page3.IsChecked == true)
            {
                SelectionChanged<Word>(words);
            }
            else if(rb_Conso_Page3.IsChecked == true)
            {
                SelectionChanged<Consonant>(consonants);
            }
            else if(rb_Vowel_Page3.IsChecked == true)
            {
                SelectionChanged<Vowel>(vowels);
            }
            else if(rb_ThaiNumber_Page3.IsChecked == true)
            {
                SelectionChanged<ThaiNumber>(numbers);
            }
            else
            {
                MessageBox.Show("Please select a list to load", "List not choosen");
            }
            

            
        }

        private void rb_words_Page3_Checked(object sender, RoutedEventArgs e)
        {

            whatListTLoad = "Word";

            clearFields();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";

            lib_LoadedWords.ItemsSource = null;

            foreach (Word word in words)
            {
                if (word.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(word.EngWord);
                }
                else
                {
                    lib_LoadedWords.ItemsSource = words;
                    break;
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

            lib_LoadedWords.ItemsSource = null;

            foreach (Consonant conso in consonants)
            {
                if (conso.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(conso.EngWord);
                }
                else
                {
                    lib_LoadedWords.ItemsSource = consonants;
                    break;
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

            lib_LoadedWords.ItemsSource = null;

            foreach (Vowel vowel in vowels)
            {
                if (vowel.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(vowel.EngWord);
                }
                else
                {
                    lib_LoadedWords.ItemsSource = vowels;
                    break;
                }
            }

        }
        
        private void btn_Next_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            //selectedChapter = CheckPage1RB();

            clearFields();

            textChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 1);

            lbl_Counter_Page1.Content = i;
        }
        
        private void btn_Prev_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            
            textChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, -1);
            lbl_Counter_Page1.Content = i;
        }

        private void btn_validate_Page1_Click(object sender, RoutedEventArgs e)
        {
            
            validateAnswear<Word>(DisplayList, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1, ckb_Helpbox_Page1);
        }

        private void rb_ThaiNumber_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Numbers";
            clearFields();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";

            lib_LoadedWords.ItemsSource = null;

            foreach (ThaiNumber number in numbers)
            {
                if (number.ThaiScript == "")
                {
                    lib_LoadedWords.Items.Add(number.EngWord);
                }
                else
                {
                    lib_LoadedWords.ItemsSource = numbers;
                    break;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = 0;
            selectedChapter = cb_Chapter_Page1.SelectedValue.ToString();

            findWordWithChapter();

            lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList.Count.ToString();

            if(DisplayList.Count > 0)
            {
                textChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 0);
            }
            else
            {
                MessageBox.Show("There are no words in that category yet");
            }
            

            lbl_Counter_Page1.Content = i;
        }

        private void ckb_Helpbox_Page1_Checked(object sender, RoutedEventArgs e)
        {

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
        string chapter;


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
        public Word(string thaiWord, string thaiFonet, string engWord, string engDesc, string Chapter)
        {
            this.ThaiScript = thaiWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = engWord;

            this.engDesc = engDesc;
            this.chapter = Chapter;

        }

        public string Chapter
        {
            get { return chapter; }
            set { chapter = value; }
        }
    }

    public abstract class ThaiSymbol : ThaiToEnglish
    {
        string thaiHelpWord;


        public string ThaiHelpWord
        {
            get { return thaiHelpWord; }
            set { thaiHelpWord = value; }

        }
    }

    public class Consonant : ThaiSymbol
    {

        


        public Consonant()
        {

        }
        public Consonant(string thaiSymbol, string thaiFonetical, string thaiHelpWord,  string englishWord)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWord = englishWord;

        }
        public Consonant(string thaiSymbol, string thaiFonetical, string thaiHelpWord, string englishWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonetical;
            this.EngWord = englishWord;
            this.EngDesc = englishDescription;

        }
        
    }

    public class Vowel : ThaiSymbol
    {
        
        public Vowel()
        {

        }
        public Vowel(string thaiSymbol,  string thaiFonet, string thaiHelpWord, string englishWord)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
        }
        public Vowel(string thaiSymbol,  string thaiFonet, string thaiHelpWord, string englishWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
            this.EngDesc = englishDescription;
        }


    }
    public class ThaiNumber : ThaiSymbol
    {
        public ThaiNumber()
        {

        }
        public ThaiNumber(string thaiSymbol, string thaiFonet, string thaiHelpWord,  string englishWord)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
        }
        public ThaiNumber(string thaiSymbol, string thaiFonet, string thaiHelpWord,  string englishWord, string englishDescription)
        {
            this.ThaiScript = thaiSymbol;
            this.ThaiHelpWord = thaiHelpWord;
            this.ThaiFonet = thaiFonet;
            this.EngWord = englishWord;
            this.EngDesc = englishDescription;
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

