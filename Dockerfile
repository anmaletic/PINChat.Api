#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PINChat.Api/PINChat.Api.csproj", "PINChat.Api/"]
RUN dotnet restore "PINChat.Api/PINChat.Api.csproj"
COPY . .
WORKDIR "/src/PINChat.Api"
RUN dotnet build "PINChat.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PINChat.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PINChat.Api.dll"]