using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Collections
{
    public interface IPriorityQueue<TItem, in TPriority> where TPriority : IComparable<TPriority>
    {
        /// <summary>
        ///   Returns true if there is an element at the head of the queue, i.e. if the queue is not
        ///   empty.
        /// </summary>
        bool HasNext { get; }

        /// <summary>
        ///   Returns the number of items in the queue.
        /// </summary>
        int Count { get; }

        /// <summary>
        ///   Returns the item at the head of the queue without removing it.
        /// </summary>
        TItem Peek();

        /// <summary>
        ///   Enqueues an item to the list. Items with lower priority values are placed ahead of the
        ///   queue.
        /// </summary>
        void Enqueue(TItem item, TPriority priority);

        /// <summary>
        ///   Removes and returns the item at the head of the queue. In the event of a priority tie the item
        ///   inserted first in the queue is returned.
        /// </summary>
        TItem Dequeue();

        /// <summary>
        ///   Returns true if the queue has 1 or more of the secified items.
        /// </summary>
        bool Contains(TItem item);

        /// <summary>
        ///   RemoveBehaviour the specified item. Note that the queue may contain multiples of the same item, in
        ///   which case this removes the one that is closest to the head.
        /// </summary>
        /// <param name="item">Item.</param>
        TItem Remove(TItem item);

        /// <summary>
        ///   Removes the first item that matches the specified predicate. Note that the queue may contain
        ///   multiples of the same item, in which case this removes the one that is closest to the head.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The item that was removed or null if no item was not found.</returns>
        TItem Remove(Func<TItem, bool> predicate);

        /// <summary>
        ///   Updates the priority of the specified item. If the item does not exist in the queue, it simply
        ///   returns.
        /// </summary>
        void UpdatePriority(TItem item, TPriority priority);

        /// <summary>
        ///   Removes every node from the queue.
        /// </summary>
        void Clear();
    }
}
