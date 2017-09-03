//  Copyright (c) 2017, Jordi Corbilla
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//
//  - Redistributions of source code must retain the above copyright notice,
//    this list of conditions and the following disclaimer.
//  - Redistributions in binary form must reproduce the above copyright notice,
//    this list of conditions and the following disclaimer in the documentation
//    and/or other materials provided with the distribution.
//  - Neither the name of this library nor the names of its contributors may be
//    used to endorse or promote products derived from this software without
//    specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//  AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
//  ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
//  LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//  CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//  SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
//  INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
//  CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//  POSSIBILITY OF SUCH DAMAGE.

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
