using System;

namespace XGraphics.DataModelGenerator
{
    public class UserViewableException : Exception
    {
        public UserViewableException(string message) : base(message)
        {
        }
    }
}