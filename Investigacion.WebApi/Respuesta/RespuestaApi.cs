using Investigacion.Model.CustomEntities;

namespace Investigacion.WebApi {
    /* Emplea clases genericas T, permitiendo que 
       la clase que reciba, de ese tipo sera el resultado */
    public class RespuestaApi<T> {

        public RespuestaApi(T Data) {
            this.Data = Data;
        }

        public T Data { get; set; }

        public Metadata Meta { get; set; }
    }
}
