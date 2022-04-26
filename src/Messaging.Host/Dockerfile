FROM mcr.microsoft.com/dotnet/runtime:6.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY bin/release/net6.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Purchasing.Messaging.Host.dll
