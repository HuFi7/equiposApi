using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace equiposWebApi.Models
{
    public class estados_equipos
    {
        [Key]
        public int id_estados_equipos { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
    }
}
