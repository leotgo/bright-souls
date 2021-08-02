using System;
using UnityEngine;

namespace Patterns.ObjectPool
{
    class ObjectPool<T> where T : UnityEngine.Object
    {
        private T prefab;
        private bool initialized = false;
        private int size;
        private T[] pool;
        private int iterator = 0;

        public ObjectPool(T prefab, int size)
        {
            this.prefab = prefab;
            this.size = size;
            this.initialized = false;
            Initialize();
        }

        private void Initialize()
        {
            if (!initialized)
            {
                pool = new T[size];
                for (int i = 0; i < size; i++)
                {
                    pool[i] = UnityEngine.Object.Instantiate<T>(prefab);
                }

                iterator = 0;
                initialized = true;
            }
        }

        public T Fetch()
        {
            return pool[(iterator < pool.Length) ? iterator++ : iterator = 0];
        }

        public T[] FetchAll()
        {
            return pool;
        }

        public void ExecAll(Action<T> a)
        {
            foreach (var clone in pool)
                a(clone);
        }
    }
}
