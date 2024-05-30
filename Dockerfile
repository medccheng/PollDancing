# syntax=docker/dockerfile:1

# Build stage with correct .NET SDK version
FROM mcr.microsoft.com/dotnet/sdk:8.0-windowsservercore-ltsc2022 AS build
WORKDIR /source

# Copy everything to the container
COPY . .

# Use ARG for architecture handling if needed
ARG TARGETARCH

# Restore and publish the project
RUN dotnet restore
RUN dotnet publish -c Release -r win-x64 --self-contained false -o /app -v n

# Final stage with ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-windowsservercore-ltsc2022 AS final
WORKDIR /app

# Copy compiled app from the build stage
COPY --from=build /app .

# Define non-privileged user
USER ContainerUser

ENTRYPOINT ["dotnet", "PollDancingWeb.dll"]
