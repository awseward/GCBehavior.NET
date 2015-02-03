# GC Behavior

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

This demonstration involves two kinds of roles:
* `Publishers`
* `Subscribers`

`Publishers` expose public events, and `Subscribers` subscribe to those events.

Consider two ways a `Subscriber` can attach a handler to a `Publisher`'s event.

* Via a local method
```csharp
somePublisher.SomeEvent += somePublisher_SomeEvent;
```
```csharp
void somePublisher_SomeEvent(object sender, EventArgs e)
{
  ...
}
```
* Via an anonymous delegate
```csharp
somePublisher.SomeEvent += (sender, e) => { ... };
```

The `MethodSubscriber` and `AnonymousSubscriber` classes each subscribe to a `Publisher` instantiated in their common base class, `Subscriber`, using these two approaches respectively.

Now, let's consider a test of sorts. Using .NET's `WeakReference` as as not to affect garbage collection, the general steps are as follows:

1. Create a weak reference to a `Subscriber` instance
2. Create another weak reference to that `Subscriber`'s internal `Publisher` instance
3. Check both of them to see if they've been garbage collected*
4. Force garbage collection
5. Check both of them again to see if one or both was actually collected by the GC

\*Of course at this stage, this seems silly to check. There's no reason either of these would have been collected immediately after being created, but let's check anyway.
