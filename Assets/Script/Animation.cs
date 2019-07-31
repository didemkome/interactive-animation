using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    public class Animation
    {
        public int ID { get; set; }
        public string QuestionID { get; set; }
        public string Subject { get; set; }
        public string Url { get; set; }
        public int NextAnimationIDIfAnswerisTrue { get; set; }
        public int NextAnimationIDIfAnswerisWrong { get; set; }
    }
}
