using Atom.VPN.Demo.UINotifiers;
using Atom.SDK.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Helpers;
using Atom.VPN.Demo.Models;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithPSK.xaml
    /// </summary>
    public partial class ConnectWithPSK : UserControlBase, IConnection
    {
        public ConnectWithPSK()
        {
            InitializeComponent();
        }

        private string _PSK = string.Empty;
        public string PSK
        {
            get { return _PSK.Trim(); }
            set
            {
                _PSK = value;
                NotifyOfPropertyChange(() => PSK);
            }
        }

        public void Initialize()
        {

        }

        public void Connect()
        {
            var response = AtomHelper.ValidateConnection();
            if (response.IsValid)
            {
                bool isConnecting = StartConnection();
                if (isConnecting)
                    ParentWindow.ShowConnectingState();
            }
            else
                Messages.ShowMessage(response.Message);
        }

        public bool CanConnect { get { return !String.IsNullOrEmpty(PSK); } }

        private bool StartConnection()
        {
            if (!CanConnect)
            {
                Messages.ShowMessage(Messages.PSKRequired);
                return false;
            }
            var properties = new VPNProperties(PSK);
            AtomHelper.Connect(properties);
            return true;
        }

        private void PSKBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                Connect();
        }
        
    }
}
