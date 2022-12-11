using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApp03.Tool
{
    public class DbHelper
    {
        public static readonly string ConnString = ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString;
    }
}