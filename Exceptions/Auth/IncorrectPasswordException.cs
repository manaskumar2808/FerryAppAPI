using System;

namespace FerryAppApi {
    class IncorrectPasswordException : ApplicationException {
        public IncorrectPasswordException(string message = "Password is incorrect!") : base(message) {}
    }
}