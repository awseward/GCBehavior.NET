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
            _run(HoldReference_AnonymousSubscriber);
            _run(HoldReference_MethodSubscriber);
        }

        private static void HoldNoReferences()
        {
            WeakReference weakSub = new WeakReference(new AnonymousSubscriber());
            WeakReference weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);

            _test(weakSub, weakPub);
        }

        private static void HoldReference_AnonymousSubscriber()
        {
            WeakReference weakSub = new WeakReference(new AnonymousSubscriber());
            WeakReference weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);
            Publisher publisher = ((AnonymousSubscriber) weakSub.Target).InternalPublisher;

            _test(weakSub, weakPub);
        }

        private static void HoldReference_MethodSubscriber()
        {
            WeakReference weakSub = new WeakReference(new MethodSubscriber());
            WeakReference weakPub = new WeakReference(((MethodSubscriber) weakSub.Target).InternalPublisher);
            Publisher publisher = ((MethodSubscriber) weakSub.Target).InternalPublisher;

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
