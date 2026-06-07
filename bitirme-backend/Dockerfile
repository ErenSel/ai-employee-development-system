FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/BitirmeBackend.Api/BitirmeBackend.Api.csproj", "src/BitirmeBackend.Api/"]
COPY ["src/BitirmeBackend.Application/BitirmeBackend.Application.csproj", "src/BitirmeBackend.Application/"]
COPY ["src/BitirmeBackend.Domain/BitirmeBackend.Domain.csproj", "src/BitirmeBackend.Domain/"]
COPY ["src/BitirmeBackend.Infrastructure/BitirmeBackend.Infrastructure.csproj", "src/BitirmeBackend.Infrastructure/"]
COPY ["src/BitirmeBackend.Contracts/BitirmeBackend.Contracts.csproj", "src/BitirmeBackend.Contracts/"]
RUN dotnet restore "src/BitirmeBackend.Api/BitirmeBackend.Api.csproj"
COPY . .
WORKDIR "/src/src/BitirmeBackend.Api"
RUN dotnet build "BitirmeBackend.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BitirmeBackend.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitirmeBackend.Api.dll"]
