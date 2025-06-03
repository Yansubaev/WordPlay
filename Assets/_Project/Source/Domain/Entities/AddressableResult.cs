namespace Source.Domain.Entities
{
    public class AddressableResult<T>
    {
        public T Result { get; set; }

        public AddressableStatus Status { get; set; }

        public AddressableResult(T result)
        {
            Result = result;
            Status = AddressableStatus.Success;
        }

        public AddressableResult(AddressableStatus status)
        {
            Status = status;
            Result = default;
        }
    }
}