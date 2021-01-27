# C# Migration Practices

[[_TOC_]]

Legends
 - 🚀: The work that is covered by automation `Sharpify`(the old tool)
 - 🔥: The works that is covered by automation `CSharpifier`(the new tool)

## Keywords
Reinterpret C++/CX keywords to C# types
| C++/CX Type                                    | C# Type                                         |
|------------------------------------------------|-------------------------------------------------|
| 🚀nullptr                                     | null                                            |
| 🚀auto                                        | var                                             |
| 🚀enum class                                  | enum                                            |
| 🚀static_cast&lt;TargetT&gt;(bar)             | (TargetT)bar                                    |
| 🚀dynamic_cast&lt;TargetT&gt;(bar)            | (bar as TargetT)                                |
| 🚀reinterpret_cast&lt;TargetT&gt;(bar)        | (bar as TargetT)                                |
| 🚀const_cast&lt;TargetT&gt;(bar)              | (bar as TargetT) ???                            |
| 🚀co_await                                    | await                                           |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |






## Types

Reinterpret C++/CX types to C# types

| C++/CX Type                                    | C# Type                                         |
|------------------------------------------------|-------------------------------------------------|
| Platform::StringReference                      | readonly string                                 |
| 🚀Platform::Object                            | Object  ??? shoulde it be `object`?              |
| 🚀std::unorderedmap&lt;KeyT,ValT&gt;          | Dictionary&lt;KeyT,ValT&gt;                     |
| 🚀std::map&lt;KeyT,ValT&gt;                   | SortedDictionary&lt;KeyT,ValT&gt;                     |
| 🚀std::vector&lt;KeyT,ValT&gt;                | List&lt;KeyT,ValT&gt;                     |
| 🚀concurrency::task&lt;T&gt;                  | Task&lt;T&gt;                                   |
| 🚀fire_and_forget                             | Task                                            |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |
|                                                |                                                 |


## Locks

### concurrency::reader_writer_lock

C++/CX
```
// definition
    concurrency::reader_writer_lock m_lockName;

// usage (RAII)
    ...
    reader_writer_lock::scoped_lock lock(m_lockName);
    // do something...
```

C#
```
// definition
    readonly object m_lockNameMutex = new object();

// usage
    ...
    lock(m_lockNameMutex)
    { // do something... }
```

## Global Functions and Variables

C++/CX 
```

// header file
namespace CalculatorNS
{
    namespace DemoNS
    {
        extern Platform::StringReference TextA;
        extern Platform::StringReference TextB;

        int AddNumber(int a, int b);
    }
}

// source file
namespace CalculatorNS
{
    namespace DemoNS
    {
        StringReference TextA(L"Hello C#");
        StringReference TextB(L"Goodbye C++/CX");

        int Add
    }
}

```

C#
```
namespace CalculatorNS
{
    namespace DemoNS
    {
        static public partial class Globals
        {
            public static readonly string TextA("Hello C#");
            public static readonly string TextB("Goodbye C++/CX");
        }
    }
}
```

## PPL Related (Async)

### create_task

C++/CX
```
void foo()
{
    create_task(some_async_call);
}
```

C#
```
void foo()
{
    _ = some_async_call;
}
```



## Miscellaneous 

### Events

C++/CX
```
    this->Suspending += ref new SuspendingEventHandler(this, &App::OnSuspending);
```

C#
```
    this.Suspending += OnSuspending;
```






