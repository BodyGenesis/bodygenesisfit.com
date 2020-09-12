#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Presentation/BodyGenesis.Presentation.Website/BodyGenesis.Presentation.Website.csproj", "src/Presentation/BodyGenesis.Presentation.Website/"]
RUN dotnet restore "src/Presentation/BodyGenesis.Presentation.Website/BodyGenesis.Presentation.Website.csproj"
COPY . .
WORKDIR "/src/src/Presentation/BodyGenesis.Presentation.Website"
RUN dotnet build "BodyGenesis.Presentation.Website.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BodyGenesis.Presentation.Website.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BodyGenesis.Presentation.Website.dll"]