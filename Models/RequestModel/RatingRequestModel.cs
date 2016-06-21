using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Models.Requests
{
    public class RatingRequestModel
    {
        public int Rating { get; set; }
        public string Comments { get; set; }
        public string dealerID { get; set; }
    }
}