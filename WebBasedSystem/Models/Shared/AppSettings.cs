using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBasedSystem.Models.Shared
{
    public class AppSettings
    {

        public static string ConnectionString()
        {
            return @"Data Source=DESKTOP-8CAK3L9\MSSQLSERVER01;Initial Catalog=WebBaseSystem;Integrated Security=True";
        }
    }
}