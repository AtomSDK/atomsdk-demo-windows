using Atom.VPN.Demo.UINotifiers;
using Atom.SDK.Net.Models;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using Atom.VPN.Demo.Extensions;
using Atom.VPN.Demo.Interfaces;
using System.Windows.Controls;
using Atom.VPN.Demo.Helpers;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows;
using Atom.VPN.Demo.Models;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithParams.xaml
    /// </summary>
    public partial class ConnectWithParams : UserControlBase, IConnection
    {
        public ConnectWithParams()
        {
            InitializeComponent();
        }

        public async void Initialize()
        {
            var protocols = new List<Protocol>();
            await Task.Factory.StartNew(() =>
            {
                try { protocols = AtomHelper.GetProtocols(); }
                catch (SDK.Net.AtomException ex) { Messages.ShowMessage(ex); }
            });
            if (ProtocolsCollection == null)
                ProtocolsCollection = protocols.ToObservableCollection();
        }

        private bool _UseOptimization;
        public bool UseOptimization
        {
            get { return _UseOptimization; }
            set
            {
                _UseOptimization = value;
                NotifyOfPropertyChange(() => UseOptimization);
            }
        }

        private ObservableCollection<Country> _CountriesCollection;
        public ObservableCollection<Country> CountriesCollection
        {
            get { return _CountriesCollection; }
            set
            {
                _CountriesCollection = value;
                NotifyOfPropertyChange(() => CountriesCollection);
            }
        }

        private Country selectedCountry;
        public Country SelectedCountry
        {
            get { return selectedCountry; }
            set
            {
                selectedCountry = value;
                NotifyOfPropertyChange(() => SelectedCountry);
            }
        }

        private Protocol _PrimaryProtocol;
        public Protocol PrimaryProtocol
        {
            get { return _PrimaryProtocol; }
            set
            {
                _PrimaryProtocol = value;
                SetCountries();
                NotifyOfPropertyChange(() => PrimaryProtocol);
            }
        }

        private Protocol _SecondaryProtocol;
        public Protocol SecondaryProtocol
        {
            get { return _SecondaryProtocol; }
            set
            {
                _SecondaryProtocol = value;
                SetCountries();
                NotifyOfPropertyChange(() => SecondaryProtocol);
            }
        }

        private Protocol _TertiaryProtocol;
        public Protocol TertiaryProtocol
        {
            get { return _TertiaryProtocol; }
            set
            {
                _TertiaryProtocol = value;
                SetCountries();
                NotifyOfPropertyChange(() => TertiaryProtocol);
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

        private async void SetCountries()
        {
            try
            {
                if (PrimaryProtocol != null)
                {
                    CountriesCollection = new ObservableCollection<Country>();
                    var countries = new List<Country>();
                    await Task.Factory.StartNew(()=> countries = AtomHelper.GetCountries());

                    if (countries != null)
                    {
                        if (SecondaryProtocol != null && TertiaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.Id == PrimaryProtocol.Id) != null &&
                            x.Protocols.FirstOrDefault(y => y.Id == SecondaryProtocol.Id) != null &&
                            x.Protocols.FirstOrDefault(y => y.Id == TertiaryProtocol.Id) != null
                            ).ToObservableCollection();
                        }
                        else if (SecondaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.Id == PrimaryProtocol.Id) != null &&
                            x.Protocols.FirstOrDefault(y => y.Id == SecondaryProtocol.Id) != null
                            ).ToObservableCollection();
                        }
                        else if (TertiaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.Id == PrimaryProtocol.Id) != null &&
                            x.Protocols.FirstOrDefault(y => y.Id == TertiaryProtocol.Id) != null
                            ).ToObservableCollection();
                        }
                        else
                        {
                            CountriesCollection = countries.Where(x => 
                            x.Protocols.FirstOrDefault(y => y.Id == PrimaryProtocol.Id) != null
                            ).ToObservableCollection();
                        }
                    }
                }
            }
            catch { }
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

        public bool CanConnect { get { return PrimaryProtocol != null && SelectedCountry != null; } }

        private bool StartConnection()
        {
            if (!CanConnect)
            {
                Messages.ShowMessage(Messages.SelectProtocolCountry);
                return false;
            }

            var properties = new VPNProperties(SelectedCountry, PrimaryProtocol);
            properties.UseOptimization = UseOptimization;
            properties.SecondaryProtocol = SecondaryProtocol;
            properties.TertiaryProtocol = TertiaryProtocol;
            AtomHelper.Connect(properties);
            return true;
        }

    }
}
