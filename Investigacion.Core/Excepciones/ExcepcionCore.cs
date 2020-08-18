using System;

namespace Investigacion.Core.Excepciones {
    class ExcepcionCore : Exception {

        public ExcepcionCore() { }

        public ExcepcionCore(string Mensaje) : base(Mensaje) { }
    }
}
