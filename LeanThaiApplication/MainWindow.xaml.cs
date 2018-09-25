using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Consonant> consonants = new List<Consonant>();
        private static int correctPoints = 0;
        private static int currentFile = 0;
        private static List<Word> DisplayList = new List<Word>();
        private static PropertyInfo foundEngDescProperty;
        private static PropertyInfo foundEngWordProperty;
        private static PropertyInfo foundEngWordsProperty;
        private static PropertyInfo foundThaiFonetProperty;
        private static PropertyInfo foundThaiHelpWordProperty;
        private static PropertyInfo foundThaiScriptProperty;
        private static PropertyInfo foundWordChapterProperty;
        private static string languageFilePath = "C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/";
        private static List<ThaiNumber> numbers = new List<ThaiNumber>();
        private static Object propertyChapter;
        private static Object propertyEngDesc;
        private static Object propertyEngWord;
        private static Object propertyFonet;
        private static Object propertyHelpWord;
        private static List<String> propertyListOfStrings = new List<String>();
        private static Object propertyScript;
        private static Random randomIndex = new Random();
        private static string selectedChapter;
        private static Object SelectedPropertyToValidate;
        private static Object SelectedPropertyToDisplay;
        private static List<Vowel> vowels = new List<Vowel>();
        private static string whatListTLoad;
        private static string whatToTrain;
        private static string whatToDisplay;
        private static List<Word> words = new List<Word>();

        public MainWindow()
        {
            InitializeComponent();

            ClearFields();

            lbl_Counter_Page2.Content = CurrentFile;
            lbl_Counter_Page1.Content = CurrentFile;
            txb_FilePath_Settings.Text = LanguageFilePath;
            txt_NewSavePath_Settings.Text = LanguageFilePath;

            //ShowAllPropertiesFoundInConsole();

            LoadFiles<Word>(Words);

            LoadFiles<Consonant>(Consonants);

            LoadFiles<Vowel>(Vowels);

            LoadFiles<ThaiNumber>(Numbers);

            CheckList();

            SetInitialStates();
        }

        #region getters and setters for properties

        public static List<Consonant> Consonants { get => consonants; set => consonants = value; }
        public static int CorrectPoints { get => correctPoints; set => correctPoints = value; }
        public static int CurrentFile { get => currentFile; set => currentFile = value; }
        public static List<Word> DisplayList1 { get => DisplayList; set => DisplayList = value; }
        public static PropertyInfo FoundEngDescProperty { get => foundEngDescProperty; set => foundEngDescProperty = value; }
        public static PropertyInfo FoundEngWordProperty { get => foundEngWordProperty; set => foundEngWordProperty = value; }
        public static PropertyInfo FoundEngWordsProperty { get => foundEngWordsProperty; set => foundEngWordsProperty = value; }
        public static PropertyInfo FoundThaiFonetProperty { get => foundThaiFonetProperty; set => foundThaiFonetProperty = value; }
        public static PropertyInfo FoundThaiHelpWordProperty { get => foundThaiHelpWordProperty; set => foundThaiHelpWordProperty = value; }
        public static PropertyInfo FoundThaiScriptProperty { get => foundThaiScriptProperty; set => foundThaiScriptProperty = value; }
        public static PropertyInfo FoundWordChapterProperty { get => foundWordChapterProperty; set => foundWordChapterProperty = value; }
        public static string LanguageFilePath { get => languageFilePath; set => languageFilePath = value; }
        public static List<ThaiNumber> Numbers { get => numbers; set => numbers = value; }
        public static object PropertyChapter { get => propertyChapter; set => propertyChapter = value; }
        public static object PropertyEngDesc { get => propertyEngDesc; set => propertyEngDesc = value; }
        public static object PropertyEngWord { get => propertyEngWord; set => propertyEngWord = value; }
        public static object PropertyFonet { get => propertyFonet; set => propertyFonet = value; }
        public static object PropertyHelpWord { get => propertyHelpWord; set => propertyHelpWord = value; }
        public static List<string> PropertyListOfStrings { get => propertyListOfStrings; set => propertyListOfStrings = value; }
        public static object PropertyScript { get => propertyScript; set => propertyScript = value; }
        public static Random RandomIndex { get => randomIndex; set => randomIndex = value; }
        public static string SelectedChapter { get => selectedChapter; set => selectedChapter = value; }
        public static object SelectedPropertyToValidate1 { get => SelectedPropertyToValidate; set => SelectedPropertyToValidate = value; }
        public static List<Vowel> Vowels { get => vowels; set => vowels = value; }
        public static string WhatListTLoad { get => whatListTLoad; set => whatListTLoad = value; }
        public static string WhatToTrain { get => whatToTrain; set => whatToTrain = value; }
        public static List<Word> Words { get => words; set => words = value; }
        public static string WhatToDisplay { get => whatToDisplay; set => whatToDisplay = value; }
        public static object SelectedPropertyToDisplay1 { get => SelectedPropertyToDisplay; set => SelectedPropertyToDisplay = value; }

        #endregion getters and setters for properties

        /// <summary>
        /// Turns the induvidual words into one string to display
        /// </summary>
        /// <returns>String of words</returns>
        public static String EngWordsString()
        {
            String engwords = "";
            foreach (String meaning in PropertyListOfStrings)
            {
                if (PropertyListOfStrings.Last() == meaning)
                {
                    engwords += meaning;
                }
                else
                {
                    engwords += meaning + "; ";
                }
            }
            return engwords;
        }

        /// <summary>
        ///Sets the properties of the object it recives.
        /// </summary>
        /// <param name="recived">what object to find properties for</param>
        public static void SetPropertyOfGenericObject(Object recived)
        {
            PropertyListOfStrings.Clear();

            FoundThaiScriptProperty = recived.GetType().GetProperty("ThaiScript");
            FoundThaiFonetProperty = recived.GetType().GetProperty("ThaiFonet");
            FoundThaiHelpWordProperty = recived.GetType().GetProperty("ThaiHelpWord");

            FoundEngWordProperty = recived.GetType().GetProperty("EngWord");

            FoundEngWordsProperty = recived.GetType().GetProperty("EngWords");

            FoundEngDescProperty = recived.GetType().GetProperty("EngDesc");
            FoundWordChapterProperty = recived.GetType().GetProperty("Chapter");

            var item = FoundEngWordsProperty.GetValue(recived);

            if (FoundThaiScriptProperty?.CanWrite == true)
            {
                PropertyScript = FoundThaiScriptProperty.GetValue(recived);
            }

            if (FoundThaiFonetProperty?.CanWrite == true)
            {
                PropertyFonet = FoundThaiFonetProperty.GetValue(recived);
            }

            if (FoundThaiHelpWordProperty?.CanWrite == true)
            {
                PropertyHelpWord = FoundThaiHelpWordProperty.GetValue(recived);
            }
            if (item != null)
            {
                if ((item as List<String>)?.Count() == 0)
                {
                    if (FoundEngWordProperty?.CanWrite == true)
                    {
                        //propertyEngWord = foundEngWordProperty.GetValue(recived);
                        String stringtosplit = (String)FoundEngWordProperty.GetValue(recived);
                        PropertyListOfStrings = stringtosplit.Split(';', ',').ToList<String>();
                    }
                }
                else
                {
                    if (FoundEngWordsProperty?.CanWrite == true && item != null)
                    {
                        foreach (var listItem in item as List<String>)
                        {
                            PropertyListOfStrings.Add(listItem);
                        }
                    }
                }
            }

            if (FoundEngDescProperty?.CanWrite == true)
            {
                PropertyEngDesc = FoundEngDescProperty.GetValue(recived);
            }

            if (FoundWordChapterProperty?.CanWrite == true)
            {
                PropertyChapter = FoundWordChapterProperty.GetValue(recived);
            }
        }

        /// <summary>
        /// test method to check contents of list to console.
        /// </summary>
        public void CheckList()
        {
            foreach (Word word in Words)
            {
                System.Console.WriteLine(word.ThaiScript + " " + word.ThaiFonet + " " + word.EngWord + " " + word.EngDesc + " " + word.Chapter);
            }
        }

        /// <summary>
        /// Clears the textboxs and textblocks to make the application look clean.
        /// </summary>
        public void ClearFields()
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

        /// <summary>
        /// Deletes the selected word from
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to delete from</param>
        public void DeleteSelected<T>(List<T> list)
        {
            MessageBox.Show("tried to remove element " + list[lib_LoadedWords.SelectedIndex].ToString());
            list.RemoveAt(lib_LoadedWords.SelectedIndex);
        }

        /// <summary>
        /// Find all words in a chapter and add to displayList
        /// </summary>
        public void FindWordWithChapter()
        {
            DisplayList1.Clear();

            foreach (Word word in Words)
            {
                if (word.Chapter == SelectedChapter)
                {
                    DisplayList1.Add(word);
                }
            }
        }

        /// <summary>
        /// Load files to lists.
        /// </summary>
        /// <typeparam name="T">What type to load</typeparam>
        /// <param name="list">What list to load into</param>
        public void LoadFiles<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml");

            List<T> newWordToAdd = new List<T>();

            foreach (T wordFoundInFile in wordsFromFIle)
            {
                SetPropertyOfGenericObject(wordFoundInFile);

                newWordToAdd.Add(wordFoundInFile);
            }

            list.AddRange(newWordToAdd);
        }

        /// <summary>
        /// Saves the content of list to file
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SaveFiles<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }

        /// <summary>
        /// Handles the selection changes in the listbox
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SelectionChanged<T>(List<T> list)
        {
            Object wordToLoad;

            Type whatIsT = typeof(T);

            if (lib_LoadedWords.SelectedIndex != -1)
            {
                #region Find properties of selected word

                wordToLoad = lib_LoadedWords.SelectedItem;

                PropertyInfo wordToLoadThaiScript = wordToLoad.GetType().GetProperty("ThaiScript");
                PropertyInfo wordToLoadThaiFonetProperty = wordToLoad.GetType().GetProperty("ThaiFonet");
                PropertyInfo wordToLoadThaiHelpWordProperty = wordToLoad.GetType().GetProperty("ThaiHelpWord");
                PropertyInfo wordToLoadEngWordProperty = wordToLoad.GetType().GetProperty("EngWord");
                PropertyInfo wordToLoadEngWordsProperty = wordToLoad.GetType().GetProperty("EngWords");

                PropertyInfo wordToLoadDescProperty = wordToLoad.GetType().GetProperty("EngDesc");
                PropertyInfo wordToLoadWordChapterProperty = wordToLoad.GetType().GetProperty("Chapter");

                Object wordToLoadScript = null;
                Object wordToLoadFonet = null;
                Object wordToLoadHelpWord = null;
                Object wordToLoadEngWord = null;
                Object wordToLoadEngWords = null;
                Object wordToLoadEngDesc = null;
                Object wordToLoadChapter = null;

                if (wordToLoadThaiScript?.CanWrite == true)
                {
                    wordToLoadScript = wordToLoadThaiScript.GetValue(wordToLoad);
                }
                if (wordToLoadThaiFonetProperty?.CanWrite == true)
                {
                    wordToLoadFonet = wordToLoadThaiFonetProperty.GetValue(wordToLoad);
                }
                if (wordToLoadThaiHelpWordProperty?.CanWrite == true)
                {
                    wordToLoadHelpWord = wordToLoadThaiHelpWordProperty.GetValue(wordToLoad);
                }
                if (wordToLoadEngWordProperty?.CanWrite == true)
                {
                    wordToLoadEngWord = wordToLoadEngWordProperty.GetValue(wordToLoad);
                }
                if (wordToLoadEngWordsProperty?.CanWrite == true)
                {
                    wordToLoadEngWords = wordToLoadEngWordsProperty.GetValue(wordToLoad);
                }
                if (wordToLoadDescProperty?.CanWrite == true)
                {
                    wordToLoadEngDesc = wordToLoadDescProperty.GetValue(wordToLoad);
                }
                if (wordToLoadWordChapterProperty?.CanWrite == true)
                {
                    wordToLoadChapter = wordToLoadWordChapterProperty.GetValue(wordToLoad);
                }

                #endregion Find properties of selected word

                foreach (T wordFromList in list)
                {
                    SetPropertyOfGenericObject(wordFromList);

                    if (wordFromList.GetType() == typeof(Word) && (String)wordToLoadChapter == (String)PropertyChapter)
                    {
                        if ((String)wordToLoadScript == (String)PropertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)PropertyScript;
                            txt_SecondSelectionProperty.Text = (String)PropertyFonet;
                            txt_ThirdSelectionProperty.Text = EngWordsString();
                            txt_FourthSelectionProperty.Text = (String)PropertyEngDesc;
                            txt_FifthSelectionProperty.Text = (String)PropertyChapter;
                            txb_Description_Page4.Text = (String)PropertyEngDesc;
                            break;
                        }
                    }
                    else if (wordFromList.GetType() == typeof(Consonant) || wordFromList.GetType() == typeof(Vowel) || wordFromList.GetType() == typeof(ThaiNumber))
                    {
                        if ((String)wordToLoadScript == (String)PropertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)PropertyScript;
                            txt_SecondSelectionProperty.Text = (String)PropertyFonet;
                            txt_ThirdSelectionProperty.Text = (String)PropertyHelpWord;
                            txt_FourthSelectionProperty.Text = EngWordsString();
                            txt_FifthSelectionProperty.Text = (String)PropertyEngDesc;
                            txb_Description_Page4.Text = (String)PropertyEngDesc;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// selects a single property from a object
        /// </summary>
        /// <param name="selectProperty">What property to select</param>
        /// <returns>The selected property</returns>
        public Object SelectProperty(String selectProperty)
        {
            if (selectProperty == "ThaiScript")
            {
                return PropertyScript;
            }
            else if (selectProperty == "ThaiHelpWord")
            {
                return PropertyHelpWord;
            }
            else if (selectProperty == "ThaiFonet")
            {
                return PropertyFonet;
            }
            else if (selectProperty == "EngWords")
            {
                return PropertyListOfStrings;
            }
            else
            {
                MessageBox.Show("Please select what you want to practise.");
                return null;
            }
        }

        /// <summary>
        /// Sets the initial states for checkboxes and the combobox.
        /// </summary>
        public void SetInitialStates()
        {
            ckb_Helpbox_Page1.IsChecked = true;
            ckb_Helpbox_Page2.IsChecked = true;
            rb_TrainFonet_Page1.IsChecked = true;
            rb_Conson_Page2.IsChecked = true;
            cb_Chapter_Page1.SelectedIndex = 0;

        }

        /// <summary>
        /// sets the new values to the old words that already can be found in list
        /// </summary>
        /// <param name="oldWord">What word to change or add values</param>
        /// <param name="whatIsT">What type to work with</param>
        /// <param name="firstProperty"></param>
        /// <param name="secondProperty"></param>
        /// <param name="thirdProperty"></param>
        /// <param name="fourthProperty"></param>
        /// <param name="FifthProperty"></param>
        public void SetNewValuesToOldWord(Object oldWord, Type whatIsT, String firstProperty, String secondProperty, String thirdProperty, String fourthProperty, String FifthProperty)
        {
            if (FoundThaiScriptProperty?.CanWrite == true)
            {
                FoundThaiScriptProperty.SetValue(oldWord, firstProperty, null);
            }

            if (FoundThaiFonetProperty?.CanWrite == true)
            {
                FoundThaiFonetProperty.SetValue(oldWord, secondProperty, null);
            }

            if ((FoundThaiHelpWordProperty?.CanWrite == true && whatIsT.GetType() == typeof(Consonant)) || whatIsT.GetType() == typeof(Vowel))
            {
                if (oldWord.GetType() != typeof(Word))
                {
                    FoundThaiHelpWordProperty.SetValue(oldWord, thirdProperty, null);
                }
            }

            if (FoundEngWordsProperty?.CanWrite == true)
            {
                if (oldWord.GetType() == typeof(Word))
                {
                    FoundEngWordsProperty.SetValue(oldWord, thirdProperty.Split(';', ',').ToList<String>(), null);
                }
                else
                {
                    FoundEngWordsProperty.SetValue(oldWord, fourthProperty.Split(';', ',').ToList<String>(), null);
                }
            }

            if (FoundWordChapterProperty?.CanWrite == true)
            {
                try
                {
                    FoundWordChapterProperty.SetValue(oldWord, FifthProperty, null);
                }
                catch (FormatException)
                {
                }
                catch (OverflowException)
                {
                }
            }
            if (FoundEngDescProperty?.CanWrite == true)
            {
                if (oldWord.GetType() == typeof(Word))
                {
                    FoundEngDescProperty.SetValue(oldWord, fourthProperty, null);
                }
                else
                {
                    FoundEngDescProperty.SetValue(oldWord, FifthProperty, null);
                }
            }
        }

        /// <summary>
        ///Test method that shows all properties in the console.
        /// </summary>
        public void ShowAllPropertiesFoundInConsole()
        {
            List<Word> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<Word>>(LanguageFilePath + "Thai_Word.xml");

            foreach (Word word in wordsFromFIle)
            {
                Console.Write(word);
                System.Console.WriteLine(word.ThaiScript + " " + word.ThaiFonet + " " + word.EngWord + " " + word.EngDesc + " " + word.Chapter);
            }
        }

        /// <summary>
        /// Adds new words to the list.
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SubmitNewWord<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            PropertyInfo foundNewWordThaiScriptProperty = null;
            PropertyInfo foundNewWordEngWordsProperty = null;
            PropertyInfo foundNewWordChapter = null;

            object newWord = null;

            object property_ThaiScript_Found_In_Word = null;
            object property_EngWords_Found_In_Word = null;
            object property_Chapter_Found_In_Word = null;

            bool existsInList = false;

            List<String> engWordList;

            #region find values and type of the new object

            if (txt_FirstSelectionProperty.Text != "" && txt_SecondSelectionProperty.Text != "" && txt_ThirdSelectionProperty.Text != "" && txt_FourthSelectionProperty.Text != "")
            {
                if (whatIsT == typeof(Word))
                {
                    engWordList = txt_ThirdSelectionProperty.Text.Split(';', ',').ToList<String>();
                    newWord = new Word(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, engWordList, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                }
                else if (whatIsT == typeof(Consonant))
                {
                    engWordList = txt_FourthSelectionProperty.Text.Split(';', ',').ToList<String>();
                    newWord = new Consonant(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, engWordList, txt_FifthSelectionProperty.Text);
                }
                else if (whatIsT == typeof(Vowel))
                {
                    engWordList = txt_FourthSelectionProperty.Text.Split(';', ',').ToList<String>();
                    newWord = new Vowel(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, engWordList, txt_FifthSelectionProperty.Text);
                }
                else
                {
                    engWordList = txt_FourthSelectionProperty.Text.Split(';', ',').ToList<String>();
                    newWord = new ThaiNumber(txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, engWordList, txt_FifthSelectionProperty.Text);
                }

                foundNewWordThaiScriptProperty = newWord.GetType().GetProperty("ThaiScript");
                foundNewWordEngWordsProperty = newWord.GetType().GetProperty("EngWords");
                foundNewWordChapter = newWord.GetType().GetProperty("Chapter");

                property_ThaiScript_Found_In_Word = foundNewWordThaiScriptProperty.GetValue(newWord);
                property_EngWords_Found_In_Word = foundNewWordEngWordsProperty.GetValue(newWord);
                if (foundNewWordChapter != null)
                {
                    property_Chapter_Found_In_Word = foundNewWordChapter.GetValue(newWord);
                }

                #endregion find values and type of the new object

                //try and combine both foreach perhaps

                #region Check if new word exists in the list

                foreach (T oldWord in list)
                {
                    SetPropertyOfGenericObject(oldWord);

                    if ((property_ThaiScript_Found_In_Word == PropertyScript && (String)property_ThaiScript_Found_In_Word != "") || (PropertyEngWord == property_EngWords_Found_In_Word && (String)property_EngWords_Found_In_Word != ""))
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            if (property_Chapter_Found_In_Word == PropertyChapter)
                            {
                                System.Console.WriteLine("exists");
                                existsInList = true;
                                break;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("exists");
                            existsInList = true;
                            break;
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Does not exist");
                        existsInList = false;
                    }
                }

                if (!existsInList)
                {
                    if ((String)property_ThaiScript_Found_In_Word != "" || (String)property_EngWords_Found_In_Word != "")
                    {
                        list.Add((T)newWord);
                        existsInList = true;
                    }
                }

                #endregion Check if new word exists in the list

                #region add new values to old word

                foreach (T oldWord in list)
                {
                    SetPropertyOfGenericObject(oldWord);

                    if ((String)PropertyScript == txt_FirstSelectionProperty.Text && txt_FirstSelectionProperty.Text != "" /*|| (List<String>)propertyEngWord == txt_ThirdSelectionProperty.Text*/ && txt_ThirdSelectionProperty.Text != "")
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            if ((String)PropertyChapter == txt_FifthSelectionProperty.Text && txt_FifthSelectionProperty.Text != "")
                            {
                                SetNewValuesToOldWord(oldWord, whatIsT, txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                                break;
                            }
                        }
                        else
                        {
                            SetNewValuesToOldWord(oldWord, whatIsT, txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                            break;
                        }
                    }
                }

                #endregion add new values to old word

                ClearFields();

                SaveFiles<T>(list);
            }
        }

        /// <summary>
        /// Changes the content of textblocks tot he next or previous value.
        /// </summary>
        /// <typeparam name="T">What type of object to handle</typeparam>
        /// <param name="list">What list to work with</param>
        /// <param name="textBlockForScript">What textblock to write the thai script too</param>
        /// <param name="textBlockDescription">What textblock to write the description too</param>
        /// <param name="checkBoxDescription">What checkbox to use</param>
        /// <param name="checkBoxRandom">what checkbox to use</param>
        /// <param name="nextValueToAdd">to move forward, backwards or stay in place in the list</param>
        public void TextChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, CheckBox checkBoxDescription, CheckBox checkBoxRandom, int nextValueToAdd) where T : new()
        {
            Type whatIsT = typeof(T);

            
            #region check I Size

            if (checkBoxRandom.IsChecked == true)
            {
                CurrentFile = RandomIndex.Next(0, list.Count);
            }
            else
            {
                if (nextValueToAdd > 0)
                {
                    CurrentFile++;
                    if (CurrentFile > list.Count - 1)
                    {
                        CurrentFile = 0;
                    }
                }
                else if (nextValueToAdd < 0)
                {
                    CurrentFile--;
                    if (CurrentFile < 0)
                    {
                        CurrentFile = list.Count - 1;
                    }
                }
                else
                {
                    textBlockForScript.Text = (String)PropertyScript;
                }
            }

            #endregion check I Size

            SetPropertyOfGenericObject(list[CurrentFile]);
            if (whatIsT == typeof(Word))
            {

                SelectedPropertyToDisplay = SelectProperty(WhatToDisplay);

            }
            if (list[CurrentFile].GetType() == typeof(Word))
            {
                if ((string)PropertyChapter == SelectedChapter)
                {
                    if(SelectedPropertyToDisplay is List<String>)
                    {
                        textBlockForScript.Text = EngWordsString();
                    }
                    textBlockForScript.Text = (String)SelectedPropertyToDisplay;
                }
                else
                {
                    MessageBox.Show("There are no content with chapter" + SelectedChapter + " available right now.");
                    return;
                }
            }
            else
            {
                textBlockForScript.Text = PropertyScript + " " + PropertyHelpWord;
            }

            if (checkBoxDescription.IsChecked == true)
            {
                textBlockDescription.Text = PropertyFonet + "\r\n" + EngWordsString() + "\r\n" + PropertyEngDesc;
            }
        }

        /// <summary>
        /// Checks what list to load and loads it.
        /// </summary>
        public void UpdateListBox()
        {
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false && rb_ThaiNumber_Page3.IsChecked == false)
            {
                MessageBox.Show("Select a list to load from");
                return;
            }

            lib_LoadedWords.ItemsSource = null;

            if (WhatListTLoad == "Word")
            {
                foreach (Word word in Words)
                {
                    if (word.ThaiScript?.Length == 0)
                    {
                        lib_LoadedWords.Items.Add(word.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.ItemsSource = Words;
                        break;
                    }
                }
            }
            else if (WhatListTLoad == "Conso")
            {
                foreach (Consonant conso in Consonants)
                {
                    if (conso.ThaiScript?.Length == 0)
                    {
                        lib_LoadedWords.Items.Add(conso.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.ItemsSource = Consonants;
                        break;
                    }
                }
            }
            else if (WhatListTLoad == "Vowel")
            {
                foreach (Vowel vowel in Vowels)
                {
                    if (vowel.ThaiScript?.Length == 0)
                    {
                        lib_LoadedWords.Items.Add(vowel.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.ItemsSource = Vowels;
                        break;
                    }
                }
            }
            else if (WhatListTLoad == "ThaiNumber")
            {
                foreach (ThaiNumber number in Numbers)
                {
                    if (number.ThaiScript?.Length == 0)
                    {
                        lib_LoadedWords.Items.Add(number.EngWord);
                    }
                    else
                    {
                        lib_LoadedWords.ItemsSource = Numbers;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Compares the written answear to the current words propterties.
        /// </summary>
        /// <typeparam name="T">What kind of object</typeparam>
        /// <param name="list">What list to load</param>
        /// <param name="textboxAnswear">What textbox to use for answears</param>
        /// <param name="textBlockStatus">What textblock to use for right or worng</param>
        /// <param name="textBlockDesc">What textblock to use for description</param>
        /// <param name="checkBoxDesc">What checkbox to use to check if description is on</param>
        public void ValidateAnswear<T>(List<T> list, TextBox textboxAnswear, TextBlock textBlockStatus, TextBlock textBlockDesc, CheckBox checkBoxDesc)
        {
            SetPropertyOfGenericObject(list[CurrentFile]);

            SelectedPropertyToValidate = SelectProperty(WhatToTrain);

            bool somethingRight = false;
            int rightAnswears = 0;
            if (SelectedPropertyToValidate is List<String>)
            {
                List<String> answears = textboxAnswear.Text.Split(';', ',').ToList<String>();

                foreach (String correctWord in SelectedPropertyToValidate as List<String>)
                {
                    foreach (String answear in answears)
                    {
                        if (answear == correctWord)
                        {
                            textBlockStatus.Text = "Correct!";
                            CorrectPoints++;
                            rightAnswears++;
                            somethingRight = true;
                        }
                        else if (!somethingRight)
                        {
                            textBlockStatus.Text = "Wrong...";
                        }
                    }
                }
                if (rightAnswears == (SelectedPropertyToValidate as List<String>)?.Count)
                {
                    textBlockStatus.Text = "You got it all correct!";
                }
            }
            else if ((String)SelectedPropertyToValidate1 == textboxAnswear.Text)
            {
                textBlockStatus.Text = "Correct!";
                CorrectPoints++;
            }
            else
            {
                textBlockStatus.Text = "Wrong...";
            }

            if (checkBoxDesc.IsChecked == true)
            {
                textBlockDesc.Text = PropertyFonet + "\r\n" + EngWordsString() + "\r\n" + PropertyEngDesc;
            }

            lbl_Counter_Page2.Content = CurrentFile;
            lbl_Points.Content = "Points: " + CorrectPoints;
            lbl_Points_Page2.Content = "Points: " + CorrectPoints;
        }

        #region component interaction

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected word?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (rb_words_Page3.IsChecked == true)
                {
                    DeleteSelected<Word>(Words);
                    SaveFiles<Word>(Words);
                }
                else if (rb_Conson_Page2.IsChecked == true)
                {
                    DeleteSelected<Consonant>(Consonants);
                    SaveFiles<Consonant>(Consonants);
                }
                else if (rb_Vowel_Page3.IsChecked == true)
                {
                    DeleteSelected<Vowel>(Vowels);
                    SaveFiles<Vowel>(Vowels);
                }
                else if (rb_ThaiNumber_Page3.IsChecked == true)
                {
                    DeleteSelected<ThaiNumber>(Numbers);
                    SaveFiles<ThaiNumber>(Numbers);
                }
                else
                {
                    MessageBox.Show("Please select a list first");
                }
            }
            UpdateListBox();
        }

        private void Btn_LoadList_Click(object sender, RoutedEventArgs e)
        {
            UpdateListBox();
        }

        private void Btn_Next_Word_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {
                TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else
            {
                MessageBox.Show("please select a category");
            }
            lbl_Counter_Page2.Content = CurrentFile;
        }

        private void Btn_Next_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            //selectedChapter = CheckPage1RB();

            ClearFields();

            TextChanger<Word>(DisplayList1, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 1);

            lbl_Counter_Page1.Content = CurrentFile;
        }

        private void Btn_Prev_Word_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {
                TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else
            {
                MessageBox.Show("please select one a category");
            }
            lbl_Counter_Page2.Content = CurrentFile;
        }

        private void Btn_Prev_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            TextChanger<Word>(DisplayList1, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, -1);
            lbl_Counter_Page1.Content = CurrentFile;
        }

        private void Btn_SubmitNewWord_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false && rb_ThaiNumber_Page3.IsChecked == false)
            {
                MessageBox.Show("Select a list to load from");
                return;
            }
            else if (rb_Conso_Page3.IsChecked == true)
            {
                SubmitNewWord<Consonant>(Consonants);
            }
            else if (rb_Vowel_Page3.IsChecked == true)
            {
                SubmitNewWord<Vowel>(Vowels);
            }
            else if (rb_words_Page3.IsChecked == true)
            {
                SubmitNewWord<Word>(Words);
            }
            else
            {
                SubmitNewWord<ThaiNumber>(Numbers);
            }
            UpdateListBox();
        }

        private void Btn_validate_Page1_Click(object sender, RoutedEventArgs e)
        {
            ValidateAnswear<Word>(DisplayList1, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1, ckb_Helpbox_Page1);
        }

        private void Btn_validate_Page2_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conson_Page2.IsChecked == true)
            {
                ValidateAnswear<Consonant>(Consonants, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                ValidateAnswear<Vowel>(Vowels, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else if (rb_Closing_Page2.IsChecked == true)
            {
                //TODO
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                ValidateAnswear<ThaiNumber>(Numbers, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else
            {
                MessageBox.Show("Please select a category.");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFile = 0;
            SelectedChapter = cb_Chapter_Page1.SelectedValue.ToString();

            FindWordWithChapter();

            lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList1.Count.ToString();

            if (DisplayList1.Count > 0)
            {
                TextChanger<Word>(DisplayList1, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 0);
            }
            else
            {
                MessageBox.Show("There are no words in that category yet");
            }

            lbl_Counter_Page1.Content = CurrentFile;
        }

        private void Lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (rb_words_Page3.IsChecked == true)
            {
                SelectionChanged<Word>(Words);
            }
            else if (rb_Conso_Page3.IsChecked == true)
            {
                SelectionChanged<Consonant>(Consonants);
            }
            else if (rb_Vowel_Page3.IsChecked == true)
            {
                SelectionChanged<Vowel>(Vowels);
            }
            else if (rb_ThaiNumber_Page3.IsChecked == true)
            {
                SelectionChanged<ThaiNumber>(Numbers);
            }
            else
            {
                MessageBox.Show("Please select a list to load", "List not choosen");
            }
        }

        private void Rb_Conso_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = "Conso";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";
            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_Conson_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFile = 0;
            //txb_ThaiScript_Page2.Text = Consonants[CurrentFile].ThaiScript + " " + Consonants[CurrentFile].ThaiHelpWord;
            TextChanger<Consonant>(consonants,txb_ThaiScript_Page2,txb_Description_page2,ckb_Helpbox_Page2,ckb_Randomized_Page2,0);
            lbl_Counter_Page2.Content = CurrentFile;
            txb_Information_Page2.Text = "To property pronounce a Thai Consonant you add the sound from อ (o).";
        }

        private void Rb_NumberSymbol_Page2_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFile = 0;
            //txb_ThaiScript_Page2.Text = Numbers[CurrentFile].ThaiScript + " " + Numbers[CurrentFile].ThaiHelpWord;
            TextChanger<ThaiNumber>(numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 0);
            lbl_Counter_Page2.Content = CurrentFile;

            txb_Information_Page2.Text = "";
        }

        private void Rb_ThaiNumber_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = "ThaiNumber";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_TrainEngWords_Checked(object sender, RoutedEventArgs e)
        {
            WhatToTrain = "EngWords";
            WhatToDisplay = "ThaiScript";
        }

        private void Rb_TrainFonet_Checked(object sender, RoutedEventArgs e)
        {
            WhatToTrain = "ThaiFonet";
            WhatToDisplay = "ThaiScript";
        }

        private void Rb_TrainHelpWord_Checked(object sender, RoutedEventArgs e)
        {
            WhatToTrain = "ThaiHelpWord";
            WhatToDisplay = "ThaiScript";
        }

        private void Rb_TrainScript_Checked(object sender, RoutedEventArgs e)
        {
            WhatToTrain = "ThaiScript";
            WhatToDisplay = "EngWords";
        }

        private void Rb_Vowel_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFile = 0;
            //txb_ThaiScript_Page2.Text = Vowels[CurrentFile].ThaiScript + " " + Vowels[CurrentFile].ThaiHelpWord;
            TextChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 0);
            lbl_Counter_Page2.Content = CurrentFile;

            txb_Information_Page2.Text = "When you are reading a vowel, you almost always pronaounce the consonant first, then the surounding vowel.";
        }

        private void Rb_Vowel_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = "Vowel";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_words_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = "Word";

            ClearFields();
            UpdateListBox();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";
        }

        #endregion component interaction

        private void ckb_Helpbox_Page2_Checked(object sender, RoutedEventArgs e)
        {
            if (ckb_Helpbox_Page2.IsChecked == true)
            {
                txb_Description_page2.Text = PropertyFonet + "\r\n" + EngWordsString() + "\r\n" + PropertyEngDesc;
            }
            else
            {
                txb_Description_page2.Text = "";
            }
        }

        private void ckb_Helpbox_Page1_Checked(object sender, RoutedEventArgs e)
        {
            if (ckb_Helpbox_Page1.IsChecked == true)
            {
                txb_Description_page1.Text = PropertyFonet + "\r\n" + EngWordsString() + "\r\n" + PropertyEngDesc;
            }
            else
            {
                txb_Description_page1.Text = "";
            }
        }
    }
}