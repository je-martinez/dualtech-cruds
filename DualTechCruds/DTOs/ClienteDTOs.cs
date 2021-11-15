using DualTechCruds.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DualTechCruds.DTOs
{
    public class ClienteDTO
    {
        public int? Id { get; set; }
        public string Nombre { get; set; }

        public ClienteDTO()
        {
            this.Id = 0;
            this.Nombre = String.Empty;
        }

        public ClienteDTO(Cliente cliente)
        {
            this.Id = cliente.Id;
            this.Nombre = cliente.Nombre;
        }

    }
}