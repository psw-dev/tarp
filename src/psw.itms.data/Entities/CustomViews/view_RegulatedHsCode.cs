using System;
using System.Collections.Generic;
using System.Linq;


namespace PSW.ITMS.Data.Entities
{
    public class ViewRegulatedHsCode
    {
        public string HsCode { get; set; }
        public List<string> ProductCode { get; set; }
        public string ItemDescription { get; set; }
        public string ItemDescriptionext { get; set; }
        public int AgencyID { get; set; }
    }
}
