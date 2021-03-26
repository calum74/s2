using System;
using System.Collections.Generic;
using System.Text;

namespace GeneratorLib
{
    public class Heap<T>
    {
        public Heap(IComparer<T> c)
        {
            comparer = c;
            items = new List<T>();
        }

        public bool Valid
        {
            get
            {
                for (int i = 1; i < Count; ++i)
                {
                    if (Less(i, Parent(i)))
                        return false;
                }
                return true;
            }
        }

        private int Parent(int pos) => (pos - 1) >> 1;
        private int Child1(int pos) => 1 + (pos << 1);
        private int Child2(int pos) => Child1(pos) + 1;

        public void Add(T item)
        {
            var pos = items.Count;
            items.Add(item);
            while (pos != 0)
            {
                var parent = Parent(pos);
                if (Less(pos, parent))
                    Swap(pos, parent);
                else
                    return;
                pos = parent;
            }
        }

        public T Pop()
        {
            var result = items[0];
            items[0] = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);

            var pos = 0;
            while (pos < items.Count)
            {
                int largest = pos;
                int left = Child1(pos);
                int right = left + 1;

                if (left < items.Count && Less(left, largest))
                    largest = left;

                if (right < items.Count && Less(right, largest))
                    largest = right;

                if (largest == pos)
                    break;

                Swap(largest, pos);
                pos = largest;
            }

            return result;
        }

        public bool TryPop(out T value)
        {
            if (Any())
            {
                value = Pop();
                return true;
            }
            else
            {
                value = default(T);
                return false;
            }
        }

        public T Peek() => items[0];

        void Swap(int i, int j)
        {
            var x = items[i];
            items[i] = items[j];
            items[j] = x;
        }

        bool Less(int i, int j) => comparer.Compare(items[i], items[j]) < 0;

        public bool Any() => items.Count > 0;

        public int Count { get => items.Count; }

        readonly IComparer<T> comparer;
        readonly List<T> items;
    }

}
