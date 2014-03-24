﻿namespace AngleSharp
{
    using AngleSharp.Network;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a helper to construct objects with externally
    /// defined classes and libraries.
    /// </summary>
    static class UriExtensions
    {
        public static Task<Stream> LoadAsync(this Uri url)
        {
            return url.LoadAsync(CancellationToken.None);
        }

        public static async Task<Stream> LoadAsync(this Uri url, CancellationToken cancel)
        {
            var requester = DependencyResolver.Current.GetService<IHttpRequester>();

            if (requester == null)
                throw new NullReferenceException("No HTTP requester has been set up. Configure one by adding an entry to the current DependencyResolver.");

            var request = DependencyResolver.Current.GetService<IHttpRequest>();
            request.Address = url;
            request.Method = HttpMethod.GET;
            var response = await requester.RequestAsync(request, cancel);
            return response.Content;
        }
    }
}
