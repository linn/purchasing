#!/bin/bash
set -ev

# upgrade node to latest version
if [ "$CI" ] && [ "$TRAVIS" ]
then 
	source ~/.nvm/nvm.sh; 
	nvm install 16;
	nvm use 16;
fi

# c# tests
dotnet test ./tests/Unit/Domain.Tests/Domain.Tests.csproj
dotnet test ./tests/Unit/Domain.LinnApps.Tests/Domain.LinnApps.Tests.csproj
dotnet test ./tests/Unit/Facade.Tests/Facade.Tests.csproj
dotnet test ./tests/Unit/Messaging.Tests/Messaging.Tests.csproj
dotnet test ./tests/Unit/Proxy.Tests/Proxy.Tests.csproj
dotnet test ./tests/Integration/Integration.Tests/Integration.Tests.csproj

echo $?
if [ $? -eq 1 ]; then
  echo dotnet test fail
  exit 1
fi

# javascript tests
cd ./src/Service.Host
./node_modules/.bin/jest
echo $?
result=$?
cd ../..

exit $result
