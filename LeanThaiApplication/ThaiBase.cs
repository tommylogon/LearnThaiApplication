using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public abstract class ThaiBase
    {
        public string Name { get; set; }

        public List<String> ThaiScript { get; set; }

        public List<String> ThaiFonet { get; set; }

        public List<String> EngWords { get; set; }

        public List<string> SoundPath { get; set; }

        public string EngDesc { get; set; }

        public List<String> Tone { get; set; }

        
    }
}