using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace Indigox.UUM.Naming.Util
{
    public static class PinYinConverter
    {
        public static bool IsPolyphone(char c)
        {
            if (ChineseChar.IsValidChar(c))
            {
                ChineseChar cc = new ChineseChar(c);
                bool isPolyphone = false;
                string pre="";
                foreach(string s in cc.Pinyins)
                {
                    if (string.IsNullOrEmpty(s)) continue;
                    string pinyin=s.Substring(0, s.Length - 1);
                    if (pre == "")
                    {
                        pre = pinyin;
                    }
                    else
                    {
                        if (pre != pinyin)
                        {
                            isPolyphone = true;
                        }
                    }
                }
                return isPolyphone;
            }
            return false;
        }

        public static bool HasPolyphone(string s)
        {
            bool has = false;
            foreach (char c in s)
            {
                if (IsPolyphone(c))
                {
                    has = true;
                    break;
                }
            }
            return has;
        }

        public static string GetPinYin(string chinese)
        {
            StringBuilder builder = new StringBuilder();
            if (chinese.Equals("叶"))
            {
                return "ye";
            }
            if (chinese.Equals("曾"))
            {
                return "zeng";
            }
            if (chinese.Equals("单"))
            {
                return "shan";
            }
            foreach (var v in chinese)
            {
                if (ChineseChar.IsValidChar(v))
                {
                    ChineseChar c = new ChineseChar(v);
                    string py = c.Pinyins[0];
                    py = py.Substring(0, py.Length - 1);
                    builder.Append(py);
                }
            }
            return builder.ToString();
        }

        public static string GetInitial(string chinese)
        {
            StringBuilder builder = new StringBuilder();
            foreach (var v in chinese)
            {
                if (ChineseChar.IsValidChar(v))
                {
                    ChineseChar c = new ChineseChar(v);
                    string py = c.Pinyins[0].Substring(0,1);
                    builder.Append(py);
                }
            }
            return builder.ToString();
        }
    }
}
