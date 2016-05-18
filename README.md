**wallabag-api** is a portable class library (PCL) that integrates the [wallabag API](http://getpocket.com/developer). An enhanced documentation can be found [in the wiki](https://github.com/jlnostr/wallabag-api/wiki).

Install it using [NuGet](https://www.nuget.org/packages/wallabag.Api):
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

- .NET >= 4.5
- [UWP](https://msdn.microsoft.com/en-us/windows/uwp/get-started/universal-application-platform-guide)
- Windows Phone 8.1
- Windows Store >= 8
- [Xamarin iOS + Android](https://www.xamarin.com)

## Dependencies
- [Newtonsoft.Json](https://www.nuget.org/packages/Newtonsoft.Json/)
