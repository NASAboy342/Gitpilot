
namespace Gitpilot.Queues
{
    public class QueueBase<T>
    {
        private readonly Queue<T> _queue;
        public QueueBase()
        {
            _queue = new Queue<T>();
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
        }
        public T Dequeue()
        {
            return _queue.Dequeue();
        }
        public bool IsEmpty()
        {
            return _queue.Count == 0;
        }
        public int Count()
        {
            return _queue.Count;
        }
        public void Clear()
        {
            _queue.Clear();
        }
        public T Peek()
        {
            return _queue.Peek();
        }
    }
}
