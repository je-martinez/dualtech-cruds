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
    [RoutePrefix("api/cliente")]
    public class ClienteController : ApiController
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
                    List<Cliente> clientes = await context.Cliente.Where(item => item.Id >= from).Take((int)limit).ToListAsync();
                    List<ClienteDTO> clientesToReturn = clientes.Select(item => new ClienteDTO(item)).ToList();

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
                        errorMsg = "Ha ocurrido un error tratando de traer la lista de clientes",
                        data = new
                        {
                            limit = limit,
                            from = from
                        }
                    };
                    return Content(HttpStatusCode.BadRequest, response);;
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
                    Cliente findById = await context.Cliente.Where(item => item.Id == id).FirstOrDefaultAsync();
                    ResponseResult response = new ResponseResult()
                    {
                        success = true,
                        errorMsg = null,
                        data = new ClienteDTO(findById)
                    };
                    return Ok(response);
                }
                catch (Exception)
                {
                    ResponseResult response = new ResponseResult()
                    {
                        success = false,
                        errorMsg = "Ha ocurrido un error tratando de encontrar al cliente",
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
                        Cliente recordToDelete = await context.Cliente.Where(item => item.Id == id).FirstOrDefaultAsync();
                        context.Cliente.Remove(recordToDelete);
                        await context.SaveChangesAsync();
                        transaction.Commit();
                        ResponseResult response = new ResponseResult()
                        {
                            success = true,
                            errorMsg = null,
                            data = new ClienteDTO(recordToDelete)
                        };
                        return Ok(response);
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se pudo eliminar el cliente",
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
        public async Task<IHttpActionResult> Create([FromBody] ClienteDTO cliente)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        Cliente newRecord = new Cliente()
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
