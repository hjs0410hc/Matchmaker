using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp3
{

    internal class preset
    {
        public String name;
        public List<String> list;
        public bool check;

        public preset(String name, List<String> list, bool check)
        {
            this.name = name;
            this.list = list;
            this.check = check;
        }


        public static List<preset> presets = new List<preset>();

        public static void Save(StreamWriter sw, preset preset)
        {
            sw.WriteLine(preset.name);
            sw.WriteLine(preset.check);
            foreach (String s in preset.list)
            {
                sw.WriteLine(s);
            }
            sw.WriteLine("\t");
        }
        public static void SaveAll()
        {
            StreamWriter sw = new StreamWriter("presets.txt");
            foreach(preset p in presets)
            {
                Save(sw,p);
            }
            sw.Close();
        }
    }
}
