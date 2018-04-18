using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Atom.VPN.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //var procList = Process.GetProcesses().Where(x => x.ProcessName == Process.GetCurrentProcess().ProcessName).ToList();
            //if (procList.Count > 1)
            //    Environment.Exit(-1);
        }
    }
}
