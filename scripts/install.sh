#!/bin/bash
set -ev

dotnet restore

# upgrade node to latest version
if [ "$CI" ] && [ "$TRAVIS" ]
then 
	source ~/.nvm/nvm.sh; 
	nvm install 16;
	nvm use 16;
fi

cd ./src/Service.Host
npm ci --legacy-peer-deps
npm run build
cd ../..
