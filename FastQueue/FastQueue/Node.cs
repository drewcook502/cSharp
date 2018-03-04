namespace FastQueue
{
    internal class Node<T>
    {
        #region fields

        private T item;
        private Node<T> next;

        #endregion fields

        #region Constructors

        public Node(T item)
        {
            this.item = item;
        }

        #endregion Constructors

        #region Properties

        public T Item
        {
            get
            {
                return item;
            }
            private set
            {
                item = value;
            }
        }

        public Node<T> Next
        {
            get
            {
                return next;
            }
            set
            {
                next = value;
            }
        }

        #endregion Properties

        #region Methods

        #endregion Methods
    }
}
