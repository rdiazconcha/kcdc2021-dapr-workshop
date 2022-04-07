#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DaprHospital.Hospital.Api/DaprHospital.Hospital.Api.csproj", "DaprHospital.Hospital.Api/"]
COPY ["DaprHospital.IntegrationEvents/DaprHospital.IntegrationEvents.csproj", "DaprHospital.IntegrationEvents/"]
RUN dotnet restore "DaprHospital.Hospital.Api/DaprHospital.Hospital.Api.csproj"
COPY . .
WORKDIR "/src/DaprHospital.Hospital.Api"
RUN dotnet build "DaprHospital.Hospital.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DaprHospital.Hospital.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DaprHospital.Hospital.Api.dll"]