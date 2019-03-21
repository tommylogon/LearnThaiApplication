using System.ComponentModel;

namespace LearnThaiApplication
{
    public class Chapter : INotifyPropertyChanged
    {
        private string name;
        public Chapter()
        {
        }

        public Chapter(string chapterName)
        {
            this.ChapterName = chapterName;
        }

        public string ChapterName
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
                    OnPropertyChanged("ChapterName");
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