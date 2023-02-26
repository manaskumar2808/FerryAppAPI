using System;

namespace FerryAppApi {
    class InvalidUsernameException : ApplicationException {
        public InvalidUsernameException(string message = "Username provided is invalid or empty!") : base(message) {}
    }
}