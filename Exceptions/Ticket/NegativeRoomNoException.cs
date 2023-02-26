using System;

namespace FerryAppApi {
    class NegativeRoomNoException : ApplicationException {
        public NegativeRoomNoException(string message = "Room No. cannot be negative!") : base(message) {}
    }
}