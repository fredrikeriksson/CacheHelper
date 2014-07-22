using System;

namespace CacheHelper.Core.Exceptions
{
    public class ExpressionBodyNotSupported : Exception
    {
        public ExpressionBodyNotSupported() : base("Provided expression body is not supported, only references to methods allowed.")
        {

        }
    }
}