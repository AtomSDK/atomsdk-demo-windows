using Atom.Core.Models;
using Atom.SDK.Core;
using Atom.SDK.Net;
using Atom.VPN.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atom.SDK.Core.Models;
using System.Windows;

namespace Atom.VPN.Demo.Helpers
{
    /// <summary>
    /// A simple helper class for AtomManager
    /// </summary>
    internal static class AtomHelper
    {
        /// <summary>
        /// Instance of AtomManager to be used for VPN connection
        /// </summary>
        internal static AtomManager AtomManagerInstance { get; set; }

        /// <summary>
        /// Sets the AtomManagerInstance property
        /// </summary>
        /// <param name="atomManager">An instance of AtomManager</param>
        internal static void SetAtomManagerInstance(AtomManager atomManager)
        {
            AtomManagerInstance = atomManager;
        }

        /// <summary>
        /// Initializes a singleton of AtomManager
        /// </summary>
        /// <param name="atomConfiguration">Configuration settings for AtomManager instance</param>
        internal static void Initialize(AtomConfiguration atomConfiguration)
        {
            AtomManagerInstance = AtomManager.Initialize(atomConfiguration);
        }

        /// <summary>
        /// Initializes a singleton of AtomManager
        /// </summary>
        /// <param name="secretKey">Key to be used for Initializing AtomManager instance</param>
        internal static void Initialize(string secretKey)
        {
            AtomManagerInstance = AtomManager.Initialize(secretKey);
        }

        /// <summary>
        /// Validates connection
        /// </summary>
        /// <returns>A Validation object which specifies if connection is valid</returns>
        internal static ResponseValidation ValidateConnection()
        {
            var result = new ResponseValidation();
            var isError = false;
            if (IsAutoCredMode)
            {
                isError = String.IsNullOrEmpty(UUID);
                if (isError)
                    result.Message = Messages.UUIDRequired;
            }
            else
            {
                isError = String.IsNullOrEmpty(Username) || String.IsNullOrEmpty(Password);
                if (!isError)
                    AtomManagerInstance.Credentials = new Credentials(Username, Password);
                else
                    result.Message = Messages.CredentialsRequired;
            }
            result.IsValid = !isError;
            return result;
        }

        /// <summary>
        /// Connect using AtomManager
        /// </summary>
        /// <param name="properties">Properties to be used for connection</param>
        internal static void Connect(VPNProperties properties)
        {
            AtomManagerInstance.Connect(properties);
        }

        /// <summary>
        /// Disconnects the VPN
        /// </summary>
        internal static void Disconnect()
        {
            AtomManagerInstance.Disconnect();
        }

        /// <summary>
        /// Cancels the ongoing VPN connection
        /// </summary>
        internal static void Cancel()
        {
            AtomManagerInstance.Cancel();
        }

        /// <summary>
        /// Registers Connected Event
        /// </summary>
        /// <param name="onConnected">EventHandler for Connected event</param>
        internal static void RegisterConnectedEvent(EventHandler<ConnectedEventArgs> onConnected)
        {
            AtomManagerInstance.Connected += onConnected;
        }

        /// <summary>
        /// Registers Disconnected Event
        /// </summary>
        /// <param name="onConnect">EventHandler for Disconnected event</param>
        internal static void RegisterDisconnectedEvent(EventHandler<DisconnectedEventArgs> onDisconnected)
        {
            AtomManagerInstance.Disconnected += onDisconnected;
        }

        /// <summary>
        /// Registers DialError Event
        /// </summary>
        /// <param name="onConnect">EventHandler for DialError event</param>
        internal static void RegisterDialErrorEvent(EventHandler<DialErrorEventArgs> onDialError)
        {
            AtomManagerInstance.DialError += onDialError;
        }

        /// <summary>
        /// Registers StateChanged Event
        /// </summary>
        /// <param name="onConnect">EventHandler for StateChanged event</param>
        internal static void RegisterStateChangedEvent(EventHandler<StateChangedEventArgs> onStateChanged)
        {
            AtomManagerInstance.StateChanged += onStateChanged;
        }

        /// <summary>
        /// Registers Redialing Event
        /// </summary>
        /// <param name="onConnect">EventHandler for Redialing event</param>
        internal static void RegisterRedialingEvent(EventHandler<ErrorEventArgs> onRedial)
        {
            AtomManagerInstance.Redialing += onRedial;
        }

        /// <summary>
        /// Gets and Sets the UUID to be used for connection
        /// </summary>
        internal static string UUID
        {
            get { return AtomManagerInstance.UUID; }
            set { AtomManagerInstance.UUID = value; }
        }

        /// <summary>
        /// Gets and Sets the Username to be used for credentials of connection
        /// </summary>
        internal static string Username { get; set; }

        /// <summary>
        /// Gets and Sets the Password to be used for credentials of connection
        /// </summary>
        internal static string Password { get; set; }

        /// <summary>
        /// Specifies if the credentials are to be generated automatically using a UUID
        /// </summary>
        internal static bool IsAutoCredMode { get; set; }

        /// <summary>
        /// Fetches the list of protocols using AtomManager instance
        /// </summary>
        /// <returns>List of allowed Protocols</returns>
        internal static List<Protocol> GetProtocols()
        {
            return AtomManagerInstance.GetProtocols();
        }

        /// <summary>
        /// Fetches the list of countries using AtomManager instance
        /// </summary>
        /// <returns>List of allowed Countries</returns>
        internal static List<Country> GetCountries()
        {
            return AtomManagerInstance.GetCountries();
        }

        /// <summary>
        /// Fetches the list of smart countries using AtomManager instance
        /// </summary>
        /// <returns>List of allowed Countries</returns>
        internal static List<Country> GetSmartCountries()
        {
            return AtomManagerInstance.GetCountriesForSmartDialing();
        }

        /// <summary>
        /// Fetches the list of cities using AtomManager instance
        /// </summary>
        /// <returns>List of allowed cities</returns>
        internal static List<City> GetCities()
        {
            try
            {
                return AtomManagerInstance.GetCities();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Fetches the list of channels using AtomManager instance
        /// </summary>
        /// <returns>List of allowed channels</returns>
        internal static List<Channel> GetChannels()
        {
            try
            {
                return AtomManagerInstance.GetChannels();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }

    }
}
