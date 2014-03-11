namespace LeetSpeak
{
    using System.Linq;
    using System.Collections.Generic;
    using System.Reflection;

    public class LeetSpeakTranslator
    {
        static readonly Dictionary<char,char> dictionary = new Dictionary<char, char>
        {
            {'a', '4'},
            {'A', '4'},
            {'e', '3'},
            {'E', '3'},
            {'l', '1'},
            {'L', '1'},
            {'o', '0'},
            {'O', '0'},
            {'s', '$'},
            {'S', '$'},
            {'t', '7'},
            {'T', '7'},
        };

        public static void LeetifyAssembly(Assembly assembly)
        {
            foreach (var str in EnumerateInternedStrings(assembly))
            {
                LeetifyString(str);
            }
        }

        static IEnumerable<string> EnumerateInternedStrings(Assembly assembly)
        {
            var metaDataReader = new MetaDataHelpers(assembly.Location);
            return metaDataReader.EnumerateUserStrings().Where(x => string.IsInterned(x) != null);
        }

        static unsafe void LeetifyString(string str)
        {
            str = string.Intern(str);

            fixed (char* p = str)
            {
                for (var i = 0; i < str.Length; i++)
                {
                    p[i] = LeetifyChar(p[i]);
                }
            }
        }

        static char LeetifyChar(char c)
        {
            return dictionary.ContainsKey(c) ? dictionary[c] : c;
        }
    }
}
