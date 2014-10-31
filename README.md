CacheHelper
===========

This is a library that provides helper methods to facilitate the caching process.

[![Build status](https://ci.appveyor.com/api/projects/status/9uyqwc93pur1dpdl/branch/master)](https://ci.appveyor.com/project/fredrikeriksson/cachehelper/branch/master)

Examples

```cs
private string Test()
{
    var cache = new CacheEngine(new MemoryCacheProvider());
    return cache.Get(() => NoParameterMethod()))
}

private string NoParameterMethod()
{
    return "Test";
}
```