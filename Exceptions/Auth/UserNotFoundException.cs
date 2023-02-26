using System;

namespace FerryAppApi {
    class UserNotFoundException : ApplicationException {
        public UserNotFoundException(string message = "User not found for the given credentials!") : base(message) {}
    }
}