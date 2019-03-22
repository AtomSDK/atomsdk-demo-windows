using Atom.VPN.Demo.UINotifiers;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Atom.VPN.Demo.Interfaces;
using System.Windows.Controls;
using Atom.VPN.Demo.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Atom.VPN.Demo.Helpers;
using System;
using Atom.VPN.Demo.Models;
using Atom.SDK.Core.Models;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithDedicatedIP.xaml
    /// </summary>
    public partial class ConnectWithDedicatedIP : UserControlBase, IConnection
    {
        public ConnectWithDedicatedIP()
        {
            InitializeComponent();
        }

        private bool _IsSkippingVerification;
        public bool IsSkippingVerification
        {
            get { return _IsSkippingVerification; }
            set
            {
                _IsSkippingVerification = value;
                NotifyOfPropertyChange(() => IsSkippingVerification);
            }
        }

        private string _Host = string.Empty;
        public string Host
        {
            get { return _Host.Trim(); }
            set
            {
                _Host = value;
                NotifyOfPropertyChange(() => Host);
            }
        }

        private ObservableCollection<Protocol> _ProtocolsCollection;
        public ObservableCollection<Protocol> ProtocolsCollection
        {
            get { return _ProtocolsCollection; }
            set
            {
                _ProtocolsCollection = value;
                NotifyOfPropertyChange(() => ProtocolsCollection);
            }
        }

        private Protocol _PrimaryProtocol;
        public Protocol PrimaryProtocol
        {
            get { return _PrimaryProtocol; }
            set
            {
                _PrimaryProtocol = value;
                NotifyOfPropertyChange(() => PrimaryProtocol);
            }
        }

        public async void Initialize()
        {
            var protocols = new List<Protocol>();
            await Task.Factory.StartNew(() =>
            {
                try { protocols = AtomHelper.GetProtocols(); }
                catch (SDK.Core.AtomException ex) { Messages.ShowMessage(ex); }
            });
            if (ProtocolsCollection == null)
                ProtocolsCollection = protocols.ToObservableCollection();
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

        public bool CanConnect { get { return !String.IsNullOrEmpty(Host) && PrimaryProtocol != null; } }

        private bool StartConnection()
        {
            if (!CanConnect)
            {
                Messages.ShowMessage(Messages.HostAndProtocolRequired);
                return false;
            }
            var properties = new VPNProperties(Host, PrimaryProtocol);
            properties.SkipUserVerification = IsSkippingVerification;
            AtomHelper.Connect(properties);
            return true;
        }

        private void DedicatedIPBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                Connect();
        }
    }
}
