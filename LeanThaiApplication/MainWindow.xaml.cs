using HtmlAgilityPack;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            WriteAllToFile();
        }

        public void LoadAllFiles()
        {
            LoadFiles<Chapter>(Chapters);

            LoadFiles<Word>(Words);

            LoadFiles<Consonant>(Consonants);

            LoadFiles<Vowel>(Vowels);

            LoadFiles<ThaiNumber>(Numbers);
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

        public string CheckSoundStatus<T>(List<T> list)
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
        /// Clears the textboxs and textblocks to make the application look clean.
        /// </summary>
        public void ClearFields(object sender)
        {
            int tabIndex = MainWindow_tabController.TabIndex; //SelectParentIndex(sender);

            if (tabIndex == 0)
            {
                txb_Description_page1.Text = "";
                txb_ThaiScript_Page1.Text = "";
                txb_Status_Page1.Text = "";
                txt_Answear_Page1.Text = "";
            }
            else if (tabIndex == 1)
            {
                txb_ThaiScript_Page2.Text = "";
                txb_Status_Page2.Text = "";
                txb_Description_page2.Text = "";
                txt_Answear_Page2.Text = "";
            }
            else if (tabIndex == 2)
            {
                if (ckb_AutoClean.IsChecked == true)
                {
                    txt_FirstSelectionProperty.Text = "";
                    txt_SecondSelectionProperty.Text = "";
                    txt_ThirdSelectionProperty.Text = "";
                    txt_FourthSelectionProperty.Text = "";
                    txt_FifthSelectionProperty.Text = "";
                }
            }
        }

        public void CreateFormWindow()
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
                txt.AcceptsReturn = true;

                i++;

                sp.Children.Add(txt);
            }

            Button submitButton = new Button
            {
                Content = "Submit",
                Name = "FormWindowButton"
            };
            submitButton.Click += Btn_SubmitNewWord_Click;
            sp.Children.Add(submitButton);
            wb.Child = sp;
            window.Content = wb;

            window.Show();
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
        /// Turns the induvidual words into one string to display
        /// </summary>
        /// <param name="list">The list to use</param>
        /// <returns>String of words</returns>
        public String ListToString(List<String> list)
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
            Image img1 = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\tommy\source\repos\LearnThaiApplication\Icons\Speaker.png"))
            };
            Image img2 = new Image
            {
                Source = new BitmapImage(new Uri(@"C:\Users\tommy\source\repos\LearnThaiApplication\Icons\Speaker.png"))
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

        public void PopulateDescription(TextBlock textBlockDescription)
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

        public bool SaveAll()
        {
            try
            {
                SaveFiles<Word>(Words);
                SaveFiles<Consonant>(Consonants);
                SaveFiles<Vowel>(Vowels);
                SaveFiles<ThaiNumber>(Numbers);
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

                FillFormTextBoxes();

                if (WordToLoad.GetType() == typeof(Word))
                {
                    txt_FirstSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiScript"));
                    txt_SecondSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiFonet"));
                    txt_ThirdSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("EngWords"));
                    txt_FourthSelectionProperty.Text = (String)GetValueFromValueList("EngDesc");
                    txt_FifthSelectionProperty.Text = (String)GetValueFromValueList("Chapter");
                    txb_Description_Page4.Text = (String)GetValueFromValueList("EngDesc");
                }
                else if (WordToLoad.GetType() == typeof(Consonant) || WordToLoad.GetType() == typeof(Vowel) || WordToLoad.GetType() == typeof(ThaiNumber))
                {
                    txt_FirstSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiScript"));
                    txt_SecondSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("ThaiFonet"));
                    txt_ThirdSelectionProperty.Text = (String)GetValueFromValueList("ThaiHelpWord");
                    txt_FourthSelectionProperty.Text = ListToString((List<String>)GetValueFromValueList("EngWords"));
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
            lbl_Counter_Page2.Content = CurrentFileIndex;
            lbl_Counter_Page1.Content = CurrentFileIndex;
            txb_FilePath_Settings.Text = LanguageFilePath;
            txt_NewSavePath_Settings.Text = LanguageFilePath;

            ckb_DescBox_Page1.IsChecked = true;
            ckb_DescBox_Page2.IsChecked = true;
            rb_SubmitNew.IsChecked = true;

            rb_TrainFonet_Page1.IsChecked = true;
            rb_Conson_Page2.IsChecked = true;
            cb_Chapter_Page1.SelectedIndex = 0;

            lib_LoadedWords.DisplayMemberPath = "Thaiscript";
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

        public void SetValueOfObject(object newValue, object propertyToChange, object objectToChange)
        {
            foreach (PropertyInfo prop in ListOfProperties)
            {
                if (prop.Name == (string)propertyToChange)
                {
                    if (prop.GetValue(objectToChange) is List<string> x)
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
        ///Sets the properties of the object it recives.
        /// </summary>
        /// <param name="recived">what object to find properties for</param>
        public void SetPropertyOfGenericObject(Object recived)
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

        public string CheckChildNode(HtmlNode tableData, bool isScript)
        {
            string attributeValue = "";

            foreach (HtmlNode tableDataChild in tableData.ChildNodes)
            {
                if (isScript)
                {
                    if (tableDataChild.InnerText.Contains("(") || tableDataChild.InnerText.Contains(")"))
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

        public bool TableDataAttributeExists(HtmlAttributeCollection attributesFromNode, string searchForName, string searchForValue)
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

        public bool SoundDownloadCompare<T>(List<string> correctText, List<string> soundID, List<T> list) where T : new()
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

                            if (result || correctText[listIndex] == (string)GetValueFromValueList("ThaiHelpWord"))
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

        public bool SoundDownloader(string correctText, string soundDownloadPath)
        {
            string savePath = @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\mp3\" + correctText + ".mp3";

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

        public bool SetSoundPathToWord(object word)
        {
            string soundPath = "";

            if (word.GetType() != typeof(Word))
            {
                var value = (string)GetValueFromValueList("ThaiHelpWord");
                soundPath = @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\mp3\" + value + ".mp3";
            }
            else
            {
                var listOfScript = (List<string>)GetValueFromValueList("ThaiScript");
                foreach (string value in listOfScript)
                {
                    soundPath = @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\mp3\" + value + ".mp3";
                }
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

        public void DownloadSoundProcess<T>(List<T> list, string url) where T : new()
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
                            if (!CompareOK)
                            {
                            }
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
                        ((ThaiBase)newWord).ThaiScript = SplitStringToList(txt.Text);
                    }
                    if (txt.Name == "txt_ThaiFonet")
                    {
                        ((ThaiBase)newWord).ThaiFonet = SplitStringToList(txt.Text);
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
                        ((ThaiBase)newWord).SoundPath = SplitStringToList(txt.Text);
                    }
                    Console.WriteLine(txt.Name);
                    Console.WriteLine(txt.Text);
                }

                list.Add((T)newWord);

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
                        SetNewValuesFromForm(oldWord, textboxList);
                        //SetNewValuesToOldWord(oldWord, whatIsT, txt_FirstSelectionProperty.Text, txt_SecondSelectionProperty.Text, txt_ThirdSelectionProperty.Text, txt_FourthSelectionProperty.Text, txt_FifthSelectionProperty.Text);
                        break;
                    }
                    else
                    {
                        SetNewValuesFromForm(oldWord, textboxList);
                        break;
                    }
                }
            }

            SaveFiles<T>(list);
        }

        public void SpeakerStatus(object sender)
        {
            List<string> soundPaths = (List<string>)GetValueFromValueList("SoundPath");
            int tabIndex = SelectParentIndex(sender);
            if (soundPaths.Count == 0)
            {
                if (tabIndex == 0)
                {
                    btn_Speaker_Page1.Background = Brushes.Red;
                }
                else if (tabIndex == 1)
                {
                    btn_Speaker_Page2.Background = Brushes.Red;
                }
            }
            foreach (string value in soundPaths)
            {
                if (string.IsNullOrEmpty(value) && !File.Exists(value))
                {
                    if (tabIndex == 0)
                    {
                        btn_Speaker_Page1.Background = Brushes.Red;
                    }
                    else if (tabIndex == 1)
                    {
                        btn_Speaker_Page2.Background = Brushes.Red;
                    }
                }
                else
                {
                    var bc = new BrushConverter();
                    if (tabIndex == 0)
                    {
                        btn_Speaker_Page1.Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
                    }
                    else if (tabIndex == 1)
                    {
                        btn_Speaker_Page2.Background = (Brush)bc.ConvertFrom("#FFDDDDDD");
                    }
                }
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
        public void TextChanger<T>(List<T> list, TextBlock textBlockForScript, TextBlock textBlockDescription, int nextValueToAdd) where T : new()
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
                else
                {
                    if (SelectedPropertyToDisplay is List<String> propertyIsList)
                    {
                        textBlockForScript.Text = ListToString(propertyIsList);
                    }
                    else
                    {
                        textBlockForScript.Text = GetValueFromValueList("ThaiScript") + " " + GetValueFromValueList("ThaiHelpWord");
                    }

                    //
                }
                PopulateDescription(textBlockDescription);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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
        }

        public void WriteAllToFile()
        {
            //WriteWordToFile<Word>(Words);
            WriteWordToFile<Consonant>(Consonants);
            WriteWordToFile<Vowel>(Vowels);
            WriteWordToFile<ThaiNumber>(Numbers);

            SaveAll();
        }

        #region auto properties

        #region lists

        public static List<Chapter> Chapters { get; set; } = new List<Chapter>();
        public static List<Consonant> Consonants { get; set; } = new List<Consonant>();
        public static List<Word> DisplayList { get; set; } = new List<Word>();
        public static List<ThaiNumber> Numbers { get; set; } = new List<ThaiNumber>();
        public static List<WordIDTEMP> TempList { get; set; } = new List<WordIDTEMP>();
        public static List<Vowel> Vowels { get; set; } = new List<Vowel>();
        public static Object WhatListTLoad { get; set; }
        public static List<Word> Words { get; set; } = new List<Word>();

        #endregion lists

        #region PropertyInfos

        public static List<TextBox> textboxList;
        public static List<PropertyInfo> ListOfProperties { get; set; } = new List<PropertyInfo>();

        public static List<Object> ListOfValues { get; set; } = new List<Object>();

        #endregion PropertyInfos

        #region activeProperties

        public static bool DescriptionOn { get; set; } = true;
        public static bool LoopChapter { get; set; } = true;
        public static bool RandomOn { get; set; }
        public static bool DisplayAllPropertiesInDescription { get; set; }
        public static int CorrectPoints { get; set; } = 0;
        public static int CurrentFileIndex { get; set; } = 0;
        public static Random RandomIndex { get; set; } = new Random();
        public static string RegexSplitString { get; set; } = @"^\s|[\s;,]{2,}";
        public static string SelectedChapter { get; set; }
        public static object SelectedPropertyToDisplay { get; set; }
        public static object SelectedPropertyToValidate { get; set; }
        public static string SelectedSymbolTypeToUse { get; set; }
        public static string WhatToDisplay { get; set; }
        public static string WhatToTrain { get; set; }
        public static string SubmitStyle { get; set; }
        public static Type WhatTypeToUse { get; set; }
        public static Object WordToLoad { get; set; }

        #endregion activeProperties

        public static string chaptersName = "Key to understanding Thai; Thai alphabet; Closing sounds of consonants; Thai vowels; Tonal Language; Special pronounciation; Nouns, people and particles; Numbers and Counting; Telling time; Colors; Easy words; Homonyms; Homophones; Words in special contexts; 101 most used words; Small talk; The body";

        public static Chapter NewChapter;

        #region Settings properties

        public static string LanguageFilePath { get; set; } = "C:/Users/" + Environment.UserName + "/source/repos/LearnThaiApplication/Language_Files/";

        public IEnumerable<Window> Windows { get; set; }

        #endregion Settings properties

        private static ContentMan window;
        private StackPanel sp = new StackPanel();

        #endregion auto properties

        public void WriteWordToFile<T>(List<T> list) where T : new()
        {
            string full = "";

            foreach (T word in list)
            {
                SetPropertyOfGenericObject(word);

                List<string> script = (List<string>)GetValueFromValueList("ThaiScript");
                string helpword = (string)GetValueFromValueList("ThaiHelpWord");

                if (script.Count == 1)
                {
                    SetValueOfObject(helpword, "ThaiScript", word);
                }

                //    if (ListOfProperties.Exists(e => e.Name == "ThaiHelpWord"))
                //    {
                //        /*foreach(string s in (List<string>)GetValueFromValueList("ThaiHelpWord"))
                //        {
                //            full += s + " ";
                //        }*/
                //        full += (string)GetValueFromValueList("ThaiHelpWord") + " ";
                //    }
                //    else
                //    {
                //        foreach (string s in (List<string>)GetValueFromValueList("ThaiScript"))
                //        {
                //            full += s + " ";
                //        }
                //    }
                //    foreach (string path in (List<string>)GetValueFromValueList("SoundPath"))
                //    {
                //        if (File.Exists(path))
                //        {
                //            full += path + " ";
                //        }
                //    }
                //    full += "\r\n";
                //    File.WriteAllText(LanguageFilePath + word.GetType() + ".txt", full);
            }
        }

        #region component interaction

        public void SelectSymbolToUse(object sender, RoutedEventArgs e)
        {
            SelectedSymbolTypeToUse = (string)((RadioButton)sender).Content;

            UpdateWhenSymbolTypeSelected();
        }

        public int SelectParentIndex(object sender)
        {
            int tabIndex;
            try
            {
                if (sender is TabControl tabcontrol)
                {
                    tabIndex = tabcontrol.SelectedIndex;
                }
                else
                {
                    tabIndex = ((TabControl)((TabItem)((Grid)((FrameworkElement)sender).Parent).Parent).Parent).SelectedIndex;
                }

                return tabIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
                return -1;
            }
        }

        public void SelectWhatToTrain(object sender, RoutedEventArgs e)
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
            PreTextChanger(0, sender);
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
                }
                else if (WhatTypeToUse == typeof(Consonant))
                {
                    DeleteSelected<Consonant>(Consonants);
                }
                else if (WhatTypeToUse == typeof(Vowel))
                {
                    DeleteSelected<Vowel>(Vowels);
                }
                else if (WhatTypeToUse == typeof(ThaiNumber))
                {
                    DeleteSelected<ThaiNumber>(Numbers);
                }
                else
                {
                    MessageBox.Show("Please select a list first");
                }
                SaveAll();
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

        public void PreTextChanger(int change, object sender)
        {
            int tabIndex = SelectParentIndex(sender);

            if (tabIndex == 0)
            {
                TextChanger<Word>(DisplayList, txb_ThaiScript_Page1, txb_Description_page1, change);
                lbl_Counter_Page1.Content = CurrentFileIndex;
                SpeakerStatus(sender);
            }
            if (tabIndex == 1)
            {
                if (SelectedSymbolTypeToUse == "Consonants")
                {
                    TextChanger<Consonant>(Consonants, txb_ThaiScript_Page2, txb_Description_page2, change);
                }
                else if (SelectedSymbolTypeToUse == "Vowels")
                {
                    TextChanger<Vowel>(Vowels, txb_ThaiScript_Page2, txb_Description_page2, change);
                }
                else if (SelectedSymbolTypeToUse == "Numbers")
                {
                    TextChanger<ThaiNumber>(Numbers, txb_ThaiScript_Page2, txb_Description_page2, change);
                }
                else if (SelectedSymbolTypeToUse == "Closing sounds")
                {
                    //TextChanger<ClosingSound>(ClosingSounds, txb_ThaiScript_Page2, txb_Description_page2, change);
                }
                else
                {
                    MessageBox.Show("please select a category");
                }
                lbl_Counter_Page2.Content = CurrentFileIndex;
                SpeakerStatus(sender);
            }
        }

        public void NextWord(object sender, RoutedEventArgs e)
        {
            ClearFields(sender);
            PreTextChanger(1, sender);
        }

        public void PrevWord(object sender, RoutedEventArgs e)
        {
            ClearFields(sender);
            PreTextChanger(-1, sender);
        }

        private void PlaySound(object sender, RoutedEventArgs e)
        {
            int TabIndex = SelectParentIndex(sender);
            PreTextChanger(0, sender);

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
            ClearFields(sender);
        }

        public void ValidateAnswear(object sender, RoutedEventArgs e)
        {
            int tabIndex = SelectParentIndex(sender);

            if (tabIndex == 0)
            {
                ValidateAnswer<Word>(DisplayList, txt_Answear_Page1, txb_Status_Page1, txb_Description_page1);
            }
            else if (tabIndex == 1)
            {
                if (SelectedSymbolTypeToUse == "Consonants")
                {
                    ValidateAnswer<Consonant>(Consonants, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
                }
                else if (SelectedSymbolTypeToUse == "Vowels")
                {
                    ValidateAnswer<Vowel>(Vowels, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
                }
                else if (SelectedSymbolTypeToUse == "Closing sounds")
                {
                    //ValidateAnswer<ClosingSound>(ClosingSounds, txb_ThaiScript_Page2, txb_Description_page2, 0);
                }
                else if (SelectedSymbolTypeToUse == "Numbers")
                {
                    ValidateAnswer<ThaiNumber>(Numbers, txt_Answear_Page2, txb_Status_Page2, txb_Description_page2);
                }
                else
                {
                    MessageBox.Show("Please select a category.");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string fullText = CheckSoundStatus<Word>(Words);
            fullText += CheckSoundStatus<Consonant>(Consonants);
            fullText += CheckSoundStatus<Vowel>(Vowels);
            fullText += CheckSoundStatus<ThaiNumber>(Numbers);

            MessageBox.Show(fullText);
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
            LoopChapter = (sender as CheckBox)?.IsChecked == true;
        }

        private void Randomized_Checked(object sender, RoutedEventArgs e)
        {
            RandomOn = (sender as CheckBox)?.IsChecked == true;
        }

        private void Rb_Conso_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Consonants;
            WhatTypeToUse = typeof(Consonant);
            ClearFields(sender);
            UpdateListBox();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";
            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_ThaiNumber_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Numbers;
            WhatTypeToUse = typeof(ThaiNumber);
            ClearFields(sender);
            UpdateListBox();
            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_Vowel_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Vowels;
            WhatTypeToUse = typeof(Vowel);
            ClearFields(sender);
            UpdateListBox();

            lbl_English_Insert.Content = "Thai help word";
            lbl_Desc_Insert.Content = "English Word";

            lbl_Chapter_Insert.Content = "English Description";
        }

        private void Rb_words_Page3_Checked(object sender, RoutedEventArgs e)
        {
            WhatListTLoad = Words;
            WhatTypeToUse = typeof(Word);

            ClearFields(sender);
            UpdateListBox();

            lbl_English_Insert.Content = "English";
            lbl_Desc_Insert.Content = "Description";
            lbl_Chapter_Insert.Content = "Chapter";
        }

        private void SoundDownloader(object sender, RoutedEventArgs e)
        {
            DownloadSoundProcess<Word>(Words, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Words.html");
            DownloadSoundProcess<Word>(Words, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Words1.html");
            DownloadSoundProcess<Word>(Words, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Words2.html");
            DownloadSoundProcess<Word>(Words, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Words3.html");

            DownloadSoundProcess<Consonant>(Consonants, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Consonants.html");
            DownloadSoundProcess<Consonant>(Consonants, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Consonants1.html");

            DownloadSoundProcess<Vowel>(Vowels, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Vowel.html");
            DownloadSoundProcess<Vowel>(Vowels, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Vowel1.html");

            DownloadSoundProcess<ThaiNumber>(Numbers, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Numbers.html");
            DownloadSoundProcess<ThaiNumber>(Numbers, @"C:\Users\tommy\source\repos\LearnThaiApplication\Sounds\website files\Numbers1.html");

            Button_Click(sender, e);
        }

        private void SubmitStyleChecked(object sender, RoutedEventArgs e)
        {
            SubmitStyle = (string)(sender as RadioButton)?.Content;
        }

        private void TabChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentFileIndex = 0;

            PreTextChanger(0, sender);
        }

        private void OnEnterKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ValidateAnswear(sender, e);
            }
        }

        private void RemoveSoundPath(object sender, RoutedEventArgs e)
        {
            foreach (Word word in Words)
            {
                word.SoundPath.Clear();
            }
            foreach (Consonant word in Consonants)
            {
                word.SoundPath.Clear();
            }
            foreach (Vowel word in Vowels)
            {
                word.SoundPath.Clear();
            }
            foreach (ThaiNumber word in Numbers)
            {
                word.SoundPath.Clear();
            }
            SaveAll();
        }

        private void FullDesc_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                DisplayAllPropertiesInDescription = true;
            }
            else
            {
                DisplayAllPropertiesInDescription = false;
            }
        }
    }

    #endregion component interaction
}