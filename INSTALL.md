Project Setup
=============

You need following software installed

- .NET Core SDK 3.1

Build command:

- `dotnet publish src/RedShot.Application/RedShot.Application.csproj -r {{platform_id}} -f netcoreapp3.1 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true -o ./build`

Supported platforms:

- `linux-x64` for Linux.
- `win-x64` for Windows.
