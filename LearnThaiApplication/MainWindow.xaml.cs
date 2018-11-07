﻿using HtmlAgilityPack;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
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

            //ClearFields();

            LoadAllFiles();

            SetInitialStates();

            GetImage();

            //WriteAllToFile();*/
        }

        #region Variables and properties

        #region lists

        private List<TextBox> textboxList;
        private List<Chapter> Chapters { get; set; } = new List<Chapter>();

        private List<Word> DisplayList { get; set; } = new List<Word>();
        private List<PropertyInfo> ListOfProperties { get; set; } = new List<PropertyInfo>();
        private List<object> ListOfValues { get; set; } = new List<object>();
        private List<Word> Words { get; set; } = new List<Word>();

        #endregion lists

        #region bools

        private bool DescriptionOn { get; set; } = true;
        private bool DisplayAllPropertiesInDescription { get; set; }
        private bool IsContinious { get; set; }
        private bool RandomOn { get; set; }
        private bool SubmitionIsNew { get; set; }
        private bool LoopChapter { get; set; } = true;

        #endregion bools

        #region strings

        private string RegexSplitString { get; set; } = @"^\s|[\s;,]{2,}";
        private string SelectedChapter { get; set; }
        private string LanguageFilePath { get; set; } = Environment.CurrentDirectory + @"\..\..\Files\Media\Language\";
        private string SoundFilePath { get; set; } = Environment.CurrentDirectory + @"\..\..\Files\Media\Sound\";
        private string WebFilePath { get; set; } = Environment.CurrentDirectory + @"\..\..\Files\Media\Website\";
        private string ImageFilePath { get; set; } = Environment.CurrentDirectory + @"\..\..\Files\Media\Icon\";
        private string SelectedSymbolTypeToUse { get; set; }
        private string WhatToDisplay { get; set; }
        private string WhatToTrain { get; set; }

        #endregion strings

        #region ints

        private static int CorrectPoints { get; set; } = 0;
        private static int CurrentFileIndex { get; set; } = 0;
        private static int CurrentListBoxIndex { get; set; } = -1;

        #endregion ints

        #region objects

        private object SelectedPropertyToDisplay { get; set; }
        private object SelectedPropertyToValidate { get; set; }
        private object WordToLoad { get; set; }
        private object WhatListTLoad { get; set; }

        #endregion objects

        #region others

        private Chapter NewChapter;
        private ContentMan window;
        private StackPanel sp = new StackPanel();
        private Type WhatTypeToUse { get; set; }
        private Random RandomIndex { get; set; } = new Random();
        private IEnumerable<Window> Windows { get; set; }

        #endregion others

        #endregion Variables and properties

        #region TestMethods

        /// <summary>
        /// Testmethod writing values from the lists to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void WriteWordToFile<T>(List<T> list) where T : new()
        {
            foreach (T word in list)
            {
                SetPropertyOfGenericObject(word);

                List<string> script = (List<string>)GetValueFromValueList("ThaiScript");
                string helpword = (string)GetValueFromValueList("ThaiHelpWord");

                if (script.Count == 1)
                {
                    SetValueOfObject(helpword, "ThaiScript", word);
                }
            }
        }

        /// <summary>
        /// Cycles trough all lists of words and then displays the result in a messagebox
        /// </summary>
        /// <param name="sender">The object that initiated the method</param>
        /// <param name="e"></param>
        private void CheckAllSoundStatuses(object sender, RoutedEventArgs e)
        {
            string fullText = CheckSoundStatus<Word>(Words);

            MessageBox.Show(fullText);
        }

        #endregion TestMethods

        #region Main

        /// <summary>
        ///Checks and changes the current file index.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="list">List to use</param>
        /// <param name="nextValueToAdd">the next value to add (or subtract) from current file index</param>
        /// <param name="textBlockForScript">textblock to use for display</param>
        private void CheckCurrentFileSize<T>(List<T> list, int nextValueToAdd, TextBlock textBlockForScript)
        {
            if (RandomOn)
            {
                CurrentFileIndex = RandomIndex.Next(0, list.Count);
            }
            else
            {
                if (LoopChapter)
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
                        textBlockForScript.Text = ListToString((List<string>)GetValueFromValueList("ThaiScript"));
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
                            if (cb_Chapter_Page1.SelectedIndex == 0)
                            {
                                cb_Chapter_Page1.SelectedIndex = cb_Chapter_Page1.Items.Count - 1;
                            }
                            else
                            {
                                cb_Chapter_Page1.SelectedIndex--;
                                CurrentFileIndex = list.Count - 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Find all words in a chapter and add to displayList
        /// </summary>
        private void FindWordWithChapter()
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

        /// <summary>
        /// Gets the image for the speaker and attaches it to the speaker buttons.
        /// </summary>
        private void GetImage()
        {
            Image img1 = new Image
            {
                Source = new BitmapImage(new Uri(ImageFilePath + @"\Speaker.png"))
            };
            Image img2 = new Image
            {
                Source = new BitmapImage(new Uri(ImageFilePath + @"\Speaker.png"))
            };

            StackPanel stackPnl1 = new StackPanel();
            StackPanel stackPnl2 = new StackPanel();

            img1.Width = 32;
            img2.Width = 32;

            stackPnl1.Children.Add(img1);
            stackPnl2.Children.Add(img2);

            btn_Speaker_Page1.Content = stackPnl1;
            btn_Speaker_Page2.Content = stackPnl2;
        }

        /// <summary>
        /// Gets the value from the property with a name equal to valueToGet
        /// </summary>
        /// <param name="valueToGet">The Name of the value to get</param>
        /// <returns>Returns the value from the current object</returns>
        private object GetValueFromValueList(string valueToGet)
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

        /// <summary>
        /// Gets the value from the vlaue list, but only if it the valueToCompare Eqists.
        /// </summary>
        /// <param name="valueToGet">The name of the value to get</param>
        /// <param name="valueToCompare">the value that must be true for the value to get returned</param>
        /// <returns>The requested property value</returns>
        private object GetValueFromValueList(string valueToGet, string valueToCompare)
        {
            object returnValue;

            foreach (PropertyInfo prop in ListOfProperties)
            {
                if (prop.Name == valueToGet && (string)ListOfValues[ListOfProperties.IndexOf(prop)] == valueToCompare)
                {
                    return returnValue = ListOfValues[ListOfProperties.IndexOf(prop)];
                }
            }
            return null;
        }

        /// <summary>
        /// Turns the induvidual words into one string to display
        /// </summary>
        /// <param name="list">The list to use</param>
        /// <returns>String of words</returns>
        private string ListToString(List<string> list)
        {
            string combinedStrings = null;
            foreach (string text in list)
            {
                if (list.Last().Equals(text))
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
        /// Changes to the next chapter in the list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        private void NextChapter<T>(List<T> list)
        {
            Type objectType = typeof(T);

            if (objectType == typeof(Word) && CurrentFileIndex >= list.Count)
            {
                cb_Chapter_Page1.SelectedIndex++;
            }
        }

        /// <summary>
        /// Goes to the next word in the current list.
        /// </summary>
        /// <param name="sender">object that sendt it</param>
        /// <param name="e"></param>
        private void NextWord(object sender, RoutedEventArgs e)
        {
            ClearFields();
            PreTextChanger(1);
        }

        /// <summary>
        /// fills the description textbox with the selected information.
        /// </summary>
        /// <param name="textBlockDescription">What textblock to use</param>
        private void PopulateDescription(TextBlock textBlockDescription)
        {
            if (DescriptionOn)
            {
                textBlockDescription.Text = "";
                if (DisplayAllPropertiesInDescription)
                {
                    foreach (var value in ListOfValues)
                    {
                        if (value is List<string> x)
                        {
                            textBlockDescription.Text += ListToString(x) + "\r\n";
                        }
                        else
                        {
                            textBlockDescription.Text += value + "\r\n";
                        }
                    }
                }
                else
                {
                    string descriptionText = ListToString((List<string>)GetValueFromValueList("ThaiScript")) + "\r\n";
                    descriptionText += ListToString((List<string>)GetValueFromValueList("ThaiFonet")) + "\r\n";
                    descriptionText += ListToString((List<string>)GetValueFromValueList("EngWords")) + "\r\n";
                    descriptionText += (string)GetValueFromValueList("EngDesc");

                    textBlockDescription.Text = descriptionText;
                }

                //textBlockDescription.Text = GetValueFromValueList("ThaiFonet") + "\r\n" + EngWordsToString(GetValueFromValueList("EngWords")) + "\r\n" + GetValueFromValueList("EngDesc");
            }
        }

        /// <summary>
        /// Checks what tab the user is on and updates the data on the current tab.
        /// </summary>
        /// <param name="change">how many places the index is going to change</param>
        private void PreTextChanger(int change)
        {
            TabIndex = MainWindow_tabController.SelectedIndex;

            if (TabIndex == 0)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, change);
                lbl_Counter_Page1.Content = CurrentFileIndex;
                SpeakerStatus();
            }
            if (TabIndex == 1)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page2, txb_Description_page2, change);
                lbl_Counter_Page2.Content = CurrentFileIndex;
                SpeakerStatus();
            }
        }

        /// <summary>
        /// goes to the previous word in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrevWord(object sender, RoutedEventArgs e)
        {
            ClearFields();
            PreTextChanger(-1);
        }

        /// <summary>
        /// Handles the selection changes in the listbox
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        private void SelectionChanged<T>(int selectedIndex)
        {
            Type whatIsT = typeof(T);

            if (selectedIndex != -1)
            {
                WordToLoad = lib_LoadedWords.SelectedItem;

                SetPropertyOfGenericObject(WordToLoad);

                FillFormTextBoxes();

                txt_FirstSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiScript"));
                txt_SecondSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiFonet"));
                txt_ThirdSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("EngWords"));
                txt_FourthSelectionProperty.Text = (String)GetValueFromValueList("EngDesc");
                txt_FifthSelectionProperty.Text = (String)GetValueFromValueList("Chapter");
                txb_Description_Page4.Text = (String)GetValueFromValueList("EngDesc");
            }
        }

        /// <summary>
        /// Selects what property the user is going to practice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectWhatToPractice(object sender, RoutedEventArgs e)
        {
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
            PreTextChanger(0);
        }

        /// <summary>
        ///Sets the properties of the object it recives.
        /// </summary>
        /// <param name="recived">what object to find properties for</param>
        private void SetPropertyOfGenericObject(Object recived)
        {
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
        /// Sets the selected value of the selected object to the new value.
        /// </summary>
        /// <param name="newValue">The new value for the object</param>
        /// <param name="nameOfPropertyToChange">The name of the property to change</param>
        /// <param name="objectToChange">What object to change</param>
        private void SetValueOfObject(object newValue, string nameOfPropertyToChange, object objectToChange)
        {
            foreach (PropertyInfo prop in ListOfProperties)
            {
                if (prop.Name == nameOfPropertyToChange)
                {
                    if (prop.PropertyType == typeof(List<string>))
                    {
                        prop.SetValue(objectToChange, SplitStringToList((string)newValue), null);
                    }
                    else
                    {
                        prop.SetValue(objectToChange, newValue, null);
                    }
                    break;
                }
            }
        }

        /// <summary>
        /// Turns a string into a list of strings.
        /// </summary>
        /// <param name="textToSplit">String to split</param>
        /// <returns></returns>
        private List<String> SplitStringToList(String textToSplit)
        {
            return Regex.Split(textToSplit, RegexSplitString).ToList<String>();
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
        private void TextChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, int nextValueToAdd) where T : new()
        {
            try
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
                            textBlockForScript.Text = ListToString(propertyIsList);
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
                PopulateDescription(textBlockDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Starts Validation of the user input agains the data of the current object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateAnswear(object sender, RoutedEventArgs e)
        {
            TabIndex = MainWindow_tabController.SelectedIndex;

            if (TabIndex == 0)
            {
                ValidateAnswer<Word>(DisplayList, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1);
            }
            else if (TabIndex == 1)
            {
                ValidateAnswer<Word>(DisplayList, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
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
        private void ValidateAnswer<T>(List<T> list, TextBox textboxAnswear, TextBlock textBlockStatus, TextBlock textBlockDesc)
        {
            SetPropertyOfGenericObject(list[CurrentFileIndex]);

            SelectedPropertyToValidate = GetValueFromValueList(WhatToTrain);

            int rightAnswears = 0;
            int totalAnswears = 0;
            List<string> answers = Regex.Split(textboxAnswear.Text, RegexSplitString).ToList<string>();

            if (SelectedPropertyToValidate is List<String>)
            {
                foreach (String correctWord in SelectedPropertyToValidate as List<string>)
                {
                    totalAnswears = ((List<string>)SelectedPropertyToValidate).Count;
                    foreach (String answer in answers)
                    {
                        if (String.Equals(correctWord, answer, StringComparison.OrdinalIgnoreCase))
                        {
                            CorrectPoints++;
                            rightAnswears++;
                        }
                    }
                }
            }
            else if (String.Equals(textboxAnswear.Text, (String)SelectedPropertyToValidate, StringComparison.OrdinalIgnoreCase))
            {
                totalAnswears = 1;
                CorrectPoints++;
                rightAnswears++;
            }

            if (rightAnswears != 0)
            {
                textBlockStatus.Text = "You got " + rightAnswears + " of " + totalAnswears + " correct!";
            }
            else
            {
                textBlockStatus.Text = "Sorry, try again!";
            }

            PopulateDescription(textBlockDesc);

            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Points.Content = "Points: " + CorrectPoints;
            lbl_Points_Page2.Content = "Points: " + CorrectPoints;
            txt_Answear_Page1.Text = "";
            txt_Answear_Page2.Text = "";
        }

        /// <summary>
        /// Handles the combobox selection and updates the displayed data when a new chapter is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;
            SelectedChapter = (string)((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;

            FindWordWithChapter();

            if (TabIndex == 0)
            {
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
            else if (TabIndex == 1)
            {
                lbl_ChapterCount_Page2.Content = "Words in chapter: " + DisplayList.Count.ToString();

                if (DisplayList.Count > 0)
                {
                    TextChanger<Word>(DisplayList, txb_ThaiScript_Page2, txb_Description_page2, 0);
                }
                else
                {
                    MessageBox.Show("There are no words in that category yet");
                }

                lbl_Counter_Page2.Content = CurrentFileIndex;
            }
            else
            {
            }
        }

        #endregion Main

        #region Settings

        /// <summary>
        /// Clears the textboxs and textblocks on the current tab
        /// </summary>
        private void ClearFields()
        {
            TabIndex = MainWindow_tabController.SelectedIndex; //SelectParentIndex(sender);

            if (TabIndex == 0)
            {
                txb_Description_page1.Text = "";
                txb_ThaiScript_Page1.Text = "";
                txb_Status_Page1.Text = "";
                txt_Answear_Page1.Text = "";
            }
            else if (TabIndex == 1)
            {
                txb_ThaiScript_Page2.Text = "";
                txb_Status_Page2.Text = "";
                txb_Description_page2.Text = "";
                txt_Answear_Page2.Text = "";
            }
            else if (TabIndex == 2)
            {
                if (ckb_AutoClean.IsChecked == true)
                {
                    txt_FirstSelectionProperty.Text = "";
                    txt_SecondSelectionProperty.Text = "";
                    txt_ThirdSelectionProperty.Text = "";
                    txt_FourthSelectionProperty.Text = "";
                    txt_FifthSelectionProperty.Text = "";

                    if (textboxList.Count != 0)
                    {
                        foreach (TextBox txt in textboxList)
                        {
                            txt.Text = "";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Loads all the files from storage
        /// </summary>
        private void LoadAllFiles()
        {
            LoadFiles<Chapter>(Chapters);

            LoadFiles<Word>(Words);
        }

        /// <summary>
        /// Load files to lists.
        /// </summary>
        /// <typeparam name="T">What type to load</typeparam>
        /// <param name="list">What list to load into</param>
        private void LoadFiles<T>(List<T> list) where T : new()
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
        /// Saves the data to files.
        /// </summary>
        /// <returns></returns>
        private bool SaveAll()
        {
            try
            {
                SaveFiles<Word>(Words);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Saves the content of list to file
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        private void SaveFiles<T>(List<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }

        /// <summary>
        /// Sets the initial states for diffrent components.
        /// </summary>
        private void SetInitialStates()
        {
            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Counter_Page1.Content = CurrentFileIndex;
            txb_FilePath_Settings.Text = LanguageFilePath;
            txt_NewSavePath_Settings.Text = LanguageFilePath;

            ckb_DescBox_Page1.IsChecked = true;
            ckb_DescBox_Page2.IsChecked = true;
            rb_SubmitNew.IsChecked = true;

            rb_TrainFonet_Page1.IsChecked = true;

            cb_Chapter_Page1.SelectedIndex = 0;

            lib_LoadedWords.DisplayMemberPath = "Thaiscript";
        }

        /// <summary>
        /// Checks if the descriptionCheckbox is checked and then populate descripton
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DescriptionBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.IsChecked == true)
            {
                DescriptionOn = true;
                PopulateDescription(txb_Description_page1);
                PopulateDescription(txb_Description_page2);
            }
            else
            {
                txb_Description_page1.Text = "";
                txb_Description_page2.Text = "";
                DescriptionOn = false;
            }
        }

        /// <summary>
        /// Checks if the full debug description is true and then displays all proeprties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullDesc_Checked(object sender, RoutedEventArgs e)
        {
            DisplayAllPropertiesInDescription = ((CheckBox)sender).IsChecked == true;
            DescriptionBox_Checked(sender, e);
        }

        /// <summary>
        /// Sets loopChapter to true or false and allows the user to continiue to the next chaper automaticaly at the end of the last chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopChapter_Checked(object sender, RoutedEventArgs e)
        {
            LoopChapter = (sender as CheckBox)?.IsChecked == true;
        }

        /// <summary>
        /// checks if the enterkey is released on textboxes to validate or submit new word.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (TabIndex == 0 || TabIndex == 1)
                {
                    ValidateAnswear(sender, e);
                }
                else if (TabIndex == 2)
                {
                    SubmitNewWordQuick(sender, e);
                }
            }
            else if (e.Key == System.Windows.Input.Key.Right)
            {
                ClearFields();
                PreTextChanger(1);
            }
            else if (e.Key == System.Windows.Input.Key.Left)
            {
                ClearFields();
                PreTextChanger(-1);
            }
        }

        /// <summary>
        /// Check if random number is on, if on randomly jump around in the chapter. also disables loop chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Randomized_Checked(object sender, RoutedEventArgs e)
        {
            RandomOn = (sender as CheckBox)?.IsChecked == true;
            ckb_LoopChapter.IsChecked = false;
        }

        /// <summary>
        /// When the user changes tabs it sets the file index to 0 and updates the display data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;
            TabIndex = MainWindow_tabController.SelectedIndex;
            PreTextChanger(0);
        }

        #endregion Settings

        #region Submit

        /// <summary>
        /// Creates a submit form window
        /// </summary>
        /// <param name="continious">Checks if the form should use the continious methods</param>
        public void CreateFormWindow(bool continious)
        {
            window = new ContentMan();
            Viewbox wb = new Viewbox
            {
                Width = window.Width
            };
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
                    txt.Text = ListToString(txtContent as List<String>);
                }
                else
                {
                    txt.Text = (string)txtContent;
                }

                txt.Name = "txt_" + prop.Name;
                txt.TextWrapping = TextWrapping.Wrap;

                txt.Width = 280;
                txt.KeyUp += OnEnterKeyUpForm;

                i++;

                sp.Children.Add(txt);
            }

            Button submitButton = new Button
            {
                Content = "Submit",
                Name = "FormWindowButton"
            };
            if (continious)
            {
                submitButton.Click += SubmitAndContinue;
            }
            else
            {
                submitButton.Click += SubmitNewWordFull;
            }

            sp.Children.Add(submitButton);
            wb.Child = sp;
            window.Content = wb;

            window.Show();
        }

        /// <summary>
        /// Creates a continious submit update form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CycleListboxItems(object sender, RoutedEventArgs e)
        {
            IsContinious = true;
            SubmitStyleChanger(false);
            CreateFormWindow(IsContinious);
        }

        private void SubmitStyleChanger(bool isNew)
        {
            if (isNew)
            {
                rb_SubmitNew.IsChecked = true;
            }
            else
            {
                rb_UpdateWord.IsChecked = true;
            }
        }

        /// <summary>
        /// Deletes the selected word from
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to delete from</param>
        private void DeleteSelected<T>(List<T> list)
        {
            MessageBox.Show("tried to remove element " + list[lib_LoadedWords.SelectedIndex].ToString());
            list.RemoveAt(lib_LoadedWords.SelectedIndex);
        }

        /// <summary>
        /// Fills the textboxes of the full submit form.
        /// </summary>
        public void FillFormTextBoxes()
        {
            textboxList = FormTextboxes();

            foreach (TextBox txt in textboxList)
            {
                foreach (PropertyInfo prop in ListOfProperties)
                {
                    if (txt.Name == "txt_" + prop.Name)
                    {
                        if (prop.GetValue(WordToLoad) is List<string> x)
                        {
                            txt.Text = ListToString(x);
                        }
                        else
                        {
                            txt.Text = (String)prop.GetValue(WordToLoad);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the textboxes from the full form and collects them in a list
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Submits a new word with the values from the full form.
        /// </summary>
        /// <param name="newWord"></param>
        public void FullSubmitNewWord(object newWord)
        {
            textboxList = FormTextboxes();

            foreach (TextBox txt in textboxList)
            {
                foreach (PropertyInfo prop in ListOfProperties)
                {
                    if (txt.Name == "txt_" + prop.Name)
                    {
                        SetValueOfObject(txt.Text, prop.Name, newWord);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Selects whether to submit a new or update a old word
        /// </summary>
        /// <param name="isQuick">checks if to submit from the full form of the quick form</param>
        public void HowToSubmit(bool isQuick)
        {
            if (SubmitionIsNew)
            {
                SubmitNewWord<Word>(Words, isQuick);
            }
            else if (!SubmitionIsNew)
            {
                SubmitUpdatedWord<Word>(Words, isQuick);
            }
            else
            {
                MessageBox.Show("Select what you want to do");
            }

            UpdateListBox();
            ClearFields();
        }

        /// <summary>
        /// Loads the content from the list into the listbox
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        private void LoadObjectsToLib<T>(List<T> list)
        {
            foreach (T word in list)
            {
                SetPropertyOfGenericObject(word);

                lib_LoadedWords.ItemsSource = list;

                lib_LoadedWords.DisplayMemberPath = "Name";

                break;
            }
        }

        /// <summary>
        /// Moves the object in the selected list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        /// <param name="oldIndex">The old index of the selected word</param>
        /// <param name="newIndex">the new index of the selected word</param>
        private void MoveObjectInList<T>(List<T> list, int oldIndex, int newIndex)
        {
            T item = list[lib_LoadedWords.SelectedIndex];

            list.RemoveAt(oldIndex);
            list.Insert(newIndex, item);
        }

        /// <summary>
        /// Checks for enter key up from the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEnterKeyUpForm(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (IsContinious)
                {
                    SubmitAndContinue(sender, e);
                }
                else
                {
                    SubmitNewWordFull(sender, e);
                }
            }
        }

        /// <summary>
        /// Sets the values of the new word from the quickForm
        /// </summary>
        /// <param name="newWord"></param>
        private void QuickSubmit(object newWord)
        {
            ((ThaiBase)newWord).ThaiScript = SplitStringToList(txt_FirstSelectionProperty.Text);
            ((ThaiBase)newWord).ThaiFonet = SplitStringToList(txt_SecondSelectionProperty.Text);
            ((ThaiBase)newWord).EngWords = SplitStringToList(txt_ThirdSelectionProperty.Text);
            ((ThaiBase)newWord).EngDesc = txt_FourthSelectionProperty.Text;
            ((ThaiBase)newWord).Tone = SplitStringToList(txt_FifthSelectionProperty.Text);
        }

        /// <summary>
        /// Selects what to move and sends the object to be moved.
        /// </summary>
        /// <param name="newIndex">The new index for the selected word</param>
        private void SelectWhatToMove(int newIndex)
        {
            MoveObjectInList<Word>(Words, lib_LoadedWords.SelectedIndex, newIndex);

            UpdateListBox();
            lib_LoadedWords.SelectedIndex = newIndex;
        }

        /// <summary>
        /// sets the new values to the old words that already can be found in list
        /// </summary>
        /// <param name="oldWord">What word to change or add values</param>
        /// <param name="textboxList">The TextBoxes to get values from</param>
        public void SetNewValuesFromForm(Object oldWord, List<TextBox> textboxList)
        {
            foreach (TextBox tb in textboxList)
            {
                foreach (PropertyInfo prop in ListOfProperties)
                {
                    if (tb.Name == "txt_" + prop.Name)
                    {
                        if (prop.GetValue(oldWord) is List<string> x)
                        {
                            prop.SetValue(oldWord, SplitStringToList(tb.Text), null);
                        }
                        else
                        {
                            prop.SetValue(oldWord, tb.Text, null);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Submits the values from the full form.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool SubmitFromForm<T>(List<T> list) where T : new()
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
                        SetNewValuesFromForm(oldWord, textboxList);
                        //SetNewValuesToOldWord(oldWord, whatIsT, txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                        return true;
                    }
                    else
                    {
                        SetNewValuesFromForm(oldWord, textboxList);
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Adds new words to the list.
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        private void SubmitNewWord<T>(List<T> list, bool isQuick) where T : new()
        {
            Type whatIsT = typeof(T);

            object newWord = null;

            newWord = new Word();

            SetPropertyOfGenericObject(newWord);

            if (isQuick)
            {
                QuickSubmit(newWord);
            }
            else
            {
                FullSubmitNewWord(newWord);
            }

            list.Add((T)newWord);

            SaveFiles<T>(list);
        }

        /// <summary>
        /// Starts a quick submit of a new word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitNewWordQuick(object sender, RoutedEventArgs e)
        {
            HowToSubmit(true);
        }

        /// <summary>
        /// The first part of the process to update old words
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void SubmitUpdatedWord<T>(List<T> list, bool isQuick) where T : new()
        {
            Type whatIsT = typeof(T);

            if (!isQuick)
            {
                textboxList = FormTextboxes();
            }

            foreach (T oldWord in list)
            {
                if (WordToLoad.Equals(oldWord))
                {
                    SetPropertyOfGenericObject(oldWord);
                    if (isQuick)
                    {
                        QuickSubmit(oldWord);
                    }
                    else
                    {
                        if (oldWord.GetType() == typeof(Word))
                        {
                            SetNewValuesFromForm(oldWord, textboxList);
                        }
                        else
                        {
                            SetNewValuesFromForm(oldWord, textboxList);
                        }
                    }
                    break;
                }
            }

            SaveFiles<T>(list);
        }

        /// <summary>
        /// Checks what list to load and loads it.
        /// </summary>
        private void UpdateListBox()
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
        }

        /// <summary>
        /// Select a object to delete and then save the remaining objects to file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the selected word?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeleteSelected<Word>(Words);

                SaveAll();
            }
            UpdateListBox();
        }

        /// <summary>
        /// Creates a non-continious form window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_FormWindow(object sender, RoutedEventArgs e)
        {
            IsContinious = false;
            CreateFormWindow(IsContinious);
        }

        /// <summary>
        /// Moves a object up or down the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ListMoveDown_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = lib_LoadedWords.SelectedIndex + 1;

            SelectWhatToMove(newIndex);
        }

        /// <summary>
        /// Moves a object up or down the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_ListMoveUp_Click(object sender, RoutedEventArgs e)
        {
            int newIndex = lib_LoadedWords.SelectedIndex - 1;

            SelectWhatToMove(newIndex);
        }

        /// <summary>
        /// Changes the textboxes for the content management to the selected object from the listbox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Lib_LoadedWords_SelectionChanged(Object sender, SelectionChangedEventArgs e)
        {
            if (lib_LoadedWords.SelectedIndex != -1)
            {
                CurrentListBoxIndex = lib_LoadedWords.SelectedIndex;
            }

            if (WhatTypeToUse == typeof(Word))
            {
                SelectionChanged<Word>(lib_LoadedWords.SelectedIndex);
            }
        }

        /// <summary>
        /// Selects what conent to manage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WhatToManage(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Words;
            WhatTypeToUse = typeof(Word);

            ClearFields();
            UpdateListBox();
        }

        /// <summary>
        /// Submits a full word and then continiues to the next word, updating the full form textboxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitAndContinue(object sender, RoutedEventArgs e)
        {
            HowToSubmit(false);

            if (lib_LoadedWords.SelectedIndex != -1 && lib_LoadedWords.Items.Count != 0)
            {
                CurrentListBoxIndex = lib_LoadedWords.SelectedIndex;
            }
            CurrentListBoxIndex++;
            lib_LoadedWords.SelectedIndex = CurrentListBoxIndex;

            SetPropertyOfGenericObject(lib_LoadedWords.SelectedItem);

            FillFormTextBoxes();

            UpdateListBox();
        }

        /// <summary>
        /// Submits all of the properties from the full form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitNewWordFull(object sender, RoutedEventArgs e)
        {
            HowToSubmit(false);
        }

        /// <summary>
        /// Checks if supposed to submit a new or update a old word.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitStyleChecked(object sender, RoutedEventArgs e)
        {
            if ((string)(sender as RadioButton)?.Content == "Submit new")
            {
                SubmitionIsNew = true;
            }
            else if ((string)(sender as RadioButton)?.Content == "Update")
            {
                SubmitionIsNew = false;
            }
            else
            {
                MessageBox.Show("Error: No submit style selected");
            }
        }

        #endregion Submit

        #region Sound

        /// <summary>
        /// Checks the childnodes and returns the properties from the html.
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="isScript"></param>
        /// <returns></returns>
        private string CheckChildNode(HtmlNode tableData, bool isScript)
        {
            string attributeValue = "";

            foreach (HtmlNode tableDataChild in tableData.ChildNodes)
            {
                if (isScript)
                {
                    if (tableDataChild.InnerText.Length == 1 && (tableDataChild.InnerText.Contains("(") || tableDataChild.InnerText.Contains(")")))
                    {
                        return null;
                    }
                    else
                    {
                        return tableDataChild.InnerText;
                    }
                }
                else
                {
                    if (tableDataChild.Attributes.Count != 0)
                    {
                        attributeValue = tableDataChild.Attributes[0].Value;
                    }

                    if (attributeValue.Contains("mp3"))
                    {
                        return attributeValue;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Cycles the list to find all words that have and dont have sounds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CheckSoundStatus<T>(List<T> list)
        {
            int hasSound = 0;
            int dosntHaveSound = 0;
            string fullText = "";

            foreach (T word in list)
            {
                SetPropertyOfGenericObject(word);
                List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");
                foreach (string x in soundPaths)
                {
                    if (string.IsNullOrEmpty(x))
                    {
                        dosntHaveSound++;
                    }
                    else
                    {
                        if (File.Exists(x))
                        {
                            hasSound++;
                        }
                        else
                        {
                            dosntHaveSound++;
                            SetValueOfObject("", "SoundPath", word);
                        }
                    }
                }
                if (soundPaths.Count == 0)
                {
                    dosntHaveSound++;
                }
            }

            fullText += hasSound + " " + typeof(T).Name + " has sound, and " + dosntHaveSound + " dont. \r\n";
            return fullText;
        }

        /// <summary>
        /// Gets the html file from local storage and starts to find the content to download sounds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="url"></param>
        private void DownloadSoundProcess<T>(List<T> list, string url) where T : new()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            doc.Load(url, Encoding.UTF8);

            List<string> correctText = new List<string>();
            List<string> soundPathList = new List<string>();

            int tableIndexMax = doc.DocumentNode.SelectNodes("//table/tbody").Count;

            for (int tableIndex = 0; tableIndex < tableIndexMax; tableIndex++)
            {
                soundPathList.Clear();
                correctText.Clear();

                try
                {
                    HtmlNodeCollection tableRows = doc.DocumentNode.SelectNodes("//table/tbody")[tableIndex].ChildNodes;

                    int tableRowIndexMax = tableRows.Count;

                    bool scriptRowDone = false;
                    bool soundIDRowDone = false;

                    for (int tableRowIndex = 0; tableRowIndex < tableRowIndexMax; tableRowIndex++)
                    {
                        if (scriptRowDone && soundIDRowDone)
                        {
                            continue;
                        }
                        HtmlNodeCollection tableData = tableRows[tableRowIndex].ChildNodes;

                        int tableDataIndexMax = tableData.Count;

                        for (int tableDataIndex = 0; tableDataIndex < tableDataIndexMax; tableDataIndex++)
                        {
                            if (tableData[tableDataIndex].HasChildNodes)
                            {
                                if ((tableData[tableDataIndex].InnerHtml.Contains("id") && TableDataAttributeExists(tableData[tableDataIndex].Attributes, "Class", "th")) || (TableDataAttributeExists(tableData[tableDataIndex].Attributes, "Class", "th") && tableData[tableDataIndex].FirstChild.Name.Contains("#text")))
                                {
                                    string value = CheckChildNode(tableData[tableDataIndex], true);
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        correctText.Add(value);
                                        continue;
                                    }
                                }

                                if (tableData[tableDataIndex].InnerHtml.Contains("mp3"))
                                {
                                    string value = CheckChildNode(tableData[tableDataIndex], false);

                                    soundPathList.Add(value);
                                    continue;
                                }
                            }
                            if (correctText.Count != 0 && scriptRowDone)
                            {
                                soundPathList.Add("");
                            }
                        }

                        if (correctText.Count != soundPathList.Count && correctText.Count != 0 && soundPathList.Count != 0)
                        {
                            Console.Write("break");
                        }
                        if (scriptRowDone && soundPathList.Count != 0)
                        {
                            soundIDRowDone = true;
                        }
                        if (correctText.Count != 0 && soundPathList.Count == 0)
                        {
                            scriptRowDone = true;
                        }
                        if (correctText.Count != 0 && soundPathList.Count != 0)
                        {
                            bool CompareOK = SoundDownloadCompare<T>(correctText, soundPathList, list);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                SaveAll();
            }
        }

        /// <summary>
        /// Sets the soundpath to the current word
        /// </summary>
        /// <param name="word">What word to add sound path too.</param>
        /// <returns>returns a true if successfull</returns>
        private bool SetSoundPathToWord(object word)
        {
            string soundPath = "";

            var listOfScript = (List<string>)GetValueFromValueList("ThaiScript");
            foreach (string value in listOfScript)
            {
                soundPath = SoundFilePath + value + ".mp3";
            }

            var listOfPaths = (List<string>)GetValueFromValueList("SoundPath");
            if (File.Exists(soundPath))
            {
                if (listOfPaths.Count == 0)
                {
                    SetValueOfObject(soundPath, "SoundPath", word);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Try and download the right sound that is connected to the thai word.
        /// </summary>
        /// <typeparam name="T">What type of word to use for comparing or setting the soundpath</typeparam>
        /// <param name="correctText">The list of thaiscript from the html file</param>
        /// <param name="soundID">the list of soundpaths</param>
        /// <param name="list">list of words.</param>
        /// <returns>returns true if succesfull</returns>
        private bool SoundDownloadCompare<T>(List<string> correctText, List<string> soundID, List<T> list) where T : new()
        {
            if (soundID.Count != correctText.Count)
            {
                MessageBox.Show("Error: soundID and correcText is not the same size!", "Error");
                return false;
            }
            else
            {
                try
                {
                    for (int listIndex = 0; listIndex < soundID.Count; listIndex++)
                    {
                        if (soundID[listIndex].Length == 0)
                        {
                            continue;
                        }
                        foreach (T word in list)
                        {
                            SetPropertyOfGenericObject(word);

                            List<string> ThaiScriptList = (List<string>)GetValueFromValueList("ThaiScript");

                            bool result = ThaiScriptList.Contains(correctText[listIndex]);

                            if (result)
                            {
                                bool DownloadOK = SoundDownloader(correctText[listIndex], soundID[listIndex]);
                            }
                            bool alreadyHasSoundPath = SetSoundPathToWord(word);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
                return true;
            }
        }

        /// <summary>
        /// The method that handles the downloading and saving to file
        /// </summary>
        /// <param name="correctText">What to name the downloaded file</param>
        /// <param name="soundDownloadPath">The URL of the soundpath</param>
        /// <returns></returns>
        private bool SoundDownloader(string correctText, string soundDownloadPath)
        {
            string savePath = SoundFilePath + correctText + ".mp3";

            using (var client = new WebClient())
            {
                if (!File.Exists(savePath))
                {
                    try
                    {
                        client.DownloadFile(soundDownloadPath, savePath);
                        return true;
                    }
                    catch (WebException wex)
                    {
                        MessageBox.Show("Error: " + wex);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the current word has a sound, and if not turns the button red.
        /// </summary>
        private void SpeakerStatus()
        {
            List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");
            TabIndex = MainWindow_tabController.SelectedIndex;
            if (soundPaths.Count == 0)
            {
                if (TabIndex == 0)
                {
                    btn_Speaker_Page1.Background = Brushes.Red;
                }
                else if (TabIndex == 1)
                {
                    btn_Speaker_Page2.Background = Brushes.Red;
                }
            }
            foreach (string value in soundPaths)
            {
                if (string.IsNullOrEmpty(value) || !File.Exists(value))
                {
                    if (TabIndex == 0)
                    {
                        btn_Speaker_Page1.Background = Brushes.Red;
                    }
                    else if (TabIndex == 1)
                    {
                        btn_Speaker_Page2.Background = Brushes.Red;
                    }
                }
                else
                {
                    var bc = new BrushConverter();
                    if (TabIndex == 0)
                    {
                        btn_Speaker_Page1.Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
                    }
                    else if (TabIndex == 1)
                    {
                        btn_Speaker_Page2.Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a set of data exists in the selected HTMLnode
        /// </summary>
        /// <param name="attributesFromNode">The node to check</param>
        /// <param name="searchForName">What the attribute is named</param>
        /// <param name="searchForValue">What value to look for</param>
        /// <returns></returns>
        private bool TableDataAttributeExists(HtmlAttributeCollection attributesFromNode, string searchForName, string searchForValue)
        {
            foreach (HtmlAttribute attribute in attributesFromNode)
            {
                if (attribute.Name.Contains(searchForName) || attribute.Value.Contains(searchForValue))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Plays the sound from the soundpath of the current word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySound(object sender, RoutedEventArgs e)
        {
            int TabIndex = MainWindow_tabController.SelectedIndex;
            //PreTextChanger(0);

            try
            {
                List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");
                foreach (string soundpath in soundPaths)
                {
                    var reader = new Mp3FileReader(soundpath);
                    var waveOut = new WaveOut();
                    waveOut.Init(reader);
                    waveOut.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex);
            }
        }

        /// <summary>
        /// Removes the soundpaths of all words.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveSoundPath(object sender, RoutedEventArgs e)
        {
            foreach (Word word in Words)
            {
                word.SoundPath.Clear();
            }

            SaveAll();
        }

        /// <summary>
        /// Starts looking for the soundpaths in the html files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SoundDownloader(object sender, RoutedEventArgs e)
        {
            DownloadSoundProcess<Word>(Words, WebFilePath + "Words.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Words1.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Words2.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Words3.html");

            DownloadSoundProcess<Word>(Words, WebFilePath + "Consonants.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Consonants1.html");

            DownloadSoundProcess<Word>(Words, WebFilePath + "Vowel.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Vowel1.html");

            DownloadSoundProcess<Word>(Words, WebFilePath + "Numbers.html");
            DownloadSoundProcess<Word>(Words, WebFilePath + "Numbers1.html");

            CheckAllSoundStatuses(sender, e);
        }

        #endregion Sound

        private void SymbolChapterChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedChapter = (string)((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;
        }
    }
}