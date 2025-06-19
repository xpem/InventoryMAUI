namespace Services.Handlers.Exceptions
{
    [Serializable]
    public class ServerOffException : Exception
    {
        public ServerOffException() { }

        public ServerOffException(string message) : base(message) { }
    }
}
