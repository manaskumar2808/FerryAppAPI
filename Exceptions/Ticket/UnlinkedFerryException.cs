using System;

namespace FerryAppApi {
    class UnlinkedFerryException : ApplicationException {
        public UnlinkedFerryException(string message = "Ferry ID must be associated with a ticket!") : base(message) {}
    }
}