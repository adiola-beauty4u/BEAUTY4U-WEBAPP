# -------------------------
# Stage 1: Build
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy the solution file
COPY New_Beauty4U.sln ./

# Copy only .csproj files for faster restore
COPY Beauty4u.ApiAccess/Beauty4u.ApiAccess.csproj Beauty4u.ApiAccess/
COPY Beauty4u.Business/Beauty4u.Business.csproj Beauty4u.Business/
COPY Beauty4u.Common/Beauty4u.Common.csproj Beauty4u.Common/
COPY Beauty4u.Data/Beauty4u.Data.csproj Beauty4u.Data/
COPY Beauty4u.DataAccess/Beauty4u.DataAccess.csproj Beauty4u.DataAccess/
COPY Beauty4u.Interfaces/Beauty4u.Interfaces.csproj Beauty4u.Interfaces/
COPY Beauty4u.Models/Beauty4u.Models.csproj Beauty4u.Models/
COPY Beauty4u.Jobs/Beauty4u.Jobs.csproj Beauty4u.Jobs/
COPY Beauty4u.WebApi/Beauty4u.WebApi.csproj Beauty4u.WebApi/

# Restore NuGet packages
RUN dotnet restore New_Beauty4U.sln

# Copy the rest of the source code
COPY . .

# Build and publish the WebAPI project
RUN dotnet publish Beauty4u.Jobs/Beauty4u.Jobs.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# -------------------------
# Stage 2: Runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

# Copy published files
COPY --from=build /app/publish .

# Expose port (assuming Kestrel default)
EXPOSE 5000
EXPOSE 5001

# Run the API
ENTRYPOINT ["dotnet", "Beauty4u.Jobs.dll"]
