using System;
using System.Collections.Generic;
using System.Text;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    public static class NameStrategyManager
    {
        private static List<INameStrategy> nameStrategys;
        //test data
        private static List<NameStrategyDescript> nameStrategyDescriptions = new List<NameStrategyDescript>(new NameStrategyDescript[]{
            new NameStrategyDescript(){
                Priority=1, Assembly="Indigox.UUM",ClassName="Indigox.UUM.Util.SurnameFirstAndNameInitialStrategy", 
                Description="姓全拼，名简写，先姓后名", Enabled=true},
            new NameStrategyDescript(){
                Priority=2, Assembly="Indigox.UUM",ClassName="Indigox.UUM.Util.NameFirstAndNameInitialStrategy", 
                Description="姓全拼，名简写，先名后姓", Enabled=true},
            new NameStrategyDescript(){
                Priority=3, Assembly="Indigox.UUM",ClassName="Indigox.UUM.Util.SurnameFirstNameSpelling", 
                Description="姓全拼，名全拼，先姓后名", Enabled=true},
            new NameStrategyDescript(){
                Priority=4, Assembly="Indigox.UUM",ClassName="Indigox.UUM.Util.SurnameFirstAndNameInitialAndNumSuffix", 
                Description="姓全拼，名简写，先姓后名，加数字后缀", Enabled=true}        
        });

        public static List<INameStrategy> GetNameStrategys()
        {
            if(nameStrategys==null){
                nameStrategys = new List<INameStrategy>();
                nameStrategys.AddRange(
                    new INameStrategy[]{
                        new SurnameFirstAndNameInitialStrategy(),
                        new NameFirstAndNameInitialStrategy(),
                        new SurnameFirstAndNameInitialAndNumSuffix(),
                        new SurnameFirstNameSpelling()
                    }
                );

                
            }
            return nameStrategys;
        }
    }
}
