# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY VSRSystemsBackend.sln .
COPY src/VSRSystemsBackend.Api/VSRSystemsBackend.Api.csproj src/VSRSystemsBackend.Api/
COPY src/VSRSystemsBackend.Application/VSRSystemsBackend.Application.csproj src/VSRSystemsBackend.Application/
COPY src/VSRSystemsBackend.Core/VSRSystemsBackend.Core.csproj src/VSRSystemsBackend.Core/
COPY src/VSRSystemsBackend.Domain/VSRSystemsBackend.Domain.csproj src/VSRSystemsBackend.Domain/
COPY src/VSRSystemsBackend.Infrastructure/VSRSystemsBackend.Infrastructure.csproj src/VSRSystemsBackend.Infrastructure/
COPY src/VSRSystemsBackend.Shared/VSRSystemsBackend.Shared.csproj src/VSRSystemsBackend.Shared/

# Restore
RUN dotnet restore VSRSystemsBackend.sln

# Copy all source
COPY . .

# Publish
RUN dotnet publish src/VSRSystemsBackend.Api/VSRSystemsBackend.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "VSRSystemsBackend.Api.dll"]