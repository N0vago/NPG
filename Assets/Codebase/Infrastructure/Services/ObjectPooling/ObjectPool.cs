using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Codebase.Infrastructure.Services.ObjectPooling
{
    public class ObjectPool<T> : IPool<T> 
        where T : MonoBehaviour, IPoolable<T>
    {
        private Queue<T> _pool = new();
        private T _prefab;
        private Transform _parent;

        private event Action<T> OnPushAction;
        private event Action<T> OnPullAction;
        private event Action<T> OnObjectCreated;
        
        public int PooledObjectsCount => _pool.Count;

        public ObjectPool(T prefab, Transform parent, int initialSize ,Action<T> onObjectCreated = null)
        {
            _prefab = prefab;
            _parent = parent;
            OnObjectCreated += onObjectCreated;
            SpawnObjects(initialSize);
            
        }

        public ObjectPool(T prefab, Transform parent,Action<T> onPushAction,
            Action<T> onPullAction, int initialSize, Action<T> onObjectCreated = null)
        {
            _prefab = prefab;
            _parent = parent;
            OnPushAction += onPushAction;
            OnPullAction += onPullAction;
            OnObjectCreated += onObjectCreated;

            SpawnObjects(initialSize);
        }

        public T Pull()
        {
            T obj;
            if (_pool.Count > 0)
            {
                obj = _pool.Dequeue();
            }
            else
            {
                obj = Object.Instantiate(_prefab);
            }
            
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(null, false);
            obj.Initialize(Push);

            OnPullAction?.Invoke(obj);

            return obj;
        }

        public T Pull(Vector3 position)
        {
            T obj = Pull();
            obj.transform.position = position;
            return obj;
        }

        public T Pull(Vector3 position, Quaternion rotation)
        {
            T obj = Pull();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }

        public void Push(T obj)
        {
            obj.gameObject.SetActive(false);

            obj.transform.position = Vector3.zero;
            obj.transform.rotation = new Quaternion(0, 0, 0, 0);
            
            obj.transform.SetParent(_parent, false);
            
            _pool.Enqueue(obj);
            
            OnPushAction?.Invoke(obj);
        }

        private void SpawnObjects(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
            {
                T obj = Object.Instantiate(_prefab, _parent);
                OnObjectCreated?.Invoke(obj);
                obj.gameObject.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
    }
}