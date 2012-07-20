using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Pocket1945")]
[assembly: AssemblyDescription("Pocket PC game")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("GreIT AS")]
[assembly: AssemblyProduct("Pocket1945")]
[assembly: AssemblyCopyright("Copyright 2004 Pocket 1945 Team")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		

// Sondre: This should not be dynamic
// Jonas: Agree, i'll increment the build number for each public release.
[assembly: AssemblyVersion("1.0.0.6")]

//
// In order to sign your assembly you must specify a key to use. Refer to the 
// Microsoft .NET Framework documentation for more information on assembly signing.
//
// Use the attributes below to control which key is used for signing. 
//
// Notes: 
//   (*) If no key is specified - the assembly cannot be signed.
//   (*) KeyName refers to a key that has been installed in the Crypto Service
//       Provider (CSP) on your machine. 
//   (*) If the key file and a key name attributes are both specified, the 
//       following processing occurs:
//       (1) If the KeyName can be found in the CSP - that key is used.
//       (2) If the KeyName does not exist and the KeyFile does exist, the key 
//           in the file is installed into the CSP and used.
//   (*) Delay Signing is an advanced option - see the Microsoft .NET Framework
//       documentation for more information on this.
//
[assembly: AssemblyDelaySign(false)]
[assembly: AssemblyKeyFile("")]
[assembly: AssemblyKeyName("")]
