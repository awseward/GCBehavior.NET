# GC Behavior

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

This demonstration involves two kinds of roles:
* `Publishers`
* `Subscribers`

`Publishers` expose public events, and `Subscribers` subscribe to those events.

Let's consider two ways a `Subscriber` can attach a handler to a `Publisher`'s event.

1. Via an anonymous delegate
```csharp
someObject.SomeEvent += (sender, e) => { ... };
```
2. Via a local method
```csharp
someObject.SomeEvent += someObject_SomeEvent;
```
```csharp
void someObject_SomeEvent(object sender, EventArgs e)
{
  ...
}
```

The `AnonymousSubscriber` and `MethodSubscriber` classes exist to illustrate examples of each way to subscribe, respectively. They each derive from the base `Subscriber` class and subscribe to an event belonging to a `Publisher` instance instantiated there.

