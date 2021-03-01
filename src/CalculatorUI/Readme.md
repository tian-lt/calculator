# C# Migration Practices

[[_TOC_]]

Legends
 - 🚀: The work that is covered by automation `Sharpify`(the old tool)
 - 🔥: The work that is covered by automation `CSharpifier`(the new tool)


## Starting Point:

This C# UI feature branch was forked from below commit of microsoft/master branch

`b7ca0eb - Updateto2101release (#1457) - Cory Volk (master)`

## Comments

Leave the comment started with `// CSHARP_MIGRATION: TODO:` whenever there's future works required for C# migration such as double-check, unresolved requirments etc.

For example:
```
private void OnConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs e)
{
    // CSHARP_MIGRATION: TODO:
    //TraceLogger.GetInstance().DecreaseWindowCount();
    if (m_parent.IsAlive)
    {
        var parent = m_parent.Target as App;
        parent.RemoveWindow(this);
    }
}
```

## Keywords
Reinterpret C++/CX keywords to C# types
| C++/CX Type                                    | C# Type                                         |
|------------------------------------------------|-------------------------------------------------|
| 🚀nullptr                                     | null                                            |
| 🚀auto                                        | var                                             |
| 🚀enum class                                  | enum                                            |
| 🚀static_cast&lt;TargetT&gt;(bar)             | (TargetT)bar                                    |
| [safe_cast](https://docs.microsoft.com/en-us/cpp/extensions/safe-cast-cpp-component-extensions?view=msvc-160)&lt;TargetT&gt;(bar)               | (TargetT)bar                               |
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

1. If `concurrency::reader_writer_lock` was being used in a way as same as what mutex does, then reinterpret it to lock statement.

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
2. However, if the code was really using `concurrency::reader_writer_lock` to distinguish reading and writing scenarios, then we need to consider `System.Threading.ReaderWriterLockSlim` in C#. 

C++/CX
```
// definition
    concurrency::reader_writer_lock m_lockName;

// usage
void bar::foo1()
{
    reader_writer_lock::scoped_lock_read lock(m_lockName);
    // read some data from container
}

void bar::foo2()
{
    reader_writer_lock::scoped_lock lock(m_lockName);
    // write some data into container
}
```

C#
```
class bar
{
    ...
// definition
    private ReaderWriterLockSlim m_lockName = new ReaderWriterLockSlim();

// usage
    public void foo1()
    {
        m_lockName.EnterReadLock();
        try
        {
            // read some data from container
        }
        finally
        {
            m_lockName.ExitReadLock();
        }
    }

    public void foo2()
    {
        m_lockName.EnterWriteLock();
        try
        {
            // write some data into container
        }
        finally
        {
            m_lockName.ExitWriteLock();
        }
    }
}
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

        int AddNumber(int a, int b) { return a + b; }
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

            public static int AddNumber(int a, int b) { return a + b; }
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
    create_task(<some_async_call>);
}
```

C#
```
void foo()
{
    _ = <some_async_call>;
}
```

### create_task().then().then()...

C++/CX
```
[...]()
{
    create_task(ReturnBoolAsync())
        .then([...](bool flag)
        {
            bar(flag);
        });
}
```

C#
```
async () =>
{
    flag = await ReturnBoolAsync();
    bar(flag);
}
```

### async methods
C++/CX
```
task<void> foo()
{
    auto bar = co_await DoSthAsync();
    // do sth with bar
    co_await bar->AsyncCall();
}
```

C#
```
async Task foo()
{
    var bar = await DoSthAsync();
    // do sth with bar
    await bar->AsyncCall();
}
```

### use_current & use_arbitrary
C++/CX
```
{
    DoSthAsync()
        .then([]()
        {
            // do sth else
        },
        task_continuation_context::use_arbitrary())
        .then([]()
        {
            // do sth else else
        },
        task_continuation_context::use_current())
}
```

C#
```
{
    await DoSthAsync();
    await Task.Run(()=>
    {
        // do sth else
    }).ConfigureAwait(false /* task_continuation_context::use_arbitrary() */);
    await Task.Run(()=>
    {
        // do sth else else
    }) /* task_continuation_context::use_current() */;
}
```

## Dependency Property

### DEPENDENCY_PROPERTY_OWNER(owner)
C++/CX
```
DEPENDENCY_PROPERTY_OWNER(KeyboardShortcutManager);
```

C#: Remove such code lines since there's no need for C# code to persist such DP context.

### DEPENDENCY_PROPERTY_WITH_CALLBACK(type, name)

C++/CX
```
public:
    DEPENDENCY_PROPERTY_WITH_CALLBACK(Windows::UI::Xaml::FrameworkElement^, Source);

...

private:
    void OnSourcePropertyChanged(Windows::UI::Xaml::FrameworkElement^ oldValue, Windows::UI::Xaml::FrameworkElement^ newValue);
```

C#
```
public FrameworkElement Source
{
    get { return (FrameworkElement)GetValue(SourceProperty); }
    set { SetValue(SourceProperty, value); }
}

// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc...
public static readonly DependencyProperty SourceProperty =
    DependencyProperty.Register("Source", typeof(FrameworkElement), typeof(ControlSizeTrigger), new PropertyMetadata(default(FrameworkElement), (sender, args)=>
    {
        var self = (ControlSizeTrigger)sender;
        self.OnSourcePropertyChanged((FrameworkElement)args.OldValue, (FrameworkElement)args.NewValue);
    }));

...

private void OnSourcePropertyChanged(FrameworkElement oldValue, FrameworkElement newValue)
{...}

```

### DEPENDENCY_PROPERTY_ATTACHED_WITH_CALLBACK(type, name)

C++/CX
```
public:
    DEPENDENCY_PROPERTY_ATTACHED_WITH_CALLBACK(Platform::String ^, Character);

...

private:
    static void OnCharacterPropertyChanged(Windows::UI::Xaml::DependencyObject ^ target, Platform::String ^ oldValue, Platform::String ^ newValue);
```

C#
```
public string Character
{
    get { return (string)GetValue(CharacterProperty); }
    set { SetValue(CharacterProperty, value); }
}

// Using a DependencyProperty as the backing store for string.  This enables animation, styling, binding, etc...
public static readonly DependencyProperty CharacterProperty =
    DependencyProperty.RegisterAttached("Character", typeof(string), typeof(KeyboardShortcutManager), new PropertyMetadata(default(string), (sender, args)=>
    {
        OnCharacterPropertyChanged(sender, args.OldValue as string, args.NewValue as string);
    }));

...

private static void OnCharacterPropertyChanged(DependencyObject target, String oldValue, String newValue)
{...}

```


## Exceptions Handling (TBD)


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






