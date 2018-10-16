using NAudio.Wave;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ClearFields();

            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Counter_Page1.Content = CurrentFileIndex;
            txb_FilePath_Settings.Text = LanguageFilePath;
            txt_NewSavePath_Settings.Text = LanguageFilePath;

            LoadFiles<Chapter>(Chapters);

            LoadFiles<Word>(Words);

            LoadFiles<Consonant>(Consonants);

            LoadFiles<Vowel>(Vowels);

            LoadFiles<ThaiNumber>(Numbers);

            SetInitialStates();

            GetImage();
        }

        #region auto properties

        #region lists

        public static List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public static List<Consonant> Consonants { get; set; } = new List<Consonant>();
        public static List<Word> DisplayList { get; set; } = new List<Word>();
        public static List<ThaiNumber> Numbers { get; set; } = new List<ThaiNumber>();
        public static List<Vowel> Vowels { get; set; } = new List<Vowel>();
        public static Object WhatListTLoad { get; set; }
        public static List<Word> Words { get; set; } = new List<Word>();

        #endregion lists

        #region PropertyInfos

        /*
        public static PropertyInfo FoundEngDescProperty { get; set; }

        public static PropertyInfo FoundEngWordsProperty { get; set; }
        public static PropertyInfo FoundThaiFonetProperty { get; set; }
        public static PropertyInfo FoundThaiHelpWordProperty { get; set; }
        public static PropertyInfo FoundThaiScriptProperty { get; set; }
        public static PropertyInfo FoundWordChapterProperty { get; set; }*/
        public static List<TextBox> textboxList;
        public static List<PropertyInfo> ListOfProperties { get; set; } = new List<PropertyInfo>();
        /*public static object PropertyChapter { get; set; }
        public static object PropertyEngDesc { get; set; }
        public static object PropertyEngWord { get; set; }
        public static object PropertyFonet { get; set; }
        public static object PropertyHelpWord { get; set; }*/

        //public static object PropertyScript { get; set; }
        public static List<Object> ListOfValues { get; set; } = new List<Object>();

        public static List<string> PropertyListEngWords { get; set; } = new List<String>();

        #endregion PropertyInfos

        #region activeProperties
        
        public bool descriptionOn = true;
        public bool loopChapter = true;
        public bool randomOn;
        public static string RegexSplitString { get; set; } = @"^\s|[\s;,]{2,}";
        public String SubmitStyle { get; set; }
        public Type WhatTypeToUse { get; set; }
        public Object WordToLoad { get; set; }
        public static int CorrectPoints { get; set; } = 0;
        public static int CurrentFileIndex { get; set; } = 0;
        public static Random RandomIndex { get; set; } = new Random();
        public static string SelectedChapter { get; set; }
        public static object SelectedPropertyToDisplay { get; set; }
        public static object SelectedPropertyToValidate { get; set; }
        public static string SelectedSymbolTypeToUse { get; set; }
        public static string WhatToDisplay { get; set; }
        public static string WhatToTrain { get; set; }

        #endregion activeProperties

        public String chaptersName = "Key to understanding Thai; Thai alphabet; Closing sounds of consonants; Thai vowels; Tonal Language; Special pronounciation; Nouns, people and particles; Numbers and Counting; Telling time; Colors; Easy words; Homonyms; Homophones; Words in special contexts; 101 most used words; Small talk; The body";

        public Chapter NewChapter;

        #region Settings properties

        public static string LanguageFilePath { get; set; } = "C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/";
        public IEnumerable<Window> Windows { get; private set; }

        #endregion Settings properties

        private StackPanel sp = new StackPanel();
        private static ContentMan window;

        #endregion auto properties

        /// <summary>
        /// Turns the induvidual words into one string to display
        /// </summary>
        /// <param name="list">The list to use</param>
        /// <returns>String of words</returns>
        public String EngWordsToString(List<String> list)
        {
            String combinedStrings = null;
            foreach (String text in list)
            {
                if (list.Last() == text)
                {
                    combinedStrings += text;
                }
                else
                {
                    combinedStrings += text + "; ";
                }
            }
            return combinedStrings;
        }

        /// <summary>
        ///Sets the properties of the object it recives.
        /// </summary>
        /// <param name="recived">what object to find properties for</param>
        public void SetPropertyOfGenericObject(Object recived)
        {
            PropertyListEngWords.Clear();
            ListOfProperties.Clear();
            ListOfValues.Clear();
            if (recived != null)
            {
                foreach (PropertyInfo prop in recived.GetType().GetProperties().ToList<PropertyInfo>())
                {
                    if (prop != null)
                    {
                        ListOfProperties.Add(prop);

                        ListOfValues.Add(prop.GetValue(recived));
                    }
                }
            }
        }

        /// <summary>
        ///Checks and changes the current file index.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="list">List to use</param>
        /// <param name="nextValueToAdd">the next value to add (or subtract) from current file index</param>
        /// <param name="textBlockForScript">textblock to use for display</param>
        public void CheckCurrentFileSize<T>(List<T> list, int nextValueToAdd, TextBlock textBlockForScript)
        {
            if (randomOn)
            {
                CurrentFileIndex = RandomIndex.Next(0, list.Count);
            }
            else
            {
                if (loopChapter)
                {
                    if (nextValueToAdd > 0)
                    {
                        CurrentFileIndex++;
                        if (CurrentFileIndex > list.Count - 1)
                        {
                            CurrentFileIndex = 0;
                        }
                    }
                    else if (nextValueToAdd < 0)
                    {
                        CurrentFileIndex--;
                        if (CurrentFileIndex < 0)
                        {
                            CurrentFileIndex = list.Count - 1;
                        }
                    }
                    else
                    {
                        textBlockForScript.Text = (String)GetValueFromValueList("ThaiScript");
                    }
                }
                else
                {
                    if (nextValueToAdd > 0)
                    {
                        CurrentFileIndex++;
                        if (CurrentFileIndex > list.Count - 1)
                        {
                            cb_Chapter_Page1.SelectedIndex++;
                        }
                    }
                    else if (nextValueToAdd < 0)
                    {
                        CurrentFileIndex--;
                        if (CurrentFileIndex < 0)
                        {
                            cb_Chapter_Page1.SelectedIndex--;
                        }
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

            if (ckb_AutoClean.IsChecked == true)
            {
                txt_FirstSelectionProperty.Text = "";
                txt_SecondSelectionProperty.Text = "";
                txt_ThirdSelectionProperty.Text = "";
                txt_FourthSelectionProperty.Text = "";

                txt_FifthSelectionProperty.Text = "";
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
        /// Find all words in a chapter and add to displayList
        /// </summary>
        public void FindWordWithChapter()
        {
            DisplayList.Clear();

            foreach (Word word in Words)
            {
                if (word.Chapter == SelectedChapter)
                {
                    DisplayList.Add(word);
                }
            }
        }

        public List<TextBox> FormTextboxes()
        {
            List<TextBox> list = new List<TextBox>();
            foreach (var element in sp.Children)
            {
                if (element is TextBox textBox)
                {
                    list.Add((TextBox)element);
                }
            }
            return list;
        }

        public void GetImage()
        {
            Image img = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\tommy\source\repos\LearnThaiApplication\Icons\Speaker.png"))
            };

            StackPanel stackPnl = new StackPanel();
            img.Width = 32;
            stackPnl.Children.Add(img);

            btn_Speaker_Page1.Content = stackPnl;
        }

        public object GetValueFromValueList(string valueToGet)
        {
            object returnValue;

            foreach (PropertyInfo prop in ListOfProperties)
            {
                if (prop.Name == valueToGet)
                {
                    return returnValue = ListOfValues[ListOfProperties.IndexOf(prop)];
                }
            }
            return null;
        }

        public object GetValueFromValueList(string valueToGet, string valueToCompare)
        {
            object returnValue;

            foreach (PropertyInfo prop in ListOfProperties)
            {
                if (prop.Name == valueToGet)
                {
                    foreach (object obj in ListOfValues)
                    {
                    }

                    if ((string)ListOfValues[ListOfProperties.IndexOf(prop)] == valueToCompare)
                    {
                        return returnValue = ListOfValues[ListOfProperties.IndexOf(prop)];
                    }
                }
            }
            return null;
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
                if (whatIsT != typeof(Chapter))
                {
                    SetPropertyOfGenericObject(wordFoundInFile);

                    newWordToAdd.Add(wordFoundInFile);
                }
            }

            list.AddRange(newWordToAdd);
        }

        /// <summary>
        /// Loads the content from the list into the listbox
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        public void LoadObjectsToLib<T>(List<T> list)
        {
            foreach (T word in list)
            {
                SetPropertyOfGenericObject(word);

                if (((String)GetValueFromValueList("ThaiScript"))?.Length == 0)
                {
                    lib_LoadedWords.Items.Add(PropertyListEngWords[0]);
                }
                else
                {
                    lib_LoadedWords.ItemsSource = list;
                    break;
                }
            }
        }

        /// <summary>
        /// Moves the object in the selected list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        /// <param name="oldIndex">The old index of the selected word</param>
        /// <param name="newIndex">the new index of the selected word</param>
        public void MoveObjectInList<T>(List<T> list, int oldIndex, int newIndex)
        {
            T item = list[lib_LoadedWords.SelectedIndex];

            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        /// <summary>
        /// Changes to the next chapter in the list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        public void NextChapter<T>(List<T> list)
        {
            Type objectType = typeof(T);

            if (objectType == typeof(Word) && CurrentFileIndex >= list.Count)
            {
                cb_Chapter_Page1.SelectedIndex++;
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

            XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }

        /// <summary>
        /// Handles the selection changes in the listbox
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SelectionChanged<T>()
        {
            Type whatIsT = typeof(T);

            if (lib_LoadedWords.SelectedIndex != -1)
            {
                WordToLoad = lib_LoadedWords.SelectedItem;

                SetPropertyOfGenericObject(WordToLoad);

                
                
                
                
                   
                
                

                if (WordToLoad.GetType() == typeof(Word))
                {
                    txt_FirstSelectionProperty.Text = (String)GetValueFromValueList("ThaiScript");
                    txt_SecondSelectionProperty.Text = (String)GetValueFromValueList("ThaiFonet");
                    txt_ThirdSelectionProperty.Text = EngWordsToString((List<String>)GetValueFromValueList("EngWords"));
                    txt_FourthSelectionProperty.Text = (String)GetValueFromValueList("EngDesc");
                    txt_FifthSelectionProperty.Text = (String)GetValueFromValueList("Chapter");
                    txb_Description_Page4.Text = (String)GetValueFromValueList("EngDesc");
                }
                else if (WordToLoad.GetType() == typeof(Consonant) || WordToLoad.GetType() == typeof(Vowel) || WordToLoad.GetType() == typeof(ThaiNumber))
                {
                    txt_FirstSelectionProperty.Text = (String)GetValueFromValueList("ThaiScript");
                    txt_SecondSelectionProperty.Text = (String)GetValueFromValueList("ThaiFonet");
                    txt_ThirdSelectionProperty.Text = (String)GetValueFromValueList("ThaiHelpWord");
                    txt_FourthSelectionProperty.Text = EngWordsToString((List<String>)GetValueFromValueList("EngWords"));
                    txt_FifthSelectionProperty.Text = (String)GetValueFromValueList("EngDesc");
                    txb_Description_Page4.Text = (String)GetValueFromValueList("EngDesc");
                }
            }
        }

        /// <summary>
        /// Selects what to move and sends the object to be moved.
        /// </summary>
        /// <param name="newIndex">The new index for the selected word</param>
        public void SelectWhatToMove(int newIndex)
        {
            if (WhatTypeToUse == typeof(Consonant))
            {
                MoveObjectInList<Consonant>(Consonants, lib_LoadedWords.SelectedIndex, newIndex);
            }
            else if (WhatTypeToUse == typeof(Vowel))
            {
                MoveObjectInList<Vowel>(Vowels, lib_LoadedWords.SelectedIndex, newIndex);
            }
            else if (WhatTypeToUse == typeof(Word))
            {
                MoveObjectInList<Word>(Words, lib_LoadedWords.SelectedIndex, newIndex);
            }
            else if (WhatTypeToUse == typeof(ThaiNumber))
            {
                MoveObjectInList<ThaiNumber>(Numbers, lib_LoadedWords.SelectedIndex, newIndex);
            }
            else
            {
                MessageBox.Show("Select a list to load from");
                return;
            }

            UpdateListBox();
            lib_LoadedWords.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Sets the initial states for checkboxes and the combobox.
        /// </summary>
        public void SetInitialStates()
        {
            ckb_DescBox_Page1.IsChecked = true;
            ckb_DescBox_Page2.IsChecked = true;
            rb_SubmitNew.IsChecked = true;

            rb_TrainFonet_Page1.IsChecked = true;
            rb_Conson_Page2.IsChecked = true;
            cb_Chapter_Page1.SelectedIndex = 0;
        }

        /// <summary>
        /// sets the new values to the old words that already can be found in list
        /// </summary>
        /// <param name="oldWord">What word to change or add values</param>
        /// <param name="textboxList">The TextBoxes to get values from</param>
        public void SetNewValuesToOldWord(Object oldWord, List<TextBox> textboxList)
        {
            foreach (TextBox tb in textboxList)
            {
                foreach (PropertyInfo prop in ListOfProperties)
                {
                    if (prop.Name == "ThaiScript" && tb.Name == "txt_ThaiScript")
                    {
                        prop.SetValue(oldWord, tb.Text, null);
                    }

                    if (prop.Name == "ThaiFonet" && tb.Name == "txt_ThaiFonet")
                    {
                        prop.SetValue(oldWord, tb.Text, null);
                    }
                    // && whatIsT.GetType() == typeof(Consonant)) || whatIsT.GetType() == typeof(Vowel)
                    if (prop.Name == "Tone" && tb.Name == "txt_Tone")
                    {
                        prop.SetValue(oldWord, SplitStringToList(tb.Text), null);
                    }

                    if (prop.Name == "EngWords" && tb.Name == "txt_EngWords")
                    {
                        prop.SetValue(oldWord, SplitStringToList(tb.Text), null);
                    }

                    if (prop.Name == "Chapter" && tb.Name == "txt_Chapter")
                    {
                        try
                        {
                            prop.SetValue(oldWord, tb.Text, null);
                        }
                        catch (FormatException)
                        {
                        }
                        catch (OverflowException)
                        {
                        }
                    }
                    if (prop.Name == "EngDesc" && tb.Name == "txt_EngDesc")
                    {
                        prop.SetValue(oldWord, tb.Text, null);
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="textToSplit"></param>
        /// <returns></returns>
        public List<String> SplitStringToList(String textToSplit)
        {
            return Regex.Split(textToSplit, RegexSplitString).ToList<String>();
        }

        /// <summary>
        /// Adds new words to the list.
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        public void SubmitNewWord<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            object newWord = null;
            List<String> ListFromTextBox;

            //ListFromTextBox = Regex.Split(txt_ThirdSelectionProperty.Text, @"[\s;,]{2,}").ToList<String>();

            if (txt_FirstSelectionProperty.Text != "" && txt_SecondSelectionProperty.Text != "" && txt_ThirdSelectionProperty.Text != "" && txt_FourthSelectionProperty.Text != "")
            {
                if (whatIsT == typeof(Word))
                {
                    newWord = new Word();
                }
                else if (whatIsT == typeof(Consonant))
                {
                    newWord = new Consonant();
                }
                else if (whatIsT == typeof(Vowel))
                {
                    newWord = new Vowel();
                }
                else
                {
                    newWord = new ThaiNumber();
                }

                textboxList = FormTextboxes();
                foreach (TextBox txt in textboxList)
                {
                    if (txt.Name == "txt_Chapter")
                    {
                        ((Word)newWord).Chapter = txt.Text;
                    }
                    if (txt.Name == "txt_ThaiScript")
                    {
                        ((ThaiBase)newWord).ThaiScript = txt.Text;
                    }
                    if (txt.Name == "txt_ThaiFonet")
                    {
                        ((ThaiBase)newWord).ThaiFonet = txt.Text;
                    }
                    if (txt.Name == "txt_ThaiHelpWord")
                    {
                        ((ThaiSymbol)newWord).ThaiHelpWord = txt.Text;
                    }
                    if (txt.Name == "txt_EngWords")
                    {
                        ListFromTextBox = SplitStringToList(txt.Text);
                        ((ThaiBase)newWord).EngWords = ListFromTextBox;
                    }
                    if (txt.Name == "txt_EngDesc")
                    {
                        ((ThaiBase)newWord).EngDesc = txt.Text;
                    }
                    if (txt.Name == "txt_Tone")
                    {
                        ListFromTextBox = SplitStringToList(txt.Text);
                        ((ThaiBase)newWord).Tone = ListFromTextBox;
                    }
                    if (txt.Name == "txt_SoundPath")
                    {
                        ((ThaiBase)newWord).SoundPath = txt.Text;
                    }
                    Console.WriteLine(txt.Name);
                    Console.WriteLine(txt.Text);
                }

                list.Add((T)newWord);

                ClearFields();

                SaveFiles<T>(list);
            }
        }

        /// <summary>
        /// The first part of the process to update old words
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public void SubmitUpdatedWord<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            textboxList = FormTextboxes();

            foreach (T oldWord in list)
            {
                if (WordToLoad.Equals(oldWord))
                {
                    SetPropertyOfGenericObject(oldWord);

                    if (oldWord.GetType() == typeof(Word))
                    {
                        SetNewValuesToOldWord(oldWord, textboxList);
                        //SetNewValuesToOldWord(oldWord, whatIsT, txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                        break;
                    }
                    else
                    {
                        SetNewValuesToOldWord(oldWord, textboxList);
                        break;
                    }
                }
            }

            ClearFields();

            SaveFiles<T>(list);
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
        public void TextChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, int nextValueToAdd) where T : new()
        {
            Type whatIsT = typeof(T);

            CheckCurrentFileSize<T>(list, nextValueToAdd, textBlockForScript);

            SetPropertyOfGenericObject(list[CurrentFileIndex]);

            SelectedPropertyToDisplay = GetValueFromValueList(WhatToDisplay);

            if (list[CurrentFileIndex].GetType() == typeof(Word))
            {
                string retrivedChapter = (string)GetValueFromValueList("Chapter", SelectedChapter);

                if (retrivedChapter == SelectedChapter)
                {
                    if (SelectedPropertyToDisplay is List<String> propertyIsList)
                    {
                        textBlockForScript.Text = EngWordsToString(propertyIsList);
                    }
                    else
                    {
                        textBlockForScript.Text = (String)SelectedPropertyToDisplay;
                    }
                }
                else
                {
                    MessageBox.Show("There are no content with chapter" + SelectedChapter + " available right now.");
                    return;
                }
            }
            else
            {
                if (SelectedPropertyToDisplay is List<String> propertyIsList)
                {
                    textBlockForScript.Text = EngWordsToString(propertyIsList);
                }
                else
                {
                    textBlockForScript.Text = GetValueFromValueList("ThaiScript") + " " + GetValueFromValueList("ThaiHelpWord");
                }

                //
            }

            if (descriptionOn)
            {
                textBlockDescription.Text = GetValueFromValueList("ThaiFonet") + "\r\n" + EngWordsToString(PropertyListEngWords) + "\r\n" + GetValueFromValueList("EngDesc");
            }
        }

        /// <summary>
        /// Checks what list to load and loads it.
        /// </summary>
        public void UpdateListBox()
        {
            if (WhatTypeToUse == null)
            {
                MessageBox.Show("Select a list to load from");
                return;
            }

            lib_LoadedWords.ItemsSource = null;

            if (WhatTypeToUse == typeof(Word))
            {
                LoadObjectsToLib<Word>(Words);
            }
            else if (WhatTypeToUse == typeof(Consonant))
            {
                LoadObjectsToLib<Consonant>(Consonants);
            }
            else if (WhatTypeToUse == typeof(Vowel))
            {
                LoadObjectsToLib<Vowel>(Vowels);
            }
            else if (WhatTypeToUse == typeof(ThaiNumber))
            {
                LoadObjectsToLib<ThaiNumber>(Numbers);
            }
        }


        public void CreateFormWindow()
        {
            
                window = new ContentMan();
                sp = new StackPanel();

                SetPropertyOfGenericObject(lib_LoadedWords.SelectedItem);
                int i = 0;

                foreach (PropertyInfo prop in ListOfProperties)
                {
                    var bc = new BrushConverter();
                    Label lbl = new Label
                    {
                        Content = prop.Name,
                        Foreground = (Brush)bc.ConvertFrom("#FFE5E5E5")
                    };

                    sp.Children.Add(lbl);
                    TextBox txt = new TextBox();
                    object txtContent = ListOfValues[i];

                    if (txtContent is List<String>)
                    {
                        txt.Text = EngWordsToString(txtContent as List<String>);
                    }
                    else
                    {
                        txt.Text = (string)txtContent;
                    }

                    txt.Name = "txt_" + prop.Name;
                    txt.TextWrapping = TextWrapping.Wrap;
                    txt.AcceptsReturn = true;

                    i++;

                    sp.Children.Add(txt);
                }

                Button submitButton = new Button
                {
                    Content = "Submit"
                };
                submitButton.Click += Btn_SubmitNewWord_Click;
                sp.Children.Add(submitButton);
                window.Content = sp;

                window.Show();
            
            
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
        public void ValidateAnswer<T>(List<T> list, TextBox textboxAnswear, TextBlock textBlockStatus, TextBlock textBlockDesc)
        {
            SetPropertyOfGenericObject(list[CurrentFileIndex]);

            SelectedPropertyToValidate = GetValueFromValueList(WhatToTrain);

            bool somethingRight = false;

            int rightAnswears = 0;

            if (SelectedPropertyToValidate is List<String>)
            {
                List<String> answers = Regex.Split(textboxAnswear.Text, RegexSplitString).ToList<String>();

                foreach (String correctWord in SelectedPropertyToValidate as List<String>)
                {
                    foreach (String answer in answers)
                    {
                        if (String.Equals(correctWord, answer, StringComparison.OrdinalIgnoreCase))
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
                if (rightAnswears == (SelectedPropertyToValidate as List<String>)?.Count && rightAnswears > 1)
                {
                    textBlockStatus.Text = "You got it all correct!";
                }
            }
            else if (String.Equals(textboxAnswear.Text, (String)SelectedPropertyToValidate, StringComparison.OrdinalIgnoreCase))
            {
                textBlockStatus.Text = "Correct!";
                CorrectPoints++;
            }
            else
            {
                textBlockStatus.Text = "Wrong...";
            }

            if (descriptionOn)
            {
                textBlockDesc.Text = GetValueFromValueList("ThaiFonet") + "\r\n" + EngWordsToString(PropertyListEngWords) + "\r\n" + GetValueFromValueList("EngDesc");
            }

            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Points.Content = "Points: " + CorrectPoints;
            lbl_Points_Page2.Content = "Points: " + CorrectPoints;
        }

        #region component interaction

        public void SelectSymbolToUse(object sender, RoutedEventArgs e)
        {
            SelectedSymbolTypeToUse = (string)((RadioButton)sender).Content;

            UpdateWhenSymbolTypeSelected();
        }

        public void SelectWhatToTrain(object sender, RoutedEventArgs e)
        {
            var tabIndex = sender;
            tabIndex = ((FrameworkElement)sender).Parent;
            tabIndex = ((Grid)tabIndex).Parent;
            tabIndex = ((TabItem)tabIndex).Parent;
            tabIndex = ((TabControl)tabIndex).SelectedIndex;

            if ((string)((RadioButton)sender).Content == "Writhing Thai")
            {
                WhatToTrain = "ThaiScript";
                WhatToDisplay = "EngWords";
            }
            else if ((string)((RadioButton)sender).Content == "Pronounciations")
            {
                WhatToTrain = "ThaiFonet";
                WhatToDisplay = "ThaiScript";
            }
            else if ((string)((RadioButton)sender).Content == "English meanings")
            {
                WhatToTrain = "EngWords";
                WhatToDisplay = "ThaiScript";
            }
            else
            {
                MessageBox.Show("Please select what to train", "ERROR");
            }
            if ((int)tabIndex == 0)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, 0);
            }
            else if ((int)tabIndex != -1)
            {
                UpdateWhenSymbolTypeSelected();
            }
        }

        public void UpdateWhenSymbolTypeSelected()
        {
            if (SelectedSymbolTypeToUse == "Consonants")
            {
                TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, 0);
            }
            else if (SelectedSymbolTypeToUse == "Vowels")
            {
                TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, 0);
            }
            else if (SelectedSymbolTypeToUse == "Closing sounds")
            {
                //TextChanger<ClosingSound>(ClosingSounds, txb_ThaiScript_Page2, txb_Description_page2, 0);
            }
            else if (SelectedSymbolTypeToUse == "Numbers")
            {
                TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, 0);
            }
            else
            {
                MessageBox.Show("No symbole type selected", "ERROR");
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected word?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if (WhatTypeToUse == typeof(Word))
                {
                    DeleteSelected<Word>(Words);
                    SaveFiles<Word>(Words);
                }
                else if (WhatTypeToUse == typeof(Consonant))
                {
                    DeleteSelected<Consonant>(Consonants);
                    SaveFiles<Consonant>(Consonants);
                }
                else if (WhatTypeToUse == typeof(Vowel))
                {
                    DeleteSelected<Vowel>(Vowels);
                    SaveFiles<Vowel>(Vowels);
                }
                else if (WhatTypeToUse == typeof(ThaiNumber))
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

        private void Btn_FormWindow(object sender, RoutedEventArgs e)
        {
            CreateFormWindow();
            
        }

        private void Btn_ListMoveDown_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = lib_LoadedWords.SelectedIndex + 1;

            SelectWhatToMove(newIndex);
        }

        private void Btn_ListMoveUp_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = lib_LoadedWords.SelectedIndex - 1;

            SelectWhatToMove(newIndex);
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
                TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, 1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, 1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, 1);
            }
            else
            {
                MessageBox.Show("please select a category");
            }
            lbl_Counter_Page2.Content = CurrentFileIndex;
        }

        private void Btn_Next_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, 1);

            lbl_Counter_Page1.Content = CurrentFileIndex;
        }

        private void Btn_Prev_Word_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();

            if (rb_Conson_Page2.IsChecked == true)
            {
                TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, -1);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, -1);
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, -1);
            }
            else
            {
                MessageBox.Show("please select one a category");
            }
            lbl_Counter_Page2.Content = CurrentFileIndex;
        }

        private void Btn_Prev_Word_Page1_Click(object sender, RoutedEventArgs e)
        {
            TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, -1);
            lbl_Counter_Page1.Content = CurrentFileIndex;
        }

        private void Btn_Speaker_Page1_Click(object sender, RoutedEventArgs e)
        {
            var reader = new Mp3FileReader(@"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\mp3\P200149.mp3");

            var waveOut = new WaveOut(); // or WaveOutEvent()
            waveOut.Init(reader);
            waveOut.Play();
        }

        private void Btn_SubmitNewWord_Click(object sender, RoutedEventArgs e)
        {
            if (SubmitStyle == "Submit new")
            {
                if (WhatTypeToUse == typeof(Consonant))
                {
                    SubmitNewWord<Consonant>(Consonants);
                }
                else if (WhatTypeToUse == typeof(Vowel))
                {
                    SubmitNewWord<Vowel>(Vowels);
                }
                else if (WhatTypeToUse == typeof(Word))
                {
                    SubmitNewWord<Word>(Words);
                }
                else if (WhatTypeToUse == typeof(ThaiNumber))
                {
                    SubmitNewWord<ThaiNumber>(Numbers);
                }
                else
                {
                    MessageBox.Show("Select a list to load from");
                }
            }
            else if (SubmitStyle == "Update")
            {
                if (WhatTypeToUse == typeof(Consonant))
                {
                    SubmitUpdatedWord<Consonant>(Consonants);
                }
                else if (WhatTypeToUse == typeof(Vowel))
                {
                    SubmitUpdatedWord<Vowel>(Vowels);
                }
                else if (WhatTypeToUse == typeof(Word))
                {
                    SubmitUpdatedWord<Word>(Words);
                }
                else if (WhatTypeToUse == typeof(ThaiNumber))
                {
                    SubmitUpdatedWord<ThaiNumber>(Numbers);
                }
                else
                {
                    MessageBox.Show("Select a list to load from");
                }
            }
            else
            {
                MessageBox.Show("Select what you want to do");
            }

            UpdateListBox();
        }

        private void Btn_validate_Page1_Click(object sender, RoutedEventArgs e)
        {
            ValidateAnswer<Word>(DisplayList, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1);
        }

        private void Btn_validate_Page2_Click(object sender, RoutedEventArgs e)
        {
            if (rb_Conson_Page2.IsChecked == true)
            {
                ValidateAnswer<Consonant>(Consonants, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
            }
            else if (rb_Vowel_Page2.IsChecked == true)
            {
                ValidateAnswer<Vowel>(Vowels, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
            }
            else if (rb_Closing_Page2.IsChecked == true)
            {
                //TODO
            }
            else if (rb_NumberSymbol_Page2.IsChecked == true)
            {
                ValidateAnswer<ThaiNumber>(Numbers, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
            }
            else
            {
                MessageBox.Show("Please select a category.");
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;
            SelectedChapter = cb_Chapter_Page1.SelectedValue.ToString();

            FindWordWithChapter();

            lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList.Count.ToString();

            if (DisplayList.Count > 0)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, 0);
            }
            else
            {
                MessageBox.Show("There are no words in that category yet");
            }

            lbl_Counter_Page1.Content = CurrentFileIndex;
        }

        private void DescriptionBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.IsChecked == true)
            {
                txb_Description_page1.Text = GetValueFromValueList("ThaiFonet") + "\r\n" + EngWordsToString(PropertyListEngWords) + "\r\n" + GetValueFromValueList("EngDesc");
                txb_Description_page2.Text = GetValueFromValueList("ThaiFonet") + "\r\n" + EngWordsToString(PropertyListEngWords) + "\r\n" + GetValueFromValueList("EngDesc");
                descriptionOn = true;
            }
            else
            {
                txb_Description_page1.Text = "";
                txb_Description_page2.Text = "";
                descriptionOn = false;
            }
        }

        private void Lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (WhatTypeToUse == typeof(Word))
            {
                SelectionChanged<Word>();
            }
            else if (WhatTypeToUse == typeof(Consonant))
            {
                SelectionChanged<Consonant>();
            }
            else if (WhatTypeToUse == typeof(Vowel))
            {
                SelectionChanged<Vowel>();
            }
            else if (WhatTypeToUse == typeof(ThaiNumber))
            {
                SelectionChanged<ThaiNumber>();
            }
            else
            {
                MessageBox.Show("Please select a list to load", "List not choosen");
            }
        }

        private void LoopChapter_Checked(object sender, RoutedEventArgs e)
        {
            loopChapter = (sender as CheckBox)?.IsChecked == true;
        }

        private void Randomized_Checked(object sender, RoutedEventArgs e)
        {
            randomOn = (sender as CheckBox)?.IsChecked == true;
        }

        private void Rb_Conso_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Consonants;
            WhatTypeToUse = typeof(Consonant);
            ClearFields();
            UpdateListBox();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";
            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_Conson_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFileIndex = 0;

            TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, 0);
            lbl_Counter_Page2.Content = CurrentFileIndex;
            txb_Information_Page2.Text = "To property pronounce a Thai Consonant you add the sound from อ (o).";
        }

        private void Rb_NumberSymbol_Page2_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFileIndex = 0;

            TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, 0);
            lbl_Counter_Page2.Content = CurrentFileIndex;

            txb_Information_Page2.Text = "";
        }

        private void Rb_ThaiNumber_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Numbers;
            WhatTypeToUse = typeof(ThaiNumber);
            ClearFields();
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

       

        private void Rb_Vowel_Checked(object sender, RoutedEventArgs e)
        {
            CurrentFileIndex = 0;
            TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, 0);
            lbl_Counter_Page2.Content = CurrentFileIndex;

            txb_Information_Page2.Text = "When you are reading a vowel, you almost always pronounce the consonant first, then the surounding vowel.";
        }

        private void Rb_Vowel_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Vowels;
            WhatTypeToUse = typeof(Vowel);
            ClearFields();
            UpdateListBox();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_words_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Words;
            WhatTypeToUse = typeof(Word);

            ClearFields();
            UpdateListBox();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";
        }

        private void SubmitStyleChecked(object sender, RoutedEventArgs e)
        {
            SubmitStyle = (string)(sender as RadioButton)?.Content;
        }

        private void TabChanged(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }
    }

    #endregion component interaction
}