using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sapr_sim
{
    public class FixedSizedStack<T>
    {
        private LinkedList<T> stack = new LinkedList<T>();
        private int maxSize = 10;

        public FixedSizedStack(int size)
        {
            this.maxSize = size;
        }

        public FixedSizedStack() { }


        public void Push(T t)
        {
            if (stack.Count == maxSize)
            {
                stack.RemoveFirst();
            }

            stack.AddLast(t);
        }

        public bool HasElements()
        {
            return stack.Count != 0 ? true : false;
        }

        public T Pop()
        {
            if (maxSize == 0 || stack.Count == 0) 
            {
                return default(T);
            }
            else { 
                T last = stack.Last();
                stack.RemoveLast();
                return last;
            }
        }

        public void Clear()
        {
            stack.Clear();
        }
    }
}
