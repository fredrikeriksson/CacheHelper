CacheHelper
===========

This is a library that provides helper methods to facilitate the caching process.

[![Build status](https://ci.appveyor.com/api/projects/status/9uyqwc93pur1dpdl/branch/master)](https://ci.appveyor.com/project/fredrikeriksson/cachehelper/branch/master)

#Examples

##Setup
Just create a CacheEngine and instantiate provider of your choice.
```
var cache = new CacheEngine(new MemoryCacheProvider());
```
Right now there is three supported providers:
* MemoryCache (System.Runtime.Caching.MemoryCache)
* Redis
* Azure Cache

##Basic
Example below shows how you can use the cache to cache and return an parameterless method.
NoParameterMethod will only be run once during the duration of the cached item.
```cs
private string Test()
{
    var cache = new CacheEngine(new MemoryCacheProvider());
    return cache.Get(() => NoParameterMethod()))
}

private string NoParameterMethod()
{
    Thread.Sleep(1000);
    return "Test";
}
```
##Expiration
You can also change the default expiration timer of 5 minutes by providing the optional TimeSpan parameter like so:
```
cache.Get(() => NoParameterMethod(), TimeSpan.FromSeconds(2));
```
