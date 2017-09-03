using System.Collections.Generic;

namespace myTestingStuff.Common
{
    public class SentenceParser
    {
        private string _sentence { get; set; }
        public SentenceParser(string sentence)
        {
            _sentence = sentence;
        }

        public List<string> Parse(List<Token> tokens)
        {
            List<string> words = new List<string>();
            string sentence = _sentence;
            foreach(Token token in tokens)
            {
                if (sentence.Length > 0)
                {
                    int index = sentence.IndexOf(token.Start) + token.Start.Length;
                    sentence = sentence.Substring(index); //remove the part till the first token.
                                                          //Find the end of the first token
                    if (!token.TillEnd)
                    {
                        index = sentence.IndexOf(token.End) + token.End.Length;
                        string word = sentence.Substring(0, index);
                        words.Add(word.Trim());
                        sentence = sentence.Substring(index); //remove the items and keep storing the sentence
                    }
                    else
                    {
                        string word = sentence;
                        words.Add(word.Trim());
                        return words;
                    }
                }
            }
            return words;
        }
    }
}
