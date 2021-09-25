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
    public class equiposController : ControllerBase
    {
        private readonly prestamosContext _contexto;

        public equiposController(prestamosContext miContexto)
        {
            this._contexto = miContexto;
        }

        /// <summary>
        /// Metodo para retornar los reg. de la tabla EQUIPOS
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipos")]
        public IActionResult Get()
        {
            IEnumerable<equipos> equiposList = from e in _contexto.equipos
                                               select e;
            if (equiposList.Count() > 0)
            {
                return Ok(equiposList);
            }
            return NotFound();
        }


        /// <summary>
        /// Metodo para retomar un reg. de la tabla Equipos filtrado por ID
        /// </summary>
        /// <param name="id">Valor Entero del campo id_equipos</param>
        /// <returns></returns>

        [HttpGet]
        [Route("api/equipos/{id}")]
        public IActionResult getbyId(int id)
        {
            equipos unEquipo = (from e in _contexto.equipos
                                where e.id_equipos == id //filtro por ID
                                select e).FirstOrDefault();
            if (unEquipo != null)
            {
                return Ok(unEquipo);
            }

            return NotFound();
        }

        /// <summary>
        /// Metodo para retornar los reg. de la tabla equipos que contenga el valor dado en el parametro.
        /// </summary>
        /// <param name="buscarNombre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/equipo/buscarnombre/{buscarNombre}")]
        public IActionResult obtenerNombre(string buscarNombre)
        {
            IEnumerable<equipos> equiposporNombre = from e in _contexto.equipos
                                                    where e.nombre.Contains(buscarNombre)
                                                    select e;
            if (equiposporNombre.Count() > 0)
            {
                return Ok(equiposporNombre);
            }

            return NotFound();
        }


        [HttpPost]
        [Route("api/equipos")]
        public IActionResult guardarEquipo([FromBody] equipos equipoNuevo)
        {
            try
            {
                IEnumerable<equipos> equipoExiste = from e in _contexto.equipos
                                                    where e.nombre == equipoNuevo.nombre
                                                    select e;
                if (equipoExiste.Count() == 0)
                {
                    _contexto.equipos.Add(equipoNuevo);
                    _contexto.SaveChanges();
                    return Ok(equipoNuevo);
                }
                return BadRequest(equipoExiste);

            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/equipos")]
        public IActionResult updateEquipo([FromBody] equipos equipoModificar, int id)
        {
            equipos equipoExiste = (from e in _contexto.equipos
                                    where e.id_equipos == equipoModificar.id_equipos
                                    select e).FirstOrDefault();
            if (equipoExiste is null)
            {
                return NotFound();
            }

            equipoExiste.nombre = equipoModificar.nombre;
            equipoExiste.descripcion = equipoModificar.descripcion;
            equipoExiste.modelo = equipoModificar.modelo;

            _contexto.Entry(equipoExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(equipoExiste);
        }
    }
}
