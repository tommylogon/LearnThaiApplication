using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnThaiApplication.Classes
{
    public class UserSetting
    {
        public bool DescriptionOn { get; set; }
        public bool LoopChapter { get; set; }
        public bool DisplayAllPropertiesInDescription { get; set; }
        public bool RandomOn { get; set; }
        public bool SkipIntro { get; set; }
        public string WhatToDisplay { get; set; }
        public string WhatToTrain { get; set; }

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