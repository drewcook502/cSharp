using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FastQueue
{
    /// <summary>
    /// 
    /// </summary>
    public class FastQueue<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
    {

        #region fields

        private Node<T> head;
        private Node<T> tail;
        private int count;

        #endregion fields

        #region Constructors

        /// <summary>
        /// Default empty constructor
        /// </summary>
        public FastQueue()
        {
            Head = null;
            Tail = null;
            count = 0;
        }

        /// <summary>
        /// Constuctor for the first element
        /// </summary>
        public FastQueue(T head)
        {
            Node<T> node = new Node<T>(head);
            Head = node;
            Tail = node;
            count = 1;
        }

        /// <summary>
        /// Constructor that creates a FastQueue from the given collection
        /// </summary>
        public FastQueue(IEnumerable<T> collection)
        {
            if (!object.ReferenceEquals(collection, null) && collection.Any())
            {
                foreach (T item in collection)
                {
                    Enqueue(item);
                }
            }
            else
            {
                Head = null;
                Tail = null;
                count = 0;
            }
        }

        #endregion Constructors

        #region Properties

        private Node<T> Head
        {
            get
            {
                return head;
            }
            set
            {
                head = value;
            }
        }

        private Node<T> Tail
        {
            get
            {
                return tail;
            }
            set
            {
                tail = value;
            }
        }

        #endregion Properties

        #region Queue Methods

        /// <summary>
        /// Adds the given item to the end of the Queue
        /// </summary>
        public void Enqueue(T item)
        {
            Node<T> newNode = new Node<T>(item);
            if (count == 0)
            {
                head = newNode;
                tail = newNode;
            }
            tail.Next = newNode;
            tail = newNode;
            count++;
        }

        /// <summary>
        /// Removes and returns the first item in the FastQueue. 
        /// </summary>
        public T Dequeue()
        {
            if (count > 0)
            {
                Node<T> oldHead = Head;
                Head = oldHead.Next;
                count--;
                return oldHead.Item;
            }
            else
            {
                throw new InvalidOperationException("FastQueue is empty");
            }
        }

        /// <summary>
        /// Returns the first item in the FastQueue without removing it.
        /// </summary>
        public T Peek()
        {
            if (count > 0)
            {
                return Head.Item;
            }
            else
            {
                throw new InvalidOperationException("FastQueue is empty");
            }
        }

        /// <summary>
        /// Clears the FastQueue
        /// </summary>
        public void Clear()
        {
            Head = null;
            Tail = null;
            count = 0;
        }

        #endregion Queue Methods

        #region IEnumerable<T>

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            Node<T> location = Head;

            for (int index = 0; index < count; index++)
            {
                yield return location.Item;
                location = location.Next;
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion IEnumerable<T>

        #region ICollection

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                return count;
            }
            private set
            {
                count = value;
            }
        }

        /// <inheritdoc/>
        /// <remarks>Is always false</remarks>
        public bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc/>
        /// <remarks>Throws NotImplementedException, as FastQueue isn't thread safe</remarks>
        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException("FastQueue is not Synchronized");
            }
        }

        /// <inheritdoc/>
        public void CopyTo(Array array, int index)
        {
            if (object.ReferenceEquals(array, null))
            {
                throw new ArgumentNullException("array is null");
            }
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index is less than zero");
            }
            if (array.Rank != 1)
            {
                throw new ArgumentException("Array is multidimensional");
            }
            if (count > (array.Length - index))
            {
                throw new ArgumentException("Not enough elements in the array");
            }
            //throw ArgumentException if array does not have zero-based indexing
            //throw ArgumentException if T cannot be cast automatically to type of destination array 

            Node<T> location = Head;
            for (int i = index; i < count; i++)
            {
                array.SetValue(location.Item, i);
                location = location.Next;
            }
        }
        #endregion ICollection
    }
}
