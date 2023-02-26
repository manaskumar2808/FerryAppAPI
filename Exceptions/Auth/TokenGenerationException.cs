using System;

namespace FerryAppApi {
    class TokenGenerationException : ApplicationException {
        public TokenGenerationException(string message = "Token cannot be generated at the moment!") : base(message) {}
    }
}