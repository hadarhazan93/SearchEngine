using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/**
 * basic document class
 */
namespace SearchEngine
{
    class Document
    {
        public String path { get; set; }
        public String id { get; set; }
        public String date { get; set; }
        public String title { get; set; }
        public String text { get; set; }
        public String city { get; set; }

        public Document(String path, String id, String date, String title, String text, String city)
        {
            this.path = path;
            this.id = id;
            this.date = date;
            this.title = title;
            this.text = text;
            this.city = city;
        }
    }
}
