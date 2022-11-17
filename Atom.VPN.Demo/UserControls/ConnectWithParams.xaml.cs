using Atom.Core.Models;
using Atom.SDK.Core.Models;
using Atom.VPN.Demo.Extensions;
using Atom.VPN.Demo.Helpers;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Models;
using Atom.VPN.Demo.UINotifiers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Atom.VPN.Demo.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWithParams.xaml
    /// </summary>
    public partial class ConnectWithParams : UserControlBase, IConnection
    {
        public List<Protocol> Protocols { get; set; }
        public List<Country> Countries { get; set; }

        public ConnectWithParams()
        {
            InitializeComponent();
        }

        public async void Initialize(List<Protocol> protocols = null, List<Country> countries = null)
        {
            Protocols = protocols;
            Countries = countries;

            if (ProtocolsCollection == null)
                ProtocolsCollection = protocols.ToObservableCollection();

            CitiesCollection = AtomHelper.GetCities().ToObservableCollection();
            ChannelsCollection = AtomHelper.GetChannels().ToObservableCollection();
            UseCountryConnection = true;
        }

        private bool _UseOptimization;
        public bool UseOptimization
        {
            get { return _UseOptimization; }
            set
            {
                _UseOptimization = value;

                if (value)
                    UseSmartDialing = false;

                NotifyOfPropertyChange(() => UseOptimization);
            }
        }

        private bool _UseSplitTunneling;
        public bool UseSplitTunneling
        {
            get { return _UseSplitTunneling; }
            set
            {
                _UseSplitTunneling = value;
                NotifyOfPropertyChange(() => UseSplitTunneling);
            }
        }

        private bool _UseSmartDialing;
        public bool UseSmartDialing
        {
            get { return _UseSmartDialing; }
            set
            {
                _UseSmartDialing = value;

                if (!value)
                    SetCountries();
                else
                    SetCountries(isUseSmartConnect: true);

                if (value)
                    UseOptimization = false;

                NotifyOfPropertyChange(() => UseSmartDialing);
            }
        }

        private bool _UseCountryConnection;

        public bool UseCountryConnection
        {
            get => _UseCountryConnection;
            set
            {
                _UseCountryConnection = value;
                NotifyOfPropertyChange(() => UseCountryConnection);
            }
        }

        private bool _UseCityConnection;

        public bool UseCityConnection
        {
            get => _UseCityConnection;
            set
            {
                _UseCityConnection = value;
                NotifyOfPropertyChange(() => UseCityConnection);
            }
        }

        private bool _UseChannelConnection;

        public bool UseChannelConnection
        {
            get => _UseChannelConnection;
            set
            {
                _UseChannelConnection = value;
                NotifyOfPropertyChange(() => UseChannelConnection);
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

        private ObservableCollection<City> _CitiesCollection;
        public ObservableCollection<City> CitiesCollection
        {
            get { return _CitiesCollection; }
            set
            {
                _CitiesCollection = value;
                NotifyOfPropertyChange(() => CitiesCollection);
            }
        }

        private ObservableCollection<Channel> _ChannelsCollection;
        public ObservableCollection<Channel> ChannelsCollection
        {
            get { return _ChannelsCollection; }
            set
            {
                _ChannelsCollection = value;
                NotifyOfPropertyChange(() => ChannelsCollection);
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

        private City selectedCity;
        public City SelectedCity
        {
            get { return selectedCity; }
            set
            {
                selectedCity = value;
                NotifyOfPropertyChange(() => SelectedCity);
            }
        }

        private Channel selectedChannel;
        public Channel SelectedChannel
        {
            get { return selectedChannel; }
            set
            {
                selectedChannel = value;
                NotifyOfPropertyChange(() => SelectedChannel);
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

        private async void SetCountries(bool isUseSmartConnect = false)
        {
            try
            {
                if (PrimaryProtocol != null)
                {
                    CountriesCollection = new ObservableCollection<Country>();
                    var countries = new List<Country>();

                    if (isUseSmartConnect)
                        await Task.Factory.StartNew(() => countries = AtomHelper.GetSmartCountries());
                    else
                        await Task.Factory.StartNew(() => countries = AtomHelper.GetCountries());

                    if (countries != null)
                    {
                        if (SecondaryProtocol != null && TertiaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == PrimaryProtocol.ProtocolSlug) != null &&
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == SecondaryProtocol.ProtocolSlug) != null &&
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == TertiaryProtocol.ProtocolSlug) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else if (SecondaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == PrimaryProtocol.ProtocolSlug) != null &&
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == SecondaryProtocol.ProtocolSlug) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else if (TertiaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == PrimaryProtocol.ProtocolSlug) != null &&
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == TertiaryProtocol.ProtocolSlug) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.ProtocolSlug == PrimaryProtocol.ProtocolSlug) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
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
                ParentWindow.ShowConnectingState();
                if (!StartConnection())
                    ParentWindow.ShowDisconnectedState();
            }
            else
                Messages.ShowMessage(response.Message);
        }

        public bool CanConnect { get { return PrimaryProtocol != null && SelectedCountry != null; } }

        private bool StartConnection()
        {
            try
            {
                if (!CanConnect)
                {
                    Messages.ShowMessage(Messages.SelectProtocolCountry);
                    return false;
                }

                VPNProperties properties;

                if (UseCountryConnection)
                {
                    properties = new VPNProperties(SelectedCountry, PrimaryProtocol);
                    properties.UseOptimization = UseOptimization;
                    properties.UseSmartDialing = UseSmartDialing;
                }
                else if (UseCityConnection)
                {
                    properties = new VPNProperties(SelectedCity, PrimaryProtocol);
                    properties.UseOptimization = UseOptimization;
                }
                else if (UseChannelConnection)
                {
                    properties = new VPNProperties(SelectedChannel, PrimaryProtocol);
                }
                else
                    return false;

                properties.SecondaryProtocol = SecondaryProtocol;
                properties.TertiaryProtocol = TertiaryProtocol;
                properties.UseSplitTunneling = UseSplitTunneling;
                properties.DoCheckInternetConnectivity = true;
                properties.EnableDNSLeakProtection = true;
                properties.EnableIPv6LeakProtection = true;
                properties.EnableIKS = true;

                AtomHelper.Connect(properties);

                return true;
            }
            catch (Exception e) 
            {
                ParentWindow.ConnectionDialog += e.Message + Environment.NewLine;
                return false;
            }
        }

    }
}
