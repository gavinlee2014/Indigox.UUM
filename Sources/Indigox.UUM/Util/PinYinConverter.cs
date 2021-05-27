using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.International.Converters.PinYinConverter;

namespace Indigox.UUM.Util
{
    public static class PinYinConverter
    {
        public static string GetPinYin(string chinese)
        {
            StringBuilder builder = new StringBuilder();
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
