using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DualTechCruds.DTOs
{
    public class ResponseResult
    {
        public bool success { get; set; }
        public string errorMsg { get; set; }
        public dynamic  data { get; set; }

        public ResponseResult()
        {
            this.success = false;
            this.errorMsg = null;
            this.data = null;
        }
    }
}