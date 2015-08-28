using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using TShockAPI;
using TerrariaApi.Server;
using System.Timers;
using Newtonsoft.Json;
using System.IO;

namespace CodeReward
{
    [ApiVersion(1, 21)]
    public class main : TerrariaPlugin
    {

        public static Config Config;

        public main(Main game) : base(game) { Order -= 1; }

        public override Version Version { get { return new Version("1.5"); } }
        public override string Name { get { return "CodeReward"; } }
        public override string Author { get { return "Teddy"; } }
        public override string Description { get { return "Kto pierwszy przepisze kod Wygrywa!"; } }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(permission.codereward, command.functionCmd, "codereward"));
            Commands.ChatCommands.Add(new Command(permission.codereward, command.functionCmd, "cr"));
            ServerApi.Hooks.ServerChat.Register(this, chat.onChat);

            string path = Path.Combine(TShock.SavePath, "CodeReward.json");
            Config = Config.Read(path);
            if (!File.Exists(path))
            {
                Config.Write(path);
            }


            varslist.var.codeon = false;
            varslist.var.Inverval = Config.Interval;
            varslist.var.RewardsBuffs = Config.RewardsBuffs;
            varslist.var.RewardsItems = Config.RewardsItems;
            varslist.var.BuffTime = Config.BuffTime;
            varslist.var.lengthCode = Config.lengthCode;
            varslist.var.letters = Config.letters;
            varslist.var.winMessage = Config.winMessage;
            varslist.var.newCode = Config.newCode;
            varslist.var.LoginIn = Config.LoginIn;
            varslist.var.Muted = Config.Muted;
            varslist.var.TwoTimes = Config.TwoTimes;
            varslist.var.twotimesblock = Config.twotimesblock;


            System.Timers.Timer timer = new System.Timers.Timer(varslist.var.Inverval * (60 * 1000));
            timer.Elapsed += run;
            timer.Start();
        }



        private void run(object sender, ElapsedEventArgs args)
        {
            if (varslist.var.codeon == false)
            {
                codeGenerate.run(varslist.var.lengthCode, false);
            }
            else
            {
                varslist.var.codeon = false;
                varslist.var.code = null;

                string message = varslist.var.winMessage;
                while (message.Contains("%gracz%"))
                {
                    message = message.Replace("%gracz%", "BRAK");
                }


                TSPlayer.All.SendMessage("[CodeReward]" + message, Color.Silver);
            }
        }
    }
}