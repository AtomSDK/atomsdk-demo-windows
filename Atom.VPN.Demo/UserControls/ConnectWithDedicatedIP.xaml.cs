using Atom.VPN.Demo.UINotifiers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Extensions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Atom.VPN.Demo.Helpers;
using System;
using Atom.VPN.Demo.Models;
using Atom.Core.Models;
using Atom.SDK.Core.Models;
using Atom.Core.Enums;
using System.Linq;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithDedicatedIP.xaml
    /// </summary>
    public partial class ConnectWithDedicatedIP : UserControlBase, IConnection
    {
        public List<Protocol> Protocols { get; set; }
        public List<Country> Countries { get; set; }

        public ConnectWithDedicatedIP()
        {
            InitializeComponent();
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

        private ObservableCollection<SmartConnectTag> _SmartConnectTagsCollection;
        public ObservableCollection<SmartConnectTag> SmartConnectTagsCollection
        {
            get { return _SmartConnectTagsCollection; }
            set
            {
                _SmartConnectTagsCollection = value;
                NotifyOfPropertyChange(() => SmartConnectTagsCollection);
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

        private bool _UseSmartConnect;
        public bool UseSmartConnect
        {
            get { return _UseSmartConnect; }
            set
            {
                _UseSmartConnect = value;
                NotifyOfPropertyChange(() => UseSmartConnect);
            }
        }

        private bool _UseDedicatedIP = true;
        public bool UseDedicatedIP
        {
            get { return _UseDedicatedIP; }
            set
            {
                _UseDedicatedIP = value;
                NotifyOfPropertyChange(() => UseDedicatedIP);
            }
        }

        public async void Initialize(List<Protocol> protocols = null, List<Country> countries = null)
        {
            Protocols = protocols;
            Countries = countries;

            if (ProtocolsCollection == null)
                ProtocolsCollection = protocols.ToObservableCollection();

            SmartConnectTagsCollection = GetSmartConnectTags().ToObservableCollection();
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
            VPNProperties properties;

            try
            {

                if (UseDedicatedIP)
                {
                    if (!CanConnect)
                    {
                        Messages.ShowMessage(Messages.HostAndProtocolRequired);
                        return false;
                    }

                    properties = new VPNProperties(Host, PrimaryProtocol);
                }
                else if (UseSmartConnect)
                {
                    List<SmartConnectTag> smartConnectTagsList = new List<SmartConnectTag>();

                    if (SmartConnectTagsListBox?.SelectedItems?.Count > 0)
                    {
                        foreach (var item in SmartConnectTagsListBox.SelectedItems)
                        {
                            smartConnectTagsList.Add((SmartConnectTag)item);
                        }
                    }

                    bool isValidate = AtomHelper.AtomManagerInstance.IsSmartConnectAvailableOnProtocol(PrimaryProtocol, smartConnectTagsList);

                    if (isValidate)
                        properties = new VPNProperties(PrimaryProtocol, smartConnectTagsList?.Count > 0 ? smartConnectTagsList : null);
                    else
                    {
                        ParentWindow.ConnectionDialog += "No SmartConnect DNS available for dialing or you are not permitted to this resource";
                        return false;
                    }
                }
                else
                    return false;

                AtomHelper.Connect(properties);
                return true;
            }
            catch (Exception ex)
            {
                ParentWindow.ConnectionDialog += ex.Message;
                return false;
            }
        }

        private void DedicatedIPBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
                Connect();
        }

        private List<SmartConnectTag> GetSmartConnectTags()
        {
            return Enum.GetValues(typeof(SmartConnectTag)).Cast<SmartConnectTag>().ToList();
        }
    }
}
