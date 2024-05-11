using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects //Repeat class added for use in other scopes of the project from ASPScenicHotel.Models.Events
{
    public class Events
    {
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }
        public string color { get; set; }
    }
}
