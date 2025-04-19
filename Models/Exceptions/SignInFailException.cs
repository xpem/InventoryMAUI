namespace Models.Exceptions
{
    [Serializable]
    public class SignInFailException : Exception
    {
        public SignInFailException() { }

        public SignInFailException(string message) : base(message) { }
    }
}
