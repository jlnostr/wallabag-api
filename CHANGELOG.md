## 1.1.0
- wallabag-api is now a PCL and therefore compatible for more platforms.
- And some bugfixes, of course :D

## 1.0.4
- Implement IDisposable for WallabagClient.
- Implement IComparable for WallabagItem and WallabagTag.
- WallabagTag.Equals is now based on the label instead of the id.

## 1.0.3
- Fix for ObjectNotReferencedExceptions when using LINQ.

## 1.0.2
- Include comments in NuGet package (they weren't until now)
- Change type of PreviewImageUri to Uri
- Add GetItemsWithEnhancedMetadataAsync which will return informations like page number, number of items along with items itself.

## 1.0.1
- some code cleanup
- New property `LastTokenRefreshDateTime` that indicates the last time when RefreshAccessTokenAsync was executed.

## 1.0.0
- Initial release.