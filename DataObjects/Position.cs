using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects
{
    public class Position
    {
        public int PositionID { get; set; }
        public string PositionTitle { get; set; }
        [DataType(DataType.MultilineText)]
        public string PositionDescription { get; set; }
    }
}
