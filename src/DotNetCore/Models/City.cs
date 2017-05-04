using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.Models
{
    public class CityInfo
    {
        public int ID { get; set; }
        [StringLength(150, ErrorMessage = "City cannot be longer than 150 characters.", MinimumLength = 3)]
        public string City { get; set; }
        [Range(1, 9999999999)]
        public decimal Lat { get; set; }
        [Range(1, 9999999999)]
        public decimal Lon { get; set; }
    }
}
