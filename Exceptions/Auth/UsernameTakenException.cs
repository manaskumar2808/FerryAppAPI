using System;

namespace FerryAppApi {
    class UsernameTakenException : ApplicationException {
        public UsernameTakenException(string message = "Username is already taken!") : base(message) {}
    }
}