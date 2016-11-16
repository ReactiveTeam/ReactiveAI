using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace ReactiveAI.Speech
{
    public class SpeechTalk
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        public static void Talk(string text)
        {
            synth.Speak(text);
        }
    }
}
