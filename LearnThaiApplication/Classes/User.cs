using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication.Classes
{
    public class User : INotifyPropertyChanged
    {
        private string userName;
        private ObservableCollection<Word> completedWords;

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

        public ObservableCollection<Word> CompletedWords
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
