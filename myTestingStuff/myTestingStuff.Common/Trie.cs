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
    public class Trie
    {
        Dictionary<char, Node> roots;

        public Trie()
        {
            roots = new Dictionary<char, Node>();
        }

        public void AddWord(string word)
        {
            Node node = null;
            for(int i=0;i<word.Length; i++)
            {
                char letter = word[i];
                bool isFirst = (i == 0);
                bool isLast = (i == word.Length - 1);
                if (isFirst)
                {
                    node = AddRoot(letter, isLast);
                }
                else
                {
                    node = node.AddChild(letter, isLast);
                }
            }
        }

        public Node AddRoot(char letter, bool isLast)
        {
            Node value;
            if (!roots.TryGetValue(letter, out value))
            {
                value = new Node(letter, isLast);
                roots.Add(letter, value);
            }
            else
            {
                value.IsWord = isLast;
            }
            return value;
        }

        public List<string> GetSuggestions(string word)
        {
            //if there is no word, then return an empty list.
            List<string> words = new List<string>();
            if (string.IsNullOrEmpty(word)) return words;

            //Find the node to be used
            Node node = null;
            for (int i = 0; i < word.Length; i++)
            {
                char letter = word[i];
                bool isFirst = (i == 0);
                if (isFirst)
                {
                    roots.TryGetValue(letter, out node);
                }
                else
                {
                    if (node != null)
                        node.Children.TryGetValue(letter, out node);
                }
            }

            if (node == null)
            {
                return words;
            }
            else
            {
                //Now find the words underneath that node recursively
                FindWords(node, words, word);
            }
            return words;
        }

        public void FindWords(Node node, List<string> list, string word)
        {
            foreach (KeyValuePair<char, Node> entry in node.Children)
            {
                string branch = word + entry.Key;
                if (entry.Value.IsWord)
                    list.Add(branch);
                FindWords(entry.Value, list, branch);
            }
        }
    }
}
