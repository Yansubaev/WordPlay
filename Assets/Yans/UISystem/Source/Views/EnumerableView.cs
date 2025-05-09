using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

namespace Yans.UI.Views
{
    public class EnumerableView<V> : View, IEnumerable<V> where V : View
    {
        #region private fields

        [SerializeField]
        private Transform _viewsContainer;

        [SerializeField]
        private V _viewPrefab;

        private IViewInstantiator<V> _viewInstantiator;
        private List<V> _activeViewsCache;
        private ObjectPool<V> _viewPool;
        #endregion

        public V this[int index]
        {
            get
            {
                if (_activeViewsCache == null) Awake();
                while (index >= _activeViewsCache.Count)
                {
                    CreateView();
                }
                V view = _activeViewsCache[index];
                view.transform.SetSiblingIndex(index);
                return view;
            }
        }

        #region public methods

        public void ReleaseView(V view)
        {
            if (_viewPool == null) Awake();
            if (_activeViewsCache.Remove(view))
            {
                _viewPool.Release(view);
            }
        }

        public void ReleaseAllViews()
        {
            if (_activeViewsCache == null) Awake();
            for (int i = _activeViewsCache.Count - 1; i >= 0; i--)
            {
                ReleaseView(_activeViewsCache[i]);
            }
        }

        public void SetViewInstantiator(IViewInstantiator<V> viewInstantiator)
        {
            _viewInstantiator = viewInstantiator;
        }

        #endregion

        #region protected methods

        protected override void Awake()
        {
            base.Awake();

            _viewInstantiator = new DefaultViewInstantiator();
            _activeViewsCache = _viewsContainer.GetComponentsInChildren<V>(false).ToList();
            _viewPool = new ObjectPool<V>(CreateNewView, OnGetFromPool, OnReleaseToPool);
        }

        #endregion

        #region private methods

        private V CreateView()
        {
            if (_viewPool == null) Awake();
            V view = _viewPool.Get();
            _activeViewsCache.Add(view);
            view.transform.SetSiblingIndex(_activeViewsCache.Count - 1);
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

        #endregion

        public interface IViewInstantiator<T>
        {
            T InstantiateView(V prefab, Transform parent);
        }

        private class DefaultViewInstantiator : IViewInstantiator<V>
        {
            #region public methods

            public V InstantiateView(V prefab, Transform parent)
            {
                return Instantiate(prefab, parent);
            }

            #endregion
        }

        #region interfaces

        public IEnumerator<V> GetEnumerator()
        {
            return _activeViewsCache.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}