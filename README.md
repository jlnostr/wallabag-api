**wallabag-api** is a .NET library written in C# that integrates the [wallabag API](http://getpocket.com/developer). An enhanced documentation can be found [in the wiki](https://github.com/jlnostr/wallabag-api/wiki).

Install it using NuGet:
```
Install-Package wallabag-api
```

## Usage Example:

Get all favorites:

```csharp
WallabagClient c = new WallabagClient("[YOUR_WALLABAG_URL]", "[YOUR_CLIENT_ID]", "[YOUR_CLIENT_SECRET]");

List<WallabagItem> items = await client.GetItemsAsync(IsStarred: false);
```

## Supported platforms

At the moment, this library is only compatible with UWP apps.

## Dependencies
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
