using Atom.SDK.Core.Models;
using Atom.VPN.Demo.Extensions;
using Atom.VPN.Demo.Helpers;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Models;
using Atom.VPN.Demo.UINotifiers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
                catch (SDK.Core.AtomException ex) { Messages.ShowMessage(ex); }
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

                if (value)
                    UseSmartDialing = false;

                NotifyOfPropertyChange(() => UseOptimization);
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
                            x.Protocols.FirstOrDefault(y => y.number == PrimaryProtocol.number) != null &&
                            x.Protocols.FirstOrDefault(y => y.number == SecondaryProtocol.number) != null &&
                            x.Protocols.FirstOrDefault(y => y.number == TertiaryProtocol.number) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else if (SecondaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.number == PrimaryProtocol.number) != null &&
                            x.Protocols.FirstOrDefault(y => y.number == SecondaryProtocol.number) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else if (TertiaryProtocol != null)
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.number == PrimaryProtocol.number) != null &&
                            x.Protocols.FirstOrDefault(y => y.number == TertiaryProtocol.number) != null
                            ).ToObservableCollection();

                            SelectedCountry = CountriesCollection.FirstOrDefault();
                        }
                        else
                        {
                            CountriesCollection = countries.Where(x =>
                            x.Protocols.FirstOrDefault(y => y.number == PrimaryProtocol.number) != null
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
            if (!CanConnect)
            {
                Messages.ShowMessage(Messages.SelectProtocolCountry);
                return false;
            }

            var properties = new VPNProperties(SelectedCountry, PrimaryProtocol);
            properties.UseOptimization = UseOptimization;
            properties.UseSmartDialing = UseSmartDialing;
            properties.SecondaryProtocol = SecondaryProtocol;
            properties.TertiaryProtocol = TertiaryProtocol;
            AtomHelper.Connect(properties);
            return true;
        }

    }
}
