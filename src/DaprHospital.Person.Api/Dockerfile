#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DaprHospital.Person.Api/DaprHospital.Person.Api.csproj", "DaprHospital.Person.Api/"]
COPY ["DaprHospital.IntegrationEvents/DaprHospital.IntegrationEvents.csproj", "DaprHospital.IntegrationEvents/"]
COPY ["DaprHospital.Core/DaprHospital.Core.csproj", "DaprHospital.Core/"]
RUN dotnet restore "DaprHospital.Person.Api/DaprHospital.Person.Api.csproj"
COPY . .
WORKDIR "/src/DaprHospital.Person.Api"
RUN dotnet build "DaprHospital.Person.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprHospital.Person.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprHospital.Person.Api.dll"]