using System;

namespace FerryAppApi {
    class InsufficientBalanceException : ApplicationException {
        public InsufficientBalanceException(string message = "Not enough amount in the wallet!") : base(message) {}
    }
}