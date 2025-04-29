FROM mcr.microsoft.com/dotnet/aspnet:9.0

COPY bin/release/net9.0/publish/ /app/bin/
ENV TZ=Europe/London

CMD dotnet /app/bin/Linn.Purchasing.Messaging.Host.dll
