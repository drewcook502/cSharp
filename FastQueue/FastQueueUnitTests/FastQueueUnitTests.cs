using FastQueue;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FastQueueUnitTests
{
    [TestClass]
    public class FastQueueUnitTests
    {
        private const int maxNumber = 100000;

        [TestMethod]
        public void EmptyConstructorAndEmptyQueueTest()
        {
            FastQueue<int> q = new FastQueue<int>();

            Assert.IsTrue(q.Count == 0, "Count is non-zero!");
            Assert.IsTrue(!q.Any(), "FastQueue has a member when empty!");

            q.Clear();

            Assert.IsTrue(q.Count() == 0, "Count is non-zero!");
            Assert.IsTrue(!q.Any(), "FastQueue has a member when empty!");

            bool looped = false;
            foreach (int i in q)
            {
                looped = true;
            }
            Assert.IsTrue(!looped, "FastQueue looped even though empty!");

            Assert.ThrowsException<InvalidOperationException>(() => q.Dequeue(), "FastQueue didn't throw InvalidOperationException on empty Dequeue()");

            Assert.ThrowsException<InvalidOperationException>(() => q.Peek(), "FastQueue didn't throw InvalidOperationException on empty Peek()");

        }

        [TestMethod]
        public void ConstructorWithSingleArgument()
        {
            const int givenItem = 0;
            FastQueue<int> q = new FastQueue<int>(givenItem);

            Assert.IsTrue(q.Count == 1, "FastQueue should have one item but doesn't!");
            Assert.IsTrue(q.Any(), "FastQueue should have one item but doesn't!");
            Assert.IsTrue(q.Peek() == givenItem, "FastQueue.Peek() didn't return the expected value");
        }

        [TestMethod]
        public void ConstructorWithCollectionAndInterationTest()
        {
            List<string> food = new List<string>();
            food.Add("Pizza");

            FastQueue<string> q = new FastQueue<string>(food);
            Assert.IsTrue(q.Count == 1, "FastQueue should have one item but doesn't!");
            Assert.IsTrue(q.Peek() == food[0], "FastQueue is missing the expected value!");

            food.Add("Eggs");
            q = new FastQueue<string>(food);
            Assert.IsTrue(q.Count == 2, "FastQueue should have two items but doesn't!");
            Assert.IsTrue(q.Peek() == food[0]);
            Assert.IsTrue(q.Dequeue() == food[0]);
            Assert.IsTrue(q.Peek() == food[1]);
            Assert.IsTrue(q.Dequeue() == food[1]);
            Assert.IsTrue(q.Count == 0);
            Assert.IsFalse(q.Any());

            food.Add("Chicken");
            food.Add("Bratz");
            food.Add("Cheese");

            q = new FastQueue<string>(food);
            Assert.IsTrue(q.Count == 5);

            for (int i = 0; i < food.Count; i++)
            {
                string f = food[i];
                Assert.IsTrue(f == q.Peek());
                Assert.IsTrue(f == q.Dequeue());
            }
            Assert.ThrowsException<InvalidOperationException>(() => q.Peek());
        }

        [TestMethod]
        public void EnqueueDequeueTest()
        {
            FastQueue<int> q = new FastQueue<int>();

            for (int i = 0; i < maxNumber; i++)
            {
                q.Enqueue(i);
            }

            Assert.IsTrue(q.Count == maxNumber);

            int expected = 0;
            for (int i = maxNumber - 1; i >= 0; i--)
            {
                Assert.IsTrue(q.Peek() == expected, "Peek() does not equal i");
                Assert.IsTrue(q.Dequeue() == expected++, "Dequeue() does not equal i");
            }

            Assert.IsTrue(q.Count == 0, "Count is not 0");
        }

        [TestMethod]
        public void EnqueuePerformanceTest()
        {
            Stopwatch listWatch = Stopwatch.StartNew();
            List<int> list = new List<int>();
            for (int i = 0; i <= maxNumber; i++)
            {
                list.Add(i);
                //list.TrimExcess();
            }
            listWatch.Stop();
            float listElapsedTime = listWatch.ElapsedTicks;
            list.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Stopwatch qWatch = Stopwatch.StartNew();
            Queue<int> q = new Queue<int>();
            for (int i = 0; i <= maxNumber; i++)
            {
                q.Enqueue(i);
                //q.TrimExcess();
            }
            qWatch.Stop();
            float qElapsedTime = qWatch.ElapsedTicks;
            q.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Stopwatch linkedWatch = Stopwatch.StartNew();
            LinkedList<int> linked = new LinkedList<int>();
            for (int i = 0; i <= maxNumber; i++)
            {
                linked.AddLast(i);
            }
            linkedWatch.Stop();
            float linkedElapedTime = linkedWatch.ElapsedTicks;
            linked.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Stopwatch fqWatch = Stopwatch.StartNew();
            FastQueue<int> fq = new FastQueue<int>();
            for (int i = 0; i <= maxNumber; i++)
            {
                fq.Enqueue(i);
            }
            fqWatch.Stop();
            float fqElapsedTime = fqWatch.ElapsedTicks;
            fq.Clear();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Assert.IsTrue(listElapsedTime > fqElapsedTime, "list is faster than FastQueue! {0} to {1}", listElapsedTime, fqElapsedTime);
            Assert.IsTrue(qElapsedTime > fqElapsedTime, "q is faster than FastQueue! {0} to {1}", qElapsedTime, fqElapsedTime);
            Assert.IsTrue(linkedElapedTime > fqElapsedTime, "linked list is faster than FastQueue! {0} to {1}", linkedElapedTime, fqElapsedTime);
        }

        [TestMethod]
        public void DequeuePerformanceTest()
        {
            List<int> storage = new List<int>(maxNumber);
            for (int i = 0; i <= maxNumber; i++)
            {
                storage.Add(i);
            }

            List<int> list = new List<int>(storage);
            Queue<int> q = new Queue<int>(storage);
            LinkedList<int> linked = new LinkedList<int>(storage);
            FastQueue<int> fq = new FastQueue<int>(storage);

            Stopwatch listTimer = Stopwatch.StartNew();
            while (list.Any())
            {
                list.RemoveAt(0);
                //list.TrimExcess();
            }
            listTimer.Stop();
            float listTime = listTimer.ElapsedTicks;

            Stopwatch qTimer = Stopwatch.StartNew();
            while (q.Any())
            {
                q.Dequeue();
                //q.TrimExcess();
            }
            qTimer.Stop();
            float qTime = qTimer.ElapsedTicks;

            Stopwatch linkedTimer = Stopwatch.StartNew();
            while (linked.Any())
            {
                linked.RemoveFirst();
            }
            linkedTimer.Stop();
            float linkedTime = linkedTimer.ElapsedTicks;

            Stopwatch fqTimer = Stopwatch.StartNew();
            while (fq.Any())
            {
                fq.Dequeue();
            }
            fqTimer.Stop();
            float fqTime = fqTimer.ElapsedTicks;

            Assert.IsTrue(listTime > fqTime, "List is faster than FastQueue! {0} to {1}, listTime, fqTime");
            Assert.IsTrue(qTime > fqTime, "Queue is faster than FastQueue! {0} to {1}", qTime, fqTime);
            Assert.IsTrue(linkedTime > fqTime, "linked list is faster than FastQueue! {0} to {1}", linkedTime, fqTime);
        }
    }
}
