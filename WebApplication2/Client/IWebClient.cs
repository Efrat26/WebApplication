using ex2_AP2.Settings.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication2.Client
{
    public interface IWebClient
    {
        IClient Client { get; set; }
        void Connect();
    }
}
