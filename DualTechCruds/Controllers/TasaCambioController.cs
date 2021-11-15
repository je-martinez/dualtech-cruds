using DualTechCruds.DB;
using DualTechCruds.DTOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace DualTechCruds.Controllers
{
    [RoutePrefix("api/tasa-cambio")]
    public class TasaCambioController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("get-all")]
        public async Task<IHttpActionResult> GetAll(int? limit, int? from)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                try
                {
                    List<TasaCambio> tasas = await context.TasaCambio.Where(item => item.Id >= from).Take((int)limit).ToListAsync();
                    List<TasaCambioDTO> tasasToReturn = tasas.Select(item => new TasaCambioDTO(item)).ToList();

                    ResponseResult response = new ResponseResult()
                    {
                        success = true,
                        errorMsg = null,
                        data = tasasToReturn
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    ResponseResult response = new ResponseResult()
                    {
                        success = false,
                        errorMsg = "Ha ocurrido un error tratando de traer la lista de tasas de cambio",
                        data = new
                        {
                            limit = limit,
                            from = from
                        }
                    };
                    return Content(HttpStatusCode.BadRequest, response); ;
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("get-by-id/{id}")]
        public async Task<IHttpActionResult> GetById(int id)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                try
                {
                    TasaCambio findById = await context.TasaCambio.Where(item => item.Id == id).FirstOrDefaultAsync();
                    ResponseResult response = new ResponseResult()
                    {
                        success = true,
                        errorMsg = null,
                        data = new TasaCambioDTO(findById)
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    ResponseResult response = new ResponseResult()
                    {
                        success = false,
                        errorMsg = "Ha ocurrido un error tratando de encontrar la tasa de cambio",
                        data = id
                    };
                    return Content(HttpStatusCode.BadRequest, response);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("delete/{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        TasaCambio recordToDelete = await context.TasaCambio.Where(item => item.Id == id).FirstOrDefaultAsync();
                        context.TasaCambio.Remove(recordToDelete);
                        await context.SaveChangesAsync();
                        transaction.Commit();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new TasaCambioDTO(recordToDelete)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se pudo eliminar la tasa de cambio",
                            data = id
                        };
                        transaction.Rollback();
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("create")]
        public async Task<IHttpActionResult> Create([FromBody] TasaCambio cliente)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        TasaCambio newRecord = new TasaCambio()
                        {
                            Nombre = cliente.Nombre
                        };
                        context.Cliente.Add(newRecord);
                        transaction.Commit();
                        await context.SaveChangesAsync();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new ClienteDTO(newRecord)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = null,
                            data = cliente
                        };
                        transaction.Rollback();
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
        }


        [HttpPut]
        [AllowAnonymous]
        [Route("edit")]
        public async Task<IHttpActionResult> Edit([FromBody] ClienteDTO cliente)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Cliente recordToEdit = await context.Cliente.Where(item => item.Id == (int)cliente.Id).FirstOrDefaultAsync();
                        recordToEdit.Nombre = cliente.Nombre;
                        transaction.Commit();
                        await context.SaveChangesAsync();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new ClienteDTO(recordToEdit)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se ha podido editar el cliente",
                            data = cliente
                        };
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
        }
    }
}
