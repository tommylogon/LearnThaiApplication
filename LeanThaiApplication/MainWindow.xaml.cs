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
        static List<Word> words = new List<Word>();

        static List<Consonant> consonants = new List<Consonant>();

        static List<Vowel> vowels = new List<Vowel>();
        static List<ThaiNumber> numbers = new List<ThaiNumber>();

        static List<Word> DisplayList = new List<Word>();
       
        int i = 0;

        int correctPoints=0;

        string languageFilePath = "C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/";


        string selectedChapter;

        string whatToTrain;

        string whatListTLoad;

        Random random = new Random();


       

        PropertyInfo foundThaiScriptProperty;
        PropertyInfo foundThaiHelpWordProperty;
        PropertyInfo foundThaiFonetProperty;
        PropertyInfo foundEngWordProperty;
        PropertyInfo foundEngWordsProperty;
        PropertyInfo foundEngDescProperty;
        PropertyInfo foundWordChapterProperty;

        Object propertyScript;
        Object propertyFonet;
        Object propertyHelpWord;
        Object propertyEngWord;
        List<String> propertyListOfStrings = new List<String>();
        Object propertyEngDesc;
        Object propertyChapter;

        Object SelectedPropertyToValidate;

        public MainWindow()
        {
            InitializeComponent();

            ClearFields();


            lbl_Counter_Page2.Content = i;
            lbl_Counter_Page1.Content = i;
            txb_FilePath_Settings.Text = languageFilePath;
            txt_NewSavePath_Settings.Text = languageFilePath;


            //ShowAllPropertiesFoundInConsole();

            LoadFiles<Word>(words);
            
            LoadFiles<Consonant>(consonants);

            LoadFiles<Vowel>(vowels);

            LoadFiles<ThaiNumber>(numbers);
            
            CheckList();

            SetInitialStates();
        }

        /// <summary>
        /// Sets the initial states for checkboxes and the combobox.
        /// </summary>
        public void SetInitialStates()
        {
            ckb_Helpbox_Page1.IsChecked = true;
            ckb_Helpbox_Page2.IsChecked = true;
            cb_Chapter_Page1.SelectedIndex = 0;
            rb_Conson_Page2.IsChecked = true;
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
                        lib_LoadedWords.ItemsSource = words;
                        break;
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
                        lib_LoadedWords.ItemsSource = consonants;
                        break;
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
                        lib_LoadedWords.ItemsSource = vowels;
                        break;
                    }
                }
            }
            else if (whatListTLoad == "ThaiNumber")
            {
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
                else if (nextValueToAdd < 0)
                {
                    i--;
                    if (i < 0)
                    {
                        i = list.Count - 1;
                    }
                }
                else
                {
                    textBlockForScript.Text = (String)propertyScript;
                }
            }
            #endregion

            SetPropertyOfGenericObject(list[i]);

            if (list[i].GetType() == typeof(Word))
            {
                

                if ((string)propertyChapter == selectedChapter)
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
                

                textBlockForScript.Text = propertyScript + " " + propertyHelpWord;
            }

            if (checkBoxDescription.IsChecked == true)
            {

                textBlockDescription.Text = propertyFonet + "\r\n" + EngWordsString() + "\r\n" + propertyEngDesc;
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
                if(foundNewWordChapter != null)
                {
                    property_Chapter_Found_In_Word = foundNewWordChapter.GetValue(newWord);
                }



                #endregion

                //try and combine both foreach perhaps

                #region Check if new word exists in the list
                foreach (T oldWord in list)
                {
                    SetPropertyOfGenericObject(oldWord);

                    if (property_ThaiScript_Found_In_Word == propertyScript && (String)property_ThaiScript_Found_In_Word != "" || propertyEngWord == property_EngWords_Found_In_Word && (String)property_EngWords_Found_In_Word != "")
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            if(property_Chapter_Found_In_Word == propertyChapter)
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
            

                if (existsInList == false)
                {
                    if ((String)property_ThaiScript_Found_In_Word != "" || (String)property_EngWords_Found_In_Word != "")
                    {
                        list.Add((T)newWord);
                        existsInList = true;
                    }                    
                }

                #endregion

                #region add new values to old word

                foreach (T oldWord in list)
                {
                        SetPropertyOfGenericObject(oldWord);

                    if ((String)propertyScript == txt_FirstSelectionProperty.Text && txt_FirstSelectionProperty.Text != "" /*|| (List<String>)propertyEngWord == txt_ThirdSelectionProperty.Text*/ && txt_ThirdSelectionProperty.Text != "")
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            if ((String)propertyChapter == txt_FifthSelectionProperty.Text && txt_FifthSelectionProperty.Text != "")
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
                #endregion

                ClearFields();

                SaveFiles<T>(list);
            }



        }

        /// <summary>
        /// sets the new values to the old words that already can be found in list
        /// </summary>
        /// <param name="oldWord">What word to change or add values</param>
        /// <param name="whatIsT">What type to work with</param>
        public void SetNewValuesToOldWord(Object oldWord, Type whatIsT, String firstProperty, String secondProperty, String thirdProperty, String fourthProperty, String FifthProperty)
        {

            if (null != foundThaiScriptProperty && foundThaiScriptProperty.CanWrite)
            {
                foundThaiScriptProperty.SetValue(oldWord, firstProperty, null);
            }

            if (null != foundThaiFonetProperty && foundThaiFonetProperty.CanWrite)
            {
                foundThaiFonetProperty.SetValue(oldWord, secondProperty, null);
            }

            if (null != foundThaiHelpWordProperty && foundThaiHelpWordProperty.CanWrite && whatIsT.GetType() == typeof(Consonant) || whatIsT.GetType() == typeof(Vowel))
            {
                if (oldWord.GetType() != typeof(Word))
                {
                    foundThaiHelpWordProperty.SetValue(oldWord, thirdProperty, null);
                }

            }

            if (null != foundEngWordsProperty && foundEngWordsProperty.CanWrite)
            {
                if (oldWord.GetType() == typeof(Word))
                {
                    foundEngWordsProperty.SetValue(oldWord, thirdProperty.Split(';', ',').ToList<String>(), null);
                }
                else
                {
                    foundEngWordsProperty.SetValue(oldWord, fourthProperty.Split(';', ',').ToList<String>(), null);
                }

            }

            if (null != foundWordChapterProperty && foundWordChapterProperty.CanWrite)
            {

                try
                {
                    foundWordChapterProperty.SetValue(oldWord, FifthProperty, null);
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
                    foundEngDescProperty.SetValue(oldWord, fourthProperty, null);
                }
                else
                {
                    foundEngDescProperty.SetValue(oldWord, FifthProperty, null);
                }

            }
        }

        /// <summary>
        /// Saves the content of list to file
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SaveFiles<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            XmlSerialization.WriteToXmlFile<List<T>>(languageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }
        
        /// <summary>
        /// Load files to lists.
        /// </summary>
        /// <typeparam name="T">What type to load</typeparam>
        /// <param name="list">What list to load into</param>
        public void LoadFiles<T>(List<T> list) where T : new()
        {

            Type whatIsT = typeof(T);

            

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>(languageFilePath + "Thai_" + whatIsT.Name + ".xml");

            List<T> newWordToAdd = new List<T>();
            

            
            foreach (T wordFoundInFile in wordsFromFIle)
            {
                SetPropertyOfGenericObject(wordFoundInFile);

                newWordToAdd.Add(wordFoundInFile);
            }

            list.AddRange(newWordToAdd);

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

            SetPropertyOfGenericObject(list[i]);
            
            
            SelectedPropertyToValidate = SelectPropertyToValidate();

            if(SelectedPropertyToValidate is List<String>)
            {
                List<String> answears = textboxAnswear.Text.Split(';', ',').ToList<String>();

                foreach (String correctWord in SelectedPropertyToValidate as List<String>)
                {
                    foreach(String answear in answears)
                    {
                        if(answear == correctWord)
                        {
                            textBlockStatus.Text = "Correct!";
                            correctPoints += 1;
                        }
                        else
                        {
                            textBlockStatus.Text = "Wrong...";
                        }
                    }
                }
                
            }
            else if ((String)SelectedPropertyToValidate == textboxAnswear.Text)
            {
                textBlockStatus.Text = "Correct!";
                correctPoints += 1;
            }
            else
            {
                textBlockStatus.Text = "Wrong...";
            }

            if (checkBoxDesc.IsChecked == true)
            {
                

                textBlockDesc.Text = propertyFonet + "\r\n" + EngWordsString() +  "\r\n" + propertyEngDesc;
            }


            lbl_Counter_Page2.Content = i;
        }


        /// <summary>
        /// Turns the induvidual words into one string to display
        /// </summary>
        /// <returns>String of words</returns>
        public String EngWordsString()
        {
            String engwords = "";
            foreach (String meaning in propertyListOfStrings)
            {
                if (propertyListOfStrings.Last() == meaning)
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
                if (null != wordToLoadEngWordsProperty && wordToLoadEngWordsProperty.CanWrite)
                {
                    wordToLoadEngWords = wordToLoadEngWordsProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadDescProperty && wordToLoadDescProperty.CanWrite)
                {
                    wordToLoadEngDesc = wordToLoadDescProperty.GetValue(wordToLoad);
                }
                if (null != wordToLoadWordChapterProperty && wordToLoadWordChapterProperty.CanWrite)
                {
                    wordToLoadChapter = wordToLoadWordChapterProperty.GetValue(wordToLoad);
                }

                #endregion

                foreach (T wordFromList in list)
                {

                    SetPropertyOfGenericObject(wordFromList);

                    if (wordFromList.GetType() == typeof(Word) && (String)wordToLoadChapter == (String)propertyChapter)
                    {
                        if ((String)wordToLoadScript == (String)propertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)propertyScript;
                            txt_SecondSelectionProperty.Text = (String)propertyFonet;
                            txt_ThirdSelectionProperty.Text = EngWordsString();
                            txt_FourthSelectionProperty.Text = (String)propertyEngDesc;
                            txt_FifthSelectionProperty.Text = (String)propertyChapter;
                            txb_Description_Page4.Text = (String)propertyEngDesc;
                            break;
                        }
                    }
                    else if (wordFromList.GetType() == typeof(Consonant) || wordFromList.GetType() == typeof(Vowel) || wordFromList.GetType() == typeof(ThaiNumber))
                    {
                        if ((String)wordToLoadScript == (String)propertyScript)
                        {
                            txt_FirstSelectionProperty.Text = (String)propertyScript;
                            txt_SecondSelectionProperty.Text = (String)propertyFonet;
                            txt_ThirdSelectionProperty.Text = (String)propertyHelpWord;
                            txt_FourthSelectionProperty.Text = EngWordsString();
                            txt_FifthSelectionProperty.Text = (String)propertyEngDesc;
                            txb_Description_Page4.Text = (String)propertyEngDesc;
                            break;
                        }

                    }
                }
            }
        }
        
        /// <summary>
        /// Find all words in a chapter and add to displayList
        /// </summary>
        public void FindWordWithChapter()
        {
            DisplayList.Clear();

            foreach (Word word in words)
            {
                if (word.Chapter == selectedChapter)
                {

                    DisplayList.Add(word);
                }

            }

        }
        
        /// <summary>
        /// test method to check contents of list to console.
        /// </summary>
        public void CheckList()
        {
            foreach (Word word in words)
            {
                System.Console.WriteLine(word.ThaiScript + " " + word.ThaiFonet + " " + word.EngWord + " " + word.EngDesc + " " + word.Chapter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowAllPropertiesFoundInConsole()
        {
            List<Word> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<Word>>(languageFilePath + "Thai_Word.xml");

            foreach(Word word in wordsFromFIle)
            {
                Console.Write(word);
                System.Console.WriteLine(word.ThaiScript + " " + word.ThaiFonet + " " + word.EngWord + " " + word.EngDesc + " " + word.Chapter);
            }

            
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
        /// 
        /// </summary>
        /// <param name="recived"></param>
        public void SetPropertyOfGenericObject(Object recived)
        {

            propertyListOfStrings.Clear();

            foundThaiScriptProperty = recived.GetType().GetProperty("ThaiScript");
            foundThaiFonetProperty = recived.GetType().GetProperty("ThaiFonet");
            foundThaiHelpWordProperty = recived.GetType().GetProperty("ThaiHelpWord");

            foundEngWordProperty = recived.GetType().GetProperty("EngWord");

            foundEngWordsProperty = recived.GetType().GetProperty("EngWords");

            foundEngDescProperty = recived.GetType().GetProperty("EngDesc");
            foundWordChapterProperty = recived.GetType().GetProperty("Chapter");

            var item = foundEngWordsProperty.GetValue(recived);

            if (null != foundThaiScriptProperty && foundThaiScriptProperty.CanWrite)
            {
                propertyScript = foundThaiScriptProperty.GetValue(recived);
            }

            if (null != foundThaiFonetProperty && foundThaiFonetProperty.CanWrite)
            {
                propertyFonet = foundThaiFonetProperty.GetValue(recived);
            }

            if (null != foundThaiHelpWordProperty && foundThaiHelpWordProperty.CanWrite)
            {
                propertyHelpWord = foundThaiHelpWordProperty.GetValue(recived);
            }
            if (null != item)
            {
                if ((item as List<String>).Count() == 0)
                {
                    if (null != foundEngWordProperty && foundEngWordProperty.CanWrite)
                    {
                        //propertyEngWord = foundEngWordProperty.GetValue(recived);
                        String stringtosplit = (String)foundEngWordProperty.GetValue(recived);
                        propertyListOfStrings = stringtosplit.Split(';', ',').ToList<String>();
                    }

                }
                else
                {
                    if (null != foundEngWordsProperty && foundEngWordsProperty.CanWrite)
                    {
                        if (null != item)
                        {                                                        
                                foreach (var listItem in item as List<String>)
                                {
                                    propertyListOfStrings.Add(listItem);
                                }                            
                        }
                    }
                }
            }

            if (null != foundEngDescProperty && foundEngDescProperty.CanWrite)
            {
                propertyEngDesc = foundEngDescProperty.GetValue(recived);
            }

            if (null != foundWordChapterProperty && foundWordChapterProperty.CanWrite)
            {
                propertyChapter = foundWordChapterProperty.GetValue(recived);
            }

            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Object SelectPropertyToValidate()
        {
            if (whatToTrain == "ThaiScript")
            {
                return propertyScript;
            }
            else if(whatToTrain == "ThaiHelpWord")
            {
                return propertyHelpWord;
            }
            else if (whatToTrain == "ThaiFonet")
            {
                return propertyFonet;
            }
            else if(whatToTrain == "EngWords")
            {
                return propertyListOfStrings;
            }
            else
            {
                MessageBox.Show("Please select what you want to practise.");
                return null;
            }
        }

        #region component interaction
        private void btn_validate_Page2_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conson_Page2.IsChecked == true)
            {
                ValidateAnswear<Consonant>(consonants, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);

            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                ValidateAnswear<Vowel>(vowels, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else if (rb_Closing_Page2.IsChecked == true)
            {
                //TODO
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                ValidateAnswear<ThaiNumber>(numbers, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2, ckb_Helpbox_Page2);
            }
            else
            {
                MessageBox.Show("Please select a category.");
            }

        }

        private void btn_Next_Word_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {

                TextChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {

                TextChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, 1);
            }
            else
            {
                MessageBox.Show("please select a category");
            }
            lbl_Counter_Page2.Content = i;

        }

        private void btn_Prev_Word_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {
                TextChanger<Consonant>(consonants, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                TextChanger<Vowel>(vowels, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(numbers, txb_ThaiScript_Page2, txb_Description_page2, ckb_Helpbox_Page2, ckb_Randomized_Page2, -1);
            }
            else
            {
                MessageBox.Show("please select one a category");
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
            if (rb_Conso_Page3.IsChecked == false && rb_Vowel_Page3.IsChecked == false && rb_words_Page3.IsChecked == false && rb_ThaiNumber_Page3.IsChecked == false)
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
            else if (rb_words_Page3.IsChecked == true)
            {
                SubmitNewWord<Word>(words);
            }
            else
            {
                SubmitNewWord<ThaiNumber>(numbers);
            }
            UpdateListBox();
        }

        private void btn_LoadList_Click(object sender, RoutedEventArgs e)
        {
            UpdateListBox();
        }

        private void lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (rb_words_Page3.IsChecked == true)
            {
                SelectionChanged<Word>(words);
            }
            else if (rb_Conso_Page3.IsChecked == true)
            {
                SelectionChanged<Consonant>(consonants);
            }
            else if (rb_Vowel_Page3.IsChecked == true)
            {
                SelectionChanged<Vowel>(vowels);
            }
            else if (rb_ThaiNumber_Page3.IsChecked == true)
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

            ClearFields();
            UpdateListBox();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";
        }

        private void rb_Conso_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Conso";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";
            lbl_Chapter_Insert.Content = "English Description";



        }

        private void rb_Vowel_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "Vowel";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void btn_Next_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            //selectedChapter = CheckPage1RB();

            ClearFields();
            
            TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 1);

            lbl_Counter_Page1.Content = i;
        }

        private void btn_Prev_Word_Page1_Click(object sender, RoutedEventArgs e)
        {

            TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, -1);
            lbl_Counter_Page1.Content = i;
        }

        private void btn_validate_Page1_Click(object sender, RoutedEventArgs e)
        {

            ValidateAnswear<Word>(DisplayList, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1, ckb_Helpbox_Page1);
        }

        private void rb_ThaiNumber_Page3_Checked(object sender, RoutedEventArgs e)
        {
            whatListTLoad = "ThaiNumber";
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";


        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = 0;
            selectedChapter = cb_Chapter_Page1.SelectedValue.ToString();

            FindWordWithChapter();

            lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList.Count.ToString();

            if (DisplayList.Count > 0)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, ckb_Helpbox_Page1, ckb_Randomized_Page1, 0);
            }
            else
            {
                MessageBox.Show("There are no words in that category yet");
            }


            lbl_Counter_Page1.Content = i;
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected word?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (rb_words_Page3.IsChecked == true)
                {
                    DeleteSelected<Word>(words);
                    SaveFiles<Word>(words);
                }
                else if (rb_Conson_Page2.IsChecked == true)
                {
                    DeleteSelected<Consonant>(consonants);
                    SaveFiles<Consonant>(consonants);
                }
                else if(rb_Vowel_Page3.IsChecked == true)
                {
                    DeleteSelected<Vowel>(vowels);
                    SaveFiles<Vowel>(vowels);
                }
                else if(rb_ThaiNumber_Page3.IsChecked == true)
                {
                    DeleteSelected<ThaiNumber>(numbers);
                    SaveFiles<ThaiNumber>(numbers);
                }
                else
                {
                    MessageBox.Show("Please select a list first");
                }
            }
            UpdateListBox();
            
        }

        private void rb_TrainScript_Checked(object sender, RoutedEventArgs e)
        {
            whatToTrain = "ThaiScript";
        }

        private void rb_TrainEngWords_Checked(object sender, RoutedEventArgs e)
        {
            whatToTrain = "EngWords";
        }

        private void rb_TrainFonet_Checked(object sender, RoutedEventArgs e)
        {
            whatToTrain = "ThaiFonet";
        }

        private void rb_TrainHelpWord_Checked(object sender, RoutedEventArgs e)
        {
            whatToTrain = "ThaiHelpWord";
        }













        #endregion


    }
}

