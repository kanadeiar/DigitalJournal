FROM mcr.microsoft.com/dotnet/aspnet:6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0

COPY /bin/Release/net6.0/publish/ DigitalJournal/

ENV ASPNETCORE_ENVIRONMENT Production

EXPOSE 5000
WORKDIR /DigitalJournal
ENTRYPOINT ["dotnet", "DigitalJournal.dll",  "--urls=http://0.0.0.0:5000"]