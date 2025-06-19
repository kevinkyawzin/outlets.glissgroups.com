using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class Reward
    {
        public Result[] result { get; set; }
   
    }

    public class Result
    {
        public bool isSuccess { get; set; }
        public string msg { get; set; }

    }
}