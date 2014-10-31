CacheHelper
===========

This is a library that provides helper methods to facilitate the caching process.

[![Build status](https://ci.appveyor.com/api/projects/status/9uyqwc93pur1dpdl/branch/master)](https://ci.appveyor.com/project/fredrikeriksson/cachehelper/branch/master)


##Setup
Just create a CacheEngine and instantiate provider of your choice.

Right now there is three supported providers:
* CacheHelper.Core -> MemoryCache (System.Runtime.Caching.MemoryCache)
* CacheHelper.Redis -> Redis
* CacheHelper.Azure -> Azure Cache
```cs
var cache = new CacheEngine(new MemoryCacheProvider());
var cache = new CacheEngine(new RedisCacheProvider());
var cache = new CacheEngine(new AzureCacheProvider());
```

#Examples
##Basics
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
##With parameters
This example shows how you easily can use an method with one ore more parameters of pretty much any type.
An key is build based on parameters of the method in the expression.
The key consists of the full name including namespace and also parameter values.
The reflected property information is also stored to minimize reflection impact.


```cs
private string Test()
{
    var cache = new CacheEngine(new MemoryCacheProvider());
    return cache.Get(() => ParameterMethod(1)))
}

private string ParameterMethod(int id)
{
    Thread.Sleep(1000);
    return "Test";
}
```

##Expiration
You can also change the default expiration timer of 5 minutes by providing the optional TimeSpan parameter like so:
```cs
cache.Get(() => NoParameterMethod(), TimeSpan.FromSeconds(2));
```
