FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ENV AzureAD__TenantId ""
ENV AzureAD__ClientId ""
ENV AzureAD__ClientSecret ""
ENV Secrets__KeyVault ""

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash && \
    apt-get install -y nodejs && \
    npm install -g sass

WORKDIR /src
COPY ["BWHazel.Aka.Web/BWHazel.Aka.Web.csproj", "BWHazel.Aka.Web/"]
COPY ["BWHazel.Aka.Data/BWHazel.Aka.Data.csproj", "BWHazel.Aka.Data/"]
COPY ["BWHazel.Aka.Model/BWHazel.Aka.Model.csproj", "BWHazel.Aka.Model/"]
RUN dotnet restore "BWHazel.Aka.Web/BWHazel.Aka.Web.csproj"
COPY . .
WORKDIR "/src/BWHazel.Aka.Web"

RUN dotnet build "BWHazel.Aka.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BWHazel.Aka.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BWHazel.Aka.Web.dll"]

FROM base AS deploy
WORKDIR /app
COPY dist .
ENTRYPOINT ["dotnet", "BWHazel.Aka.Web.dll"]