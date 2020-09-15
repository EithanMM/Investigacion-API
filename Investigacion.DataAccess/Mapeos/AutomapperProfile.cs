using AutoMapper;

namespace Investigacion.DataAccess.Mapeos {
    public class AutomapperProfile : Profile {

        public AutomapperProfile() {
            /******************************************************/
            /*     AQUI SE CONFIGURAN LOS MAPEOS ENTRE CLASES     */
            /******************************************************/
            /***************** EJEMPLO CASTEO *********************/
            //CreateMap<TablaOrigen, TablaDestino>();
            //CreateMap<AgregarTrabajoDTO, TAB_TRABAJO>().ForMember(entidad => entidad.LLF_Investigador, dto => dto.MapFrom(src => src.IdInvestigador))
            /******************************************************/

            /**************** MAPEO 2 DIRECCIONES *****************/
            //Genera un mapeo en dos direcciones.
            //CreateMap<TOrigen, TDestino>().ReverseMap();
            /******************************************************/
        }
    }
}
