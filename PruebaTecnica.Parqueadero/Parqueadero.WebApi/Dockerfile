#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
ENV ASPNETCORE_URLS="http://localhost:5000"
WORKDIR /src
COPY ["Parqueadero.WebApi/Parqueadero.WebApi.csproj", "Parqueadero.WebApi/"]
RUN dotnet restore "Parqueadero.WebApi/Parqueadero.WebApi.csproj"
COPY . .
WORKDIR "/src/Parqueadero.WebApi"
RUN dotnet build "Parqueadero.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Parqueadero.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Parqueadero.WebApi.dll"]