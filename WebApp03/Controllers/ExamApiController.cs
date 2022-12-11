using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using WebApp03.Models;
using WebApp03.Services;
using WebApp03.Tool;

namespace WebApp03.Controllers;

[RoutePrefix("api/exam")]
public class ExamApiController: ApiController
{
    private readonly IExamService _service;

    public ExamApiController(IExamService service)
    {
        this._service = service;
    }
    
    /// <summary>
    ///  返回所查詢頁數的數據 
    /// </summary>
    /// <param name="subjectId">查詢科目編號</param>
    /// <param name="pageNo">查詢頁數</param>
    /// <returns></returns>
    [Route("{pageNo:int?}/{subjectId:int?}")]
    [HttpGet] 
    public Page<ExamModel> Get(int subjectId, int pageNo=1)
    {
        return _service.Page(pageNo, subjectId);
    }
    
    [Route("get_emp_list/{subjectId:int}")]
    [HttpGet] 
    public IEnumerable<Emp> GetValidEmpList(int subjectId)
    {
        return _service.FindValidEmpList(subjectId);
    }
    
    [Route("get_by_emp_id/{pageNo:int?}/{empId:int?}")]
    [HttpGet] 
    public IHttpActionResult GetByEmpId(int empId, int pageNo=1)
    {
        var res = Ok(_service.PageByEmpId(pageNo, empId));
        try
        {
            return res;
        }
        catch (Exception e)
        {
            return Content(HttpStatusCode.BadRequest, "err");
        }
    }
    
    
    // POST: api/exam
    [HttpPost]
    [Route("")]
    public IHttpActionResult Post([FromBody]ExamModel model)
    {
        if (model == null)
        {
            return Content(HttpStatusCode.BadRequest, "empty value");
        }
        
        Console.WriteLine(model);

        var res = 0;
        try
        {
            res = _service.Insert(model);
        }
        catch (Exception e)
        {
            res = -1;
        }
            
        return res >= 1
            ? Ok(new ErrMsg())
            : Ok(new ErrMsg
            {
                Err = true, 
                Msg = "Add record fail"
            });
    }
    
    // PUT: api/exam
    [HttpPut]
    [Route("")]
    public IHttpActionResult Put([FromBody]ExamModel examModel)
    {
        var res = 0;
        if (examModel == null)
        {
            return Content(HttpStatusCode.BadRequest, "empty value");
        }
        
        res = _service.Update(examModel);

        return res == 1
            ? Ok(new ErrMsg())
            : Ok(new ErrMsg
            {
                Err = true, 
                Msg = "Update record fail"
            });
    }
    
    // PUT: api/exam/{id:int}
    [HttpDelete]
    [Route("{id:int}")]
    public IHttpActionResult Delete(int id)
    {
        var res = 0;

        res = _service.Delete(id);

        return res == 1
            ? Ok(new ErrMsg())
            : Ok(new ErrMsg
            {
                Err = true, 
                Msg = "Update record fail"
            });
    }
    
    
    
    
}