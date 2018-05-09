#! /bin/bash

# setup for dotnet repo
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-trusty-prod trusty main" > /etc/apt/sources.list.d/dotnetdev.list'

# update and install dotnet core
apt-get update

export DOTNET_CLI_TELEMETRY_OPTOUT=1

apt-get install -y dotnet-sdk-2.1.104
