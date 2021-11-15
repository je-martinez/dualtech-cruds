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
    [RoutePrefix("api/poliza")]
    public class PolizaController : ApiController
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
                    List<Poliza> polizas = await context.Poliza.Where(item => item.Id >= from).Take((int)limit).ToListAsync();
                    List<PolizaDTO> clientesToReturn = polizas.Select(item => new PolizaDTO(item)).ToList();

                    ResponseResult response = new ResponseResult()
                    {
                        success = true,
                        errorMsg = null,
                        data = clientesToReturn
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    ResponseResult response = new ResponseResult()
                    {
                        success = false,
                        errorMsg = "Ha ocurrido un error tratando de traer la lista de polizas",
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
                    Poliza findById = await context.Poliza.Where(item => item.Id == id).FirstOrDefaultAsync();
                    ResponseResult response = new ResponseResult()
                    {
                        success = true,
                        errorMsg = null,
                        data = new PolizaDTO(findById)
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    ResponseResult response = new ResponseResult()
                    {
                        success = false,
                        errorMsg = "Ha ocurrido un error tratando de encontrar la poliza",
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
                        Poliza recordToDelete = await context.Poliza.Where(item => item.Id == id).FirstOrDefaultAsync();
                        context.Poliza.Remove(recordToDelete);
                        await context.SaveChangesAsync();
                        transaction.Commit();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new PolizaDTO(recordToDelete)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se pudo eliminar la poliza",
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
        public async Task<IHttpActionResult> Create([FromBody] PolizaDTO poliza)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Poliza newRecord = new Poliza()
                        {
                            ClienteId = poliza.ClienteId,
                            Moneda = poliza.Moneda,
                            SumaAsegurada = poliza.SumaAsegurada
                        };
                        context.Poliza.Add(newRecord);
                        transaction.Commit();
                        await context.SaveChangesAsync();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new PolizaDTO(newRecord)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = null,
                            data = poliza
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
        public async Task<IHttpActionResult> Edit([FromBody] PolizaDTO poliza)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Poliza recordToEdit = await context.Poliza.Where(item => item.Id == (int)poliza.Id).FirstOrDefaultAsync();
                        recordToEdit.ClienteId = poliza.ClienteId;
                        recordToEdit.Moneda = poliza.Moneda;
                        recordToEdit.SumaAsegurada = poliza.SumaAsegurada;
                        transaction.Commit();
                        await context.SaveChangesAsync();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new PolizaDTO(recordToEdit)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se ha podido editar la poliza",
                            data = poliza
                        };
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
        }
    }
}
