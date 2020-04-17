using System;

namespace TestFlow.DevSoftware.Authentication
{
    public class AuthenticationException : ApplicationException
    {
        internal AuthenticationException(string message) : base(message)
        {
            
        }
    }
}