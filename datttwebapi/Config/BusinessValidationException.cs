namespace datttwebapi.Config
{
    public class BusinessValidationException : Exception
    {
        public string Code { get; }
        public BusinessValidationException(string code, string message) : base(message)
        {
            {
                Code = code;
            }
        }
    }
}
