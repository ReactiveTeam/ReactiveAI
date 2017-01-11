using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Utils.Processors
{
    /// <summary>
    /// A natural language processor
    /// </summary>
    public class LanguageProcessor
    {
        public static Dictionary<string, List<string>> keywords = new Dictionary<string, List<string>>();

        /// <summary>
        /// Registers a list of  keywords within the processor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public static void RegisterKeywords(string id, List<string> key)
        {
            keywords.Add(id, key);
        }

        /// <summary>
        /// Processes a string to check wether if it contains a keyword
        /// </summary>
        /// <param name="keyID"></param>
        /// <param name="toProcess"></param>
        /// <param name="toFind"></param>
        /// <returns>Returns the keyword</returns>
        public string processString(string keyID, string toProcess, string toFind = null)
        {
            if (String.IsNullOrEmpty(keyID) || String.IsNullOrEmpty(toProcess))
                return null;

            if (String.IsNullOrEmpty(toFind))
            {
                if (keywords[keyID].Any(toProcess.Contains))
                {
                    return keywords[keyID].Where(s => keywords[keyID].Any(toProcess.Contains)).FirstOrDefault();
                }
                return null;
            }
            else
            {
                return checkIfContains(toProcess, keyID, toFind);
            }

            return null;
        }

        /// <summary>
        /// Checks if a string contains a specific keyword
        /// </summary>
        /// <param name="toProcess"></param>
        /// <param name="keyID"></param>
        /// <param name="toFind"></param>
        /// <returns></returns>
        string checkIfContains(string toProcess, string keyID, string toFind)
        {
            if (keywords[keyID].Contains(toFind))
            {
                if (toProcess.Contains(toFind))
                {
                    return toFind;
                }
                return null;
            }
            return null;
        }
    }
}
