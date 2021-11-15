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

    public class TasaCambioDTOWithValidation
    {
        public int Id { get; set; }
        public decimal? Tasa { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFinal { get; set; }
        public bool hasErrors { get; set; }
        public string validationErrors { get; set; }


        public TasaCambioDTOWithValidation()
        {
            this.Id = 0;
            this.Tasa = 0;
            this.FechaInicio = null;
            this.FechaFinal = null;
            this.hasErrors = false;
            this.validationErrors = String.Empty;
        }

        public TasaCambioDTOWithValidation(TasaCambio tasa, bool hasErrors, string validationErrors)
        {
            this.Id = tasa.Id;
            this.Tasa = tasa.Tasa;
            this.FechaInicio = tasa.FechaInicio;
            this.FechaFinal = tasa.FechaFinal;
            this.hasErrors = hasErrors;
            this.validationErrors = validationErrors;
        }

        public TasaCambioDTOWithValidation(TasaCambioDTO tasa, bool hasErrors, string validationErrors)
        {
            this.Id = tasa.Id;
            this.Tasa = tasa.Tasa;
            this.FechaInicio = tasa.FechaInicio;
            this.FechaFinal = tasa.FechaFinal;
            this.hasErrors = hasErrors;
            this.validationErrors = validationErrors;
        }

    }

}