using System;

namespace Source.Application.ErrorHandling
{
    public interface IExceptionHandler
    {
        T Handle<T>(Exception ex, T defaultValue = default);

        void Handle(Exception ex);
    }
}