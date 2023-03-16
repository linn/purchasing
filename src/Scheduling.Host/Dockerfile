FROM mcr.microsoft.com/dotnet/runtime:7.0
FROM mcr.microsoft.com/dotnet/aspnet:7.0

ENV TZ=Europe/London

COPY /bin/release/net7.0/publish/ /app/bin/

CMD dotnet /app/bin/Linn.Purchasing.Scheduling.Host.dll
