using System;

namespace FerryAppApi {
    class UnauthorizedUserException : ApplicationException {
        public UnauthorizedUserException(string message = "The user is unauthorized for the action!") : base(message) {}
    }
}