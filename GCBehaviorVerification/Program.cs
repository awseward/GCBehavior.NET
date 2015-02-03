using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace GCBehaviorVerification
{
    class Program
    {
        static void Main(string[] args)
        {
            Run(HoldNoReferences_AnonymousSubscriber);
            Run(HoldNoReferences_MethodSubscriber);
            Run(HoldReference_AnonymousSubscriber);
            Run(HoldReference_MethodSubscriber);

            Debugger.Break();
        }

        private static void HoldNoReferences_AnonymousSubscriber()
        {
            WeakReference weakSub = new WeakReference(new AnonymousSubscriber());
            WeakReference weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);

            Test(weakSub, weakPub);
        }

        private static void HoldNoReferences_MethodSubscriber()
        {
            WeakReference weakSub = new WeakReference(new MethodSubscriber());
            WeakReference weakPub = new WeakReference(((MethodSubscriber) weakSub.Target).InternalPublisher);

            Test(weakSub, weakPub);
        }

        private static void HoldReference_AnonymousSubscriber()
        {
            WeakReference weakSub = new WeakReference(new AnonymousSubscriber());
            WeakReference weakPub = new WeakReference(((AnonymousSubscriber) weakSub.Target).InternalPublisher);
            Publisher publisher = ((AnonymousSubscriber) weakSub.Target).InternalPublisher;

            Test(weakSub, weakPub);
        }

        private static void HoldReference_MethodSubscriber()
        {
            WeakReference weakSub = new WeakReference(new MethodSubscriber());
            WeakReference weakPub = new WeakReference(((MethodSubscriber) weakSub.Target).InternalPublisher);
            Publisher publisher = ((MethodSubscriber) weakSub.Target).InternalPublisher;

            Test(weakSub, weakPub);
        }

        private static void Run(Action actn)
        {
            Console.WriteLine("==== {0} ====", actn.Method.Name);
            actn();
            Console.WriteLine();
        }

        private static void Test(WeakReference weakSub, WeakReference weakPub)
        {
            var sub = new { Ref = weakSub, Name = "weakSub" };
            var pub = new { Ref = weakPub, Name = "weakPub" };
            Action<dynamic> check = anon => Print(anon.Ref, anon.Name);

            check(sub);
            check(pub);
            Console.WriteLine("Collecting garbage...");
            GC.Collect();
            check(sub);
            check(pub);
        }

        private static void Print(WeakReference reference, string name)
        {
            Console.WriteLine("{0} has been collected: {1}", name, !reference.IsAlive);
        }
    }
}
