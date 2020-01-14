﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace XGraphics.ImageLoading.Cache
{
    public interface IDiskCache
    {
        /// <summary>
        /// Adds the file to cache and file saving queue if it does not exists.
        /// </summary>
        /// <param name="key">Key to store/retrieve the file.</param>
        /// <param name="bytes">File data in bytes.</param>
        /// <param name="duration">Specifies how long an item should remain in the cache.</param>
        Task AddToSavingQueueIfNotExistsAsync(string key, byte[] bytes, TimeSpan duration);

        Task<Stream?> TryGetStreamAsync(string key);

        Task RemoveAsync(string key);

        Task ClearAsync();

        Task<bool> ExistsAsync(string key);

        Task<string?> GetFilePathAsync(string key);
    }
}
