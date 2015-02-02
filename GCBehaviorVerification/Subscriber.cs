using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCBehaviorVerification
{
    public abstract class Subscriber
    {
        protected Publisher _internalPublisher = new Publisher();

        // This property kind of breaks the idea, since if the publisher was
        // truly 'internal', PublisherHolder wouldn't expose it like this, but
        // we need to in order to use the GC in different ways
        public Publisher InternalPublisher { get { return _internalPublisher; } }
    }
}
