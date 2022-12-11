using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Services;
using WebApp03.Services.Impl;
using WebApp03.Tool;

namespace WebApp03.Controllers
{
    [RoutePrefix("api/subject")]
    public class SubjectApiController: ApiController
    {
        // private readonly ISubjectService _service = new SubjectService();
        private readonly ISubjectService _service;

        public SubjectApiController(ISubjectService service)
        {
            this._service = service;
        }
        
        /// <summary>
        ///  返回所查詢頁數的數據 
        /// </summary>
        /// <param name="keyword">查詢關鍵字，由query string獲取</param>
        /// <param name="pageNo">查詢頁數</param>
        /// <returns></returns>
        [Route("{pageNo:int?}/{keyword?}")]
        [HttpGet] 
        public Page<Subject> Get(string keyword, int pageNo=1)
        {
            // return _service.Page(pageNo, keyword);
            return _service.PageWithEntity(pageNo, keyword);
        }
        
        
        // POST: api/subject
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Subject subject)
        {
            if (subject == null)
            {
                return Content(HttpStatusCode.BadRequest, "empty value");
            }
            // 模型驗證
            if (!ModelState.IsValid)
            {
                var errMsg = MVCHelper.GetValidMsg(ModelState);
                return Content(HttpStatusCode.BadRequest, errMsg);
            }
            
            var res = 0;
            try
            {
                res = _service.Insert(subject);
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
        
        // PUT: api/subject
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]Subject subject)
        {
            var res = 0;
            if (subject == null)
            {
                return Content(HttpStatusCode.BadRequest, "empty value");
            }
            if (ModelState.IsValid)
            {
                res = _service.Update(subject);
            }

            return res == 1
                ? Ok(new ErrMsg())
                : Ok(new ErrMsg
                {
                    Err = true, 
                    Msg = "Update record fail"
                });
        }

        // DELETE: api/subject/id
        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var res = _service.Delete(id);
            
            return res == 1
                ? Ok(new ErrMsg())
                : Ok(new ErrMsg
                {
                    Err = true, 
                    Msg = "Delete record fail"
                });
        }

        [HttpGet]
        [Route("getAll")]
        public IEnumerable<Subject> GetAllSubjects()
        {
            return _service.SelectAll();
        }
    }    
}

