using System;
using System.IO;
using System.Xml.Linq;
using System.Speech.Recognition;
using System.Collections.Generic;

namespace ReactiveAI.Speech
{
    public class SpeechGrammar
    {
        static string modelsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "SpeechModels");
        public static Grammar grammar;
        public static List<string> commands = new List<string>();
  
        static SpeechGrammar()
        {
            LoadGrammar();
            LoadCommands();
        }

        public static Grammar LoadGrammar()
        {
            Choices grammerChoice = new Choices();
            GrammarBuilder gb = new GrammarBuilder();

            foreach(string file in Directory.EnumerateFiles("SpeechModels", "*.xml"))
            {
                XDocument doc = XDocument.Load(file);

                var choice = doc.Descendants("grammar");

                foreach (var item in choice)
                {
                    grammerChoice.Add(item.Value);
                }

                gb.Append(grammerChoice);
            }

            grammar = new Grammar(gb);
            return grammar;
        }

        public static void LoadCommands()
        {
            foreach (string file in Directory.EnumerateFiles("SpeechModels/Commands", "*.xml"))
            {
                XDocument doc = XDocument.Load(file);

                var choice = doc.Descendants("command");

                foreach (var item in choice)
                {
                    commands.Add(item.Value);
                }
            }
        }

        public static bool checkCommand(string text)
        {
            char splitDelimiter = ' ';
            string[] splitText = text.Split(splitDelimiter);

            foreach(string s in splitText)
            {
                if (commands.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
