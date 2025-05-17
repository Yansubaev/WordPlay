using Yans.UI.Views;

namespace Yans.UI.Adapters
{
    public abstract class ListAdapter<V> where V : View
    {
        #region private fields
        private EnumerableView<V> _view;
        private int _count;
        #endregion

        #region protected properties
        protected EnumerableView<V> View => _view;
        protected int Count => _count;
        #endregion

        #region public methods

        public ListAdapter(EnumerableView<V> view)
        {
            _view = view;
        }

        #endregion

        #region protected methods
        protected void NotifyDatasetChanged(int count)
        {
            _count = count;
            DistributeData();
        }

        protected abstract void OnBindView(V view, int position);
        #endregion

        #region private methods

        private void DistributeData()
        {
            if (_count > 0)
            {
                for (int i = 0; i < _count; i++)
                {
                    OnBindView(_view[i], i);
                }
            }

        }

        #endregion
    }
}