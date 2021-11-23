FROM mcr.microsoft.com/dotnet/runtime:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0

ARG gitBranch=unspecified
ENV TZ=Europe/London

EXPOSE 5050

COPY bin/release/net5.0/publish/ /app/bin/
COPY client/build/ /app/client/build/
COPY views/ /app/views/
     
ENV APP_PATH /app/
ENV GIT_BRANCH $gitBranch

CMD dotnet /app/bin/Linn.Purchasing.Service.Host.dll
