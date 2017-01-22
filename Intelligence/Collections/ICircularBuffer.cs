using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAI.Intelligence.Collections
{
    public interface ICircularBuffer<T>
    {
        /// <summary>
        ///   Number of elements in the buffer.
        /// </summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>
        ///   The maximum capacity of the buffer.
        /// </summary>
        /// <value>The capacity.</value>
        int Capacity { get; set; }

        /// <summary>
        ///   Returns the value at the head of the buffer.
        /// </summary>
        /// <value>The head.</value>
        T Head { get; }

        /// <summary>
        ///   Returns the value at the tail of the buffer.
        /// </summary>
        /// <value>The tail.</value>
        T Tail { get; }

        /// <summary>
        ///   Enqueue the specified item.
        /// </summary>
        T Enqueue(T item);

        /// <summary>
        ///   Dequeue this instance.
        /// </summary>
        T Dequeue();

        /// <summary>
        ///   Clears the buffer.
        /// </summary>
        void Clear();

        /// <summary>
        ///   Indexs the of.
        /// </summary>
        /// <returns>The of.</returns>
        /// <param name="item">Item.</param>
        int IndexOf(T item);

        /// <summary>
        ///   Removes the element at the given index.
        /// </summary>
        /// <param name="index">Index.</param>
        void RemoveAt(int index);

        /// <summary>
        ///   Gets or sets the <see cref="ICircularBuffer{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">Index.</param>
        T this[int index] { get; set; }
    }
}
