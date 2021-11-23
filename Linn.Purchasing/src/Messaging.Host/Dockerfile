FROM mcr.microsoft.com/dotnet/runtime:5.0
FROM mcr.microsoft.com/dotnet/aspnet:5.0

COPY bin/release/net5.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Purchasing.Messaging.Host.dll
