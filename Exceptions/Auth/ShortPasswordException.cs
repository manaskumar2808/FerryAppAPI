using System;

namespace FerryAppApi {
    class ShortPasswordException : ApplicationException {
        public ShortPasswordException(string message = "Password length should be atleast 7!") : base(message) {}
    }
}