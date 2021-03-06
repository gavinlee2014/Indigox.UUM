using System;
using System.Collections.Generic;
using System.Text;
using Indigox.UUM.Util;

namespace Indigox.UUM.Service
{
    public class NameService
    {
        private bool ordered = false;
        private static List<string> CompundSurname = new List<string>(new string[]{
            "上官","司马","东方","独孤","东宫","仲长","子书","子桑","即墨","达奚","南宫","万俟","闻人","夏侯","公坚","左丘","公伯","西门","公祖","第五","公乘","诸葛","尉迟","公羊","公孙","慕容","仲孙","钟离","长孙","宇文","司徒","鲜于","司空","闾丘","子车","亓官","司寇","巫马","公西","颛孙","壤驷","公良","漆雕","乐正","宰父","谷梁","拓跋","夹谷","轩辕","令狐","段干","百里","呼延","东郭","南门","羊舌","微生","公户","公玉","公仪","梁丘","赫连","澹台","皇甫","宗政","濮阳","公冶","太叔","申屠","公仲","公上","公门","欧阳","太史","端木","公山","贯丘","公皙","南荣","东里","褚师","吴铭"
        });
        public List<INameStrategy> NameStrategys { get; set; }

        /*test data*/
        public List<string> Names = new List<string>(new string[]{
        });
        public void AddName(string name)
        {
            Names.Add(name);
        }
        /*end test data*/

        public bool Exists(string name)
        {
            return Names.Contains(name);
        }

        public string Naming(string chineseName)
        {
            if (ordered == false)
            {
                NameStrategys.Sort();
                ordered = true;
            }
            string name=string.Empty;
            foreach (INameStrategy rule in NameStrategys)
            {
                rule.ChineseName = chineseName;
                rule.CompundSurname = CompundSurname;
               // rule.AnylazeName();
                name = rule.Naming().ToLower();
                if (!Exists(name))
                {
                    break;
                }
            }
            return name;
        }

    }
}
