using System;

namespace FerryAppApi {
    class UnlinkedUserException : ApplicationException {
        public UnlinkedUserException(string message = "User ID must be associated to a ticket!") : base(message) {}
    }
}