## 1.2.0
- Add `CredentialsRefreshed`, `PreRequestExecution` and `AfterRequestExecution` events.

## 1.1.4
- Fix for `RemoveTagsAsync` returning true even if the request failed.
- Fix for `GetItemsAsync` producing error 500 due to misformed request uris.

## 1.1.3
- Fix for not absolute `PreviewImageUri`s.
- Option for specifiying a timeout. 

## 1.1.2
- Fix for language code not recognized

## 1.1.1
- Return `null` or `false` if a request failed.
- Add ability to submit tags as type of `IEnumerable`.
- Better MVVM support with the implementation of `INotifyPropertyChanged`.

## 1.1.0
- wallabag-api is now a PCL and therefore compatible for more platforms.
- And some bugfixes, of course :D

## 1.0.4
- Implement `IDisposable` for `WallabagClient`.
- Implement `IComparable` for `WallabagItem` and `WallabagTag`.
- `WallabagTag.Equals` is now based on the label instead of the id.

## 1.0.3
- Fix for `ObjectNotReferencedException`s when using LINQ.

## 1.0.2
- Include comments in NuGet package (they weren't until now)
- Change type of `PreviewImageUri` to `Uri`
- Add `GetItemsWithEnhancedMetadataAsync` which will return informations like page number, number of items along with items itself.

## 1.0.1
- some code cleanup
- New property `LastTokenRefreshDateTime` that indicates the last time when `RefreshAccessTokenAsync` was executed.

## 1.0.0
- Initial release.