# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

### Version 4.5.1

### Fixed
- Update Obfuscation

### Version 4.5.0

### Added

#### New Dedicated VPS Dialing
- Introducing the capability for users to connect through a dedicated Virtual Private Server (VPS) for enhanced and secure connectivity.

#### Improved Handling of DNS Leak with IKS
- Enhanced the handling of DNS leaks when utilizing the Internet Kill Switch (IKS), further strengthening privacy and security.

#### Enabled DNS Leak Protection for Split Tunneling with WireGuard Protocol
- Users can now benefit from DNS leak protection when employing split tunneling with the WireGuard protocol, ensuring comprehensive security.

#### Improved WireGuard Adapter for Better Connectivity
- Optimized the WireGuard adapter to enhance connectivity, providing a smoother and more reliable user experience.

#### Enhanced Manual IKS Functionality
- Improved the functionality of Manual Internet Kill Switch (IKS), offering resllers greater control and customization options.

#### Add new "AllowLocalNetworkTraffic" Property in VPN Properties Class
- Added the `AllowLocalNetworkTraffic` property in the VPN properties class, enabling users to manage their local LAN connection while connected to the VPN.
**Note:**
The current version of IKEv2 does not support the `AllowLocalNetworkTraffic` feature. Future updates may include support for IKEv2.

#### Integrated New OpenVPN Version 2.6.8
- Updated the OpenVPN version to 2.6.8, incorporating the latest improvements and security enhancements.

### Fixed

#### Fixed Bug in Connection Over Connection
- Resolved a bug related to connection over connection, ensuring a stable and reliable connection experience.

#### Fixed DNS Leak Issue on Auto Disconnect
- Addressed and fixed a DNS leak issue occurring during auto disconnect, enhancing overall privacy and security.

### Version 4.4.0

### Added

#### Enhanced IPv6 Protection:
- Strengthened the Windows service with improved IPv6 protection to ensure advanced security measures.

#### Improved WireGuard Connection:
- Enhanced the overall performance and reliability of WireGuard connections for a seamless user experience.

#### Improved Obfuscated Dialing of OpenVPN:
- Upgraded the OpenVPN integration with improved obfuscated dialing mechanisms, enhancing security and privacy.

#### Whitelabeling of WireGuard Adapter Name:
- Implemented the ability to whitelist WireGuard adapter names, providing users with more control and customization options.

#### Added Support for IP Translation (NAT/Non-NAT) DNSs:
- Introduced support for IP translation (NAT/Non-NAT) DNSs, expanding compatibility and flexibility in network configurations.

#### New Mechanism to Detect UTB:
- Implemented a new mechanism to detect when the internet is not available during the connection state.

#### Manual IKS Feature:
- Introduced a manual In-Kind Service (IKS) feature, allowing users to manually configure and manage specific aspects of the service according to their preferences.

#### Updated .NET Framework Version:
- Atom SDK has been upgraded from .NET Framework 4.5 to 4.6

### Version 4.3.0

#### Added
- Enabled obfuscated server dialing from policy JSON.
- Support for quantum resistance servers.
- New property `EnableDownloadSpeedDiagnostics` in `VPNProperties` for post-connection speed diagnostics.

### Version 4.2.0

#### Added
- Mechanism to detect inability to access the internet when VPN is connected.
- Split tunneling support for WireGuard.

#### Fixed
- Bugs related to WireGuard connections.

### Version 4.0.1

#### Fixed
- Issue where VPN was unable to connect when auto-disconnected.

### Version 4.0.0

#### Added
- Enabled support for WireGuard protocol dialing
- Dependency on Curve25519 NuGet package.
- `SessionID` property in `ConnectionDetails`.
- Updated telemetry features.
- Support for "Connection over connection".
- Method `PingDedicatedIPServer` to return latency of the Dedicated IP.
- Policy JSON mechanism for recommended protocol, country, and city dialing.
- Iperf enabled for WireGuard.
- Port randomization in Iperf.

#### Changed
- Exposed new constructor for Multiple Dedicated DNS dialing in `VPNProperties`.

#### Fixed
- WireGuard server issue when user has port forwarding enabled.
- Updated the mechanism for getting recommended location.
- Updated S3 base URL.

### Version 3.8.0

#### Added
- Support for OpenVPN obfuscation dialing.
  
#### Fixed
- General bug fixes.
