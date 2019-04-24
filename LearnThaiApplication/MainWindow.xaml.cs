using HtmlAgilityPack;
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
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Speech.Recognition;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using static LearnThaiApplication.User;
using System.Globalization;

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
            Loaded += MainWindow_Loaded;
            AppWindow = this;

            ci = new CultureInfo("en-US");
            sre = new SpeechRecognitionEngine(ci);
            ss = new SpeechSynthesizer();
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += sre_SpeechRecogniced;
            sre.RecognizeCompleted += sre_RecognizedCompleted;
            sre.SpeechHypothesized += sre_SpeechHypotized;

            var voices = ss.GetInstalledVoices();

            ss.SetOutputToDefaultAudioDevice();
            ss.Speak("Sa wat dee krap");

            Grammar g_HelloGoodbye = GetHelloGoodbyeGrammar();

            LoadSettings();
            LoadAllFiles();
            SetInitialStates();
            GetImage();
            words.CollectionChanged += ContentCollectionChanged;
            displayList.CollectionChanged += ContentCollectionChanged;

            sre.LoadGrammarAsync(g_HelloGoodbye);
            //chapters.CollectionChanged += ChaptersCollectionChanged;
        }

        #region Variables and properties

        #region lists

        private ObservableCollection<Chapter> chapters = new ObservableCollection<Chapter>();
        private ObservableCollection<Word> displayList = new ObservableCollection<Word>();
        private List<PropertyInfo> ListOfProperties = new List<PropertyInfo>();
        private List<object> ListOfValues = new List<object>();
        private List<UserSetting> UserSettings = new List<UserSetting>();
        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        private List<MediaPlayer> mediaPlayers = new List<MediaPlayer>();

        public ObservableCollection<Word> DisplayList
        {
            get
            {
                return displayList;
            }
            set
            {
                if (displayList != value)
                {
                    displayList = value;
                    OnPropertyChanged("DisplayList");
                }
            }
        }

        private ObservableCollection<User> users = new ObservableCollection<User>();

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
        }

        public ObservableCollection<Chapter> Chapters
        {
            get
            {
                return chapters;
            }
            set
            {
                if (chapters != value)
                {
                    chapters = value;
                    OnPropertyChanged("Chapters");
                }
            }
        }

        public ObservableCollection<User> Users
        {
            get
            {
                return users;
            }
            set
            {
                if (users != value)
                {
                    users = value;
                    OnPropertyChanged("Users");
                }
            }
        }

        #endregion lists

        #region bools

        private bool trainScript = false;
        private bool trainFonet = false;
        private bool trainWords = false;

        public bool IsListening
        {
            get
            {
                return settings.ActivateSpeechRecognition;
            }
            set
            {
                if (settings.ActivateSpeechRecognition != value)
                {
                    settings.ActivateSpeechRecognition = value;
                    OnPropertyChanged("IsListening");
                }
            }
        }

        public bool TrainScript
        {
            get
            {
                return trainScript;
            }
            set
            {
                if (trainScript != value)
                {
                    trainScript = value;
                    OnPropertyChanged("TrainScript");
                }
            }
        }

        public bool TrainFonet
        {
            get
            {
                return trainFonet;
            }
            set
            {
                if (trainFonet != value)
                {
                    trainFonet = value;
                    OnPropertyChanged("TrainFonet");
                }
            }
        }

        public bool TrainWords
        {
            get
            {
                return trainWords;
            }
            set
            {
                if (trainWords != value)
                {
                    trainWords = value;
                    OnPropertyChanged("TrainWords");
                }
            }
        }

        public bool ShowSaveLocation
        {
            get
            {
                return settings.ShowSaveLocation;
            }
            set
            {
                if (settings.ShowSaveLocation != value)
                {
                    settings.ShowSaveLocation = value;
                    OnPropertyChanged("ShowSaveLocation");
                    SaveSetting();
                }
            }
        }

        public bool DisplayAll
        {
            get
            {
                return settings.DisplayAllPropertiesInDescription;
            }
            set
            {
                if (settings.DisplayAllPropertiesInDescription != value)
                {
                    settings.DisplayAllPropertiesInDescription = value;
                    HasDescription = value;
                    OnPropertyChanged("DisplayAll");
                    SaveSetting();
                }
            }
        }

        public bool HasDescription
        {
            get
            {
                return settings.DescriptionOn;
            }
            set
            {
                if (settings.DescriptionOn != value)
                {
                    settings.DescriptionOn = value;
                    OnPropertyChanged("HasDescription");
                    SaveSetting();
                }
            }
        }

        public bool IsLooping
        {
            get
            {
                return settings.IsLooping;
            }
            set
            {
                if (settings.IsLooping != value)
                {
                    settings.IsLooping = value;
                    OnPropertyChanged("IsLooping");
                    SaveSetting();
                }
            }
        }

        public bool IsRandom
        {
            get
            {
                return settings.RandomOn;
            }
            set
            {
                if (settings.RandomOn != value)
                {
                    settings.RandomOn = value;
                    OnPropertyChanged("IsRandom");
                    SaveSetting();
                }
            }
        }

        public bool AutoPlay
        {
            get
            {
                return settings.AutoPlaySounds;
            }
            set
            {
                if (settings.AutoPlaySounds != value)
                {
                    settings.AutoPlaySounds = value;
                    OnPropertyChanged("AutoPlay");
                    SaveSetting();
                }
            }
        }

        public bool SkipCompleted
        {
            get
            {
                return settings.SkipCompletedWords;
            }
            set
            {
                if (settings.SkipCompletedWords != value)
                {
                    settings.SkipCompletedWords = value;
                    OnPropertyChanged("SkipCompleted");
                    SaveSetting();
                }
            }
        }

        public bool SkipIntro
        {
            get
            {
                return settings.SkipIntro;
            }
            set
            {
                if (settings.SkipIntro != value)
                {
                    settings.SkipIntro = value;
                    OnPropertyChanged("SkipIntro");
                    SaveSetting();
                }
            }
        }

        #endregion bools

        #region strings

        private static string RegexSplitString = @" ^\s|[\s;,]{2,}";
        private string answear;
        private string DebugFilePath = Environment.CurrentDirectory + @"\Files\Settings\DEBUG\";
        private string ImageFilePath = Environment.CurrentDirectory + @"\Files\Media\Icon\";
        private string LanguageFilePath = Environment.CurrentDirectory + @"\Files\Media\Language\";
        private string WebFilePath = Environment.CurrentDirectory + @"\Files\Media\Website\";
        private string SettingsFilePath = Environment.CurrentDirectory + @"\Files\Settings\";
        private string SoundFilePath = Environment.CurrentDirectory + @"\Files\Media\Sound\";

        private string descriptionText;
        private string result;
        private string searchString;
        private string SelectedChapter;
        private string thaiScript_String;
        private string trainWhatChoosen;
        private string currentWord;

        private string chapterCounter;
        private string speechResult;

        public string WhatToTrain
        {
            get
            {
                return settings.WhatToTrain;
            }
            set
            {
                if (settings.WhatToTrain != value)
                {
                    settings.WhatToTrain = value;
                    OnPropertyChanged("WhatToTrain");
                    SaveSetting();
                }
            }
        }

        public string WhatToDisplay
        {
            get
            {
                return settings.WhatToDisplay;
            }
            set
            {
                if (settings.WhatToDisplay != value)
                {
                    settings.WhatToDisplay = value;
                    OnPropertyChanged("WhatToDisplay");
                    SaveSetting();
                }
            }
        }

        public string SpeechResult
        {
            get
            {
                return speechResult;
            }
            set
            {
                if (speechResult != value)
                {
                    speechResult = value;
                    OnPropertyChanged("SpeechResult");
                }
            }
        }

        public string ChapterCounter
        {
            get
            {
                return chapterCounter;
            }
            set
            {
                if (chapterCounter != value)
                {
                    chapterCounter = value;
                    OnPropertyChanged("ChapterCounter");
                }
            }
        }

        public string TrainWhatChoosen
        {
            get
            {
                return trainWhatChoosen;
            }
            set
            {
                if (trainWhatChoosen != value)
                {
                    trainWhatChoosen = value;
                    OnPropertyChanged("TrainWhatChoosen");
                }
            }
        }

        public string CurrentWord
        {
            get
            {
                return currentWord;
            }
            set
            {
                if (currentWord != value)
                {
                    currentWord = value;
                    OnPropertyChanged("CurrentWord");
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

        public string SearchString
        {
            get
            {
                return searchString;
            }
            set
            {
                if (searchString != value)
                {
                    searchString = value;
                    OnPropertyChanged("SearchString");
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

        #endregion strings

        #region ints

        private static int correctPoints = 0;
        private static int currentFileIndex = 0;
        private int selectedChapterIndex = 0;
        private int tabIndex = 0;
        private int movementValue;

        public new int TabIndex
        {
            get
            {
                return tabIndex;
            }
            set
            {
                if (tabIndex != value)
                {
                    tabIndex = value;
                    OnPropertyChanged("TabIndex");
                }
            }
        }

        public int MovementValue
        {
            get
            {
                return movementValue;
            }
            set
            {
                if (movementValue != value)
                {
                    movementValue = value;
                    OnPropertyChanged("MovementValue");
                }
            }
        }

        public int SelectedChapterIndex
        {
            get
            {
                return selectedChapterIndex;
            }
            set
            {
                if (selectedChapterIndex != value)
                {
                    selectedChapterIndex = value;
                    OnPropertyChanged("SelectedChapterIndex");
                }
            }
        }

        #endregion ints

        #region objects

        private User currentUser;
        public MainWindow AppWindow;
        private object SelectedPropertyToDisplay;
        private object SelectedPropertyToValidate;
        private Word selectedWordDataGrid;
        private double progressValue;
        private static CultureInfo ci;
        private SpeechRecognitionEngine sre;
        private SpeechSynthesizer ss;

        public double ProgressValue
        {
            get
            {
                return progressValue;
            }
            set
            {
                if (progressValue != value)
                {
                    progressValue = value;
                    OnPropertyChanged("ProgressValue");
                }
            }
        }

        public Word SelectedWordDG
        {
            get
            {
                return selectedWordDataGrid;
            }
            set
            {
                if (selectedWordDataGrid != value)
                {
                    selectedWordDataGrid = value;
                    OnPropertyChanged("SelectedWordDG");
                }
            }
        }

        #endregion objects

        #region others

        private UserSetting settings = new UserSetting();

        public event PropertyChangedEventHandler PropertyChanged;

        private Random RandomIndex { get; set; } = new Random();

        #endregion others

        #endregion Variables and properties

        #region TestMethods

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
            SaveFiles<Chapter>(Chapters, "Thai_Chapters");
        }

        /// <summary>
        /// Testmethod writing values from the lists to file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        private void WriteWordToFile(List<Word> list)
        {
            foreach (Word word in list)
            {
                List<string> script = (List<string>)GetValueFromValueList("ThaiScript");

                if (script.Count == 1)
                {
                }
            }
        }

        #endregion TestMethods

        #region Main

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
        /// Turns a string into a list of strings.
        /// </summary>
        /// <param name="textToSplit">String to split</param>
        /// <returns></returns>
        public static List<string> SplitStringToList(string textToSplit)
        {
            return Regex.Split(textToSplit, RegexSplitString).ToList<String>();
        }

        /// <summary>
        /// Handles the combobox selection and updates the displayed data when a new chapter is selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChapterChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;
            SelectedChapter = Chapters[SelectedChapterIndex].ChapterName;

            FindWordWithChapter();

            if (TabIndex == 0)
            {
                if (SelectedChapter == "All")
                {
                    if (SelectedChapterIndex <= 0 && MovementValue < 0)
                    {
                        SelectedChapterIndex = Chapters.Count - 1;
                    }
                    else
                    {
                        SelectedChapterIndex++;
                    }
                }
                ChapterCounter = "Words in chapter: " + DisplayList.Count.ToString();
                if (DisplayList.Count() != 0)
                {
                    Grammar g_AllInList = GetAllWordsGrammar();
                    sre.UnloadAllGrammars();
                    sre.LoadGrammarAsync(g_AllInList);
                }

                if (DisplayList.Count > 0)
                {
                    MovementValue = 0;
                    TextChanger();
                }
                else
                {
                    MessageBox.Show("There are no words in that category yet");
                }

                CurrentWord = "Current File in list: " + (CurrentFileIndex + 1);
            }
        }

        private bool CheckIfChapterIsDone()
        {
            int counter = 0;
            foreach (Word word in DisplayList)
            {
                if (CheckIfUserHasCompleted(currentUser.CompletedWords, word, false))
                {
                    counter++;
                }
            }
            if (counter == DisplayList.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///Checks and changes the current file index.
        /// </summary>
        /// <typeparam name="T">Type to use</typeparam>
        /// <param name="list">List to use</param>
        /// <param name="nextValueToAdd">the next value to add (or subtract) from current file index</param>
        /// <param name="textBlockForScript">textblock to use for display</param>
        private void CheckAndChangePosisionInList()
        {
            if (IsRandom)
            {
                CurrentFileIndex = RandomIndex.Next(0, DisplayList.Count);
            }
            else
            {
                if (IsLooping)
                {
                    if (MovementValue > 0)
                    {
                        CurrentFileIndex += MovementValue;
                        if (CurrentFileIndex > DisplayList.Count - 1)
                        {
                            CurrentFileIndex = 0;
                        }
                    }
                    else if (MovementValue < 0)
                    {
                        CurrentFileIndex += MovementValue;

                        if (CurrentFileIndex < 0)
                        {
                            CurrentFileIndex = DisplayList.Count - 1;
                        }
                    }
                }
                else
                {
                    if (MovementValue > 0)
                    {
                        CurrentFileIndex += MovementValue;
                        if (CurrentFileIndex > DisplayList.Count - 1 && SelectedChapterIndex < Chapters.Count - 1)
                        {
                            SelectedChapterIndex++;
                        }
                        else if (CurrentFileIndex > DisplayList.Count - 1 && SelectedChapterIndex == Chapters.Count - 1)
                        {
                            selectedChapterIndex = 1;
                        }
                    }
                    else if (MovementValue < 0)
                    {
                        CurrentFileIndex += MovementValue;

                        if (CurrentFileIndex < 0)
                        {
                            SelectedChapterIndex--;
                            CurrentFileIndex = DisplayList.Count - 1;
                            if (SelectedChapterIndex < 0)
                            {
                                SelectedChapterIndex = Chapters.Count - 1;
                            }
                            else if (SelectedChapterIndex == 0)
                            {
                                SelectedChapterIndex = Chapters.Count - 1;
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

            if (SelectedChapter == "All")
            {
                DisplayList = new ObservableCollection<Word>(Words);
                return;
            }

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

            StackPanel stackPnl1 = new StackPanel();

            img1.Width = 32;

            stackPnl1.Children.Add(img1);

            btn_Speaker_Page1.Content = stackPnl1;
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
        /// Changes to the next chapter in the list
        /// </summary>
        /// <typeparam name="T">What type to use</typeparam>
        /// <param name="list">What list to use</param>
        private void NextChapter(List<Word> list)
        {
            if (CurrentFileIndex >= list.Count)
            {
                SelectedChapterIndex++;
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
            MovementValue = 1;
            PreTextChanger();
            if (AutoPlay)
            {
                PlaySound();
            }
        }

        /// <summary>
        /// fills the description textbox with the selected information.
        /// </summary>
        /// <param name="textBlockDescription">What textblock to use</param>
        private void PopulateDescription()
        {
            if (HasDescription)
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
            }
        }

        /// <summary>
        /// Checks what tab the user is on and updates the data on the current tab.
        /// </summary>
        /// <param name="change">how many places the index is going to change</param>
        private void PreTextChanger()
        {
            if (TabIndex == 0)
            {
                TextChanger();
                CurrentWord = "Current File in list: " + (CurrentFileIndex + 1);
                SpeakerStatus();
            }
            //else if (TabIndex == 1)
            //{
            //    TextChanger();
            //    lbl_Counter_Page2.Content = CurrentFileIndex;
            //    SpeakerStatus();
            //}
        }

        /// <summary>
        /// goes to the previous word in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrevWord(object sender, RoutedEventArgs e)
        {
            ClearFields();
            MovementValue = -1;
            PreTextChanger();
            if (AutoPlay)
            {
                PlaySound();
            }
        }

        /// <summary>
        /// Selects what property the user is going to practice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectWhatToPractice(object sender, RoutedEventArgs e)
        {
            if (TrainScript)
            {
                WhatToTrain = "ThaiScript";
                WhatToDisplay = "EngWords";
            }
            else if (TrainFonet)
            {
                WhatToTrain = "ThaiFonet";
                WhatToDisplay = "ThaiScript";
            }
            else if (TrainWords)
            {
                WhatToTrain = "EngWords";
                WhatToDisplay = "ThaiScript";
            }
            else
            {
                MessageBox.Show("Please select what to train", "ERROR");
                return;
            }

            MovementValue = 0;
            if (SelectedChapter != null)
            {
                PreTextChanger();
            }
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

        private bool CheckIfUserHasCompleted(List<CompletedWord> list, Word wordTwo, bool justCheckWord)
        {
            foreach (CompletedWord wordOne in list)
            {
                if (wordOne.word.ThaiScript_String == wordTwo.ThaiScript_String)
                {
                    if (wordOne.word.ThaiFonet_String == wordTwo.ThaiFonet_String)
                    {
                        if (wordOne.word.EngWords_String == wordTwo.EngWords_String)
                        {
                            if (wordOne.word.EngDesc == wordTwo.EngDesc)
                            {
                                if (wordOne.word.Chapter == wordTwo.Chapter)
                                {
                                    if (justCheckWord)
                                    {
                                        return true;
                                    }

                                    if (WhatToTrain == "ThaiFonet")
                                    {
                                        return wordOne.foneticCompleted;
                                    }
                                    else if (WhatToTrain == "ThaiScript")
                                    {
                                        return wordOne.scriptCompleted;
                                    }
                                    else if (WhatToTrain == "EngWords")
                                    {
                                        return wordOne.meaningCompleted;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        private bool CheckIfCompleted()
        {
            if (CheckIfUserHasCompleted(currentUser.CompletedWords, DisplayList[CurrentFileIndex], false))
            {
                if (movementValue == 0)
                {
                    CurrentFileIndex++;
                }
                else if (movementValue == -1 && CurrentFileIndex < 0)
                {
                    //movementValue = 0;
                    if (IsLooping)
                    {
                        CurrentFileIndex = DisplayList.Count - 1;
                    }
                    else
                    {
                        SelectedChapterIndex--;
                        CurrentFileIndex = DisplayList.Count - 1;
                    }
                }

                return true;
            }
            return false;
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
        private void TextChanger()
        {
            //if (DisplayList.Count == 0)
            //{
            //    return;
            //}

            try
            {
                if (SkipCompleted)
                {
                    if (CheckIfChapterIsDone())
                    {
                        if (MovementValue >= 0)
                        {
                            SelectedChapterIndex++;
                            if (SelectedChapterIndex >= Chapters.Count)
                            {
                                SelectedChapterIndex = 1;
                            }
                        }
                        else if (MovementValue < 0)
                        {
                            if (SelectedChapterIndex <= 1)
                            {
                                SelectedChapterIndex = Chapters.Count - 1;
                            }
                            else
                            {
                                SelectedChapterIndex--;
                            }
                        }

                        return;
                    }
                }

                CheckAndChangePosisionInList();

                if (SkipCompleted)
                {
                    if (DisplayList.Count != 0 && CheckIfCompleted())
                    {
                        TextChanger();
                    }
                }

                if (DisplayList.Count == 0)
                {
                    return;
                }

                SetPropertyOfGenericObject(DisplayList[CurrentFileIndex]);

                SelectedPropertyToDisplay = GetValueFromValueList(WhatToDisplay);

                string retrivedChapter = (string)GetValueFromValueList("Chapter", SelectedChapter);

                if (retrivedChapter == SelectedChapter)
                {
                    if (SelectedPropertyToDisplay is List<string> propertyIsList)
                    {
                        ThaiScriptString = ListToString(propertyIsList);
                    }
                    else
                    {
                        ThaiScriptString = (string)SelectedPropertyToDisplay;
                    }
                }
                else
                {
                    Dispatcher.BeginInvoke(new Action(() => MessageBox.Show("There are no content with chapter" + SelectedChapter + " available right now.")));
                    return;
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
                ValidateAnswer();
            }
        }

        private int FindIndexInCompletedWords(List<CompletedWord> list, Word wordTwo)
        {
            foreach (CompletedWord wordOne in list)
            {
                if (wordOne.word.ThaiScript_String == wordTwo.ThaiScript_String)
                {
                    if (wordOne.word.ThaiFonet_String == wordTwo.ThaiFonet_String)
                    {
                        if (wordOne.word.EngWords_String == wordTwo.EngWords_String)
                        {
                            if (wordOne.word.EngDesc == wordTwo.EngDesc)
                            {
                                if (wordOne.word.Chapter == wordTwo.Chapter)
                                {
                                    return list.IndexOf(wordOne);
                                }
                            }
                        }
                    }
                }
            }
            return -1;
        }

        private void AddWordToCompleted(Word word)
        {
            CompletedWord newCompleted = new CompletedWord
            {
                word = DisplayList[CurrentFileIndex]
            };
            currentUser.CompletedWords.Add(newCompleted);
            int index = FindIndexInCompletedWords(currentUser.CompletedWords, word);
            if (WhatToTrain == "ThaiFonet")
            {
                currentUser.CompletedWords[index].foneticCompleted = true;
            }
            else if (WhatToTrain == "ThaiScript")
            {
                currentUser.CompletedWords[index].scriptCompleted = true;
            }
            else if (WhatToTrain == "EngWords")
            {
                currentUser.CompletedWords[index].meaningCompleted = true;
            }
            SaveFiles<User>(Users, "Users");
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
        private void ValidateAnswer()
        {
            Word word = DisplayList[currentFileIndex];
            SetPropertyOfGenericObject(word);

            SelectedPropertyToValidate = GetValueFromValueList(WhatToTrain);

            int rightAnswears = 0;
            int totalAnswears = 0;
            int indexToRemove = -1;
            bool removeCorrect = false;
            if (string.IsNullOrEmpty(Answear))
            {
                MessageBox.Show("Please enter an answear.");
            }
            else
            {
                List<string> answers = Regex.Split(Answear, RegexSplitString).ToList();
                if (SelectedPropertyToValidate is List<string>)
                {
                    totalAnswears = ((List<string>)SelectedPropertyToValidate).Count;
                    foreach (string correctWord in SelectedPropertyToValidate as List<string>)
                    {
                        if (removeCorrect)
                        {
                            answers.RemoveAt(indexToRemove);
                        }
                        foreach (string answer in answers)
                        {
                            if (string.Equals(correctWord, answer, StringComparison.OrdinalIgnoreCase))
                            {
                                removeCorrect = true;
                                indexToRemove = answers.IndexOf(answer);
                                correctPoints++;
                                rightAnswears++;
                                if (rightAnswears == totalAnswears)
                                {
                                    if (!CheckIfUserHasCompleted(currentUser.CompletedWords, word, true))
                                    {
                                        AddWordToCompleted(word);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                removeCorrect = false;
                            }
                        }
                    }
                }
                else if (string.Equals(Answear, (string)SelectedPropertyToValidate, StringComparison.OrdinalIgnoreCase))
                {
                    totalAnswears = 1;
                    correctPoints++;
                    rightAnswears++;

                    if (!CheckIfUserHasCompleted(currentUser.CompletedWords, word, true))
                    {
                        AddWordToCompleted(word);
                    }
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

                lbl_Points.Content = "Points: " + CorrectPoints;
            }
        }

        #endregion Main

        #region Settings

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

        /// <summary>
        /// Clears the textboxs and textblocks on the current tab
        /// </summary>
        private void ClearFields()
        {
            Result = "";
            Answear = "";
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
                HasDescription = true;
                PopulateDescription();
                PopulateDescription();
            }
            else
            {
                DescriptionString = "";
                DescriptionString = "";
                HasDescription = false;
            }
        }

        /// <summary>
        /// Loads all the files from storage
        /// </summary>
        private void LoadAllFiles()
        {
            LoadFiles<Chapter>(Chapters, "Thai_Chapter");

            LoadFiles<Word>(words, "Thai_Word");

            CheckForUsers();
        }

        private void CheckForUsers()
        {
            LoadFiles<User>(Users, "Users");

            currentUser = Users.First();
            if (currentUser == null)
            {
                currentUser = new User
                {
                    UserName = "Default",
                    CompletedWords = new List<CompletedWord>()
                };
                Users.Add(currentUser);
                SaveFiles<User>(Users, "Users");
            }
        }

        /// <summary>
        /// Load files to lists.
        /// </summary>
        /// <typeparam name="T">What type to load</typeparam>
        /// <param name="list">What list to load into</param>
        private void LoadFiles<T>(ObservableCollection<T> list, string fileName) where T : new()
        {
            if (list.Count > 0)
            {
                list.Clear();
            }
            Type whatIsT = typeof(T);

            List<T> wordsFromFIle = XmlSerialization.ReadFromXmlFile<List<T>>(LanguageFilePath + fileName + ".xml");

            List<T> newWordToAdd = new List<T>();

            foreach (T wordFoundInFile in wordsFromFIle)
            {
                list.Add(wordFoundInFile);
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void LoadSettings()
        {
            if (File.Exists(SettingsFilePath + "settings.xml"))
            {
                settings = XmlSerialization.ReadFromXmlFile<UserSetting>(SettingsFilePath + "Settings.xml");
            }
        }

        /// <summary>
        /// Sets loopChapter to true or false and allows the user to continiue to the next chaper automaticaly at the end of the last chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopChapter_Checked(object sender, RoutedEventArgs e)
        {
            if (IsLooping)
            {
                IsRandom = false;
            }
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
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (TabIndex == 0 || TabIndex == 1)
                {
                    ValidateAnswear(sender, e);
                }
            }
            else if (e.Key == Key.Right)
            {
                ClearFields();
                MovementValue = 1;
                PreTextChanger();
                if (AutoPlay)
                {
                    PlaySound();
                }
            }
            else if (e.Key == Key.Left)
            {
                ClearFields();
                MovementValue = -1;
                PreTextChanger();
                if (AutoPlay)
                {
                    PlaySound();
                }
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
                ClearFields();

                ResetChapter();
                if (TabIndex == 0)
                {
                    MovementValue = 0;
                    PreTextChanger();
                }
                else if (MainWindow_tabController.SelectedIndex == 3)
                {
                    DisplayList = new ObservableCollection<Word>(Words);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintAllWordsToFile(object sender, RoutedEventArgs e)
        {
            try
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
                if (!Directory.Exists(DebugFilePath))
                {
                    Directory.CreateDirectory(DebugFilePath);
                }
                if (!File.Exists(DebugFilePath + "File.txt"))
                {
                    File.Create(DebugFilePath + "File.txt");
                }
                File.WriteAllText(DebugFilePath + "File.txt", fullText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Check if random number is on, if on randomly jump around in the chapter. also disables loop chapter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Randomized_Checked(object sender, RoutedEventArgs e)
        {
            if (IsRandom)
            {
                IsLooping = false;
            }
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
        private void ResetChapter()
        {
            if (TabIndex == 0)
            {
                SelectedChapterIndex = 0;
                if (Chapters.Count != 0)
                {
                    SelectedChapter = Chapters[SelectedChapterIndex].ChapterName;
                    if (SelectedChapter == "All")
                    {
                        selectedChapterIndex++;
                        SelectedChapter = Chapters[selectedChapterIndex].ChapterName;
                    }
                    else
                    {
                        SelectedChapter = Chapters[SelectedChapterIndex].ChapterName;
                    }
                }
            }
            else if (TabIndex == 1)
            {
                SelectedChapterIndex = 0;
                if (SelectedChapter == "All")
                {
                    SelectedChapter = Chapters[selectedChapterIndex++].ChapterName;
                }
                else
                {
                    SelectedChapter = Chapters[SelectedChapterIndex].ChapterName;
                }
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
                SaveFiles<Word>(Words, "Thai_Word");

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
        private void SaveFiles<T>(ObservableCollection<T> sourceList, string fileName) where T : new()
        {
            Type whatIsT = typeof(T);

            List<T> list = new List<T>(sourceList);

            XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + fileName + ".xml", list, false);
            //XmlSerialization.WriteToXmlFile<List<T>>(LanguageFilePath + "Thai_" + whatIsT.Name + ".xml", list, false);
        }

        /// <summary>
        ///
        /// </summary>
        private void SaveSetting()
        {
            XmlSerialization.WriteToXmlFile<UserSetting>(SettingsFilePath + "Settings.xml", settings, false);
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
                CurrentWord = "Current File in list: " + (CurrentFileIndex + 1);

                DisplayList = new ObservableCollection<Word>(Words);

                if (WhatToTrain == "ThaiFonet")
                {
                    TrainFonet = true;
                }
                else if (WhatToTrain == "ThaiScript")
                {
                    TrainScript = true;
                }
                else if (WhatToTrain == "EngWords")
                {
                    TrainWords = true;
                }

                PopulateManageChapterCB();

                //cb_SymbolChapters.SelectedIndex = 0;

                SelectedChapterIndex = 0;
                ChapterCounter = "Words in chapter: " + DisplayList.Count.ToString();
            }

            cb_SelectList.DisplayMemberPath = "ChapterName";
        }

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
        }

        #endregion Settings

        #region Submit

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DG_ConMan_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                try
                {
                    if (SelectedWordDG == null)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select the full row you want to delete");
                    }
                    else if (MessageBox.Show("Do you really want to delete the word " + SelectedWordDG.ThaiScript_String + " ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Words.Remove(SelectedWordDG);
                        SaveAll();
                        if (string.IsNullOrEmpty(searchString))
                        {
                            DisplayList = Words;
                        }
                        else
                        {
                            Search();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        private void PopulateManageChapterCB()
        {
            cb_SelectList.ItemsSource = null;

            cb_SelectList.ItemsSource = Chapters;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (e.Row.Item != null)
            {
                if (!Words.Contains(e.Row.Item))
                {
                    Words.Add((Word)e.Row.Item);
                }
                SaveAll();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seach_Clicked(object sender, RoutedEventArgs e)
        {
            Search();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Seach_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchValue"></param>
        private void Search()
        {
            DisplayList = new ObservableCollection<Word>(words);

            ObservableCollection<Word> searchResults = new ObservableCollection<Word>();

            List<Word> listToSeach = new List<Word>(DisplayList);

            try
            {
                foreach (Word word in listToSeach)
                {
                    SetPropertyOfGenericObject(word);

                    foreach (var value in ListOfValues)
                    {
                        if (value == null)
                        {
                            continue;
                        }
                        if (value is List<string> sublist)
                        {
                            foreach (string s in sublist)
                            {
                                if (s.CaseInsensitiveContains(searchString))
                                {
                                    if (!searchResults.Contains(word))
                                    {
                                        searchResults.Add(word);
                                        continue;
                                    }
                                }
                            }
                        }
                        else if (((string)value).CaseInsensitiveContains(searchString))
                        {
                            if (!searchResults.Contains(word))
                            {
                                searchResults.Add(word);
                                continue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisplayList = searchResults;
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

        #endregion Submit

        #region Sound

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
                fullText = CheckSoundStatus(new ObservableCollection<Word>(words));
            }
            else
            {
                fullText = CheckSoundStatus(DisplayList);
            }

            MessageBox.Show(fullText);
        }

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

        /// <summary>
        /// Cycles the list to find all words that have and dont have sounds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CheckSoundStatus(ObservableCollection<Word> list)
        {
            int hasSound = 0;
            int dosntHaveSound = 0;
            string fullText = "";

            foreach (Word word in list)
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

            fullText += hasSound + " " + typeof(Word).Name + " has sound, and " + dosntHaveSound + " dont. \r\n";
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

            List<string> correctText = new List<string>();

            List<string> soundPathList = new List<string>();

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

        private void PlaySound()
        {
            BackgroundWorker worker = new BackgroundWorker
            {
                WorkerReportsProgress = true
            };
            worker.DoWork += Worker_PlaySoundFile;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Plays the sound from the soundpath of the current word
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaySound_Clicked(object sender, RoutedEventArgs e)
        {
            PlaySound();
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
            word.SoundPath.Clear();

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
        /// Checks if the current word has a sound, and if not turns the button red.
        /// </summary>
        private void SpeakerStatus()
        {
            try
            {
                List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");

                if (soundPaths.Count == 0)
                {
                    if (TabIndex == 0)
                    {
                        btn_Speaker_Page1.Background = Brushes.Red;
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
                    }
                    else
                    {
                        var bc = new BrushConverter();
                        if (TabIndex == 0)
                        {
                            btn_Speaker_Page1.Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                    //sre_RecognizeFromSoundFile(soundPath);
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSoundPaths_DoWork(object sender, DoWorkEventArgs e)
        {
            double currentIndex;

            foreach (Word word in Words)
            {
                SetSoundPathToWord(word);

                currentIndex = Words.IndexOf(word);
                ProgressValue = (currentIndex / Words.Count) * 100;
            }
            ProgressValue = 100;
            SaveAll();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 99.00)
            {
                Console.WriteLine("break");
            }
        }

        #endregion Async

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

        private void sre_RecognizeFromSoundFile(string path)
        {
            sre.RecognizeAsyncCancel();

            sre.SetInputToAudioStream(File.OpenRead(path), new SpeechAudioFormatInfo(12000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

            sre.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void sre_RecognizedCompleted(object sender, RecognizeCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Result != null)
            {
                MessageBox.Show("Results: \r\n"
                                + "Words: " + e.Result.Words + "\r\n"
                                + "Text: " + e.Result.Text + "\r\n"
                                + "Semantics: " + e.Result.Semantics + "\r\n"
                                + "Replacement Words units: " + e.Result.ReplacementWordUnits + "\r\n"
                                + "Homophones: " + e.Result.Homophones + "\r\n"
                                + "Grammar: " + e.Result.Grammar + "\r\n");
            }
        }

        private void ClearCurrentUserKnownWords(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you really want remove all known words from your profile?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                currentUser.CompletedWords.Clear();
                SaveFiles<User>(Users, "Users");
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                ClearFields();
                MovementValue = 1;
                PreTextChanger();
            }
            else if (e.Key == Key.Left)
            {
                ClearFields();
                MovementValue = -1;
                PreTextChanger();
            }
        }

        private void SearchInputChanged(object sender, TextChangedEventArgs e)
        {
            //Search();
            //((TextBox)sender).Focus();
        }

        private void SetupSpeech()
        {
            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += sre_SpeechRecogniced;
            Grammar g_HelloGoodbye = GetHelloGoodbyeGrammar();
            Grammar g_SetTextBox = GetTextBoxGrammar();
            sre.LoadGrammarAsync(g_HelloGoodbye);
            sre.LoadGrammarAsync(g_SetTextBox);
        }

        private Grammar GetTextBoxGrammar()
        {
            throw new NotImplementedException();
        }

        private Grammar GetHelloGoodbyeGrammar()
        {
            Choices ch_HelloGoodbye = new Choices();
            ch_HelloGoodbye.Add("Hello");
            ch_HelloGoodbye.Add("Goodbye");
            GrammarBuilder gb_result = new GrammarBuilder(ch_HelloGoodbye);
            Grammar g_result = new Grammar(gb_result);
            return g_result;
        }

        private Grammar GetAllWordsGrammar()
        {
            Choices ch_allChoices = new Choices();

            foreach (Word word in DisplayList)
            {
                if (!string.IsNullOrEmpty(word.ThaiFonet_String))
                {
                    ch_allChoices.Add(word.ThaiFonet_String.Replace(";", ""));
                }
            }

            GrammarBuilder gb_result = new GrammarBuilder(ch_allChoices);
            Grammar g_result = new Grammar(gb_result);
            return g_result;
        }

        private void sre_SpeechRecogniced(object sender, SpeechRecognizedEventArgs e)
        {
            string text = e.Result.Text;
            string procounc = "";
            foreach (var recognizedWord in e.Result.Words)
            {
                procounc += recognizedWord.Pronunciation + " ";
            }

            float conf = e.Result.Confidence;
            ss.SpeakAsync(text);
            SpeechResult = text + "\r\n " + procounc + " " + conf.ToString();
        }

        private void sre_SpeechRecognitationRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            string text = e.Result.Text;
            float conf = e.Result.Confidence;
            SpeechResult = text + " " + conf.ToString() + "!?";
        }

        private void sre_SpeechHypotized(object sender, SpeechHypothesizedEventArgs e)
        {
            string text = e.Result.Text;
            float conf = e.Result.Confidence;
            SpeechResult = text + " " + conf.ToString() + "?";
        }

        private void Listen_Checked(object sender, RoutedEventArgs e)
        {
            if (IsListening == true)
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            else if (IsListening == false)
            {
                sre.RecognizeAsyncCancel();
            }
        }

        private void TestWordReco(object sender, RoutedEventArgs e)
        {
            try
            {
                sre.RecognizeAsyncStop();
                sre.EmulateRecognizeAsync(DisplayList[CurrentFileIndex].ThaiFonet_String.Replace(";", ""));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}