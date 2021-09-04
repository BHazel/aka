FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG azureAdAppTenantId
ARG azureAdAppClientId
ARG azureAdAppClientSecret
ARG azureKeyVault

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
RUN find . -type f -exec sed -i "s/{{secrets.azureAdAppTenantId}}/${azureAdAppTenantId}/g" {} + && \
    find . -type f -exec sed -i "s/{{secrets.azureAdAppClientId}}/${azureAdAppClientId}/g" {} + && \
    find . -type f -exec sed -i "s/{{secrets.azureAdAppClientSecret}}/${azureAdAppClientSecret}/g" {} + && \
    find . -type f -exec sed -i "s/{{secrets.azureKeyVault}}/${azureKeyVault}/g" {} +

RUN dotnet build "BWHazel.Aka.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BWHazel.Aka.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BWHazel.Aka.Web.dll"]

FROM base AS deploy
WORKDIR /app
COPY dist /app/publish
ENTRYPOINT ["dotnet", "BWHazel.Aka.Web.dll"]