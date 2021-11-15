using DualTechCruds.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DualTechCruds.DTOs
{
    public class TasaCambioDTO
    {
        public  int Id { get; set; }
        public decimal? Tasa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }

        public TasaCambioDTO()
        {
            this.Id = 0;
            this.Tasa = 0;
            this.FechaInicio = null;
            this.FechaFinal = null;
        }

        public TasaCambioDTO(TasaCambio tasa)
        {
            this.Id = tasa.Id;
            this.Tasa = tasa.Tasa;
            this.FechaInicio = tasa.FechaInicio;
            this.FechaFinal = tasa.FechaFinal;
        }

    }

}