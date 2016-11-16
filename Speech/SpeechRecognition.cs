using System;
using System.Speech.Recognition;

using ReactiveAI.Bot;
using System.Speech.Synthesis;

namespace ReactiveAI.Speech
{
    public class SpeechRecognition
    {
        static SpeechRecognizer speechRecognizer = new SpeechRecognizer();
        static SpeechGrammar speechGrammar = new SpeechGrammar();
        static SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        BotMain botMain = new BotMain();

        public static string lastRecognizedText;
        public static string botReturnMessage;

        static SpeechRecognition()
        {
            speechRecognizer.LoadGrammar(new DictationGrammar());
            speechRecognizer.SpeechRecognized += SpeechRecognized;
        }

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            lastRecognizedText = e.Result.Text;
            botReturnMessage = BotMain.Chat(e.Result.Text);
            speechSynthesizer.Speak(botReturnMessage);
            if (SpeechGrammar.checkCommand(e.Result.Text))
            {
                System.Windows.Forms.MessageBox.Show("Found a command");
            }
        }
    }
}
