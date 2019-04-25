namespace LearnThaiApplication
{
    public class UserSetting
    {
        public bool DescriptionOn { get; set; }
        public bool IsLooping { get; set; }
        public bool DisplayAllPropertiesInDescription { get; set; }
        public bool RandomOn { get; set; }
        public bool SkipIntro { get; set; }
        public bool AutoPlaySounds { get; set; }
        public bool ShowSaveLocation { get; set; }
        public bool SkipCompletedWords { get; set; }
        public bool ActivateSpeechRecognition { get; set; }
        public string WhatToDisplay { get; set; }
        public string WhatToTrain { get; set; }
        public string SelectedVoice { get; set; }

        public UserSetting(bool descriptionOn, bool loopChapter, bool displayAllPropertiesInDescription, bool randomOn, bool skipIntro, string whatToDisplay, string whatToTrain)
        {
            DescriptionOn = descriptionOn;
            IsLooping = loopChapter;
            DisplayAllPropertiesInDescription = displayAllPropertiesInDescription;
            RandomOn = randomOn;
            SkipIntro = skipIntro;
            WhatToDisplay = whatToDisplay;
            WhatToTrain = whatToTrain;
        }

        public UserSetting()
        {
        }
    }
}