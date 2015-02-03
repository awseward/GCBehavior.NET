# GC Behavior

Events and event handlers can have some unexpected impacts on when objects subscribing to and raising them get cleaned up by the garbage collector. This is an attempt to illustrate what those impacts can be in just a few example cases.  

This demonstration involves two kinds of roles:
* `Publishers`
* `Subscribers`

`Publishers` expose public events, and `Subscribers` subscribe to those events.

Let's consider two ways a `Subscriber` can attach a handler to a `Publisher`'s event.

1. Via an anonymous delegate
2. Via a local method