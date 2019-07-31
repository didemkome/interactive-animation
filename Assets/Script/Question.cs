using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string TrueAnswer { get; set; }
        public string TrueAnswerImageURL { get; set; }
        public string WrongAnswer { get; set; }
        public string WrongAnswerImageURL { get; set; }
        public string UserAnswer { get; set; }
        public Question()
        {
        }
        public Question(int questionID, string questionText, string trueAnswer, string trueAnswerImageURL , string wrongAnswer, string wrongAnswerImageURL)
        {
            QuestionID = questionID;
            QuestionText = questionText;
            TrueAnswer = trueAnswer;
            TrueAnswerImageURL = trueAnswerImageURL;
            WrongAnswer = wrongAnswer;
            WrongAnswerImageURL = wrongAnswerImageURL;
        }
    }
}
