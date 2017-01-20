using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Collections
{
    public class ThreadSafePriorityQueue<TItem, TPriority> : IPriorityQueue<TItem, TPriority>
       where TPriority : IComparable<TPriority>
    {
        readonly PriorityQueue<TItem, TPriority> _queue;

        readonly ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim();

        /// <summary>
        ///   Returns true if there is an element at the head of the queue, i.e. if the queue is not
        ///   empty.
        /// </summary>
        public bool HasNext
        {
            get
            {
                using (_rwlock.Read())
                    return _queue.HasNext;
            }
        }

        /// <summary>
        ///   Returns the number of items in the queue.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get
            {
                using (_rwlock.Write())
                    return _queue.Count;
            }
        }

        /// <summary>
        ///   Determines whether the binary heap of the internal binary heap queue is valid.
        /// </summary>
        /// <returns><c>true</c> if this instance is binary heap valid; otherwise, <c>false</c>.</returns>
        public bool IsBinaryHeapValid()
        {
            using (_rwlock.Read())
                return _queue.IsBinaryHeapValid();
        }

        /// <summary>
        ///   Returns the item at the head of the queue without removing it.
        /// </summary>
        public TItem Peek()
        {
            using (_rwlock.Write())
                return _queue.Count <= 0 ? default(TItem) : _queue.Peek();
        }

        /// <summary>
        ///   Enqueues an item to the list. Items with lower priority values are placed ahead of the
        ///   queue.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="priority">Priority.</param>
        public void Enqueue(TItem item, TPriority priority)
        {
            using (_rwlock.Write())
                _queue.Enqueue(item, priority);
        }

        /// <summary>
        ///   Removes and returns the item at the head of the queue. In the event of a priority tie the item
        ///   inserted first in the queue is returned.
        /// </summary>
        public TItem Dequeue()
        {
            using (_rwlock.Write())
                return _queue.Count <= 0 ? default(TItem) : _queue.Dequeue();
        }

        /// <summary>
        ///   Returns true if the queue has 1 or more of the secified items.
        /// </summary>
        /// <param name="item">Item.</param>
        public bool Contains(TItem item)
        {
            using (_rwlock.Read())
                return _queue.Contains(item);
        }

        /// <summary>
        ///   RemoveBehaviour the specified item. Note that the queue may contain multiples of the same item, in
        ///   which case this removes the one that is closest to the head.
        /// </summary>
        public TItem Remove(TItem item)
        {
            using (_rwlock.Write())
                return _queue.Remove(item);
        }

        /// <summary>
        ///   RemoveBehaviour the specified item. Note that the queue may contain multiples of the same item, in
        ///   which case this removes the one that is closest to the head.
        /// </summary>
        public TItem Remove(Func<TItem, bool> predicate)
        {
            using (_rwlock.Write())
                return _queue.Remove(predicate);
        }

        /// <summary>
        ///   Updates the priority of the specified item. If the item does not exist in the queue, it simply
        ///   returns.
        /// </summary>
        /// <param name="item">Item.</param>
        /// <param name="priority">Priority.</param>
        public void UpdatePriority(TItem item, TPriority priority)
        {
            using (_rwlock.Write())
                _queue.UpdatePriority(item, priority);
        }

        /// <summary>
        ///   Removes every node from the queue.
        ///   O(n)
        /// </summary>
        public void Clear()
        {
            using (_rwlock.Write())
                _queue.Clear();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Crystal.ThreadSafePriorityQueue`2"/>
        ///   class.
        /// </summary>
        public ThreadSafePriorityQueue()
        {
            _queue = new PriorityQueue<TItem, TPriority>(DefaultSize);
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="Crystal.ThreadSafePriorityQueue`2"/>
        ///   class.
        /// </summary>
        /// <param name="size">Size.</param>
        public ThreadSafePriorityQueue(int size)
        {
            _queue = new PriorityQueue<TItem, TPriority>(size);
        }

        const int DefaultSize = 128;
    }
}
