# GC Behavior

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

This demonstration involves two kinds of roles:
* `Publishers`
* `Subscribers`

`Publishers` expose public events, and `Subscribers` subscribe to those events.

Let's consider two ways a `Subscriber` can attach a handler to a `Publisher`'s event.

* Via an anonymous delegate
```csharp
somePublisher.SomeEvent += (sender, e) => { ... };
```
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

The `AnonymousSubscriber` and `MethodSubscriber` classes each subscribe to a `Publisher` instantiated in their common base class, `Subscriber`, using these two ways respectively.
