![Logo](http://littlebits.github.com/args/console32.png) Args
===========================================

A .NET library designed to allow for binding command line arguments to POCOs.  The release will contain assemblies compiled in v3.5 and v4.0.

Args uses [Semantic Versioning] (http://semver.org/)

Release Notes
--------
_Version 1.1.0_
_2013/1/2_

-Feature Enhancements
  - Issue #8: Args now respects the `RequiredAttribute`
  - Issue #9: In the help output, arguments are only listed in the description area if there is help text provided
  - Issue #13: The default type resolver will use non-public parameterless constructors
  - Issue #14: Args supports collections (details below)

If the declared type of an argument implements `IEnumerable<T>`, Args will take each argument and convert it to type `T`.
For ordinal parameters, this behavior is only supported for the last ordinal. `String` is a special case (it implements `IEnumerable<char>`) and is ignored.

If the declared type can handle an array being assigned to it, then an array is created and assigned.
Otherwise, the type must implement `IList` or `ICollection<T>`, and an instance of the collection is created and the `Add` method is invoked for each item.

__Version 1.0.4__ 
_2011/10/17_

- Bug Fixes
  - Issue #6: Generating help fails when model does not have any public properties
  - Issue #7: Arguments class cannot have super class
- Using latest version of NuGet.exe

__Version 1.0.3__ 
_2011/04/06_

- Bug Fixes
  - Issue #3: Switch arguments not working when last, or only, argument
- Using latest version of NuGet.exe

__Version 1.0.2__ 
_2011/03/31_

- Bug Fixes
  - Issue #2: Lambda expression causing stack overflow exception


__Version 1.0.1__ 
_2011/03/01_

- No code change, only package changes for NuGet to properly display icon

__Version 1.0.0__ 
_2011/03/01_

- Initial Release
