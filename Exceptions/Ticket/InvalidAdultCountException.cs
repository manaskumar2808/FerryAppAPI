using System;

namespace FerryAppApi {
    class InvalidAdultCountException : ApplicationException {
        public InvalidAdultCountException(string message = "Adult count should always be between 1 and 10 (inclusive)!") : base(message) {}
    }
}