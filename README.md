# GC Behavior in .NET Using Events

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

#### Pubs and Subs
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

#### Observing Pubs and Subs
Now, let's consider a test of sorts. Using .NET's `WeakReference` so as not to affect garbage collection, the general steps are as follows:

1. Create a weak reference to a `Subscriber` instance
2. Create another weak reference to that `Subscriber`'s internal `Publisher` instance
3. Check both of them to see if they've been garbage collected*
4. Force garbage collection
5. Check both of them again to see if one or both was actually collected by the GC

\*Of course at this stage, this seems silly to check. There's no reason either of these would have been collected immediately after being created, but let's check anyway.

##### Testing `MethodSubscriber`
We expect that, after garbage collection, both objects we created (the `Publisher` and the `Subscriber`) got cleaned up by the GC. We can check this using the `IsAlive` property of each weak reference we held on to. If the property returns `false`, the object was collected.

We would expect this outcome for the following reasons
* Only the `Subscriber` holds any references to its `Publisher`
* No references to the `Subscriber` exist anywhere
  * Therefore, no references to the `Publisher` can possibly exist to keep it in scope

Sure enough, both objects are successfully collected.
  
