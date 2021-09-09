using System;

namespace CaloriesSmartCalulator.Handlers.Contracts.Results
{
    public abstract class OperationResultBase<T>
    {
        public bool Success { get; }
        public Exception Exception { get; }
        public string FailureMessage { get; }
        public T Value { get; set; }

        protected OperationResultBase(T value)
        {
            Success = true;
            Value = value;
        }
        protected OperationResultBase(string message)
        {
            Success = false;
            FailureMessage = message;
        }
        protected OperationResultBase(Exception ex)
        {
            Success = false;
            Exception = ex;
            FailureMessage = ex.Message;
        }
    }
}
