FROM mcr.microsoft.com/dotnet/aspnet:9.0

ENV TZ=Europe/London

COPY /bin/release/net9.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Purchasing.Scheduling.Host.dll
