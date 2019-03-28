using System.Collections.Generic;
using System.ComponentModel;

namespace LearnThaiApplication
{
    public class User : INotifyPropertyChanged
    {
        private string userName;

        public class CompletedWord
        {
            public Word word;
            public bool scriptCompleted;
            public bool foneticCompleted;
            public bool meaningCompleted;
        }

        private List<CompletedWord> completedWords;

        public event PropertyChangedEventHandler PropertyChanged;

        public User()
        {
        }

        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                if (userName != value)
                {
                    userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        public List<CompletedWord> CompletedWords
        {
            get
            {
                return completedWords;
            }
            set
            {
                if (completedWords != value)
                {
                    completedWords = value;
                    OnPropertyChanged("CompletedWords");
                }
            }
        }

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}