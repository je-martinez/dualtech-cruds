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
        public async Task<IHttpActionResult> Create([FromBody] List<TasaCambioDTO> tasas)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<TasaCambioDTOWithValidation> tasasACrear = await ValidarTasasCambio(tasas);
                        List<TasaCambioDTOWithValidation> tasasErrors = tasasACrear.Where(item => item.hasErrors == true).ToList();
                        if (tasasErrors != null && tasasErrors.Count > 0)
                        {
                            ResponseResult response = new ResponseResult()
                            {
                                success = false,
                                errorMsg = "Se han encontrado errores en la lista enviada",
                                data = tasasErrors
                            };
                            return Content(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            ResponseResult response = new ResponseResult()
                            {
                                success = false,
                                errorMsg = "Se han encontrado errores en la lista enviada",
                                data = tasasErrors
                            };
                            return Content(HttpStatusCode.BadRequest, response);
                        }
                    }
                    catch (Exception ex)
                    {
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = null,
                            data = tasas
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
        public async Task<IHttpActionResult> Edit([FromBody] List<TasaCambioDTO> tasas)
        {
            using (DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        List<TasaCambioDTOWithValidation> tasasACrear = await ValidarTasasCambio(tasas);
                        List<TasaCambioDTOWithValidation> tasasErrors = tasasACrear.Where(item => item.hasErrors == true).ToList();
                        if(tasasErrors != null && tasasErrors.Count > 0)
                        {
                            ResponseResult response = new ResponseResult()
                            {
                                success = false,
                                errorMsg = "Se han encontrado errores en la lista enviada",
                                data = tasasErrors
                            };
                            return Content(HttpStatusCode.BadRequest, response);
                        }
                        else
                        {
                            ResponseResult response = new ResponseResult()
                            {
                                success = false,
                                errorMsg = "Se han encontrado errores en la lista enviada",
                                data = tasasErrors
                            };
                            return Content(HttpStatusCode.BadRequest, response);
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ResponseResult response = new ResponseResult()
                        {
                            success = false,
                            errorMsg = "No se ha podido editar las tasas de cambio",
                            data = tasas
                        };
                        return Content(HttpStatusCode.BadRequest, response);
                    }
                }
            }
        }

        //True = OK, False = HasErrors
        private async Task<List<TasaCambioDTOWithValidation>> ValidarTasasCambio(List<TasaCambioDTO> tasas)
        {
            using(DualTechCrudsBDEntities context = new DualTechCrudsBDEntities())
            {
                try
                {
                    List<TasaCambio> tasasDB = await context.TasaCambio.ToListAsync();
                    List<TasaCambioDTOWithValidation> tasasConValidaciones = new List<TasaCambioDTOWithValidation>();
                    TasaCambioDTO elementoFechaInicialMasReciente = tasas.Where(item => item.FechaFinal == null).OrderByDescending(item => item.FechaInicio).FirstOrDefault();
                    tasas.ForEach(item =>
                    {
                        TasaCambioDTO find = tasas.Where(itemb => itemb.FechaInicio == item.FechaInicio && item.FechaFinal == itemb.FechaFinal && item.Tasa != itemb.Tasa).FirstOrDefault();
                        TasaCambio findDB = tasasDB.Where(itemb => itemb.FechaInicio == item.FechaInicio && item.FechaFinal == itemb.FechaFinal).FirstOrDefault();
                        TasaCambioDTOWithValidation between = tasasConValidaciones.Where(itemb => item.FechaInicio >= itemb.FechaInicio && item.FechaFinal <= item.FechaFinal).FirstOrDefault();
                        TasaCambio betweenDB = tasasDB.Where(itemb => item.FechaInicio >= itemb.FechaInicio && item.FechaFinal <= (itemb.FechaFinal == null ? DateTime.Now:itemb.FechaFinal)).FirstOrDefault();
                        bool datesInverted = item.FechaInicio > item.FechaFinal ? true : false;
                        bool isFechaInicialMasReciente = elementoFechaInicialMasReciente == item ? true : false;
                        if (find == null 
                        && between == null 
                        && betweenDB == null 
                        && findDB == null 
                        && datesInverted == false 
                        && (isFechaInicialMasReciente == true || item.FechaFinal != null)
                        && item.Tasa > 0)
                        {
                            tasasConValidaciones.Add(new TasaCambioDTOWithValidation(item, false, null));
                        }
                        else
                        {
                            List<string> errors = new List<string>();
                            if(find != null)
                            {
                                errors.Add("Duplicado en lista enviada");
                            }
                            if (findDB != null)
                            {
                                errors.Add("Duplicado en base de datos");
                            }
                            if (between != null)
                            {
                                errors.Add("Se encuentra interpolada con uno o mas elementos en la lista enviada");
                            }
                            if (betweenDB != null)
                            {
                                errors.Add("Se encuentra interpolada con uno o mas elementos en la base de datos");
                            }
                            if (datesInverted == true)
                            {
                                errors.Add("La fecha de inicio no puede ser mayor a la fecha final");
                            }
                            if(item.Tasa == 0)
                            {
                                errors.Add("La tasa debe ser mayor de cero");
                            }
                            if(item.FechaFinal == null && isFechaInicialMasReciente == false)
                            {
                                errors.Add("Solo la tasa de cambio con el fecha inicial mas reciente puede tener su fecha final como NULL");
                            }
                            bool hasError = errors.Count > 0 ? true:false;
                            string validationErrors = string.Join(", ", errors);
                            TasaCambioDTOWithValidation tasa = new TasaCambioDTOWithValidation(item, hasError, validationErrors);
                            tasasConValidaciones.Add(tasa);
                        }
                    });
                    return tasasConValidaciones;
                }
                catch (Exception ex)
                {
                    return new List<TasaCambioDTOWithValidation>();
                }
            }
        }
    }
}
