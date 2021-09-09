using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChillDiscordGrabber
{
    class StringUtils
    {
        public static string RemoveInvalidTokenCharacters(string text)
        {
            string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_\n";
            string ret = "";
            foreach (char ch in text)
                if (Valid.Contains(ch))
                    ret += ch;
            return ret;
        }

        public static bool isValidTokenString(string text)
        {
            string Valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_";
            string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            bool HasCapital = false;
            foreach (char ch in text)
                if (Capitals.Contains(ch.ToString()) == true)
                {
                    HasCapital = true;
                    break;
                }

            if (!HasCapital)
                return false;
            foreach (char ch in text)
                if (Valid.Contains(ch.ToString()) == false)
                    return false;

            return true;
        }

        public static string RemoveEverythingAfterLastIncluding(string input, string remove)
        {
            try { input = input.Substring(0, input.LastIndexOf(remove)); } catch { }
            return input;
        }
        public static string RemoveEverythingAfterFirstIncluding(string input, string remove)
        {
            try { input = input.Substring(0, input.IndexOf(remove)); } catch { } //if there is none
            return input;
        }
        public static string RemoveEverythingBeforeFirst(string input, string remove)
        {
            try { input = input.Substring(input.IndexOf(remove)); } catch { } //if there is none
            return input;
        }
        public static string RemoveEverythingBeforeLast(string input, string remove)
        {
            try { input = input = input.Substring(input.LastIndexOf(remove) + 1); } catch { }
            return input;
        }
        public static string RemoveLastChar(string s)
        {
            if (s == null || s.Length == 0)
            {
                return s;
            }
            return s.Substring(0, s.Length - 1);
        }
    }
}
