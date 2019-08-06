using Atom.VPN.Demo.UINotifiers;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Helpers;
using Atom.VPN.Demo.Models;
using Atom.Core.Models;
using Atom.SDK.Core.Models;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithPSK.xaml
    /// </summary>
    public partial class ConnectWithPSK : UserControlBase, IConnection
    {
        public List<Protocol> Protocols { get; set; }
        public List<Country> Countries { get; set; }

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

        public void Connect()
        {
            var response = AtomHelper.ValidateConnection();
            if (response.IsValid)
            {
                ParentWindow.ShowConnectingState();
                if (!StartConnection())
                    ParentWindow.ShowDisconnectedState();
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

        public void Initialize(List<Protocol> protocols = null, List<Country> countries = null)
        {
            
        }
    }
}
