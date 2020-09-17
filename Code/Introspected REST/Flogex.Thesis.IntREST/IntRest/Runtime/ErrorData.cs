namespace Flogex.Thesis.IntRest.Runtime
{
    public class ErrorData
    {
        public ErrorData(int errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        public int ErrorCode { get; }

        public string Message { get; }
    }
}
