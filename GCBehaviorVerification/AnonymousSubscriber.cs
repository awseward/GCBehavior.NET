using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCBehaviorVerification
{
    public class AnonymousSubscriber : Subscriber
    {
        public AnonymousSubscriber()
        {
            _internalPublisher.DidSomething += (sender, e) => { };
        }
    }
}
