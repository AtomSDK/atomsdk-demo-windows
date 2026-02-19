 # ATOM VPN SDK demo for Windows Desktop Applications

﻿
This is a demo application for Windows Desktop Applications with basic usage of ATOM VPN SDK which will help the developers to create smooth applications over ATOM SDK quickly.

## SDK Features covered in this Demo
* Connection with Parameters
* Connection with Dedicated IP
* Connection with Multiple Protocols (Auto-Retry Functionality)
* Connection with Real-time Optimized Servers (Countries based on latency from user in Real-time)
* Connection with Smart Dialing (Use GetCountriesForSmartDialing() to get the Advanced VPN Dialing supported countries)

## SDK Features present in the Demo Application Source Code
* Split Tunneling
* Internet Kill Switch

 
## Compatibility
 
* Compatible with Microsoft Visual Studio 2015 and onwards
* Minimum .Net Framework 4.5 required
* Compatible with ATOM SDK Version 3.0 and onwards 

## Supported Protocols

* IKEv2
* TCP
* UDP
* WireGuard
 
## SDK Installation
 
Install the latest version of Atom Network SDK through NuGet.
 
```csharp
NuGet\Install-Package Atom.SDK.Net -Version 5.2.0
```

Run Atom.SDK.Installer.exe to install supporting services and drivers on any system. The same Installer should be accompanied with your application setup to get the required files installed on your customer's system.


Atom.SDK.Net.dll.config file should be copied to the output directory where Atom.SDK.Net.dll is present (only if the config file is not already there) since AtomSDK will read the name of your RAS VPN Adapter mentioned in this config against RAS_ADAPTER_NAME key.

# Getting Started with the Code

ATOM SDK needs to be initialized using an instance of AtomConfiguration. It should have a VpnInterfaceName which will be used to create the Network Interface for VPN connection.

```csharp
var atomConfiguration = new AtomConfiguration(“SECRETKEY_GOES_HERE”, “INSTALLED_SERVICE_NAME_GOES_HERE”, “INSTALLED_DIRECTORY_FOLDER_NAME_GOES_HERE”, “CUSTOM_VPN_INTERFACE_NAME_GOES_HERE”);
atomConfiguration.VpnInterfaceName = "AtomDemo";
atomConfiguration.BaseUrl = new Uri("YOUR_BASE_URL"); // Optional
var atomManagerInstance = AtomManager.Initialize(atomConfiguration);
```

PS: ATOM SDK is a singleton, and must be initialized before accessing its methods, otherwise NullObjectReference Exception will be thrown.

 ## Enable Local Inventory Support
 
 ATOM SDK offers a feature to enable the local inventory support. This can help Application to fetch Countries and Protocols even when device network is not working.

* To enable it, Log In to the Atom Console
* Download the local data file in json format
* File name should be localdata.json. Please rename the file to localdata.json if you find any discrepancy in the file name.
* Paste the file in the output directory of your application
 
 
 
## Events to Register

ATOM SDK offers few events to register for the ease of the developer.
 
* StateChanged
* Connected
* Disconnected
* DialError
* Redialing
* OnUnableToAccessInternet
* AtomInitialized
* AtomDependenciesMissing
* PacketsTransmitted 

 Details of these events can be seen in the inline documentation or method summaries. You need to register these events to get notified about what’s happening behind the scenes
 
```csharp
atomManagerInstance.Connected += AtomManagerInstance_Connected;
atomManagerInstance.DialError += AtomManagerInstance_DialError;
atomManagerInstance.Disconnected += AtomManagerInstance_Disconnected;
atomManagerInstance.StateChanged += AtomManagerInstance_StateChanged;
atomManagerInstance.Redialing += AtomManagerInstance_Redialing;
atomManagerInstance.OnUnableToAccessInternet+= AtomManagerInstance_OnUnableToAccessInternet;
atomManagerInstance.AtomInitialized += AtomManagerInstance_AtomInitialized;
atomManagerInstance.AtomDependenciesMissing += AtomManagerInstance_AtomDependenciesMissing;
```
Events will be registered with the respective EventArgs customized for the ease of the developer.
 
## VPN Authentication

Atom SDK offers VPN Credentials directly to the SDK which you may create through the Admin Panel provided by ATOM.

```csharp
atomManagerInstance.Credentials = new Credentials("VPNUsername","VPNPassword");
```

# VPN Connection
You need to declare an object of *VPNProperties* class to define your connection preferences. Details of all the available properties can be seen in the inline documentation of *VPNProperties* Class. For the least, you need to give Country and Protocol with which you want to connect.

```csharp
var vpnProperties = new VPNProperties(Country country, Protocol protocol);
```

## Fetch Countries
You can get the Countries list through ATOM SDK. 
```csharp
var countries = atomManagerInstance.GetCountries();
```

## Fetch Countries For Smart Dialing
You can get the Countries those support Smart Dialing through ATOM SDK.
```csharp
var countries = AtomManagerInstance.GetCountriesForSmartDialing();
```

## Fetch Recommended Country
You can get the Recommended Country for user's location through ATOM SDK.
```csharp
var recommendedCountry = AtomManagerInstance.GetRecommendedCountry();
```

## Fetch Protocols
Protocols can be obtained through ATOM SDK as well.

```csharp
var protocols = atomManagerInstance.GetProtocols(); 
```

##  VPN Connection Speed
For VPN connection speed you need to registor PacketsTransmitted event from AtomManager class to get the VPN connection speed in bytes per second. This callback is recieve only in VPN connected state.

```csharp
atomManagerInstance.PacketsTransmitted  
```

## Protocol switch 
You can enable or disable protocol switch from VPNProperties class. By default its value is set to true.

````
VPNPorperties.EnableProtocolSwitch = false;
````
or
````
VPNPorperties.EnableProtocolSwitch = true;
````

## Recommended protocol
If you didn't specify the protocol in case of Country, City and Channel dailing then Atom SDK dialed with recommended protocol according to the specified country, city and channel.
It did not work in PSK, Smart connect dialing and dedicated IP.

## Use Failover
Failover is a mechanism in which Atom dialed with nearest server if requested server is busy or not found for any reason. You can control this mechanism from VPNPorperties class. By default its value is set to true.

````
VPNPorperties.UseFailover = false;
````
or
````
VPNPorperties.UseFailover = true;
````

## How to Connect

As soon as you call Connect method, the events you were listening to will get the updates about the states being changed and Dial Error (if any occurs as well).

After initializing the VPNProperties, just call Connect method of ATOM SDK.
 
### Connection with Parameters
It is the simplest way of connection which is well explained in the steps above. You just need to provide the Country and the Protocol objects and call the Connect method.


```csharp
var vpnProperties = new VPNProperties(Country country, Protocol protocol);
atomManagerInstance.Connect(properties);
``` 
From version 3.0 onwards, Atom has introduced connection with Cities and Channels. You can found their corresponding *VPNProperties* constructors in the Demo Application.

### Include or Exclude Server with Nas Identifier
When connecting with parameters, a server can be included or excluded with its Nas Identifier
```csharp
var vpnProperties = new VPNProperties(Country country, Protocol protocol);
vpnProperties.ServerFilter = new ServerFilter { FilterType = SDK.Core.Enumerations.ServerFilterType.Include, NASIdentifier = "nas-identifier-here" };
vpnProperties.ServerFilter = new ServerFilter { FilterType = SDK.Core.Enumerations.ServerFilterType.Exclude, NASIdentifier = "nas-identifier-here" };
``` 

 
### Connection with Dedicated IP
You can also make your user comfortable with this type of connection by just providing them with a Dedicated DNS Host and they will always connect to a dedicated server! For this purpose, ATOM SDK provides you with the following constructor.
```csharp
var vpnProperties = new VPNProperties(string hostName, Protocol protocol); 
atomManagerInstance.Connect(properties);
```
 
### Connection with Real-time Optimized Servers
This one is same as the first one i.e. "Connection with Parameters" with a slight addition of using Real-time optimized servers best from your user’s location. You just need to set this property to TRUE and rest will be handled by the ATOM SDK.
```csharp
var vpnProperties = new VPNProperties(Country country, Protocol protocol);
vpnProperties.UseOptimization= true;
atomManagerInstance.Connect(properties);
```
For more information, please see the inline documentation of VPNProperties Class.

If you want to show your user the best location for him on your GUI then ATOM SDK have it ready for you as well! ATOM SDK has a method exposed namely *GetOptimizedCountries()* which adds a property *Latency* in the country object which has the real-time latency of all countries from your user’s location (only if ping is enabled on your user’s system and ISP doesn’t blocks any of our datacenters). You can use this property to find the best speed countries from your user’s location.

### Connection with Smart Dialing
“Connection with Parameters” with a slight addition of using smart dialing to connect. You just need to set *UseSmartDialing* property of VPNProperties *true* and rest will handled by the ATOM SDK.

```csharp
var vpnProperties = new VPNProperties(Country country, Protocol protocol);
vpnProperties.UseSmartDialing= true;
atomManagerInstance.Connect(properties);
```
For more information, please see the inline documentation of VPNProperties Class.


### Connection with Multiple Protocols (Auto-Retry Functionality)
You can provide THREE Protocols at max so ATOM SDK can attempt automatically on your behalf to get your user connected with the Secondary or Tertiary protocol if your primary Protocol fails to connect. 

```
vpnProperties.SecondaryProtocol = ProtocolObject;
vpnProperties.TertiaryProtocol = ProtocolObject;
```
For more information, please see the inline documentation of VPNProperties Class.

# Cancel VPN Connection
You can cancel connection between dialing process by calling the Cancel method.
```
atomManagerInstance.Cancel();
```
# Disconnect VPN Connection
 To disconnect, simply call the Disconnect method of AtomManager.

```
atomManagerInstance.Disconnect();
```


# OpenVPN Dialing Using Wintun Adapter
This section outlines the steps required to establish an OpenVPN connection using the Wintun adapter. It involves enabling the **UseTunnelAdapterForOpenVPNDialing** property within the **VPNProperties** class.

## Steps to Enable Wintun Adapter for OpenVPN
1. **Initialize VPN Properties**
   Begin by instantiating the `VPNProperties` class. This class contains the necessary properties to configure the VPN connection.

2. **Enable Tunnel Adapter for OpenVPN Dialing**
   Set the `UseTunnelAdapterForOpenVPNDialing` property to `true` within the `VPNProperties` class. This enables the use of the Wintun adapter for OpenVPN connections.

   **Example Code:**
```csharp
	var vpnProperties = new VPNProperties(Country country, Protocol protocol);
	vpnProperties.UseTunnelAdapterForOpenVPNDialing = true;
```

3. **Create the Connection**
   After enabling the property, proceed to establish the OpenVPN connection. With the configuration set, the connection will automatically use the new Wintun adapter.

***Note: This property will only work with UDP and TCP protocols.***

---

# Pause and Resume VPN Feature
This section details about the **VPN Pause and Resume** feature using the **Atom SDK**, enabling users to temporarily pause VPN connections manually or for a specific duration. This is useful when users need to suspend VPN activity without a full disconnect.

## Feature Overview

### VPN Pause Modes

- **Manual Pause**: Indefinitely pauses the VPN until resumed manually.
- **Timed Pause**: Pauses the VPN for a predefined duration, automatically resuming afterward. Users can also resume manually before the timer ends.

### Key Rules & Conditions

- VPN can only be paused if in a **Connected** state.
- VPN can only be resumed if in a **Paused** state.
- A paused VPN can still be disconnected using the SDK's `Disconnect` method.
- During a timed pause, users can manually resume the VPN using the `Resume` method.

##  Events & Methods

###  Events

#### `OnVPNPaused`

Triggered when the VPN is paused (manually or timed).

```csharp
atomManagerInstance.OnVPNPaused += AtomManager_OnVPNPaused;

private void AtomManager_OnVPNPaused(object sender, PausedEventArgs e)
{
    // Handle pause state
}
```

```csharp
public class PausedEventArgs
{
    public ConnectionDetails ConnectionDetails { get; set; }
    public VPNState State { get; private set; }
}
```

###  Pause Method

```csharp
enum PauseVPNTimer 
{
    MANUAL,
    MINUTES_5,
    MINUTES_10,
    MINUTES_15,
    MINUTES_30,
    MINUTES_60
}

async Task Pause(PauseVPNTimer pauseInterval)
{
    await AtomManagerInstance.Pause(pauseInterval);
}
```

---

###  Resume Method

```csharp
atomManagerInstance.Connected += AtomManager_Connected;
atomManagerInstance.DialError += AtomManager_DialError;

void Resume()
{
    AtomManagerInstance.Resume();
}

private void AtomManager_Connected(object sender, ConnectedEventArgs e)
{
    // VPN resumed successfully
}

private void AtomManager_DialError(object sender, DialErrorEventArgs e)
{
    // Handle resume error
}
```

---

## VPN State Management

The SDK provides a `VPNState` enum for current state:

```csharp
public enum VPNState
{
    PAUSING,   // VPN is in the process of pausing
    PAUSED,    // VPN is currently paused
    RESUMING   // VPN is resuming
}
```

Subscribe to state changes:

```csharp
atomManagerInstance.StateChanged += AtomManagerInstance_StateChanged;

private void AtomManagerInstance_StateChanged(object sender, StateChangedEventArgs e)
{
    // Handle state changes: PAUSING, PAUSED, RESUMING, etc.
}
```

---


##  Conclusion

The Atom SDK’s VPN Pause functionality provides powerful control over connection management, with both manual and timed pause options. By following the integration steps above, developers can enhance user experience with smooth, responsive VPN control.

---

# Tracker and Ad Blocker Feature

This section outlines the deatils about the **Tracker and Ad Blocker** feature in the Atom VPN SDK. This feature allows VPN applications built using the Atom SDK to block tracking scripts and advertisements for enhanced privacy and performance.

---

## About This Feature

As a VPN service provider, we offer a powerful SDK that our clients can use to build custom VPN applications. In our latest release, we’ve introduced support for **Tracker and Ad Blocker** functionality.

When enabled, this feature will actively block trackers and advertisements during a VPN session. It is supported across all connection types provided by the SDK:

1. **Connect with Param**
2. **Connect with Dedicated IP**
3. **Connect with Multiple Dedicated IPs**
4. **Connect with Dedicated VPS**

---

##  Integration on Windows

### 1. Define Features

Use these constants to specify the features you want to enable:

```csharp
AtomShieldFeatureType.TRACKER
AtomShieldFeatureType.AD_BLOCKER
```

---

### 2. Request Features

Specify the desired features in the `VPNProperties` object:

```csharp
properties.AtomShieldFeatures = new List<SDK.Core.Enumerations.AtomShield.AtomShieldFeatureType>
{
    SDK.Core.Enumerations.AtomShield.AtomShieldFeatureType.TRACKER,
    SDK.Core.Enumerations.AtomShield.AtomShieldFeatureType.AD_BLOCKER
};
```

---

### 3. Observe Status and Data

Subscribe to the following events to monitor status and blocked data:

```csharp
atomManagerInstance.AtomShieldStatusChanged += AtomManagerInstance_AtomShieldStatusChanged;
atomManagerInstance.AtomShieldDataRecieved += AtomManagerInstance_AtomShieldDataRecieved;
```

---

### 4. Status Definitions

The `AtomShieldStatus` object represents the state of the feature:

- `Establishing(String)`: Connecting the Tracker/Ad Blocker
- `Established(String)`: Tracker/Ad Blocker connected successfully
- `Disconnected(String)`: Tracker/Ad Blocker disconnected
- `Error(AtomException)`: An error occurred (refer to error codes for details)

---

### 5. Data Monitoring

Monitor the number of blocked trackers or ads using:

```csharp
(int) AtomShieldData.BlockerCount
```

---

### 6. Requested Status

Verify if features were requested using the following properties:

```csharp
ConnectionDetails.IsTrackerBlockerRequested
ConnectionDetails.IsAdBlockerRequested
```

---

## Conclusion

The Tracker and Ad Blocker feature in the Atom SDK enables clients to provide users with enhanced privacy and a better browsing experience. This feature seamlessly integrates with all supported VPN connection types, ensuring consistent functionality across diverse configurations.

---

# Local LAN Access Feature

## Overview
Our VPN SDK now includes a new feature that allows users to access their locally connected devices over the internet while maintaining a VPN connection. This functionality ensures seamless connectivity to local resources without compromising security.

## How It Works
By default, VPN connections restrict access to locally connected devices. However, our SDK introduces the BYPassLocalLanConnection property within the VPNProperties class, which allows users to toggle this capability on or off.

### Key Functionality:
- When **enabled**, the user's locally connected devices remain accessible while using the VPN.
- When **disabled**, the standard VPN restrictions apply, blocking access to local network devices.

## Implementation
To enable or disable Local LAN Access, set the AllowLocalNetworkTraffic property in your VPN configuration:

```csharp
	var vpnProperties = new VPNProperties(Country country, Protocol protocol);
	vpnProperties.AllowLocalNetworkTraffic = true; // Enable access to local network devices
```


## Use Cases
- Access network printers, shared drives, or IoT devices while connected to the VPN.
- Maintain a secure VPN connection while still using local services.
- Ideal for remote workers needing access to both VPN-protected resources and local network devices.

---

# Resolve dependencies conflicts if any :

In case any dependency conflict is faced while building/running ATOM SDK with your application e.g. different version of Newtonsoft.Json used in your application, define the binding redirect in your app configuration like following:

```
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-11.0.0.0" newVersion="11.0.0.0" />
      </dependentAssembly>
```

where "0.0.0.0-11.0.0.0" is the minimum and maximum version range of Newtonsoft.Json.
