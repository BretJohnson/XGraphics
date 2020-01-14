using System;

namespace XGraphics.ImageLoading.Cache
{
	public interface IMemoryCache
	{
		LoadedImage Get(string key);

		void Add(string key, LoadedImage bitmap);

		void Clear();

		void Remove(string key);

		void RemoveSimilar(string baseKey);
	}
}

