using System;

namespace FerryAppApi {
    class NegativeCostException : ApplicationException {
        public NegativeCostException(string message = "Ticket cost cannot be negative!") : base(message) {}
    }
}