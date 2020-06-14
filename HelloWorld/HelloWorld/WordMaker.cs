using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloNamespace
{
    class WordMaker
    {
       static string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "y", "z" };
       static string[] vowels = { "a", "e", "i", "o", "u", };

        public string WordFinder(int length, int seed = 0, string[] vw = null, string[] cn = null)
        {
            Random rnd;

            if (seed == 0)
            {
               rnd = new Random(DateTime.Now.Millisecond);
            }
            else
            {
                rnd = new Random(seed);
            }

            if (vw == null)
            {
                vw = vowels;
            }
            if (cn == null)
            {
                cn = consonants;
            }

            string word = "";


            // Generate the word in consonant / vowel pairs
            while (word.Length < length)
            {
                if (length != 1 || rnd.Next(100) >= 90)
                {
                    // Add the consonant
                    string consonant = GetRandomLetter(rnd, cn);

                    if (consonant == "q" && word.Length + 3 <= length) // check +3 because we'd add 3 characters in this case, the "qu" and the vowel.  Change 3 to 2 to allow words that end in "qu"
                    {
                        word += "qu";
                    }
                    else
                    {
                        while (consonant == "q")
                        {
                            // Replace an orphaned "q"
                            consonant = GetRandomLetter(rnd, cn);
                        }

                        if (word.Length + 1 <= length)
                        {
                            if (word.Length +2 <= length && rnd.Next(100) >= 80)
                            {
                                word += consonant + consonant;
                            }
                            // Only add a consonant if there's enough room remaining
                            word += consonant;
                        }
                    }
                }
                if (word.Length + 1 <= length)
                {
                    // Only add a vowel if there's enough room remaining
                    word += GetRandomLetter(rnd, vw);
                    if (word.Length + 1 <= length && rnd.Next(0, 100) >= 75) //add secondary vowel
                    {
                        word += GetRandomLetter(rnd, vw);
                    }
                }
            }
            return word;
        }

        private static string GetRandomLetter(Random rnd, string[] letters)
        {
            return letters[rnd.Next(0, letters.Length - 1)];
        }
    }
}
