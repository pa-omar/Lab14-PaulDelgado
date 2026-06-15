FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore Lab14-PaulDelgado.sln
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

CMD ["sh", "-c", "dotnet Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
