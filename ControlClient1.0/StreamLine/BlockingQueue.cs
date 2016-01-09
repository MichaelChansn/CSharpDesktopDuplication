using ControlClient1._0.ErrorMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ControlClient1._0.StreamLine
{/**a blocking queue */
    class BlockingQueue<T>
    {
        private readonly Queue<T> queue = null;
        private readonly int maxSize = 10;
        public BlockingQueue(int maxSize) { this.maxSize = maxSize; queue = new Queue<T>(); }
        public int getQueueSize()
        {
            return queue.Count;
        }
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
                        return;
                    }
                }
                queue.Enqueue(item);
                if (queue.Count == 1)
                {
                    // wake up any blocked dequeue
                    Monitor.PulseAll(queue);
                }
            }
        }
        public T Dequeue()
        {
            lock (queue)
            {
                while (queue.Count == 0)
                {
                    try
                    {
                        /**wait for add*/
                        Monitor.Wait(queue);
                    }
                    catch (ThreadInterruptedException ex)
                    {
                        ErrorInfo.getErrorWriter().writeErrorMassageToFile(ex.Message + ex.StackTrace);
                        return default(T);
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
        }
    }
}
