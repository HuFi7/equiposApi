using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using equiposWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace equiposWebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class tipo_equiposController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public tipo_equiposController(prestamosContext miContexto)
        {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/tipo_equipos")]
        public IActionResult Get()
        {
            IEnumerable<tipo_equipos> tipos_equiposListList = from e in _contexto.tipo_Equipos
                                                              select e;
            if (tipos_equiposListList.Count() > 0)
            {
                return Ok(tipos_equiposListList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/tipo_equipos/{id}")]
        public IActionResult getbyId(int id)
        {
            tipo_equipos unTipo_Equipo = (from e in _contexto.tipo_Equipos
                                          where e.id_tipo_equipo == id //filtro por ID
                                          select e).FirstOrDefault();
            if (unTipo_Equipo != null)
            {
                return Ok(unTipo_Equipo);
            }

            return NotFound();
        }
        [HttpPost]
        [Route("api/tipo_equipos")]
        public IActionResult guardarTipoEquipo([FromBody] tipo_equipos tipoEquipoNuevo)
        {
            try
            {
                IEnumerable<tipo_equipos> tipoEquipoExiste = from e in _contexto.tipo_Equipos
                                                             where e.descripcion == tipoEquipoNuevo.descripcion
                                                             select e;
                if (tipoEquipoExiste.Count() == 0)
                {
                    _contexto.tipo_Equipos.Add(tipoEquipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(tipoEquipoNuevo);
                }
                return BadRequest(tipoEquipoExiste);

            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/tipo_equipos")]
        public IActionResult updateTipoEquipo([FromBody] tipo_equipos tipoEquipoModificar, int id)
        {
            tipo_equipos tipoEquipoExiste = (from e in _contexto.tipo_Equipos
                                             where e.id_tipo_equipo == tipoEquipoModificar.id_tipo_equipo
                                             select e).FirstOrDefault();
            if (tipoEquipoExiste is null)
            {
                return NotFound();
            }


            tipoEquipoExiste.descripcion = tipoEquipoModificar.descripcion;

            _contexto.Entry(tipoEquipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(tipoEquipoExiste);
        }
    }
}
