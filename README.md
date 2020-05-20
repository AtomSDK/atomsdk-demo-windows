# ATOM VPN SDK demo for Windows Desktop Applications

﻿
This is a demo application for Windows Desktop Applications with basic usage of ATOM VPN SDK which will help the developers to create smooth applications over ATOM SDK quickly.

## SDK Features covered in this Demo
* Connection with Parameters
* Connection with Pre-Shared Key (PSK)
* Connection with Dedicated IP
* Connection with Multiple Protocols (Auto-Retry Functionality)
* Connection with Real-time Optimized Servers (Countries based on latency from user in Real-time)
* Connection with Smart Dialing (Use GetCountriesForSmartDialing() to get the Advanced VPN Dialing supported countries)
* Connection with Smart Connect (Tags based dialing)

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
 
## SDK Installation
 
Install the latest version of Atom Network SDK through NuGet.
 
```csharp
Install-Package Atom.SDK.Net -Version 3.5.0
```

Run Atom.SDK.Installer.exe to install supporting services and drivers on any system. The same Installer should be accompanied with your application setup to get the required files installed on your customer's system.


Atom.SDK.Net.dll.config file should be copied to the output directory where Atom.SDK.Net.dll is present (only if the config file is not already there) since AtomSDK will read the name of your RAS VPN Adapter mentioned in this config against RAS_ADAPTER_NAME key.

# Getting Started with the Code
 ATOM SDK needs to be initialized with a “SecretKey” provided to you after you buy the subscription which is typically a hex-numeric literal.

```csharp
var atomManagerInstance = AtomManager.Initialize(“SECRETKEY_GOES_HERE”);
```


OR 

It can be initialized using an instance of AtomConfiguration. It should have a VpnInterfaceName which will be used to create the Network Interface for VPN connection.

```csharp
var atomConfiguration = new AtomConfiguration(“SECRETKEY_GOES_HERE”);
atomConfiguration.VpnInterfaceName = "Atom";
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

ATOM SDK offers five events to register for the ease of the developer.
 
* StateChanged
* Connected
* Disconnected
* DialError
* Redialing
* OnUnableToAccessInternet

 Details of these events can be seen in the inline documentation or method summaries. You need to register these events to get notified about what’s happening behind the scenes
 
```csharp
atomManagerInstance.Connected += AtomManagerInstance_Connected;
atomManagerInstance.DialError += AtomManagerInstance_DialError;
atomManagerInstance.Disconnected += AtomManagerInstance_Disconnected;
atomManagerInstance.StateChanged += AtomManagerInstance_StateChanged;
atomManagerInstance.Redialing += AtomManagerInstance_Redialing;
atomManagerInstance.OnUnableToAccessInternet+= AtomManagerInstance_OnUnableToAccessInternet;
```
Events will be registered with the respective EventArgs customized for the ease of the developer.
## VPN Authentication
ATOM SDK provided two ways to authenticate your vpn user.
First one is to offer VPN Credentials directly to the SDK which you may create through the Admin Panel provided by ATOM.

```csharp
atomManagerInstance.Credentials = new Credentials("VPNUsername","VPNPassword");
```
Alternatively, if you don't want to take hassle of creating users yourself, leave it on us and we will do the rest for you! Easy isn't it.

```csharp
atomManagerInstance.UUID = “UniqueUserID”;
```
 
You just need to provide a Unique User ID for your user e.g. any unique hash or even user’s email which you think remains consistent and unique for your user. ATOM SDK will generate VPN Account behind the scenes automatically and gets your user connected!

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

## Fetch Protocols
Protocols can be obtained through ATOM SDK as well.

```csharp
var protocols = atomManagerInstance.GetProtocols(); 
```

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

### Connection with Pre-Shared Key (PSK)
In this way of connection, it is pre-assumed that you have your own backend server which communicates with ATOM Backend APIs directly and creates a Pre-Shared Key (usually called as PSK) which you can then provide to the SDK for dialing. While providing PSK, no VPN Property other than PSK is required to make the connection. ATOM SDK will handle the rest.
```csharp
var vpnProperties = new VPNProperties(string PSK);
atomManagerInstance.Connect(properties);
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

### Connection with Smart Connect

If you want us to connect your user with what's best for him, you can now do it using *SmartConnect* feature. Atom has introduced an enum list of feature a.k.a *Tags* you want to apply over those smart connections which can be found under  *Atom.Core.Enums.SmartConnectTag* namespace. An example usage of SmartConnect is depicted below.

```csharp
var tagsList = new List<SmartConnectTag>();
tagsList.Add(SmartConnectTag.FILE_SHARING);
var vpnProperties = new VPNProperties(Protocol protocol, List<SmartConnectTag> TagsList);
atomManagerInstance.Connect(properties);
```
Tags aren't mandatory and is a nullable parameter. You can only provide Protocol to connect and rest Atom will manage.


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

# Resolve dependencies conflicts if any :

In case any dependency conflict is faced while building/running ATOM SDK with your application e.g. different version of Newtonsoft.Json used in your application, define the binding redirect in your app configuration like following:

```
      <dependentAssembly>
        <assemblyIdentity name="Newtonsoft.Json" publicKeyToken="30ad4fe6b2a6aeed" culture="neutral" />
        <bindingRedirect oldVersion="0.0.0.0-11.0.0.0" newVersion="11.0.0.0" />
      </dependentAssembly>
```

where "0.0.0.0-11.0.0.0" is the minimum and maximum version range of Newtonsoft.Json.
