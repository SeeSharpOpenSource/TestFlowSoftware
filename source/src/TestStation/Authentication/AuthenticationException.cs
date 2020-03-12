using System;

namespace TestStation.Authentication
{
    public class AuthenticationException : ApplicationException
    {
        internal AuthenticationException(string message) : base(message)
        {
            
        }
    }
}