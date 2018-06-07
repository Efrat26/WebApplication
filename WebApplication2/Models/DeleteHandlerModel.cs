using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class DeleteHandlerModel
    {
        private String handler;
        public String Handler { get { return this.handler; } set { this.handler = value; } }
        public DeleteHandlerModel(String handlerName)
        {
            Handler = handlerName;
        }
            
    }
}
