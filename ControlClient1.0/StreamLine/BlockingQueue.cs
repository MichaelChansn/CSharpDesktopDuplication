using ControlClient1._0.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControlClient1._0.StreamLine
{
            class BlockingQueue<T>
            {
                public readonly Queue<T> queue = null;
                private readonly int maxSize=10;
                public BlockingQueue(int maxSize) { this.maxSize = maxSize; queue = new Queue<T>(); }

                public void Enqueue(T item)
                {
                    lock (queue)
                    {
                        while (queue.Count >= maxSize)
                        {
                            try
                            {
                                Monitor.Wait(queue);
                            }
                            catch (ThreadInterruptedException ex)
                            {
                                ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + ex.StackTrace);
                                goto END;
                            }
                        }
                        queue.Enqueue(item);
                        if (queue.Count == 1)
                        {
                            // wake up any blocked dequeue
                            Monitor.PulseAll(queue);
                        }
                    }
                END:
                    return;
                }
                public T Dequeue()
                {
                    lock (queue)
                    {
                        while (queue.Count == 0)
                        {
                            try
                            {
                                Monitor.Wait(queue);
                            }
                            catch(ThreadInterruptedException ex)
                            {
                                ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + ex.StackTrace);
                                goto END;
                            }
                        }
                        T item = queue.Dequeue();
                        if (queue.Count == maxSize - 1)
                        {
                            // wake up any blocked enqueue
                            Monitor.PulseAll(queue);
                        }
                        return item;
                    }
                END:
                    return default(T);
                }
        }

    
}
