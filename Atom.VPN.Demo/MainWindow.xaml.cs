using Atom.SDK.Net;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Atom.VPN.Demo.UINotifiers;
using Atom.VPN.Demo.UserControls;
using Atom.VPN.Demo.Interfaces;
using Atom.VPN.Demo.Helpers;
using Atom.VPN.Demo.Extensions;
using Atom.VPN.Demo.Models;
using Atom.SDK.Core;
using System.Collections.Generic;
using Atom.Core.Models;
using Atom.SDK.Core.Enumerations;

namespace Atom.VPN.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        AtomManager atomManagerInstance;

        #region Properties

        #region TabItems (Controls)

        private IConnection _ConnectionWithParams;
        public IConnection ConnectionWithParams
        {
            get { return (_ConnectionWithParams = _ConnectionWithParams ?? new ConnectWithParams()); }
        }

        private IConnection _ConnectionWithPSK;
        public IConnection ConnectionWithPSK
        {
            get { return (_ConnectionWithPSK = _ConnectionWithPSK ?? new ConnectWithPSK()); }
        }

        private IConnection _ConnectionWithDedicatedIP;
        public IConnection ConnectionWithDedicatedIP
        {
            get { return (_ConnectionWithDedicatedIP = _ConnectionWithDedicatedIP ?? new ConnectWithDedicatedIP()); }
        }

        private TabItem _SelectedTab;
        public TabItem SelectedTab
        {
            get { return _SelectedTab; }
            set
            {
                _SelectedTab = value;
                NotifyOfPropertyChange(() => SelectedTab);
            }
        }

        #endregion

        private bool _IsAutoCredMode;
        public bool IsAutoCredMode
        {
            get { return _IsAutoCredMode; }
            set
            {
                _IsAutoCredMode = value;
                AtomHelper.IsAutoCredMode = value;
                NotifyOfPropertyChange(() => IsAutoCredMode);
            }
        }

        private string _UUID = string.Empty;

        public string UUID
        {
            get { return _UUID.Trim(); }
            set
            {
                _UUID = value;
                AtomHelper.UUID = value;
                NotifyOfPropertyChange(() => UUID);
            }
        }

        private string _Username = string.Empty;

        public string Username
        {
            get { return _Username.Trim(); }
            set
            {
                _Username = value;
                AtomHelper.Username = value;
                NotifyOfPropertyChange(() => Username);
            }
        }

        private string _Password = string.Empty;

        public string Password
        {
            get { return _Password.Trim(); }
            set
            {
                _Password = value;
                AtomHelper.Password = value;
                NotifyOfPropertyChange(() => Password);
            }
        }

        private bool _IsConnDisconnAllowed;

        public bool IsConnDisconnAllowed
        {
            get { return _IsConnDisconnAllowed; }
            set
            {
                _IsConnDisconnAllowed = value;
                NotifyOfPropertyChange(() => IsConnDisconnAllowed);
            }
        }

        private bool _IsDisconnected = true;

        public bool IsDisconnected
        {
            get { return _IsDisconnected; }
            set
            {
                _IsDisconnected = value;
                if (value)
                    ActionButtonText = Messages.Connect;
                NotifyOfPropertyChange(() => IsDisconnected);
            }
        }

        private bool _IsConnected;

        public bool IsConnected
        {
            get { return _IsConnected; }
            set
            {
                _IsConnected = value;
                if (value)
                    ActionButtonText = Messages.Disconnect;
                NotifyOfPropertyChange(() => IsConnected);
            }
        }

        private bool _IsConnecting;

        public bool IsConnecting
        {
            get { return _IsConnecting; }
            set
            {
                _IsConnecting = value;
                if (value)
                    ActionButtonText = Messages.Cancel;
                NotifyOfPropertyChange(() => IsConnecting);
            }
        }

        private string _ConnectionDialog;

        public string ConnectionDialog
        {
            get { return _ConnectionDialog; }
            set
            {
                _ConnectionDialog = value;
                NotifyOfPropertyChange(() => ConnectionDialog);
            }
        }

        private bool _ISSDKInitialized;

        public bool ISSDKInitialized
        {
            get { return _ISSDKInitialized; }
            set
            {
                _ISSDKInitialized = value;
                NotifyOfPropertyChange(() => ISSDKInitialized);
            }
        }

        private bool _IsSDKInitializing;

        public bool IsSDKInitializing
        {
            get { return _IsSDKInitializing; }
            set
            {
                _IsSDKInitializing = value;
                NotifyOfPropertyChange(() => IsSDKInitializing);
            }
        }

        private string _SecretKey = string.Empty;

        public string SecretKey
        {
            get { return _SecretKey.Trim(); }
            set
            {
                _SecretKey = value;
                NotifyOfPropertyChange(() => SecretKey);
            }
        }

        private string _ActionButtonText = Messages.Connect;

        public string ActionButtonText
        {
            get { return _ActionButtonText; }
            set
            {
                _ActionButtonText = value;
                NotifyOfPropertyChange(() => ActionButtonText);
            }
        }

        private Messages _Messages;
        public Messages Messages { get { return (_Messages = _Messages ?? new Messages()); } }

        #endregion

        private void InitializeSDK(object sender, RoutedEventArgs e)
        {
            InitializeSDK();
        }

        private async void InitializeSDK()
        {
            if (String.IsNullOrEmpty(SecretKey))
            {
                Messages.ShowMessage(Messages.SecretKeyRquired);
                return;
            }

            IsSDKInitializing = true;

            var countries = new List<Country>();
            var protocols = new List<Protocol>();

            await Task.Factory.StartNew(() =>
            {
                //Can be initialized using this
                atomManagerInstance = AtomManager.Initialize(SecretKey);
                //Or this
                //var atomConfig = new AtomConfiguration(SecretKey);
                //atomManagerInstance = AtomManager.Initialize(atomConfig);

                atomManagerInstance.Connected += AtomManagerInstance_Connected;
                atomManagerInstance.DialError += AtomManagerInstance_DialError;
                atomManagerInstance.Disconnected += AtomManagerInstance_Disconnected;
                atomManagerInstance.StateChanged += AtomManagerInstance_StateChanged;
                atomManagerInstance.Redialing += AtomManagerInstance_Redialing;
                atomManagerInstance.OnUnableToAccessInternet += AtomManagerInstance_OnUnableToAccessInternet;
                atomManagerInstance.SDKAlreadyInitialized += AtomManagerInstance_SDKAlreadyInitialized;
                atomManagerInstance.ConnectedLocation += AtomManagerInstance_ConnectedLocation;
                
                atomManagerInstance.AutoRedialOnConnectionDrop = true;

                //To get countries
                try
                {
                    countries = atomManagerInstance.GetCountries();
                }
                catch { }

                //To get protocols
                try
                {
                    protocols = atomManagerInstance.GetProtocols();
                }
                catch { }

                //AtomHelper lets you use the functionality of above created instance in all usercontrols and pages
                AtomHelper.SetAtomManagerInstance(atomManagerInstance);
            });

            ConnectionWithDedicatedIP.Initialize(protocols, countries);
            ConnectionWithParams.Initialize(protocols, countries);
            IsSDKInitializing = false;
            ISSDKInitialized = true;
            IsConnDisconnAllowed = true;
        }

        private void AtomManagerInstance_ConnectedLocation(object sender, ConnectedLocationEventArgs e)
        {
            try
            {
                var location = atomManagerInstance.GetConnectedLocation();

                if (location != null)
                {
                    ConnectionDialog += $"IP: {location.Ip} {Environment.NewLine}";

                    if (location.Country != null)
                        ConnectionDialog += $"Country: {location.Country.Name} {Environment.NewLine}";

                    if (location.City != null)
                        ConnectionDialog += $"City: {location.City.Name} {Environment.NewLine}";
                }
            }
            catch (Exception ex)
            {
                ConnectionDialog += ex.Message + Environment.NewLine;

                if (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    ConnectionDialog += ex.InnerException.Message + Environment.NewLine;

                    if (ex.InnerException.InnerException != null && string.IsNullOrWhiteSpace(ex.InnerException.InnerException.Message))
                        ConnectionDialog += ex.InnerException.InnerException.Message + Environment.NewLine;
                    else
                        ConnectionDialog += "No other inner exception message" + Environment.NewLine;
                }
            }
        }

        #region AtomRegisteredEvents

        private void AtomManagerInstance_SDKAlreadyInitialized(object sender, ErrorEventArgs e)
        {
            Dispatcher.Invoke(() => MessageBox.Show(e.Message + Environment.NewLine + e.Exception?.Message));
        }

        private void AtomManagerInstance_OnUnableToAccessInternet(object sender, SDK.Core.CustomEventArgs.UnableToAccessInternetEventArgs e)
        {
            var a = e.ConnectionDetails;
            string message = "UTB Occured, Due want to Reconnect?";
            string caption = "Confirmation";
            MessageBoxButton buttons = MessageBoxButton.YesNo;
            MessageBoxImage icon = MessageBoxImage.Question;

            if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
            {
                ConnectionDialog += "Reconnecting..." + Environment.NewLine;
                ShowConnectingState(false);
                atomManagerInstance.ReConnect();
            }
        }

        private void AtomManagerInstance_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (e.State == VPNState.RECONNECTING)
            {
                ShowConnectingState(false);
            }

            ConnectionDialog += e.State.ToString() + Environment.NewLine;
        }

        private void AtomManagerInstance_Connected(object sender, EventArgs e)
        {
            ConnectionDialog += "CONNECTED" + Environment.NewLine;
            IsDisconnected = false;
            IsConnecting = false;
            IsConnected = true;
            IsConnDisconnAllowed = true;

            if (atomManagerInstance.VPNProperties.UseSplitTunneling)
            {
                atomManagerInstance.ApplySplitTunneling(new SDK.Core.Models.SplitApplication() { CompleteExePath = "Chrome.exe" });
            }
        }

        private void AtomManagerInstance_Disconnected(object sender, DisconnectedEventArgs e)
        {
            ConnectionDialog += e.Cancelled ? "CANCELLED" : "DISCONNECTED" + Environment.NewLine;
            IsDisconnected = true;
            IsConnecting = false;
            IsConnected = false;
            IsConnDisconnAllowed = true;
        }

        private void AtomManagerInstance_DialError(object sender, DialErrorEventArgs e)
        {
            ConnectionDialog += e.Message + Environment.NewLine;
            IsDisconnected = true;
            IsConnecting = false;
            IsConnected = false;
            IsConnDisconnAllowed = true;
        }

        private void AtomManagerInstance_Redialing(object sender, ErrorEventArgs e)
        {
            ConnectionDialog += e.Message + Environment.NewLine;
        }

        #endregion

        // Indicates Connecting State
        public void ShowConnectingState(bool isClear = true)
        {
            if (isClear)
                ConnectionDialog = string.Empty;

            IsConnDisconnAllowed = false;
            IsDisconnected = false;
            IsConnecting = true;
        }

        public void ShowDisconnectedState()
        {
            IsDisconnected = true;
            IsConnecting = false;
            IsConnected = false;
            IsConnDisconnAllowed = true;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnecting)
                Cancel();
            else if (IsConnected)
                Disconnect();
            else
                Connect();
        }

        // Here we call the connect method of the Selected Tab
        private void Connect()
        {
            var type = (SelectedTab.Content as ContentControl).Content.GetType();

            var connection =
                type.Equals<ConnectWithPSK>() ? ConnectionWithPSK :
                type.Equals<ConnectWithDedicatedIP>() ? ConnectionWithDedicatedIP :
                ConnectionWithParams;

            connection.Connect();
        }

        private void Disconnect()
        {
            try
            {
                AtomHelper.Disconnect();
            }
            catch (Exception ex)
            {
                ConnectionDialog += $"{ex.Message}";
            }
        }

        private void Cancel()
        {
            try
            {
                AtomHelper.Cancel();
            }
            catch (Exception ex)
            {
                ConnectionDialog += $"{ex.Message}";
            }
        }

        private void ClosingApp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!IsDisconnected)
            {
                Messages.ShowMessage(Messages.DisconnectBeforeExit);
                e.Cancel = true;
            }
        }

        private void SecretKeyBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter || e.Key == System.Windows.Input.Key.Return)
                InitializeSDK();
        }

    }
}
