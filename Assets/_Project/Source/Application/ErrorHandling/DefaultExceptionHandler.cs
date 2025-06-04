using Source.Application.ErrorHandling;
using System;
using UnityEngine;

namespace Source.Application.Services
{
    internal class DefaultExceptionHandler : IExceptionHandler
    {
        #region public methods

        public T Handle<T>(Exception ex, T defaultValue = default)
        {
            Debug.LogError($"Exception handled: {ex.Message}. By {GetType().Name}");

            return defaultValue;
        }

        public void Handle(Exception ex)
        {
            Debug.LogError($"Exception handled: {ex.Message}");
        }

        #endregion
    }
}