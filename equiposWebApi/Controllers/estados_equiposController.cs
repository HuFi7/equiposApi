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
    public class estados_equiposController : ControllerBase
    {

        private readonly prestamosContext _contexto;
        public estados_equiposController(prestamosContext miContexto)
        {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/estado_equipo")]
        public IActionResult Get()
        {
            IEnumerable<estados_equipos> estados_EquiposList = from e in _contexto.estados_equipos
                                                               select e;
            if (estados_EquiposList.Count() > 0)
            {
                return Ok(estados_EquiposList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/estado_equipo/{id}")]
        public IActionResult getbyId(int id)
        {
            estados_equipos unEstado_Equipo = (from e in _contexto.estados_equipos
                                               where e.id_estados_equipos == id //filtro por ID
                                               select e).FirstOrDefault();
            if (unEstado_Equipo != null)
            {
                return Ok(unEstado_Equipo);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/estado_equipo")]
        public IActionResult guardarEstadoEquipo([FromBody] estados_equipos estadoEquipoNuevo)
        {
            try
            {
                IEnumerable<estados_equipos> estadoEquipoExiste = from e in _contexto.estados_equipos
                                                                  where e.descripcion == estadoEquipoNuevo.descripcion
                                                                  select e;
                if (estadoEquipoExiste.Count() == 0)
                {
                    _contexto.estados_equipos.Add(estadoEquipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(estadoEquipoNuevo);
                }
                return BadRequest(estadoEquipoExiste);

            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/estado_equipo")]
        public IActionResult updateEstadoEquipo([FromBody] estados_equipos estadoEquipoModificar, int id)
        {
            estados_equipos estadoEquipoExiste = (from e in _contexto.estados_equipos
                                                  where e.id_estados_equipos == estadoEquipoModificar.id_estados_equipos
                                                  select e).FirstOrDefault();
            if (estadoEquipoExiste is null)
            {
                return NotFound();
            }

            estadoEquipoExiste.descripcion = estadoEquipoModificar.descripcion;

            _contexto.Entry(estadoEquipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(estadoEquipoExiste);
        }
    }
}