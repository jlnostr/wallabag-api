using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using wallabag.Api.Models;
using wallabag.Api.Responses;
using static wallabag.Api.WallabagClient;

namespace wallabag.Api
{
    /// <summary>
    /// Defines a class for collaborating with wallabag.
    /// </summary>
    public interface IWallabagClient : IDisposable
    {
        /// <summary>
        /// Gets or sets the URI of the wallabag instance.
        /// </summary>
        Uri InstanceUri { get; set; }

        /// <summary>
        /// Gets or sets the client id for accessing the API.
        /// </summary>
        string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret for accessing the API.
        /// </summary>
        string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the access token for accessing the API.
        /// <para>In most cases this property is set automatically after an execution of <see cref="GetAccessTokenAsync(CancellationToken)" /> or <see cref="RefreshAccessTokenAsync(CancellationToken)"/>.</para>
        /// </summary>
        string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token for accessing the API.
        /// <para>In most cases this property is set automatically after an execution of <see cref="GetAccessTokenAsync(CancellationToken)" /> or <see cref="RefreshAccessTokenAsync(CancellationToken)"/>.</para>
        /// </summary>
        string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets if exceptions should be thrown.
        /// </summary>
        bool ThrowHttpExceptions { get; set; }

        /// <summary>
        /// Gets the timeout in milliseconds for each HTTP request.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// The DateTime value that specifies the last execution method of <see cref="RefreshAccessTokenAsync"/>.
        /// </summary>
        DateTime LastTokenRefreshDateTime { get; set; }

        /// <summary>
        /// Returns the version number of the API as plain string.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<string> GetVersionNumberAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the version number of the API as instance of <see cref="Version"/>.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<Version> GetVersionAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Returns the value of <see cref="AccessToken"/>. If it's outdated, the <see cref="RefreshAccessTokenAsync(CancellationToken)"/> method will be called automatically.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Refreshes the <see cref="AccessToken"/> that is necessary for further API calls. In most cases it gets automatically executed if necessary.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RefreshAccessTokenAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Requests both <see cref="AccessToken"/> and <see cref="RefreshToken"/> using the given credentials.
        /// <para>Should be executed only once, for further functionality save both tokens somewhere.</para>
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RequestTokenAsync(string username, string password, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds an article to the server.
        /// </summary>
        /// <param name="uri">The URI (or URL in most cases) of the article.</param>
        /// <param name="tags">A list of tags that should be added to the article.</param>
        /// <param name="title">The title of the new article. This will override the default value by wallabag!</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<WallabagItem> AddAsync(Uri uri, IEnumerable<string> tags = null, string title = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches a list of articles from the server.
        /// </summary>
        /// <param name="isRead">Filters the list by read articles that have the given value.</param>
        /// <param name="isStarred">Filters the list by starred articles that have the given value.</param>
        /// <param name="dateOrder">Defines the ordering by date.</param>
        /// <param name="sortOrder">Defines the ordering by ascending/descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="since">Filters items where the <see cref="WallabagItem.LastUpdated"/> value is greater or equal to the given value.<para>Requires wallabag 2.1 or greater.</para></param>
        /// <param name="tags">Filters items by the given list of tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <see cref="WallabagItem"/>.</returns>
        Task<IEnumerable<WallabagItem>> GetItemsAsync(
            bool? isRead = null,
            bool? isStarred = null,
            WallabagDateOrder? dateOrder = null,
            WallabagSortOrder? sortOrder = null,
            int? pageNumber = null,
            int? itemsPerPage = null,
            DateTime? since = null,
            IEnumerable<string> tags = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches a list of articles with additional metadata from the server.
        /// </summary>
        /// <param name="isRead">Filters the list by read articles that have the given value.</param>
        /// <param name="isStarred">Filters the list by starred articles that have the given value.</param>
        /// <param name="dateOrder">Defines the ordering by date.</param>
        /// <param name="sortOrder">Defines the ordering by ascending/descending.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="itemsPerPage">The number of items per page.</param>
        /// <param name="since">Filters items where the <see cref="WallabagItem.LastUpdated"/> value is greater or equal to the given value.<para>Requires wallabag 2.1 or greater.</para></param>
        /// <param name="tags">Filters items by the given list of tags.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<ItemCollectionResponse> GetItemsWithEnhancedMetadataAsync(
           bool? isRead = null,
           bool? isStarred = null,
           WallabagDateOrder? dateOrder = null,
           WallabagSortOrder? sortOrder = null,
           int? pageNumber = null,
           int? itemsPerPage = null,
           DateTime? since = null,
           IEnumerable<string> tags = null,
           CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches one item from the server.
        /// </summary>
        /// <param name="itemId">The id of the item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<WallabagItem> GetItemAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches a list of tags from the server.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<IEnumerable<WallabagTag>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Marks an item as archived.
        /// </summary>
        /// <param name="itemId">The ID of the item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> ArchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Marks an item as archived.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> ArchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Unmarks an item as archived.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> UnarchiveAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Unmarks an item as archived.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> UnarchiveAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Marks an item as favorite.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> FavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Marks an item as favorite.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> FavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Unmarks an item as favorite.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> UnfavoriteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Unmarks an item as favorite.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> UnfavoriteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an item PERMANENTLY.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        Task<bool> DeleteAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds tags to an item.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="tags">The tags that should be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <see cref="WallabagTag"/>.</returns>
        Task<IEnumerable<WallabagTag>> AddTagsAsync(int itemId, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds tags to an item.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="tags">The tags that should be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <see cref="WallabagTag"/>.</returns>
        Task<IEnumerable<WallabagTag>> AddTagsAsync(WallabagItem item, IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes tags from an item.
        /// </summary>
        /// <param name="itemId">The ID of the item that should be updated.</param>
        /// <param name="tags">The tags that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagsAsync(int itemId, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes tags from an item.
        /// </summary>
        /// <param name="item">The item that should be updated.</param>
        /// <param name="tags">The tags that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagsAsync(WallabagItem item, IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes a tag from all items.
        /// </summary>
        /// <param name="tag">The tag that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagFromAllItemsAsync(WallabagTag tag, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes a tag from all items.
        /// </summary>
        /// <param name="tag">The tag that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagFromAllItemsAsync(string tag, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes tags from all items.
        /// </summary>
        /// <param name="tags">The tags that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<WallabagTag> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes tags from all items.
        /// </summary>
        /// <param name="tags">The tags that should be removed.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> RemoveTagsFromAllItemsAsync(IEnumerable<string> tags, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches all annotations for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <see cref="WallabagAnnotation"/>.</returns>
        Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(WallabagItem item, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Fetches all annotations for an item.
        /// </summary>
        /// <param name="itemId">The ID of the item.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A list of <see cref="WallabagAnnotation"/>.</returns>
        Task<IEnumerable<WallabagAnnotation>> GetAnnotationsAsync(int itemId, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds an annotation to an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="annotation">The annotation that should be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The final <see cref="WallabagAnnotation"/>.</returns>
        Task<WallabagAnnotation> AddAnnotationAsync(WallabagItem item, WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Adds an annotation to an item.
        /// </summary>
        /// <param name="itemId">The ID of the item.</param>
        /// <param name="annotation">The annotation that should be added.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The final <see cref="WallabagAnnotation"/>.</returns>
        Task<WallabagAnnotation> AddAnnotationAsync(int itemId, WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates an existing annotation.
        /// </summary>
        /// <param name="oldAnnotation">The old annotation that should be updated.</param>
        /// <param name="newAnnotation">The new annotation that should replace or update the old one.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated <see cref="WallabagAnnotation"/>.</returns>
        Task<WallabagAnnotation> UpdateAnnotationAsync(WallabagAnnotation oldAnnotation, WallabagAnnotation newAnnotation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Updates an existing annotation.
        /// </summary>
        /// <param name="oldAnnotationId">The ID of the old annotation that should be updated.</param>
        /// <param name="newAnnotation">The new annotation that should replace or update the old one.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated <see cref="WallabagAnnotation"/>.</returns>
        Task<WallabagAnnotation> UpdateAnnotationAsync(int oldAnnotationId, WallabagAnnotation newAnnotation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an annotation.
        /// </summary>
        /// <param name="annotation">The annotation that should be deleted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> DeleteAnnotationAsync(WallabagAnnotation annotation, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Deletes an annotation.
        /// </summary>
        /// <param name="annotationId">The ID of the annotation that should be deleted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<bool> DeleteAnnotationAsync(int annotationId, CancellationToken cancellationToken = default(CancellationToken));
        
        /// <summary>
        /// Event that is fired if <see cref="RefreshAccessTokenAsync"/> was successful.
        /// </summary>
        event EventHandler CredentialsRefreshed;

        /// <summary>
        /// Event that is fired before a HTTP request to the server is started.
        /// </summary>
        event EventHandler<PreRequestExecutionEventArgs> PreRequestExecution;

        /// <summary>
        /// Event that is fired after the HTTP request is complete.
        /// </summary>
        event EventHandler<HttpResponseMessage> AfterRequestExecution;
    }
}
