# NaturalStringComparer
[![Build status](https://ci.appveyor.com/api/projects/status/acwd0d2hwng5l5yh?svg=true)](https://ci.appveyor.com/project/chiefmb/naturalstringcomparer)

## install
```sh
dotnet add package GihanSoft.String.NaturalComparer --version 3.0.0
```
or
```pwsh
Install-Package GihanSoft.String.NaturalComparer -Version 3.0.0
```
or
```xml
<!-- add this to .csproj -->
<ItemGroup>
    <PackageReference Include="GihanSoft.String.NaturalComparer" Version="3.0." />
</ItemGroup>
```

`LINQ` like extension methods and `Comparer<string>` that 
can be used to sort strings by number in them.

as what windows explorer do {"text1", "text2", "text10"} instead of {"text1", "text10", "text2"}). 

### Features
* **Pure C#**
* **Fast** (because of internal unmanaged code).
* .NET Standard 1.0
* Cross-Platform
* Optional use of `StringComparison` `enum` as base.

### Usage

```C#
var stringList = new List<string>
{
    "number1", "number2", "number3", "number4", "number10", "number15", "number22", "number26"
    , "number9", "number33", "number5", "number12"
};

stringList.Sort(new NaturalComparer(StringComparison.Ordinal));
//or
stringList.NaturalSort();

Console.WriteLine("Natural Sort:");
foreach(var item in stringList)
{
  Console.WriteLine(item);
}

stringList.Sort();
Console.WriteLine();
Console.WriteLine("Normal Sort:");
foreach(var item in stringList)
{
  Console.WriteLine(item);
}

/* output
Natural Sort:
number1
number2
number3
number4
number5
number9
number10
number12
number15
number22
number26
number33

Normal Sort:
number1
number10
number12
number15
number2
number22
number26
number3
number33
number4
number5
number9
*/
```

#### types

```C#
public class NaturalComparer : IComparer<string?> 
{
}
```

#### extension methods

```C#
// List<T> extension
public static void NaturalSort<TSource>(
            this List<TSource> src,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal);

// T[] extension
public static void NaturalSort<TSource>(
            this TSource[] src,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal);

// IEnumerable<T> extensions
public static IOrderedEnumerable<TSource> NaturalOrderBy<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal);

public static IOrderedEnumerable<TSource> NaturalOrderByDescending<TSource>(
            this IEnumerable<TSource> source,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal);
            
public static IOrderedEnumerable<TSource> NaturalThenBy<TSource>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal);
            
public static IOrderedEnumerable<TSource> NaturalThenByDescending<TSource>(
            this IOrderedEnumerable<TSource> source,
            Func<TSource, string?>? keySelector = null,
            StringComparison stringComparison = StringComparison.Ordinal)
```
