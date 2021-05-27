using System;
using System.Collections.Generic;
using System.Text;
using Indigox.UUM.Util;
using NUnit.Framework;
using Indigox.UUM.Service;

namespace Indigox.UUM.Tests.Util
{
    [TestFixture]
    public class NameServiceTest
    {
        NameService service = new NameService();
      

        [SetUp]
        public void Init()
        {
           
        }

        [Test]
        public void TestRuleC()
        {
            //NameService service1 = new NameService();

            //service1.NameStrategys = new List<BaseNameRule>();
            //service1.NameStrategys.Add(new SurnameFirstAndNameInitialAndNumSuffix() { priority = 1 });
            //service1.CompundSurname = service.CompundSurname;
            //string name1 =  service1.Naming("张三");
            //string name2 = service1.Naming("张四");
            //Console.WriteLine(name1);
            //Console.WriteLine(name2);


            //NameService service2 = new NameService();
            //service2.NameStrategys = new List<BaseNameRule>();
            //service2.NameStrategys.Add(new SurnameFirstAndNameInitialAndNumSuffix() { priority = 1 });
            //service2.CompundSurname = service.CompundSurname;
            //string name3 = service2.Naming("张三");
            //string name4 = service2.Naming("张四");

            //Console.WriteLine(name3);
            //Console.WriteLine(name4);
        }

        [Test]
        public void TestNaming()
        {
            for(int i=0;i<srcNames.Count;i++)
            {
                string dest = service.Naming(srcNames[i]);
                Assert.AreEqual(destNames[i],dest );
                service.AddName(dest);
            }
        }
        public static List<string> srcNames = new List<string>(new string[] { 
            "刘儽","刘宇","李郭旻","王翔","张翼","文斌","刘宇","毛泽东","周恩来","萧敬腾","刘宇","刘宇",
            "欧阳峰","西门飘雪","宇文成都","欧阳峰","刘燮","翀"
        });
        public static List<string> destNames = new List<string>(new string[] { 
            "liul","liuy","ligm","wangx","zhangy","wenb","yliu","maozd","zhouel","xiaojt","liuy1","liuy2",
            "ouyangf","ximenpx","yuwencd","fouyang","liux","chong"
        });
     
    }
}
