using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Indigox.UUM.Naming.Util;

namespace Indigox.UUM.Naming.Tests
{
    [TestFixture]
    public class UtilTest
    {
        [Test]
        public void TestIsPolyphone()
        {
            string s = "红色的太阳,刘宇,毛贼东,周恩来，冯岚";
            foreach (char c in s)
            {
                Console.WriteLine(c.ToString()+" is Polyphone: "+PinYinConverter.IsPolyphone(c));
            }
        }

        [Test]
        public void TestHasPolyphone()
        {
            string s = "冯岚";
            Console.WriteLine(s + " has polyphone: " + PinYinConverter.HasPolyphone(s));
        }
    }
}
