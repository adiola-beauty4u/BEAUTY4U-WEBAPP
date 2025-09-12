FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./Beauty4u.WebApi ./Beauty4u.WebApi
RUN dotnet restore ./Beauty4u.WebApi
RUN dotnet publish ./Beauty4u.WebApi -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV DOTNET_ENVIRONMENT=Production
ENV ConnectionStrings__B4uConnection=""
ENV Cors__AllowedOrigins=""
ENTRYPOINT ["dotnet", "Beauty4u.WebApi.dll"]
