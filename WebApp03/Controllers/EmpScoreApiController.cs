using System.Collections.Generic;
using System.Web.Http;
using WebApp03.Models;
using WebApp03.Services;

namespace WebApp03.Controllers;

[RoutePrefix("api/emp_score")]
public class EmpScoreApiController: ApiController
{
    private readonly IEmpScoreService _service;

    public EmpScoreApiController(IEmpScoreService service)
    {
        _service = service;
    }
    
    [Route("{pageNo:int?}")]
    [HttpGet] 
    public IHttpActionResult Get(int pageNo=1)
    {
        return Ok(_service.Page(pageNo));
    }
}