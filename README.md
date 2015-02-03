# GC Behavior in .NET Using Events

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

### Pubs and Subs
This demonstration involves two kinds of roles:
* `Publishers`
* `Subscribers`

`Publishers` expose events to the world, and `Subscribers` subscribe to those events. We may also refer to them more casually as pubs and subs.

Consider two ways a `Subscriber` can attach a handler to a `Publisher`'s event:

* Via a local method
```csharp
somePublisher.SomeEvent += somePublisher_SomeEvent;
```
```csharp
void somePublisher_SomeEvent(object sender, EventArgs e)
{
    // Some code here...
}
```
* Via an anonymous delegate
```csharp
somePublisher.SomeEvent += (sender, e) =>
{
    // Some code here...
};
```

The `MethodSubscriber` and `AnonymousSubscriber` classes each subscribe to a common base class, `Subscriber`. In that base class, a reference to a `Subscriber` has been instantiated.

The constructor of each child class immediately subscribes to its `Publishers`'s event, so that we have a subscription set up just by creating a new `MethodSubscriber` or `AnonymousSubscriber`.

### Observing Pubs and Subs
Now, let's consider a test of sorts. Using .NET's `WeakReference` so as not to affect garbage collection, the general steps are as follows:

1. Create a weak reference to a new `Subscriber`
2. Create a weak reference to the `Publisher` instance that our new `Subscriber` holds
3. Check both of them to see if they've been garbage collected
  * Of course at this stage, this seems silly to check. There's no reason either of these would have been collected immediately after creation, but let's check anyway
4. Force garbage collection
5. Check both of them again to see if one or both was actually collected by the GC
  * We can check this using the `IsAlive` property of each weak reference. A value of `false` means that the referenced object has indeed been garbage-collected.

##### Testing `MethodSubscriber`
We expect both objects we create to be cleaned up by the GC after step 4 for the following reasons:
* Only the `Subscriber` holds any references to its `Publisher`
* No references to the `Subscriber` exist anywhere
  * Therefore, no references to the `Publisher` can possibly exist to keep it in scope

After running our test, we can see that both objects were successfully collected.
  
##### Testing `AnonymousSubscriber`
Again, we expect both our pub and its sub to be collected after step 4, for the same reasons. The only difference this time is that our sub has attached an anonymous delegate to its pub's event, rather than a local method.
