FROM debian:12-slim

WORKDIR /src

RUN apt-get update && apt-get install -y wget ca-certificates apt-transport-https gnupg \
    && wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb -O packages-microsoft-prod.deb \
    && dpkg -i packages-microsoft-prod.deb \
    && rm packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get install -y dotnet-sdk-9.0 \
    && rm -rf /var/lib/apt/lists/*

COPY . .

RUN dotnet restore Lab14-PaulDelgado.sln
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

WORKDIR /app/publish

EXPOSE 10000

CMD ["sh", "-c", "dotnet Api.dll --urls http://0.0.0.0:${PORT:-10000}"]
