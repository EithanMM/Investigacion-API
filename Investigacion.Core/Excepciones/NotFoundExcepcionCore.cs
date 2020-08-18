using System;

namespace Investigacion.Core.Excepciones {
    class NotFoundExcepcionCore : Exception {

        public NotFoundExcepcionCore() { }

        public NotFoundExcepcionCore(string Mensaje) : base(Mensaje) { }
    }
}
