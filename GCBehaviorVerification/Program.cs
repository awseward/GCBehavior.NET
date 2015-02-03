using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GCBehaviorVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            _run(HoldNoReferences);
            _run(HoldReferenceToPublisher_AnonymousSubscriber);
            _run(HoldReferenceToPublisher_MethodSubscriber);
        }

        private static void HoldReferenceToPublisher_AnonymousSubscriber()
        {
            var weakSub = new WeakReference(new AnonymousSubscriber());
            var weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);
            var publisher = ((AnonymousSubscriber) weakSub.Target).InternalPublisher;

            _test(weakSub, weakPub);
        }

        private static void HoldReferenceToPublisher_MethodSubscriber()
        {
            var weakSub = new WeakReference(new MethodSubscriber());
            var weakPub = new WeakReference(((MethodSubscriber) weakSub.Target).InternalPublisher);
            var publisher = ((MethodSubscriber) weakSub.Target).InternalPublisher;

            _test(weakSub, weakPub);
        }

        private static void HoldNoReferences()
        {
            var weakSub = new WeakReference(new AnonymousSubscriber());
            var weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);

            _test(weakSub, weakPub);
        }

        private static void _run(Action fn)
        {
            Console.WriteLine("==== {0} ====", fn.Method.Name);
            fn();
            Console.WriteLine();
        }

        private static void _test(WeakReference weakSub, WeakReference weakPub)
        {
            _printStatus(weakSub, "weakSub");
            _printStatus(weakPub, "weakPub");

            Console.WriteLine("Collecting...");
            GC.Collect();

            _printStatus(weakSub, "weakSub");
            _printStatus(weakPub, "weakPub");
        }

        private static void _printStatus(WeakReference reference, string name)
        {
            Console.WriteLine("{0} has been collected: {1}", name, !reference.IsAlive);
        }
    }
}
