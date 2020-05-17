using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

public class Example
{
    private static Semaphore full, empty;
    private static object lockobj = new object();
    private static Queue<int> myqueue;
    public static void Main()
    {
        full = new Semaphore(0, 5);
        empty = new Semaphore(5, 5);
        myqueue = new Queue<int>();

        Thread[] prodThread = new Thread[5];
        Thread[] consThread = new Thread[5];

        for(int i=0; i<5; i++)
        {
            prodThread[i] = new Thread(produce);
            prodThread[i].Start();
        }

        for (int i = 0; i < 5; i++)
        {
            consThread[i] = new Thread(consume);
            consThread[i].Start();
        }

        prodThread[0].Join();
        prodThread[1].Join();
        prodThread[2].Join();
        prodThread[3].Join();
        prodThread[4].Join();

        consThread[0].Join();
        consThread[1].Join();
        consThread[2].Join();
        consThread[3].Join();
        consThread[4].Join();

        Console.ReadKey();

    }

    private static void consume()
    {
        for(int i=0; i<3; i++)
        {
            full.WaitOne();
            lock(lockobj)
            {
                myqueue.Dequeue();
                Console.WriteLine("Consumer Consumed, current queue count= " + myqueue.Count);
            }
            empty.Release();
        }
    }

    private static void produce()
    {
        for(int i = 0; i<3; i++)
        {
            empty.WaitOne();

            lock(lockobj)
            {
                myqueue.Enqueue(i);
                Console.WriteLine("Producer Produced, current queue count= " + myqueue.Count);
            }
            full.Release();
        }
    }
}