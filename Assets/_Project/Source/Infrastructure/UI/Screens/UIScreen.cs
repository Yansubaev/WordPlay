using UnityEngine;

namespace Source.Infrastructure.Services
{
    public abstract class UIScreen : MonoBehaviour
    {
        public virtual void Show() { }
        public virtual void Hide() { }
    }
}
