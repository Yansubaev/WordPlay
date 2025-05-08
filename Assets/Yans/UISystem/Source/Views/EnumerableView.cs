using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Yans.UI.Views
{
    public class EnumerableView<V> : View, IEnumerable<V> where V : View
    {
        [SerializeField] private Transform _viewsContainer;
        [SerializeField] private V _viewPrefab;

        private IViewInstantiator<V> _viewInstantiator;
        private List<V> _activeViewsCache;
        private ObjectPool<V> _viewPool;

        public V this[int index]
        {
            get
            {
                while (index >= _activeViewsCache.Count)
                {
                    CreateView();
                }
                return _activeViewsCache[index];
            }
        }

        public void ReleaseView(V view)
        {
            if (_activeViewsCache.Remove(view))
            {
                _viewPool.Release(view);
            }
        }

        public IEnumerator<V> GetEnumerator()
        {
            return _activeViewsCache.GetEnumerator();
        }

        public void SetViewInstantiator(IViewInstantiator<V> viewInstantiator)
        {
            _viewInstantiator = viewInstantiator;
        }

        protected override void Awake()
        {
            base.Awake();
            _viewInstantiator = new DefaultViewInstantiator();
            _activeViewsCache = _viewsContainer.GetComponentsInChildren<V>(false).ToList();
            _viewPool = new ObjectPool<V>(CreateNewView, OnGetFromPool, OnReleaseToPool);
        }

        private V CreateView()
        {
            V view = _viewPool.Get();
            _activeViewsCache.Add(view);
            return view;
        }

        private V CreateNewView()
        {
            return _viewInstantiator.InstantiateView(_viewPrefab, _viewsContainer);
        }

        private void OnGetFromPool(V view)
        {
            view.gameObject.SetActive(true);
        }

        private void OnReleaseToPool(V view)
        {
            view.gameObject.SetActive(false);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public interface IViewInstantiator<T>
        {
            T InstantiateView(V prefab, Transform parent);
        }

        private class DefaultViewInstantiator : IViewInstantiator<V>
        {
            public V InstantiateView(V prefab, Transform parent)
            {
                return Instantiate(prefab, parent);
            }
        }
    }
}