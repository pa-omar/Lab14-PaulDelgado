FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /src

COPY . .

RUN dotnet restore Lab14-PaulDelgado.sln
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

WORKDIR /app/publish

CMD ["sh", "-c", "dotnet Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
