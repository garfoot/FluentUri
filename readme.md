# FluentURI
This library is a simple URI building library for any .NET Standard 2.0
compatible project.

The library is still in development and subject to change, documentation will
be provided upon intial release.

## License
The project is licensed under an MIT open source license, please read
[license.md](license.md) for more details.

## Development
### Building
#### Building with Visual Studio
The project consists of a Visual Studio solution. Load the solution, restore
packages and build.

Note that building will usually restore the packages for you if you have that
option enabled in settings. If manual restore is required then use
```dotnet restore``` from the solution root.

#### Building with ```dotnet``` CLI
The main library project is a .NET Standard 2.0 library, the unit test project
is a .NET Core 2.0 project. Both projects may be built with
```dotnet build```.

Packages should be automatically restored but this may be manually done with
```dotnet restore```.

### Testing
#### Testing with Visual Studio
The unit tests can be run with the Visual Studio test runner or via other tools
as required (e.g. ReSharper, nCrunch).

#### Testing with ```dotnet``` CLI
Run the unit tests from the command line (assuming in solution root) with
```dotnet test src/FluentUriBuilder.UnitTests```.

