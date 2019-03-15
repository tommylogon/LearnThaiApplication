using HtmlAgilityPack;
using LearnThaiApplication.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LearnThaiApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            LoadSettings();

            LoadAllFiles();

            SetInitialStates();

            GetImage();

            Loaded += MainWindow_Loaded;

            ReadWebsiteFiles();
            AppWindow = this;
            words.CollectionChanged += ContentCollectionChanged;
            //WriteAllToFile();*/
        }

        #region Variables and properties

        #region lists

        private List<TextBox> textboxList;
        private ObservableCollection<Chapter> Chapters = new ObservableCollection<Chapter>();
        private List<UserSetting> UserSettings = new List<UserSetting>();
        private List<Word> DisplayList = new List<Word>();
        private List<PropertyInfo> ListOfProperties = new List<PropertyInfo>();
        private List<object> ListOfValues = new List<object>();
        private ObservableCollection<Word> words = new ObservableCollection<Word>();

        public ObservableCollection<Word> Words
        {
            get
            {
                return words;
            }
            set
            {
                if (words != value)
                {
                    words = value;
                    OnPropertyChanged("Words");
                }
            }
        }

        private void ContentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SaveAll();
        }

        #endregion lists

        #region bools

        private bool hasDescription = true;
        private bool displayAllPropertiesInDescription;
        private bool isContinious;
        private bool loopChapter = true;
        private bool isRandom;
        private bool submitionIsNew;
        private bool skipIntro;
        private bool autoPlay;

        #endregion bools

        #region strings

        private string ImageFilePath = Environment.CurrentDirectory + @"\Files\Media\Icon\";
        private string LanguageFilePath = Environment.CurrentDirectory + @"\Files\Media\Language\";
        private string SettingsFilePath = Environment.CurrentDirectory + @"\Files\Settings\";
        private string SoundFilePath = Environment.CurrentDirectory + @"\Files\Media\Sound\";
        private string WebFilePath = Environment.CurrentDirectory + @"\Files\Media\Website\";
        private string DebugFilePath = Environment.CurrentDirectory + @"\Files\Settings\DEBUG\";
        private static string RegexSplitString = @" ^\s|[\s;,]{2,}";

        private string SelectedChapter;
        private string UserName = "Default";
        private string SelectedSymbolTypeToUse;
        
        private string WhatToDisplay;
        private string WhatToTrain;
        private string thaiScript_String;
        private string descriptionText;
        private string result;
        private string answear;
        private string searchString;

        #endregion strings

        #region ints

        private static int correctPoints = 0;
        private static int currentFileIndex = 0;
        private static int CurrentListBoxIndex = -1;

        #endregion ints

        #region objects

        private object SelectedPropertyToDisplay;
        private object SelectedPropertyToValidate;
        private object WhatListTLoad;
        private object WordToLoad;
        public MainWindow AppWindow;

        #endregion objects

        #region Notifiers

        public string CorrectPoints
        {
            get
            {
                return "Points: " + correctPoints;
            }
        }

        public int CurrentFileIndex
        {
            get
            {
                return currentFileIndex;
            }
            set
            {
                if (currentFileIndex != value)
                    currentFileIndex = value;
                OnPropertyChanged("CurrentFileIndex");
            }
        }

        public bool HasDescription
        {
            get
            {
                return hasDescription;
            }
            set
            {
                if (hasDescription != value)
                {
                    hasDescription = value;
                    OnPropertyChanged("HasDescription");
                }
            }
        }

        public bool IsRandom
        {
            get
            {
                return isRandom;
            }
            set
            {
                if (isRandom != value)
                {
                    isRandom = value;
                    OnPropertyChanged("IsRandom");
                }
            }
        }

        public bool IsContinious
        {
            get
            {
                return isContinious;
            }
            set
            {
                if (isContinious != value)
                {
                    isContinious = value;
                    OnPropertyChanged("IsContinious");
                }
            }
        }

        public bool DisplayAll
        {
            get
            {
                return displayAllPropertiesInDescription;
            }
            set
            {
                if (displayAllPropertiesInDescription != value)
                {
                    displayAllPropertiesInDescription = value;
                    HasDescription = value;
                    OnPropertyChanged("DisplayAll");
                }
            }
        }

        public bool AutoPlay
        {
            get
            {
                return autoPlay;
            }
            set
            {
                if (autoPlay != value)
                {
                    autoPlay = value;
                    OnPropertyChanged("AutoPlay");
                }
            }
        }

        public bool SkipIntro
        {
            get
            {
                return skipIntro;
            }
            set
            {
                if (skipIntro != value)
                {
                    skipIntro = value;
                    OnPropertyChanged("SkipIntro");
                }
            }
        }

        public bool LoopChapter
        {
            get
            {
                return loopChapter;
            }
            set
            {
                if (loopChapter != value)
                {
                    loopChapter = value;
                    OnPropertyChanged("LoopChapter");
                }
            }
        }

        public string ThaiScriptString
        {
            get
            {
                return thaiScript_String;
            }
            set
            {
                if (thaiScript_String != value)
                {
                    thaiScript_String = value;
                    OnPropertyChanged("ThaiScriptString");
                }
            }
        }

        public string DescriptionString
        {
            get
            {
                return descriptionText;
            }
            set
            {
                if (descriptionText != value)
                {
                    descriptionText = value;
                    OnPropertyChanged("DescriptionString");
                }
            }
        }


        public string Result
        {
            get
            {
                return result;
            }
            set
            {
                if (result != value)
                {
                    result = value;
                    OnPropertyChanged("Result");
                }
            }
        }


        public string Answear
        {
            get
            {
                return answear;
            }
            set
            {
                if (answear != value)
                {
                    answear = value;
                    OnPropertyChanged("Answear");
                }
            }
        }


        public string SearchString
        {
            get
            {
                return SearchString;
            }
            set
            {
                if (SearchString != value)
                {
                    SearchString = value;
                    OnPropertyChanged("SearchString");
                }
            }
        }
        #endregion Notifiers

        #region others

        private StackPanel sp = new StackPanel();
        private ContentMan window;
        private Random RandomIndex { get; set; } = new Random();
        private Type WhatTypeToUse { get; set; }
        private IEnumerable<Window> Windows { get; set; }
        private UserSetting settings = new UserSetting();

        public event PropertyChangedEventHandler PropertyChanged;

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

                if (script.Count == 1)
                {
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void AddNewChapters()
        {
            bool alreadyExists;
            foreach (Word word in Words)
            {
                Chapter newChapter = new Chapter(word.Chapter);

                alreadyExists = false;

                foreach (Chapter chap in Chapters)
                {
                    if (chap.ChapterName == word.Chapter)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                if (!alreadyExists)
                {
                    Chapters.Add(newChapter);
                }
            }
            SaveFiles<Chapter>(Chapters);
        }

        #endregion TestMethods

        #region Main

        /// <summary>
        /// Handles the combobox selection and updates the displayed data when a new chapter is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChapterChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;

            if (((ComboBox)sender).SelectedItem == null)
            {
            }
            else if (((ComboBox)sender).SelectedItem is string)
            {
                SelectedChapter = (string)((ComboBox)sender).SelectedItem;
            }
            else if (((ComboBox)sender).SelectedItem is Chapter chap)
            {
                SelectedChapter = chap.ChapterName;
            }
            else if (((ComboBox)sender).SelectedItem is ComboBoxItem)
            {
                SelectedChapter = (string)((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;
            }

            FindWordWithChapter();

            if (TabIndex == 0)
            {
                lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList.Count.ToString();

                if (DisplayList.Count > 0)
                {
                    TextChanger<Word>(DisplayList, 0);
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
                    TextChanger<Word>(DisplayList, 0);
                }
                else
                {
                    MessageBox.Show("There are no words in that category yet");
                }

                lbl_Counter_Page2.Content = CurrentFileIndex;
            }
            else if (TabIndex == 2)
            {
                UpdateListBox();
            }
        }

        /// <summary>
        ///Checks and changes the current file index.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="list">List to use</param>
        /// <param name="nextValueToAdd">the next value to add (or subtract) from current file index</param>
        /// <param name="textBlockForScript">textblock to use for display</param>
        private void CheckAndChangePosisionInList<T>(List<T> list, int nextValueToAdd)
        {
            if (IsRandom)
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
                        ThaiScriptString = ListToString((List<string>)GetValueFromValueList("ThaiScript"));
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
        public static string ListToString(List<string> list)
        {
            try
            {
                string combinedStrings = null;
                foreach (string text in list)
                {
                    if (list.IndexOf(text) == list.Count - 1)
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
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Changes to the next chapter in the list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        private void NextChapter(List<Word> list)
        {
            if (CurrentFileIndex >= list.Count)
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
            if (AutoPlay)
            {
                PlaySound(sender, e);
            }
        }

        /// <summary>
        /// fills the description textbox with the selected information.
        /// </summary>
        /// <param name="textBlockDescription">What textblock to use</param>
        private void PopulateDescription()
        {
            if (hasDescription)
            {
                DescriptionString = "";
                if (DisplayAll)
                {
                    foreach (var value in ListOfValues)
                    {
                        if (value is List<string> x)
                        {
                            DescriptionString += ListToString(x) + "\r\n";
                        }
                        else
                        {
                            DescriptionString += value + "\r\n";
                        }
                    }
                }
                else
                {
                    DescriptionString = ListToString((List<string>)GetValueFromValueList("ThaiScript")) + "\r\n";
                    DescriptionString += ListToString((List<string>)GetValueFromValueList("ThaiFonet")) + "\r\n";
                    DescriptionString += ListToString((List<string>)GetValueFromValueList("EngWords")) + "\r\n";
                    DescriptionString += (string)GetValueFromValueList("EngDesc");
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
            if (TabIndex == 0)
            {
                TextChanger<Word>(DisplayList, change);
                lbl_Counter_Page1.Content = CurrentFileIndex;
                SpeakerStatus();
            }
            if (TabIndex == 1)
            {
                TextChanger<Word>(DisplayList, change);
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
            if (AutoPlay)
            {
                PlaySound(sender, e);
            }
        }

        /// <summary>
        /// Handles the selection changes in the listbox
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        private void SelectionChanged(int selectedIndex)
        {
            if (selectedIndex != -1)
            {
                WordToLoad = lib_LoadedWords.SelectedItem;

                SetPropertyOfGenericObject(WordToLoad);

                FillFormTextBoxes();

                txt_FirstSelectionProperty.Text = ListToString((List<string>)GetValueFromValueList("ThaiScript"));
                txt_SecondSelectionProperty.Text = ListToString((List<string>)GetValueFromValueList("ThaiFonet"));
                txt_ThirdSelectionProperty.Text = ListToString((List<string>)GetValueFromValueList("EngWords"));
                txt_FourthSelectionProperty.Text = (string)GetValueFromValueList("EngDesc");
                txt_FifthSelectionProperty.Text = (string)GetValueFromValueList("Chapter");
                txb_Description_Page4.Text = (string)GetValueFromValueList("EngDesc");
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
            SetSettings();
            PreTextChanger(0);
        }

        /// <summary>
        ///Sets the properties of the object it recives.
        /// </summary>
        /// <param name="recived">what object to find properties for</param>
        private void SetPropertyOfGenericObject(object recived)
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
        public static List<string> SplitStringToList(string textToSplit)
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
        /// <param name="movementValue">to move forward, backwards or stay in place in the list</param>
        private void TextChanger<T>(List<T> list, int movementValue) where T : new()
        {
            if (list.Count == 0)
            {
                return;
            }
            try
            {
                Type whatIsT = typeof(T);

                CheckAndChangePosisionInList<T>(list, movementValue);

                SetPropertyOfGenericObject(list[CurrentFileIndex]);

                SelectedPropertyToDisplay = GetValueFromValueList(WhatToDisplay);

                if (list[CurrentFileIndex].GetType() == typeof(Word))
                {
                    string retrivedChapter = (string)GetValueFromValueList("Chapter", SelectedChapter);

                    if (retrivedChapter == SelectedChapter)
                    {
                        if (SelectedPropertyToDisplay is List<String> propertyIsList)
                        {
                            ThaiScriptString = ListToString(propertyIsList);
                        }
                        else
                        {
                            ThaiScriptString = (String)SelectedPropertyToDisplay;
                        }
                    }
                    else
                    {
                        MessageBox.Show("There are no content with chapter" + SelectedChapter + " available right now.");
                        return;
                    }
                }
                PopulateDescription();
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
            if (TabIndex == 0)
            {
                ValidateAnswer(DisplayList);
            }
            else if (TabIndex == 1)
            {
                ValidateAnswer(DisplayList);
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
        private void ValidateAnswer(List<Word> list)
        {
            SetPropertyOfGenericObject(list[CurrentFileIndex]);

            SelectedPropertyToValidate = GetValueFromValueList(WhatToTrain);

            int rightAnswears = 0;
            int totalAnswears = 0;
            List<string> answers = Regex.Split(Answear, RegexSplitString).ToList();

            if (SelectedPropertyToValidate is List<String>)
            {
                foreach (String correctWord in SelectedPropertyToValidate as List<string>)
                {
                    totalAnswears = ((List<string>)SelectedPropertyToValidate).Count;
                    foreach (String answer in answers)
                    {
                        if (String.Equals(correctWord, answer, StringComparison.OrdinalIgnoreCase))
                        {
                            correctPoints++;
                            rightAnswears++;
                        }
                    }
                }
            }
            else if (string.Equals(Answear, (string)SelectedPropertyToValidate, StringComparison.OrdinalIgnoreCase))
            {
                totalAnswears = 1;
                correctPoints++;
                rightAnswears++;
            }

            if (rightAnswears != 0)
            {
                Result = "You got " + rightAnswears + " of " + totalAnswears + " correct!";
            }
            else
            {
                Result = "Sorry, try again!";
            }

            PopulateDescription();

            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Points.Content = "Points: " + CorrectPoints;
            lbl_Points_Page2.Content = "Points: " + CorrectPoints;
            //txt_Answear_Page1.Text = "";
            //txt_Answear_Page2.Text = "";
        }

        #endregion Main

        #region Settings

        /// <summary>
        /// Clears the textboxs and textblocks on the current tab
        /// </summary>
        private void ClearFields()
        {

            Result = "";
            Answear = "";

            //    if (TabIndex == 0)
            //    //{
            //    //    txb_Description_page1.Text = "";
            //    //    txb_ThaiScript_Page1.Text = "";
            //    //    txb_Status_Page1.Text = "";
            //    //    txt_Answear_Page1.Text = "";
            //    }
            //    else if (TabIndex == 1)
            //    {
            //        txb_ThaiScript_Page2.Text = "";
            //        txb_Status_Page2.Text = "";
            //        txb_Description_page2.Text = "";
            //        txt_Answear_Page2.Text = "";
            //    }
            //    else if (TabIndex == 2)
            //    {
            //        if (ckb_AutoClean.IsChecked == true)
            //        {
            //            txt_FirstSelectionProperty.Text = "";
            //            txt_SecondSelectionProperty.Text = "";
            //            txt_ThirdSelectionProperty.Text = "";
            //            txt_FourthSelectionProperty.Text = "";
            //            txt_FifthSelectionProperty.Text = "";

            //            if (textboxList.Count != 0)
            //            {
            //                foreach (TextBox txt in textboxList)
            //                {
            //                    txt.Text = "";
            //                }
            //            }
            //        }
            //    }
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
                hasDescription = true;
                PopulateDescription();
                PopulateDescription();
            }
            else
            {
                DescriptionString = "";
                DescriptionString = "";
                hasDescription = false;
            }
            SetSettings();
        }

        /// <summary>
        /// Checks if the full debug description is true and then displays all proeprties.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FullDesc_Checked(object sender, RoutedEventArgs e)
        {
            SetSettings();
        }

        /// <summary>
        /// Loads all the files from storage
        /// </summary>
        private void LoadAllFiles()
        {
            LoadFiles<Chapter>(Chapters);

            LoadFiles<Word>(words);
        }

        /// <summary>
        ///
        /// </summary>
        private void LoadSettings()
        {
            if (File.Exists(SettingsFilePath + "settings.xml"))
            {
                settings = XmlSerialization.ReadFromXmlFile<UserSetting>(SettingsFilePath + "Settings.xml");

                hasDescription = settings.DescriptionOn;
                LoopChapter = settings.LoopChapter;
                DisplayAll = settings.DisplayAllPropertiesInDescription;
                IsRandom = settings.RandomOn;
                SkipIntro = settings.SkipIntro;
                WhatToDisplay = settings.WhatToDisplay;
                WhatToTrain = settings.WhatToTrain;
                SkipIntro = settings.SkipIntro;
                AutoPlay = settings.AutoPlaySounds;
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveSetting()
        {
            XmlSerialization.WriteToXmlFile<UserSetting>(SettingsFilePath + "Settings.xml", settings, false);
        }

        /// <summary>
        /// Load files to lists.
        /// </summary>
        /// <typeparam name="T">What type to load</typeparam>
        /// <param name="list">What list to load into</param>
        private void LoadFiles<T>(ObservableCollection<T> list) where T : new()
        {
            Type whatIsT = typeof(T);

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml");

            List<T> newWordToAdd = new List<T>();

            foreach (T wordFoundInFile in wordsFromFIle)
            {
                if (whatIsT != typeof(Chapter))
                {
                    SetPropertyOfGenericObject(wordFoundInFile);
                }
                list.Add(wordFoundInFile);
            }
        }

        /// <summary>
        /// Sets loopChapter to true or false and allows the user to continiue to the next chaper automaticaly at the end of the last chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopChapter_Checked(object sender, RoutedEventArgs e)
        {
            LoopChapter = (sender as CheckBox)?.IsChecked == true;
            SetSettings();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!SkipIntro)
            {
                MessageBox.Show("Welcome to Learn Thai!\r\n Here you can learn some of the basics of thai, including how to approximately pronounce thai words, what the thai words mean and the sounds they make with the help of audio clips.", "สวัสดีครับ");
            }
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
        /// When the user changes tabs it sets the file index to 0 and updates the display data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTabChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabIndex != MainWindow_tabController.SelectedIndex)
            {
                TabIndex = MainWindow_tabController.SelectedIndex;
                CurrentFileIndex = 0;
                ResetChapter();

                PreTextChanger(0);

                if (MainWindow_tabController.SelectedIndex == 2)
                {
                    WhatListTLoad = Words;
                    WhatTypeToUse = typeof(Word);

                    ClearFields();
                    UpdateListBox();
                }
            }
        }

        /// <summary>
        /// Check if random number is on, if on randomly jump around in the chapter. also disables loop chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Randomized_Checked(object sender, RoutedEventArgs e)
        {
            LoopChapter = false;
            SetSettings();
        }

        /// <summary>
        ///
        /// </summary>
        private void ResetChapter()
        {
            if (TabIndex == 0)
            {
                cb_Chapter_Page1.SelectedIndex = 0;
                SelectedChapter = (string)((ComboBoxItem)cb_Chapter_Page1.SelectedItem).Content;
            }
            else if (TabIndex == 1)
            {
                cb_SymbolChapters.SelectedIndex = 0;
                SelectedChapter = (string)((ComboBoxItem)cb_SymbolChapters.SelectedItem).Content;
            }
            else if (TabIndex == 2)
            {
                cb_ManageOnChapter.SelectedIndex = 0;
                SelectedChapter = ((Chapter)cb_ManageOnChapter.SelectedItem).ChapterName;
            }
            FindWordWithChapter();
        }

        /// <summary>
        /// Saves the data to files.
        /// </summary>
        /// <returns></returns>
        private bool SaveAll()
        {
            try
            {
                SaveFiles<Word>(words);

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
        private void SaveFiles<T>(ObservableCollection<T> sourceList) where T : new()
        {
            Type whatIsT = typeof(T);
            List<T> list = new List<T>(sourceList);
            XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }

        /// <summary>
        /// Sets the initial states for diffrent components.
        /// </summary>
        private void SetInitialStates()
        {
            if (UserSettings.Count != 0)
            {
                //DO NOTHING
            }
            else
            {
                lbl_Counter_Page2.Content = CurrentFileIndex;
                lbl_Counter_Page1.Content = CurrentFileIndex;

                //if (hasDescription)
                //{
                //    ckb_DescBox_Page1.IsChecked = true;
                //    ckb_DescBox_Page2.IsChecked = true;
                //    ckb_DescBox_Setting.IsChecked = true;
                //}
                //if (IsRandom)
                //{
                //    ckb_Randomized_Page1.IsChecked = RandomOn;
                //    ckb_Randomized_Page2.IsChecked = RandomOn;
                //    ckb_Randomized_Setting.IsChecked = RandomOn;
                //}
                //if (SkipIntro)
                //{
                //    ckb_SkipIntro.IsChecked = SkipIntro;
                //}

                //if (AutoPlay)
                //{
                //    ckb_AutoPlay.IsChecked = AutoPlay;
                //}
                //if (LoopChapter)
                //{
                //    ckb_LoopChapter.IsChecked = LoopChapter;
                //}
                //if (DisplayAllPropertiesInDescription)
                //{
                //    ckb_FullDesc.IsChecked = DisplayAllPropertiesInDescription;
                //}
                rb_SubmitNew.IsChecked = true;

                if (WhatToTrain == "ThaiFonet")
                {
                    rb_TrainFonet_Page1.IsChecked = true;
                    rb_TrainFonet_Page2.IsChecked = true;
                    rb_TrainFonet_Setting.IsChecked = true;
                }
                else if (WhatToTrain == "ThaiScript")
                {
                    rb_TrainScript_Page1.IsChecked = true;
                    rb_TrainScript_Page2.IsChecked = true;
                    rb_TrainScript_Setting.IsChecked = true;
                }
                else if (WhatToTrain == "EngWords")
                {
                    rb_TrainEngWords_Page1.IsChecked = true;
                    rb_TrainEngWords_Page2.IsChecked = true;
                    rb_TrainEngWords_Setting.IsChecked = true;
                }

                PopulateManageChapterCB();

                cb_SymbolChapters.SelectedIndex = 0;
                lbl_ChapterCount_Page2.Content = "Words in chapter: " + DisplayList.Count.ToString();
                cb_Chapter_Page1.SelectedIndex = 0;
                lbl_ChapterCount_Page1.Content = "Words in chapter: " + DisplayList.Count.ToString();
            }

            lib_LoadedWords.DisplayMemberPath = "Name";
            cb_ManageOnChapter.DisplayMemberPath = "ChapterName";
            cb_SelectList.DisplayMemberPath = "ChapterName";
            SetSettings();
        }

        /// <summary>
        ///
        /// </summary>
        private void SetSettings()
        {
            settings.DescriptionOn = hasDescription;
            settings.RandomOn = IsRandom;
            settings.LoopChapter = LoopChapter;
            settings.WhatToDisplay = WhatToDisplay;
            settings.WhatToTrain = WhatToTrain;
            settings.SkipIntro = SkipIntro;
            settings.DisplayAllPropertiesInDescription = DisplayAll;
            settings.SkipIntro = SkipIntro;
            settings.AutoPlaySounds = AutoPlay;
            SaveSetting();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveSetting();
        }

        /// <summary>
        ///
        /// </summary>
        private void ReadWebsiteFiles()
        {
            List<string> websiteUrls = new List<string>();
            string line;
            StreamReader file = new StreamReader(WebFilePath + "Websites.txt");
            while ((line = file.ReadLine()) != null)
            {
                websiteUrls.Add(line);
            }
            file.Close();

            foreach (string s in websiteUrls)
            {
                Console.WriteLine(s);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintAllWordsToFile(object sender, RoutedEventArgs e)
        {
            string fullText = "";

            foreach (Word word in Words)
            {
                foreach (string script in word.ThaiScript)
                {
                    string soundPath = SoundFilePath + script + ".wma";

                    if (!File.Exists(soundPath))
                    {
                        fullText += script + "\r\n";
                    }
                }
            }

            File.WriteAllText(DebugFilePath + "File.txt", fullText);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckSoundStatus_Clicked(object sender, RoutedEventArgs e)
        {
            CheckAllSoundStatuses();
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
            //if (SubmitionIsNew)
            //{
            //    SubmitNewWord(isQuick);
            //}
            //else if (!SubmitionIsNew)
            //{
            //    SubmitUpdatedWord(isQuick);
            //}
            //else
            //{
            //    MessageBox.Show("Select what you want to do");
            //}
            //PopulateManageChapterCB();

            //FindWordWithChapter();
            //UpdateListBox();
            //ClearFields();
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
        public bool SubmitFromForm()
        {
            textboxList = FormTextboxes();

            foreach (Word oldWord in Words)
            {
                if (WordToLoad.Equals(oldWord))
                {
                    SetPropertyOfGenericObject(oldWord);

                    SetNewValuesFromForm(oldWord, textboxList);

                    return true;
                }
            }
            return false;
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
                DeleteSelected(new List<Word>(words));

                SaveAll();
            }

            PopulateManageChapterCB();
            FindWordWithChapter();
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

        /// <summary>
        /// Deletes the selected word from
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to delete from</param>
        private void DeleteSelected(List<Word> list)
        {
            MessageBox.Show("tried to remove element " + list[lib_LoadedWords.SelectedIndex].ToString());
            list.Remove((Word)lib_LoadedWords.SelectedItem);
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
                SelectionChanged(lib_LoadedWords.SelectedIndex);
            }
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
        ///
        /// </summary>
        private void PopulateManageChapterCB()
        {
            cb_ManageOnChapter.ItemsSource = null;
            cb_SelectList.ItemsSource = null;

            cb_SelectList.ItemsSource = Chapters;
            cb_ManageOnChapter.ItemsSource = Chapters;
        }

        /// <summary>
        /// Sets the values of the new word from the quickForm
        /// </summary>
        /// <param name="newWord"></param>
        private void QuickSubmit(Word newWord)
        {
            newWord.ThaiScript = SplitStringToList(txt_FirstSelectionProperty.Text);
            newWord.ThaiFonet = SplitStringToList(txt_SecondSelectionProperty.Text);
            newWord.EngWords = SplitStringToList(txt_ThirdSelectionProperty.Text);
            newWord.EngDesc = txt_FourthSelectionProperty.Text;
            newWord.Chapter = txt_FifthSelectionProperty.Text;
        }

        /// <summary>
        /// Selects what to move and sends the object to be moved.
        /// </summary>
        /// <param name="newIndex">The new index for the selected word</param>
        private void SelectWhatToMove(int newIndex)
        {
            MoveObjectInList<Word>(new List<Word>(words), lib_LoadedWords.SelectedIndex, newIndex);

            UpdateListBox();
            lib_LoadedWords.SelectedIndex = newIndex;
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
        /// Adds new words to the list.
        /// </summary>
        /// <typeparam name="T">What type to work with</typeparam>
        /// <param name="list">What list to work with</param>
        private void SubmitNewWord(bool isQuick)
        {
            Word newWord = new Word();

            SetPropertyOfGenericObject(newWord);

            if (isQuick)
            {
                QuickSubmit(newWord);
            }
            else
            {
                FullSubmitNewWord(newWord);
            }

            AddNewChapters();

            Words.Add(newWord);

            SaveAll();
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
        /// Starts a quick submit of a new word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitNewWordQuick(object sender, RoutedEventArgs e)
        {
            HowToSubmit(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isNew"></param>
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
        /// Checks if supposed to submit a new or update a old word.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitStyleChecked(object sender, RoutedEventArgs e)
        {
            //if ((string)(sender as RadioButton)?.Content == "Submit new")
            //{
            //    SubmitionIsNew = true;
            //}
            //else if ((string)(sender as RadioButton)?.Content == "Update")
            //{
            //    SubmitionIsNew = false;
            //}
            //else
            //{
            //    MessageBox.Show("Error: No submit style selected");
            //}
        }

        /// <summary>
        /// The first part of the process to update old words
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void SubmitUpdatedWord(bool isQuick)
        {
            if (!isQuick)
            {
                textboxList = FormTextboxes();
            }

            foreach (Word oldWord in Words)
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
                        SetNewValuesFromForm(oldWord, textboxList);
                    }
                    break;
                }
            }

            SaveAll();
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

            if (SelectedChapter == "All")
            {
                LoadObjectsToLib<Word>(new List<Word>(words));
            }
            else
            {
                LoadObjectsToLib<Word>(DisplayList);
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
                    else if (attributeValue.Contains("wma"))
                    {
                        var lstring = attributeValue.Split('\'');
                        foreach (string s in lstring)
                        {
                            if (s.Contains("wma"))
                            {
                                return s;
                            }
                        }
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
        private void DownloadSoundProcess(List<Word> list, string url)
        {
            HtmlWeb web = new HtmlWeb();

            HtmlDocument doc = web.Load(url);

            //HtmlDocument doc = new HtmlDocument();

            //doc.Load(url, Encoding.UTF8);

            List<string> correctText = new List<string>();

            List<string> soundPathList = new List<string>();

            // var tableIndexTEST = webdoc.DocumentNode.SelectNodes("//table");

            // var tabletest = doc.DocumentNode.SelectNodes("//table/tbody");

            int tableIndexMax = doc.DocumentNode.SelectNodes("//table").Count;

            for (int tableIndex = 0; tableIndex < tableIndexMax; tableIndex++)
            {
                soundPathList.Clear();
                correctText.Clear();

                try
                {
                    HtmlNodeCollection tableRows = doc.DocumentNode.SelectNodes("//table")[tableIndex].ChildNodes;

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

                                if (tableData[tableDataIndex].InnerHtml.Contains("mp3") || tableData[tableDataIndex].InnerHtml.Contains("wma"))
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
                            bool CompareOK = SoundDownloadCompare(correctText, soundPathList, list);
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
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_PlaySoundFile(object sender, DoWorkEventArgs e)
        {
            MediaPlayer player = new MediaPlayer();

            try
            {
                foreach (String soundPath in DisplayList[CurrentFileIndex].SoundPath)
                {
                    int waitTime = 1000 + (DisplayList[CurrentFileIndex].ThaiScript.Count * 50);

                    player.Open(new Uri(soundPath));
                    player.Play();

                    Thread.Sleep(waitTime);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex);
            }
        }

        /// <summary>
        /// Plays the sound from the soundpath of the current word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySound(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += Worker_PlaySoundFile;

            worker.RunWorkerAsync();
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
            CheckAllSoundStatuses();
            SaveAll();
        }

        /// <summary>
        /// Sets the soundpath to the current word
        /// </summary>
        /// <param name="word">What word to add sound path too.</param>
        /// <returns>returns a true if successfull</returns>
        private bool SetSoundPathToWord(Word word)
        {
            string soundPath = "";

            foreach (string script in word.ThaiScript)
            {
                soundPath = SoundFilePath + script + ".wma";

                if (File.Exists(soundPath))
                {
                    if (word.SoundPath.Count != word.ThaiScript.Count && !word.SoundPath.Contains(soundPath))
                    {
                        word.SoundPath.Add(soundPath);
                    }
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
        private bool SoundDownloadCompare(List<string> correctText, List<string> soundID, List<Word> list)
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
                        foreach (Word word in list)
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
            string savePath = SoundFilePath + correctText + ".wma";

            string FullSoundPath = "http://www.thai-language.com" + soundDownloadPath;

            using (var client = new WebClient())
            {
                if (!File.Exists(savePath))
                {
                    try
                    {
                        client.DownloadFile(FullSoundPath, savePath);
                        return true;
                    }
                    catch (WebException wex)
                    {
                        MessageBox.Show("Error: " + wex.Message);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Starts looking for the soundpaths in the html files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartSoundDownload(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += RunWebsiteFiles_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Checks if the current word has a sound, and if not turns the button red.
        /// </summary>
        private void SpeakerStatus()
        {
            List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");

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
        /// Cycles trough all lists of words and then displays the result in a messagebox
        /// </summary>
        /// <param name="sender">The object that initiated the method</param>
        /// <param name="e"></param>
        private void CheckAllSoundStatuses()
        {
            string fullText;

            if (SelectedChapter == "All")
            {
                fullText = CheckSoundStatus<Word>(new List<Word>(words));
            }
            else
            {
                fullText = CheckSoundStatus<Word>(DisplayList);
            }

            MessageBox.Show(fullText);
        }

        #endregion Sound

        #region Async

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunWebsiteFiles_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> websiteUrls = new List<string>();
            string line;
            StreamReader file = new StreamReader(WebFilePath + "Websites.txt");
            while ((line = file.ReadLine()) != null)
            {
                websiteUrls.Add(line);
            }

            file.Close();
            double length = websiteUrls.Count;
            double current = 0;
            foreach (string s in websiteUrls)
            {
                DownloadSoundProcess(new List<Word>(words), s);
                current++;
                double done = (current / length) * 100;
                (sender as BackgroundWorker).ReportProgress((int)done);
            }

            CheckAllSoundStatuses();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbStatus.Value = e.ProgressPercentage;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSoundPaths_DoWork(object sender, DoWorkEventArgs e)
        {
            double currentIndex;
            double progress;
            foreach (Word word in Words)
            {
                SetSoundPathToWord(word);

                currentIndex = Words.IndexOf(word);
                progress = (currentIndex / Words.Count) * 100;
                (sender as BackgroundWorker).ReportProgress((int)progress);
            }
            SaveAll();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSoundPath_clicked(object sender, RoutedEventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += SetSoundPaths_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        #endregion Async

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SkipMessage_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.IsChecked == true)
            {
                SkipIntro = true;
            }
            else
            {
                SkipIntro = false;
            }
            SetSettings();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoPlay_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox)?.IsChecked == true)
            {
                AutoPlay = true;
            }
            else
            {
                AutoPlay = false;
            }
            SetSettings();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seach_Clicked(object sender, RoutedEventArgs e)
        {
            Search(searchString);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchValue"></param>
        private void Search(string searchValue)
        {
            ObservableCollection<Word> searchResults = new ObservableCollection<Word>();

            List<Word> listToSeach;

            
            
                listToSeach = new List<Word>(Words);
            
            //else
            //{
            //    listToSeach = DisplayList;
            //}

            try
            {
                foreach (Word word in listToSeach)
                {
                    SetPropertyOfGenericObject(word);

                    foreach (var value in ListOfValues)
                    {
                        if (value is List<string> sublist)
                        {
                            foreach (string s in sublist)
                            {
                                if (s.CaseInsensitiveContains(searchValue))
                                {
                                    searchResults.Add(word);
                                    continue;
                                }
                            }
                        }
                        else if (((string)value).CaseInsensitiveContains(searchValue))
                        {
                            searchResults.Add(word);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Words = searchResults;
            //LoadObjectsToLib<Word>(searchResults);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckSoundChapter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedItem is string)
            {
                SelectedChapter = (string)((ComboBox)sender).SelectedItem;
            }
            else if (((ComboBox)sender).SelectedItem is Chapter chap)
            {
                SelectedChapter = chap.ChapterName;
            }
            else if (((ComboBox)sender).SelectedItem is ComboBoxItem)
            {
                SelectedChapter = (string)((ComboBoxItem)((ComboBox)sender).SelectedItem).Content;
            }

            FindWordWithChapter();
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }

        private void NewItemAdded(object sender, AddingNewItemEventArgs e)
        {
            SaveAll();
        }

        private void RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            SaveAll();
        }
    }
}