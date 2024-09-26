# Use the official .NET images
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Adjust the path to the csproj file according to its location
COPY ["AspnetCoreWithDocker/AspnetCoreWithDocker.csproj", "./"]
RUN dotnet restore "AspnetCoreWithDocker.csproj"

# Copy the rest of the project files
COPY AspnetCoreWithDocker/. ./

# Build the project
RUN dotnet build "AspnetCoreWithDocker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AspnetCoreWithDocker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AspnetCoreWithDocker.dll"]
