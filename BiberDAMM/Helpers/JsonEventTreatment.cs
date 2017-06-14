using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiberDAMM.Helpers
{
    //Class for displaying treatment-data in a calendar view [HansesM]
    public class JsonEventTreatment
    {
        // attributes have to be in lowercase because javascripts works with these lowercase variable names
        public string id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}