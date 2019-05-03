using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DNWS
{
   public class TwitterApiPlugin : TwitterPlugin
    {
        public string GetAllUser()
        {
            return "TEST";
        }
    }
}