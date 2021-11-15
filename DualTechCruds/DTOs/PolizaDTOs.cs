using DualTechCruds.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DualTechCruds.DTOs
{
    public class PolizaDTO
    {
        public int Id { get; set; }
        public int? ClienteId { get; set; }
        public string Moneda { get; set; }
        public decimal? SumaAsegurada { get; set; }

        public PolizaDTO()
        {
            this.Id = 0;
            this.ClienteId = 0;
            this.Moneda = String.Empty;
            this.SumaAsegurada = 0;
        }

        public PolizaDTO(Poliza poliza)
        {
            this.Id = poliza.Id;
            this.ClienteId = poliza.ClienteId;
            this.Moneda = poliza.Moneda;
            this.SumaAsegurada = poliza.SumaAsegurada;
        }

    }
}