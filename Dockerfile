﻿# Multi-stage build for ProjectBeta
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy solution and restore
COPY . .
RUN dotnet restore ProjectBeta.sln

# publish the WebAPI project
RUN dotnet publish ProjectBeta.WebAPI -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_EnableDiagnostics=0
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectBeta.WebAPI.dll"]
