# Change Log
All notable changes to this project will be documented in this file.
 
The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

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
