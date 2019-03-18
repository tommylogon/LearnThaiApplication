using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace LearnThaiApplication
{
    public abstract class ThaiBase : INotifyPropertyChanged
    {
        private string name;
        private string engDesc;
        private List<string> thaiScript;
        private List<string> thaiFonet;
        private List<string> engWords;
        private List<string> soundPath;
        private List<string> tone;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }

        public List<string> ThaiScript
        {
            get
            {
                return thaiScript;
            }
            set
            {
                if (thaiScript != value)
                {
                    thaiScript = value;
                    OnPropertyChanged("ThaiScript");
                }
            }
        }

        [XmlIgnore]
        public string ThaiScript_String
        {
            get
            {
                return MainWindow.ListToString(ThaiScript);
            }
            set
            {
                if (MainWindow.ListToString(ThaiScript) != value)
                {
                    ThaiScript = MainWindow.SplitStringToList(value);
                    OnPropertyChanged("ThaiScript_String");
                }
            }
        }

        public List<string> ThaiFonet
        {
            get
            {
                return thaiFonet;
            }
            set
            {
                if (thaiFonet != value)
                {
                    thaiFonet = value;
                    OnPropertyChanged("ThaiFonet");
                }
            }
        }

        [XmlIgnore]
        public string ThaiFonet_String
        {
            get
            {
                return MainWindow.ListToString(ThaiFonet);
            }
            set
            {
                if (MainWindow.ListToString(ThaiFonet) != value)
                {
                    ThaiFonet = MainWindow.SplitStringToList(value);
                    OnPropertyChanged("ThaiFonet_String");
                }
            }
        }

        public List<string> EngWords
        {
            get
            {
                return engWords;
            }
            set
            {
                if (engWords != value)
                {
                    engWords = value;
                    OnPropertyChanged("EngWords");
                }
            }
        }

        [XmlIgnore]
        public string EngWords_String
        {
            get
            {
                return MainWindow.ListToString(EngWords);
            }
            set
            {
                if (MainWindow.ListToString(EngWords) != value)
                {
                    EngWords = MainWindow.SplitStringToList(value);
                    OnPropertyChanged("EngWords_String");
                }
            }
        }

        public List<string> SoundPath
        {
            get
            {
                return soundPath;
            }
            set
            {
                if (soundPath != value)
                {
                    soundPath = value;
                    OnPropertyChanged("SoundPath");
                }
            }
        }

        [XmlIgnore]
        public string SoundPath_String
        {
            get
            {
                return MainWindow.ListToString(SoundPath);
            }
            set
            {
                if (MainWindow.ListToString(SoundPath) != value)
                {
                    SoundPath = MainWindow.SplitStringToList(value);
                    OnPropertyChanged("SoundPath_String");
                }
            }
        }

        public string EngDesc
        {
            get
            {
                return engDesc;
            }
            set
            {
                if (engDesc != value)
                {
                    engDesc = value;
                    OnPropertyChanged("EngDesc");
                }
            }
        }

        public List<string> Tone
        {
            get
            {
                return tone;
            }
            set
            {
                if (tone != value)
                {
                    tone = value;
                    OnPropertyChanged("Tone");
                }
            }
        }

        [XmlIgnore]
        public string Tone_String
        {
            get
            {
                return MainWindow.ListToString(Tone);
            }
            set
            {
                if (MainWindow.ListToString(Tone) != value)
                {
                    Tone = MainWindow.SplitStringToList(value);
                    OnPropertyChanged("Tone_String");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}