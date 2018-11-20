namespace LearnThaiApplication.Classes
{
    public class UserSetting
    {
        public bool DescriptionOn { get; set; }
        public bool LoopChapter { get; set; }
        public bool DisplayAllPropertiesInDescription { get; set; }
        public bool RandomOn { get; set; }
        public bool SkipIntro { get; set; }
        public bool AutoPlaySounds { get; set; }
        public string WhatToDisplay { get; set; }
        public string WhatToTrain { get; set; }
        public int Page1Chapter { get; set; }
        public int Page2Chapter { get; set; }
        public int Page3Chapter { get; set; }
        public int Page4Chapter { get; set; }

        public UserSetting(bool descriptionOn, bool loopChapter, bool displayAllPropertiesInDescription, bool randomOn, bool skipIntro, string whatToDisplay, string whatToTrain)
        {
            DescriptionOn = descriptionOn;
            LoopChapter = loopChapter;
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