using System.Reflection;
#if !WinRT
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Bobasoft.Presentation")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Bobasoft Corporation")]
[assembly: AssemblyProduct("Bobasoft.Presentation")]
[assembly: AssemblyCopyright("Copyright © Bobasoft 2012")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.6.0.0")]
[assembly: AssemblyFileVersion("0.6.0.0")]

#if !WinRT
[assembly: XmlnsPrefix("http://schemas.bobasoft.com/xaml", "bobasoft")]
[assembly: XmlnsDefinition("http://schemas.bobasoft.com/xaml", "Bobasoft.Presentation", AssemblyName = "Bobasoft.Presentation")]
[assembly: XmlnsDefinition("http://schemas.bobasoft.com/xaml", "Bobasoft.Presentation.Converters", AssemblyName = "Bobasoft.Presentation")]
[assembly: XmlnsDefinition("http://schemas.bobasoft.com/xaml", "Bobasoft.Presentation.Actions", AssemblyName = "Bobasoft.Presentation")]
[assembly: XmlnsDefinition("http://schemas.bobasoft.com/xaml", "Bobasoft.Presentation.MVVM", AssemblyName = "Bobasoft.Presentation")]
#endif
