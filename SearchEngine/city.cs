using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchEngine
{
    class city
    {
        public city()
        {
        }
        public String[] returnCities()
        {
            String dictionary = File.ReadAllText(@"city.txt");
            String[] cities = dictionary.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            return cities;
        }
    }
}
