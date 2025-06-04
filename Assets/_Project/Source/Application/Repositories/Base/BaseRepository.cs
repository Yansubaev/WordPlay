using Cysharp.Threading.Tasks;
using Source.Application.ErrorHandling;
using Source.Application.Services;
using System;
using UnityEngine;

namespace Source.Application.Repositories
{
    public abstract class BaseRepository
    {
        private readonly IExceptionHandler _errorHandler;

        protected BaseRepository(IExceptionHandler exceptionHandler = null)
        {
            _errorHandler = exceptionHandler ?? new DefaultExceptionHandler();
        }

        protected async UniTask<T> Execute<T>(Func<UniTask<T>> apiCall, T defaultValue = default)
        {
            try
            {
                var result = await apiCall();
                return result;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in repository action: {ex.Message}");

                return _errorHandler.Handle(ex, defaultValue);
            }
        }
    }
}