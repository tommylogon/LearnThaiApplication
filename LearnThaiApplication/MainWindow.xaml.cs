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
using Microsoft.WindowsAPICodePack.Dialogs;

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

            
            LoadAllFiles();
            SetInitialStates();
            GetImage();
            words.CollectionChanged += ContentCollectionChanged;
            displayList.CollectionChanged += ContentCollectionChanged;
            SetupSpeech();
            //chapters.CollectionChanged += ChaptersCollectionChanged;
        }

        #region Variables and properties

        #region lists

        private ObservableCollection<Chapter> chapters = new ObservableCollection<Chapter>();
        private ObservableCollection<Word> displayList = new ObservableCollection<Word>();
        private ObservableCollection<Word> searchResults = new ObservableCollection<Word>();
        private List<UserSetting> UserSettings = new List<UserSetting>();
        private ObservableCollection<Word> words = new ObservableCollection<Word>();
        private List<InstalledVoice> voices = new List<InstalledVoice>();
        private List<MediaPlayer> players = new List<MediaPlayer>();
        private ObservableCollection<User> users = new ObservableCollection<User>();
        public ObservableCollection<Word> SearchResults
        {
            get
            {
                return searchResults;
            }
            set
            {
                if (searchResults != value)
                {
                    searchResults = value;
                    OnPropertyChanged("SearchResults");
                }
            }
        }

        public List<InstalledVoice> Voices
        {
            get
            {
                return voices;
            }
            set
            {
                if (voices != value)
                {
                    voices = value;
                    OnPropertyChanged("Voices");
                }
            }
        }

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
        private bool testWordReco = false;

        public bool IsListening
        {
            get
            {
                return Properties.Settings.Default.ActivateSpeechRecognition;
            }
            set
            {
                if (Properties.Settings.Default.ActivateSpeechRecognition != value)
                {
                    Properties.Settings.Default.ActivateSpeechRecognition = value;
                    OnPropertyChanged("IsListening");
                    Properties.Settings.Default.Save();
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
                return Properties.Settings.Default.ShowSaveLocation;
            }
            set
            {
                if (Properties.Settings.Default.ShowSaveLocation != value)
                {
                    Properties.Settings.Default.ShowSaveLocation = value;
                    OnPropertyChanged("ShowSaveLocation");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool DisplayAll
        {
            get
            {
                return Properties.Settings.Default.DisplayAllPropertiesInDescription;
            }
            set
            {
                if (Properties.Settings.Default.DisplayAllPropertiesInDescription != value)
                {
                    Properties.Settings.Default.DisplayAllPropertiesInDescription = value;
                    HasDescription = value;
                    OnPropertyChanged("DisplayAll");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool HasDescription
        {
            get
            {
                return Properties.Settings.Default.DescriptionOn;
            }
            set
            {
                if (Properties.Settings.Default.DescriptionOn != value)
                {
                    Properties.Settings.Default.DescriptionOn = value;
                    OnPropertyChanged("HasDescription");
                    Properties.Settings.Default.Save();
                    
                }
            }
        }

        public bool IsLooping
        {
            get
            {
                return Properties.Settings.Default.IsLooping;
            }
            set
            {
                if (Properties.Settings.Default.IsLooping != value)
                {
                    Properties.Settings.Default.IsLooping = value;
                    OnPropertyChanged("IsLooping");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool IsRandom
        {
            get
            {
                return Properties.Settings.Default.RandomOn;
            }
            set
            {
                if (Properties.Settings.Default.RandomOn != value)
                {
                    Properties.Settings.Default.RandomOn = value;
                    OnPropertyChanged("IsRandom");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool AutoPlay
        {
            get
            {
                return Properties.Settings.Default.AutoPlaySounds;
            }
            set
            {
                if (Properties.Settings.Default.AutoPlaySounds != value)
                {
                    Properties.Settings.Default.AutoPlaySounds = value;
                    OnPropertyChanged("AutoPlay");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool SkipCompleted
        {
            get
            {
                return Properties.Settings.Default.SkipCompletedWords;
            }
            set
            {
                if (Properties.Settings.Default.SkipCompletedWords != value)
                {
                    Properties.Settings.Default.SkipCompletedWords = value;
                    OnPropertyChanged("SkipCompleted");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public bool SkipIntro
        {
            get
            {
                return Properties.Settings.Default.SkipIntro;
            }
            set
            {
                if (Properties.Settings.Default.SkipIntro != value)
                {
                    Properties.Settings.Default.SkipIntro = value;
                    OnPropertyChanged("SkipIntro");
                    Properties.Settings.Default.Save();
                }
            }
        }

        #endregion bools

        #region strings

        private static string RegexSplitString = @" ^\s|[\s;,]{2,}";
        private string answear;
        private string DebugFilePath = Environment.CurrentDirectory + @"\Files\Settings\DEBUG\";
        private string ImageFilePath = Environment.CurrentDirectory + @"\Files\Media\Icon\";
        private string languageFilePath = Environment.CurrentDirectory + @"\Files\Media\Language\";
        private string webFilePath = Environment.CurrentDirectory + @"\Files\Media\Website\";
        
        private string soundFilePath = Environment.CurrentDirectory + @"\Files\Media\Sound\";
        private string correctPointsText;
        private string descriptionText;
        private string result;
        private string searchString;
        private string SelectedChapter;
        private string thaiScript_String;
        private string trainWhatChoosen;
        private string currentWord;
        private string chapterCounter;
        private string speechResult;

        public string WebFilePath
        {
            get
            {
                return Properties.Settings.Default.WebFilePath;
            }
            set
            {
                if (Properties.Settings.Default.WebFilePath != value)
                {
                    Properties.Settings.Default.WebFilePath = value;
                    OnPropertyChanged("WebFilePath");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public string LanguageFilePath
        {
            get
            {
                return Properties.Settings.Default.LanguageFilePath;
            }
            set
            {
                if (Properties.Settings.Default.LanguageFilePath != value)
                {
                    Properties.Settings.Default.LanguageFilePath = value;
                    OnPropertyChanged("LanguageFilePath");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public string SoundFilePath
        {
            get
            {
                return Properties.Settings.Default.SoundFilePath;
            }
            set
            {
                if (Properties.Settings.Default.SoundFilePath != value)
                {
                    Properties.Settings.Default.SoundFilePath = value;
                    OnPropertyChanged("SoundFilePath");
                    Properties.Settings.Default.Save();
                }
            }
        }
        public string SelectedVoice
        {
            get
            {
                return Properties.Settings.Default.SelectedVoice;
            }
            set
            {
                if (Properties.Settings.Default.SelectedVoice != value)
                {
                    Properties.Settings.Default.SelectedVoice = value;
                    OnPropertyChanged("SelectedVoice");
                    if (ss != null)
                    {
                        ss.SelectVoice(Properties.Settings.Default.SelectedVoice);
                    }
                    Properties.Settings.Default.Save();
                }
            }
        }

        public string WhatToTrain
        {
            get
            {
                return Properties.Settings.Default.WhatToTrain;
            }
            set
            {
                if (Properties.Settings.Default.WhatToTrain != value)
                {
                    Properties.Settings.Default.WhatToTrain = value;
                    OnPropertyChanged("WhatToTrain");
                    Properties.Settings.Default.Save();
                }
            }
        }

        public string WhatToDisplay
        {
            get
            {
                return Properties.Settings.Default.WhatToDisplay;
            }
            set
            {
                if (Properties.Settings.Default.WhatToDisplay != value)
                {
                    Properties.Settings.Default.WhatToDisplay = value;
                    OnPropertyChanged("WhatToDisplay");
                    Properties.Settings.Default.Save();
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
                return correctPointsText;
            }
            set
            {
                if (correctPointsText != value)
                {
                    correctPointsText = value;
                    OnPropertyChanged("CorrectPoints");
                }
                    
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

        public int W_Width
        {
            get
            {
                if (Properties.Settings.Default.Window_Width < 600 | Properties.Settings.Default.Window_Width > 1500)
                {
                    Properties.Settings.Default.Window_Width = 600;
                }
                return Properties.Settings.Default.Window_Width;
            }
            set
            {
                if(Properties.Settings.Default.Window_Width != value & value > 600)
                {
                    
                    Properties.Settings.Default.Window_Width = value;
                    OnPropertyChanged("W_Width");
                   
                    Properties.Settings.Default.Save();

                }
            }
        }
        public int W_Height
        {
            get
            {
                if(Properties.Settings.Default.Window_Height < 400 | Properties.Settings.Default.Window_Height > 1500)
                {
                    Properties.Settings.Default.Window_Height = 400;
                }
                return Properties.Settings.Default.Window_Height;
            }
            set
            {
                if (Properties.Settings.Default.Window_Height != value & value > 400)
                {
                    double height = value;
                    double witdh = height * 0.5625;
                    
                    Properties.Settings.Default.Window_Height = value;
                    OnPropertyChanged("W_Height");
                    //W_Width = (int)witdh;
                    Properties.Settings.Default.Save();

                }
            }
        }


        #endregion ints

        #region objects

        private User currentUser;
        public MainWindow AppWindow;
        MediaPlayer player = new MediaPlayer();
        private int selectedManagementIndex;
        private double progressValue;
        private static CultureInfo ci;
        private SpeechRecognitionEngine sre;
        private SpeechSynthesizer ss;

        public InstalledVoice CurrentVoice
        {
            get
            {
                return Voices.First(v => v.VoiceInfo.Name == SelectedVoice);
            }
        }

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

        public int ManagementIndex
        {
            get
            {
                return selectedManagementIndex;
            }
            set
            {
                if (selectedManagementIndex != value)
                {
                    selectedManagementIndex = value;
                    OnPropertyChanged("ManagementIndex");
                }
            }
        }

        #endregion objects

        #region others

        

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
            correctPoints=0;
            SelectedChapter = Chapters[SelectedChapterIndex].ChapterName;

            FindWordWithChapter();
            try
            {
                Grammar g_AllInList = GetGrammarFromDisplayList();
                sre.UnloadAllGrammars();
                sre.LoadGrammarAsync(g_AllInList);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            
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
            correctPoints = 0;
            CorrectPoints = "Points: " + correctPoints;
            foreach (Word word in DisplayList)
            {
                if (CheckIfUserHasCompleted(currentUser.CompletedWords, word, false))
                {
                    counter++;
                    correctPoints++;
                    CorrectPoints = "Points: " + correctPoints;
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

            //if (DisplayList.Count() != 0)
            //{
            //    Grammar g_AllInList = GetAllWordsGrammar();
            //    sre.UnloadAllGrammars();
            //    sre.LoadGrammarAsync(g_AllInList);
            //}
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
                    foreach (PropertyInfo prop in DisplayList[CurrentFileIndex].GetType().GetProperties())
                    {
                        if (prop.GetValue(DisplayList[CurrentFileIndex]) is List<string> x)
                        {
                            DescriptionString += ListToString(x) + "\r\n";
                        }
                        else
                        {
                            DescriptionString += prop.GetValue(DisplayList[CurrentFileIndex]) + "\r\n";
                        }
                    }
                }
                else
                {
                    DescriptionString = DisplayList[CurrentFileIndex].ThaiScript_String + "\r\n";
                    DescriptionString += DisplayList[CurrentFileIndex].ThaiFonet_String + "\r\n";
                    DescriptionString += DisplayList[CurrentFileIndex].EngWords_String + "\r\n";
                    DescriptionString += DisplayList[CurrentFileIndex].EngDesc;
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
            try
            {
                SpeechResult = "";
                FindWordWithChapter();

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

                if (DisplayList[CurrentFileIndex].Chapter == SelectedChapter)
                {
                    if (WhatToDisplay == "ThaiScript")
                    {
                        ThaiScriptString = DisplayList[CurrentFileIndex].ThaiScript_String;
                    }
                    else if (WhatToDisplay == "ThaiFonet")
                    {
                        ThaiScriptString = DisplayList[CurrentFileIndex].ThaiFonet_String;
                    }
                    else if (WhatToDisplay == "EngWords")
                    {
                        ThaiScriptString = DisplayList[CurrentFileIndex].EngWords_String;
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
                ValidateAnswer(Answear);
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

        private object GetValueFromWord()
        {
            if (TrainScript)
            {
                return DisplayList[CurrentFileIndex].ThaiScript;
            }
            else if (TrainFonet)
            {
                return DisplayList[CurrentFileIndex].ThaiFonet;
            }
            else if (TrainWords)
            {
                return DisplayList[CurrentFileIndex].EngWords;
            }
            return null;
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
        private void ValidateAnswer(string inputetAnswear)
        {
            int rightAnswears = 0;
            int totalAnswears = 0;
            int indexToRemove = -1;
            bool removeCorrect = false;
            try
            {
                if (string.IsNullOrEmpty(inputetAnswear))
                {
                    MessageBox.Show("Please enter an answear.");
                }
                else
                {
                    List<string> answers = Regex.Split(inputetAnswear, RegexSplitString).ToList();

                    if (GetValueFromWord() is List<string> allAnswears)
                    {
                        totalAnswears = allAnswears.Count();

                        foreach (string correctWord in allAnswears)
                        {
                            if (removeCorrect)
                            {
                                answers.RemoveAt(indexToRemove);
                                removeCorrect = false;
                            }
                            if(answers.Count() == 0)
                            {
                                break;
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
                                        if (!CheckIfUserHasCompleted(currentUser.CompletedWords, DisplayList[CurrentFileIndex], true))
                                        {
                                            AddWordToCompleted(DisplayList[CurrentFileIndex]);
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
                    else if (string.Equals(Answear, (string)GetValueFromWord(), StringComparison.OrdinalIgnoreCase))
                    {
                        totalAnswears = 1;
                        correctPoints++;
                        rightAnswears++;

                        if (!CheckIfUserHasCompleted(currentUser.CompletedWords, DisplayList[CurrentFileIndex], true))
                        {
                            AddWordToCompleted(DisplayList[CurrentFileIndex]);
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
                    CorrectPoints = "Points: " + correctPoints;


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error" + ex.Message);
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
            if (string.IsNullOrEmpty(LanguageFilePath))
            {
                LanguageFilePath = languageFilePath;
            }
            if (string.IsNullOrEmpty(SoundFilePath))
            {
                SoundFilePath = soundFilePath;
            }
            if (string.IsNullOrWhiteSpace(WebFilePath))
            {
                WebFilePath = WebFilePath;
            }

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

                //ResetChapter();
                if (TabIndex == 0)
                {
                    if (SelectedChapterIndex == 0)
                    {
                        SelectedChapterIndex = 1;
                    }
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Save();
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
                SearchString = "";

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
                    if (ManagementIndex == -1)
                    {
                        System.Windows.Forms.MessageBox.Show("Please select the full row you want to delete");
                    }
                    else if (MessageBox.Show("Do you really want to delete the word " + ((Word)dg_ContentManagement.SelectedItem).ThaiScript_String + " ?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Words.Remove(((Word)dg_ContentManagement.SelectedItem));
                        SaveAll();
                        if (string.IsNullOrEmpty(searchString))
                        {
                            DisplayList = Words;
                        }
                        else
                        {
                            StartSearchWorker();
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error: " + ex.Message, "Error");
                }
            }
        }

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
        private void Search_Clicked(object sender, RoutedEventArgs e)
        {
            if (SearchString != null)
            {
                txt_SearchBar.Background = Brushes.White;
                StartSearchWorker();
            }
            else
            {
                txt_SearchBar.Background = Brushes.Red;

                StartSearchWorker();
            }
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchString != null)
                {
                    txt_SearchBar.Background = Brushes.White;
                    StartSearchWorker();
                }
                else
                {
                    txt_SearchBar.Background = Brushes.Red;
                    SelectedChapterIndex = 0;
                    StartSearchWorker();
                }
            }
        }

        private void StartSearchWorker()
        {
            BackgroundWorker bgw_Searcher = new BackgroundWorker();
            bgw_Searcher.DoWork += SearchWorker_DoWork;
            bgw_Searcher.RunWorkerAsync();
        }

        private void SearchWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Search();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchValue"></param>
        private void Search()
        {

            try
            {
                if (string.IsNullOrEmpty(SearchString))
                {
                    SearchResults = Words;
                }
                else
                {
                    ObservableCollection<Word> result = new ObservableCollection<Word>();
                    SearchResults = result;
                    foreach (var word in Words)
                    {
                        foreach (var property in word.GetType().GetProperties())
                        {
                            var value = property.GetValue(word);

                            if (value != null)
                            {
                                value = value.ToString();
                                if (((string)value).CaseInsensitiveContains(SearchString))
                                {
                                    if (!result.Contains(word))
                                    {
                                        result.Add(word);
                                    }
                                }
                            }
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
                foreach (string path in word.SoundPath)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        dosntHaveSound++;
                    }
                    else
                    {
                        if (File.Exists(path))
                        {
                            hasSound++;
                        }
                        else
                        {
                            dosntHaveSound++;
                        }
                    }
                }
                if (word.SoundPath.Count == 0)
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
                            bool result = word.ThaiScript.Contains(correctText[listIndex]);

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
                List<string> soundPaths = DisplayList[CurrentFileIndex].SoundPath;

                if (soundPaths.Count == 0)
                {
                    
                        btn_Speaker_Page1.Background = Brushes.Red;
                    
                }
                foreach (string value in soundPaths)
                {
                    if (string.IsNullOrEmpty(value) || !File.Exists(value))
                    {
                        
                            btn_Speaker_Page1.Background = Brushes.Red;
                        
                    }
                    else
                    {
                        var bc = new BrushConverter();
                        
                            btn_Speaker_Page1.Background = Brushes.Azure;
                        
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
            Dispatcher.Invoke(() => player.Stop());
            

            try
            {

                foreach (String soundPath in DisplayList[CurrentFileIndex].SoundPath)
                {
                    //sre_RecognizeFromSoundFile(soundPath);
                    int waitTime = 1000 + (DisplayList[CurrentFileIndex].ThaiScript.Count * 50);

                    Dispatcher.Invoke(() => player.Open(new Uri(soundPath)));
                    Dispatcher.Invoke(() => player.Play());

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
                //MessageBox.Show("Results: \r\n"
                //                + "Words: " + e.Result.Words + "\r\n"
                //                + "Text: " + e.Result.Text + "\r\n"
                //                + "Semantics: " + e.Result.Semantics + "\r\n"
                //                + "Replacement Words units: " + e.Result.ReplacementWordUnits + "\r\n"
                //                + "Homophones: " + e.Result.Homophones + "\r\n"
                //                + "Grammar: " + e.Result.Grammar + "\r\n");
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
            ci = new CultureInfo("en-US");
            sre = new SpeechRecognitionEngine(ci);
            ss = new SpeechSynthesizer();

            sre.SetInputToDefaultAudioDevice();
            sre.SpeechRecognized += sre_SpeechRecogniced;
            sre.RecognizeCompleted += sre_RecognizedCompleted;
            sre.SpeechHypothesized += sre_SpeechHypotized;
            sre.EmulateRecognizeCompleted += Sre_EmulateRecognizeCompleted;
            voices = ss.GetInstalledVoices().ToList();

            if (string.IsNullOrEmpty(SelectedVoice) | !Voices.Any(v => v.VoiceInfo.Name == SelectedVoice))
            {
                SelectedVoice = voices.First(v => v.VoiceInfo.Name == "Microsoft Zira Desktop").VoiceInfo.Name;
            }
            ss.SelectVoice(SelectedVoice);
            ss.SetOutputToDefaultAudioDevice();
            if (IsListening)
            {
                if (ss.Voice.Culture.Name.CaseInsensitiveContains("en-"))
                {
                    ss.SpeakAsync("Sa wat dee krap");
                }
                else if (ss.Voice.Culture.Name.CaseInsensitiveContains("th-"))
                {
                    if (ss.Voice.Gender.ToString().CaseInsensitiveContains("male"))
                    {
                        ss.SpeakAsync("สวัสดีครับ");
                    }
                    else
                    {
                        ss.SpeakAsync("สวัสดีค่ะ");
                    }
                }
            }
            

            Grammar g_HelloGoodbye = GetHelloGoodbyeGrammar();

            Grammar g_AllInList = GetGrammarFromDisplayList();

            sre.LoadGrammarAsync(g_AllInList);
        }

        private void Sre_EmulateRecognizeCompleted(object sender, EmulateRecognizeCompletedEventArgs e)
        {
            if (testWordReco)
            {
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            testWordReco = false;
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

        private Grammar GetGrammarFromDisplayList()
        {
            try
            {
                Choices ch_allChoices = new Choices();

                foreach (Word word in DisplayList)
                {
                    if (!string.IsNullOrEmpty(word.ThaiFonet_String))
                    {
                        ch_allChoices.Add(word.ThaiFonet_String.Replace(";", ""));
                        ch_allChoices.Add(word.ThaiFonet_String);
                    }
                }

                GrammarBuilder gb_result = new GrammarBuilder(ch_allChoices);
                Grammar g_result = new Grammar(gb_result);
                return g_result;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return null;
            }
            
        }

        private void sre_SpeechRecogniced(object sender, SpeechRecognizedEventArgs e)
        {
            string text = "";
            string procounc = "";
            foreach (var recognizedWord in e.Result.Words)
            {
                text += recognizedWord.Text.Replace(";", " ") + " ";
                procounc += recognizedWord.Pronunciation + " ";
            }
            text = text.Trim();
            float conf = e.Result.Confidence;
            if (ss.Voice.Culture.Name.CaseInsensitiveContains("en-"))
            {
                ss.SpeakAsync(text);
            }
            else if (ss.Voice.Culture.Name == "th-TH")
            {
                ss.SpeakAsync(DisplayList[CurrentFileIndex].ThaiScript_String);
            }

            SpeechResult = text + "\r\n " + procounc + " " + conf.ToString();

            if (DisplayList[CurrentFileIndex].ThaiFonet_String.Replace(";", "").Trim() == text & !testWordReco)
            {
                ValidateAnswer(DisplayList[CurrentFileIndex].ThaiFonet_String);
            }
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
            SpeechResult = text + " " + conf.ToString() + " ?";
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
                testWordReco = true;
                sre.EmulateRecognizeAsync(DisplayList[CurrentFileIndex].ThaiFonet_String/*.Replace(";", "")*/);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cb_Voices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems != null)
            {
                SelectedVoice = ((InstalledVoice)e.AddedItems[0]).VoiceInfo.Name;
            }
        }

        private void Dg_ContentManagement_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //if (SearchResults.Count != 0)
            //{
            //    if (SearchResults.Last() == e.Row.Item)
            //    {
            //        dg_ContentManagement.CurrentCell = new DataGridCellInfo(dg_ContentManagement.Items[0], dg_ContentManagement.Columns[0]);
            //        dg_ContentManagement.SelectedCells.Clear();
            //        dg_ContentManagement.SelectedCells.Add(dg_ContentManagement.CurrentCell);
            //    }
            //}
        }

        private void LanguagePath_Clicked(object sender, RoutedEventArgs e)
        {
            SelectPath("Language");
        }

        private void SoundPath_Clicked(object sender, RoutedEventArgs e)
        {
            SelectPath("Sound");
        }

        private void WebFilePath_Clicked(object sender, RoutedEventArgs e)
        {
            SelectPath("Web");
        }
        private void SelectPath(string targetPath)
        {

            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (targetPath == "Language")
                    {
                        LanguageFilePath = dialog.FileName;
                    }
                else if (targetPath == "Sound")
                    {

                    }
                else if (targetPath == "Web")
                    {

                    }

            }
        }

        private void ClearWord_Clicked(object sender, RoutedEventArgs e)
        {
            AddWordToCompleted(DisplayList[CurrentFileIndex]);
            ClearFields();
            MovementValue = 1;
            PreTextChanger();
            correctPoints++;
            CorrectPoints = "Points: " + correctPoints;
        }
    }
}