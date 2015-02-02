using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCBehaviorVerification
{
    public class MethodSubscriber : Subscriber
    {
        public MethodSubscriber()
        {
            _internalPublisher.DidSomething += _internalPublisher_DidSomething;
        }

        private void _internalPublisher_DidSomething(object sender, EventArgs e) { }
    }
}
