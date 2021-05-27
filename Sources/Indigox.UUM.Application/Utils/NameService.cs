using System;
using System.Collections.Generic;
using System.Text;

namespace Indigox.UUM.Application.Utils
{
    public class NameService
    {
        private string lastName;
        private string givenName;

        private static List<string> CompundSurname = new List<string>(new string[]{
            "上官","司马","东方","独孤","东宫","仲长","子书","子桑","即墨","达奚","南宫",
            "万俟","闻人","夏侯","公坚","左丘","公伯","西门","公祖","第五","公乘","诸葛",
            "尉迟","公羊","公孙","慕容","仲孙","钟离","长孙","宇文","司徒","鲜于","司空",
            "闾丘","子车","亓官","司寇","巫马","公西","颛孙","壤驷","公良","漆雕","乐正",
            "宰父","谷梁","拓跋","夹谷","轩辕","令狐","段干","百里","呼延","东郭","南门",
            "羊舌","微生","公户","公玉","公仪","梁丘","赫连","澹台","皇甫","宗政","濮阳",
            "公冶","太叔","申屠","公仲","公上","公门","欧阳","太史","端木","公山","贯丘",
            "公皙","南荣","东里","褚师","吴铭"
        });

        private int GetLastNameLength(string name)
        {
            foreach (string compundSurname in CompundSurname)
            {
                if (name.StartsWith(compundSurname))
                {
                    return compundSurname.Length;
                }
            }
            return 1;
        }

        public NameService(string cnName)
        {
            int lastNameLength = GetLastNameLength(cnName);

            lastName = cnName.Substring(0, lastNameLength);
            givenName = cnName.Substring(lastNameLength);
        }

        public string GetLastName()
        {
            return lastName;
        }

        public string GetGivenName()
        {
            return givenName;
        }
    }
}
