//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DualTechCruds.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class TasaCambio
    {
        public int Id { get; set; }
        public Nullable<decimal> Tasa { get; set; }
        public Nullable<System.DateTime> FechaInicio { get; set; }
        public Nullable<System.DateTime> FechaFinal { get; set; }
    }
}
