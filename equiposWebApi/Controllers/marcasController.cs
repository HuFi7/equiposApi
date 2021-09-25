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
    public class marcasController : ControllerBase
    {
        private readonly prestamosContext _contexto;
        public marcasController(prestamosContext miContexto)
        {
            this._contexto = miContexto;
        }

        [HttpGet]
        [Route("api/marcas")]
        public IActionResult Get()
        {
            IEnumerable<marcas> marcasList = from e in _contexto.marcas
                                             select e;
            if (marcasList.Count() > 0)
            {
                return Ok(marcasList);
            }
            return NotFound();
        }

        [HttpGet]
        [Route("api/marcas/{id}")]
        public IActionResult getbyId(int id)
        {
            marcas unaMarca = (from e in _contexto.marcas
                               where e.id_marcas == id //filtro por ID
                               select e).FirstOrDefault();
            if (unaMarca != null)
            {
                return Ok(unaMarca);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("api/marcas")]
        public IActionResult GuardarMarca([FromBody] marcas marcaNuevo)
        {
            try
            {
                IEnumerable<marcas> marcaExiste = from e in _contexto.marcas
                                                   where e.nombre_marca == marcaNuevo.nombre_marca
                                                   select e;
                if (marcaExiste.Count() == 0)
                {
                    _contexto.marcas.Add(marcaNuevo);
                    _contexto.SaveChanges();
                    return Ok(marcaNuevo);
                }
                return BadRequest(marcaExiste);

            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }


        [HttpPut]
        [Route("api/marcas")]
        public IActionResult updateMarca([FromBody] marcas marcaModificar, int id)
        {
            marcas marcaExiste = (from e in _contexto.marcas
                                    where e.id_marcas == marcaModificar.id_marcas
                                    select e).FirstOrDefault();
            if (marcaExiste is null)
            {
                return NotFound();
            }

            marcaExiste.nombre_marca = marcaModificar.nombre_marca;
            marcaExiste.id_marcas = marcaModificar.id_marcas;
            marcaExiste.estados = marcaModificar.estados;

            _contexto.Entry(marcaExiste).State = EntityState.Modified;
            _contexto.SaveChanges();
            return Ok(marcaExiste);
        }
    }
}