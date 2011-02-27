Args
====

A .NET library designed to allow for binding command line arguments to POCOs.  The release will contain assemblies compiled in v3.5 and v4.0.

Features
--------

- Conventions-based configuration
- Fluent API
- POCO classes
- Both swithced arguments and ordinal arguments are supported
- Interfaces for most classes to allow for easy mocking
- Auto-generated help document
  - Comes packaged with a console formatter
  - Writing your own is as simple as implementing IHelpFormatter
- Out-of-the-box support for type conversion of all "simple" .NET types
- Enum support, including flags support
- Use existing TypeConverters for custom types
  - Can also implement a simple class for type conversion
  - Can also provide a lambda expression for type conversion
- Ability to define default values if one is not provided
- Attributes used from System.ComponentModel namespace whenever possible
- Available via [http://www.nuget.org NuGet] _comming soon_

Example
--------

If [#standard_convensions] are used, then usage is extremely simple. Define a POCO to hold your command line argument values:

    public class CommandObject
	{
	    public string Source{get;set;}
		public string Destination{get;set;}
		public bool Force{get;set;}
	}
	
	public class Program
	{
	    public static void Main(string[] args)
		{
		    var command = Args.Configuration.Configure<CommandObject>().CreateAndBind(args);
		}
	}
	
	C:\> MyProgram.exe /s readme.txt /d readme2.txt /f
	
Conventions
-----------

The default conventions in Args are:

- The switch delimiter is forward slash '/'
- The switches are checked for equality using CurrentCultureIgnoreCase
- The switch delimiter and string comparer can be set via the ArgsModelAttribute
- The System.ComponentModel.DescriptionAttribute can be used on both the model and members to set the help text for each
- All public fields and public settable properties are configured
  - Each property/field has two switches configured by default
    - The minimum number of characters to make it unique from all the other properties/fields
	- The entire name of the property/field
- System.ComponentModel.TypeResolverAttribute can be used on members to use a specific type converter for that member
- Args.ArgsSwitchMemberAttribute can be used to override the allowed swithces or to set as an [#ordinal_parameter]


Ordinal Parameters
------------------

These are required parameters that do not have any switches and have a specified order.  They are used in the command line after the executable,
but before any switched arguments.

	

License
=======
Copyright (c) 2011 Brian Ball and contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.