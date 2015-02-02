using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCBehaviorVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            _run(HoldNoReferences, "HoldNoReferences");
            _run(HoldRefToPublisher1, "HoldRefToPublisher1");
            _run(HoldRefToPublisher2, "HoldRefToPublisher2");
        }

        private static void HoldRefToPublisher1()
        {
            var weakSub = new WeakReference(new AnonymousSubscriber());
            var weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);
            var publisher = ((AnonymousSubscriber) weakSub.Target).InternalPublisher;

            _runTests(weakSub, weakPub);
        }

        private static void HoldRefToPublisher2()
        {
            var weakSub = new WeakReference(new MethodSubscriber());
            var weakPub = new WeakReference(((MethodSubscriber) weakSub.Target).InternalPublisher);
            var publisher = ((MethodSubscriber) weakSub.Target).InternalPublisher;

            _runTests(weakSub, weakPub);
        }

        private static void HoldNoReferences()
        {
            var weakSub = new WeakReference(new AnonymousSubscriber());
            var weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);

            _runTests(weakSub, weakPub);
        }

        private static void _run(Action fn, string name)
        {
            Console.WriteLine("==== {0} ====", name);
            fn();
            Console.WriteLine();
        }

        private static void _runTests(WeakReference weakSub, WeakReference weakPub)
        {
            _printStatus(weakSub, "weakSub");
            _printStatus(weakPub, "weakPub");
            _runGC();
            _printStatus(weakSub, "weakSub");
            _printStatus(weakPub, "weakPub");
        }

        private static void _runGC()
        {
            Console.WriteLine("Running GC...");
            GC.Collect();
        }

        private static void _printStatus(WeakReference reference, string name)
        {
            Console.WriteLine("{0} has been collected: {1}", name, !reference.IsAlive);
        }
    }
}
