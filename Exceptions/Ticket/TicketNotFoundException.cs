using System;

namespace FerryAppApi {
    class TicketNotFoundException : ApplicationException {
        public TicketNotFoundException(string message = "No matching ticket found!") : base(message) {}
    }
}