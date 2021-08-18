using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ofakim.Models
{
    public class BlockModel
    {
        public string Value { get; set; }
        [XmlIgnoreAttribute]
        public string Uri { get; set; }
        public string Type { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;
    }
}
