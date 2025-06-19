using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BigMac.Models
{
    public class AppSettings
    {
        public string favicon { 
            get 
               { 
                  return ConfigurationManager.AppSettings["favicon"];
               }
        }

        public string logo
        {
            get
            {
                return ConfigurationManager.AppSettings["logo"];
            }
   
        }

        public string home
        {
            get
            {
                return ConfigurationManager.AppSettings["home"]; 
            }
 
        }

        public string appName
        {
            get
            {
                return ConfigurationManager.AppSettings["appName"]; 
            }
     
        }

        
        public string contact
        {
            get
            {
                return ConfigurationManager.AppSettings["contact"]; 
            }
    
        }

        public string emailMain
        {
            get
            {
                return ConfigurationManager.AppSettings["emailMain"]; 
            }

        }

        public string emailRewards
        {
            get
            {
                return ConfigurationManager.AppSettings["emailRewards"]; 
            }
 
        }

        public string bcc1
        {
            get
            {
                return ConfigurationManager.AppSettings["bcc1"]; 
            }

        }

        public string bcc2
        {
            get
            {
                return ConfigurationManager.AppSettings["bcc2"]; 
            }
       
        }

        public string bcc3
        {
            get
            {
                return ConfigurationManager.AppSettings["bcc3"]; 
            }

        }

        public string membersSite
        {
            get
            {
                return ConfigurationManager.AppSettings["membersSite"];
            }

        }
        public string outletsSite
        {
            get
            {
                return ConfigurationManager.AppSettings["outletsSite"];
            }

        }
        public string backofficeSite
        {
            get
            {
                return ConfigurationManager.AppSettings["backofficeSite"];
            }

        }
    }
}