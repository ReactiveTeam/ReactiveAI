using System;
using Syn.Bot.Siml;
using System.IO;
using System.Xml.Linq;

namespace ReactiveAI.Bot
{
    public class BotMain
    {
        public static SimlBot bot = new SimlBot();
        static BotUser user;

        static BotMain()
        {
            user = bot.CreateUser();

            foreach (var simlFile in Directory.EnumerateFiles("BotModels", "*.siml"))
            {
                var simlDocument = XDocument.Load(simlFile);
                bot.AddSiml(simlDocument);
            }
            user.Settings["name"].Value = "Haikal";
        }

        public static string Chat(string text)
        {
            
            var chatRequest = bot.Chat(text);

            if (chatRequest.Success)
            {
                return chatRequest.BotMessage;
            }
            else
            {
                return "I don't know what you're saying";
            }
        }

    }
}
