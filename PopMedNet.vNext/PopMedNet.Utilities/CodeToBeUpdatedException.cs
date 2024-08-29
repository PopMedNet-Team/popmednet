using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopMedNet.Utilities
{
    /// <summary>
    /// This exception type represents areas of code that need to be updated.
    /// </summary>
    [Serializable]
    public sealed class CodeToBeUpdatedException : Exception
    {

        public CodeToBeUpdatedException()
            : base("This code needs to be updated!")
        {
        }

        public CodeToBeUpdatedException(string message)
            : base(message)
        {
        }

        public CodeToBeUpdatedException(Exception innerException) 
            : base("This code needs to be updated!", innerException) 
        { 
        }

        public CodeToBeUpdatedException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        public CodeToBeUpdatedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context )
            : base(info, context)
        {
        }

    }
}
