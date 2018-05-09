# Development
## Building
### Building with Visual Studio
The project consists of a Visual Studio solution. Load the solution, restore
packages and build.

Note that building will usually restore the packages for you if you have that
option enabled in settings. If manual restore is required then use
```dotnet restore``` from the solution root.

### Building with ```dotnet``` CLI
The main library project is a .NET Standard 2.0 library, the unit test project
is a .NET Core 2.0 project. Both projects may be built with
```dotnet build```.

Packages should be automatically restored but this may be manually done with
```dotnet restore```.

## Testing
### Testing with Visual Studio
The unit tests can be run with the Visual Studio test runner or via other tools
as required (e.g. ReSharper, nCrunch).

### Testing with ```dotnet``` CLI
Run the unit tests from the command line (assuming in solution root) with
```dotnet test src/FluentUriBuilder.UnitTests```.

## Branches
There are two main branches, dev and master. Dev is where day to day
development takes place, master is where releases and NuGet packages are
built from. This allows for continued development and integration builds
without potentially destabilising the official releases.

## Environment
There is a basic docker image configured in the /docker/dev folder on github
which can be used to build and test the library. This image is based on
travisci/ci-garnet to match the Travis CI build system.

The provided dockerfile will build and image with dotnet-sdk installed and
will pull the latest source code from GitHub. There is a script in
the /scripts folder on github that can be used to build the image.

The image will clone the latest master branch from github but dev or a specific
version can always be checked out if needed.

### Transient container
This is useful if you just need to quickly perform an action such as running
the tests or publishing the library.

The image has a default entry point of ```/bin/bash``` so it can be run via
```docker run -it --rm fluenturi-dev``` for a transient container with a
bash shell.

### Longer lived container
If you wish to do more detailed development in the container and wish to run it
in the background and connect as needed then this is for you.

Run the image with:
```bash
docker run --name fluenturi-dev -dit --entrypoint /sbin/init fluenturi-dev
```
and then connect with:
```bash
docker exec -it fluenturi-dev bash -l
```

This container can then be stopped and started as needed.

## Current build status
[<img src="https://travis-ci.org/garfoot/FluentUri.svg?branch=master" />](https://travis-ci.org/garfoot/FluentUri)
