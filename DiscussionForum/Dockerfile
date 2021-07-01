FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 

COPY *.sln .

COPY DiscussionForumREST/*.csproj ./DiscussionForumREST/
COPY DFBL/*.csproj ./DFBL/
COPY DFDL/*.csproj ./DFDL/
COPY DFModels/*.csproj ./DFModels/
COPY DFTests/*.csproj ./DFTests/

RUN cd DiscussionForumREST && dotnet restore

COPY . ./


RUN dotnet publish DiscussionForumREST -c Release -o publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app 

COPY --from=build /app/publish ./
CMD ["dotnet", "DiscussionForumREST.dll"]