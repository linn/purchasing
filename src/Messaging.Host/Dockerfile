FROM mcr.microsoft.com/dotnet/runtime:7.0
FROM mcr.microsoft.com/dotnet/aspnet:7.0

COPY bin/release/net6.0/publish/ /app/bin/
ENV TZ=Europe/London

CMD dotnet /app/bin/Linn.Purchasing.Messaging.Host.dll
