using System;
using System.Collections.Generic;
using Indigox.UUM.Naming.Factory;
using Indigox.UUM.Naming.Model;
using Indigox.UUM.Naming.Service;
using Indigox.UUM.Naming.Strategies;
using Indigox.UUM.Naming.Tests.TestModel;
using NUnit.Framework;

namespace Indigox.UUM.Naming.Tests.ServiceTest
{
    [TestFixture]
    public class NameServiceTest
    {
        private NameService service;

        [SetUp]
        protected void SetUp()
        {
            MemNameManager.Clear();

            NameStrategyManager nameStrategyManager = new NameStrategyManager(new INameStrategy[]{
                new LastNameAndGivenNameInitalsStrategy(),
                new GivenNameInitalsAndLastNameStrategy(),
                new LastNameAndGivenNameStrategy(),
                new LastNameAndGivenNameInitalsAndNumStrategy()
            });

            service = new NameService(nameStrategyManager, new MemNameManager());
        }

        [Test]
        public void TestLastNameAndGiventNameInitalsForEqualOneAndNumStrategy()
        {
            NameStrategyManager nameStrategyManager1 = new NameStrategyManager(new INameStrategy[]{
                new LastNameAndGiventNameInitalsForEqualOneStrategy(),
                new LastNameAndGiventNameInitalsForEqualOneAndNumStrategy()
            });
            NameService service1 = new NameService(nameStrategyManager1, new MemNameManager());
            List<string> cnName = new List<string>(new string[]{
                "张三","张三","西门飘雪","西门飘雪","西门飘雪"
            });
            List<string> expectName = new List<string>(new string[]{
               "zhangsan","zhangsan02","ximenpx","ximenpx02","ximenpx03",
            });
            for (int i = 0; i < cnName.Count; i++)
            {
                string actual = service1.Naming(cnName[i]);
                string expected = expectName[i];
                Assert.AreEqual(expected, actual);
                MemNameManager.AddName(actual);
            }
        }

        [Test]
        public void TestNaming()
        {
            NameStrategyManager nameStrategyManager1 = new NameStrategyManager(new INameStrategy[]{
                new LastNameAndGiventNameInitalsForEqualOneStrategy(),
                new LastNameAndGiventNameInitalsForEqualOneAndNumStrategy()
            });
            NameService service = new NameService(nameStrategyManager1, new MemNameManager());
            string name = service.Naming("红");
            Console.WriteLine(name);
        }

        [Test]
        public void TestMultiServices()
        {
            NameStrategyManager nameStrategyManager = new NameStrategyManager(new INameStrategy[]{
                new LastNameAndGivenNameInitalsStrategy(),
                new GivenNameInitalsAndLastNameStrategy(),
                new LastNameAndGivenNameStrategy(),
                new LastNameAndGivenNameInitalsAndNumStrategy()
            });

            NameService service1 = new NameService(nameStrategyManager, new MemNameManager());

            string name1 = service1.Naming("张三");
            MemNameManager.AddName(name1);
            string name2 = service1.Naming("张四");
            MemNameManager.AddName(name2);

            NameService service2 = new NameService(nameStrategyManager, new MemNameManager());
            string name3 = service2.Naming("张三");
            MemNameManager.AddName(name3);
            string name4 = service2.Naming("张四");
            MemNameManager.AddName(name4);
            string name5 = service2.Naming("张四");
            MemNameManager.AddName(name5);

            Assert.AreEqual("zhangs", name1);
            Assert.AreEqual("szhang", name2);
            Assert.AreEqual("zhangsan", name3);
            Assert.AreEqual("zhangsi", name4);
            Assert.AreEqual("zhangs1", name5);
        }

        public static List<string> srcNames = new List<string>(new string[] {
            "刘儽","刘宇","李郭旻","王翔","张翼","文斌","刘宇","毛泽东","周恩来","萧敬腾","刘宇","刘宇",
            "欧阳峰","西门飘雪","宇文成都","欧阳峰","刘燮","翀","刘宇","刘予","毛泽东","毛泽东"
            //,"王翔","王翔","王翔","王翔"
        });

        public static List<string> destNames = new List<string>(new string[] {
            "liul","liuy","ligm","wangx","zhangy","wenb","yliu","maozd","zhouel","xiaojt","liuyu","liuy1",
            "ouyangf","ximenpx","yuwencd","fouyang","liux","chong","liuy2","liuy3","zdmao","maozedong"
            //,"xwang","wangxiang","wangx1","wangx2"  // wangx1 can't pass, return wangx4
        });
    }
}