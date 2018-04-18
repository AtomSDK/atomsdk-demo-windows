using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.VPN.Demo.Models
{
    public sealed class Messages
    {
        public const string UUIDRequired ="UUID is required for generating vpn credentials to connect VPN.";
        public const string CredentialsRequired = "Username & Password is required for connecting VPN.";
        public const string SelectProtocolCountry = "Select a primary protocol and country to connect";
        public const string Success ="Success";
        public const string HostAndProtocolRequired ="Enter a host and select a protocol to continue.";
        public const string SecretKeyRquired ="Secret Key is required.";
        public const string DisconnectBeforeExit ="Disconnect VPN before closing app.";
        public const string PSKRequired ="Enter your Pre-Shared Key to continue.";
        public const string Connect ="Connect";
        public const string Disconnect ="Disconnect";
        public const string Cancel ="Cancel";

        public string TooltipAutoGenCred { get { return  "If checked, a unique user identifier (UUID) is required to generate username and password."; } }
        public string TooltipUUID { get { return  "A unique user identifier such as an email, to generate username and password"; } }
        public string TooltipCred { get { return  "Credentials required for connection when \"Auto Generate user credentials\" is not checked."; } }
        public string TooltipPSK { get { return  "A Pre-Shared Key is generated using your selection of country or protocol which is used to get fastest servers for connection."; } }
        public string TooltipDedIP { get { return  "A dedicated IP/Host is allowed to particular usernames. Enter if you are allowed one."; } }
        public string TooltipSkipVerif { get { return  "Connects to your specified host even if not allowed to your username."; } }
        public string TooltipPrimaryProtocol { get { return  "This protocol will be used as primary protocol to dial the vpn connection."; } }
        public string TooltipSecondaryProtocol { get { return  "This protocol will be used as secondary protocol to dial the vpn connection."; } }
        public string TooltipTertiaryProtocol { get { return  "This protocol will be used as tertiary protocol to dial the vpn connection."; } }
        public string TooltipCountry { get { return  "An attempt to connect will get fastest servers from the selected country."; } }
        public string TootlipOptimization { get { return  "If checked, fastest servers will be fetched based on the smartest ping response."; } }
        public string TooltipCallbacks { get { return  "Displays connection callbacks"; } }

    }
}
