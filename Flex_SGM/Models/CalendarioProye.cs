using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Flex_SGM.Models
{
    public class CalendarioProye
    {
        [Key]
        public Int64 EventID { get; set; }
        [MaxLength(100)]
        public String Subject { get; set; }
        [MaxLength(300)]
        public String Description { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
        [MaxLength(30)]
        public string daysOfWeek { get; set; }
        [MaxLength(10)]
        public string ThemeColor { get; set; }

        public bool IsFullDay { get; set; }


    }
}