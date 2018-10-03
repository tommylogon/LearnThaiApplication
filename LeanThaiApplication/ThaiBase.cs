using System;
using System.Collections.Generic;

namespace LearnThaiApplication
{
    public abstract class ThaiBase
    {
        public string ThaiScript { get; set; }

        public string ThaiFonet { get; set; }

        public List<String> EngWords { get; set; }

        public string EngWord { get; set; }

        public string EngDesc { get; set; }
    }
}