using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using WebApp03.Models;
using WebApp03.Services;
using WebApp03.Services.Impl;
using WebApp03.Tool;

namespace WebApp03.Controllers
{
    [RoutePrefix("api/emp")]
    public class EmpApiController : ApiController
    {
        // private readonly IEmpService _service = new EmpService();
        private readonly IEmpService _service;

        public EmpApiController(IEmpService service)
        {
            this._service = service;
        }
        
        /// <summary>
        ///  返回所查詢頁數的數據 
        /// </summary>
        /// <param name="keyword">查詢關鍵字，由query string獲取</param>
        /// <param name="pageNo">查詢頁數</param>
        /// <returns></returns>
        [Route("{pageNo:int?}")]
        [HttpGet] 
        public Page<Emp> Get(string? keyword, int pageNo=1)
        {
            // 獲取當前頁碼
            // 調用EmpService中的Page方法
            return _service.Page(pageNo, keyword);
        }
        
        [Route("getAll")]
        [HttpGet] 
        public IHttpActionResult GetAll()
        {
            return Ok(_service.SelectAll());
        }

        // POST: api/emp
        [HttpPost]
        [Route("")]
        public IHttpActionResult Post([FromBody]Emp emp)
        {
            if (emp == null)
            {
                return Content(HttpStatusCode.BadRequest, "empty value");
            }
            // 模型驗證
            if (!ModelState.IsValid)
            {
                string errMsg = MVCHelper.GetValidMsg(ModelState);
                return Content(HttpStatusCode.BadRequest, errMsg);
            }
            
            var res = 0;
            try
            {
                res = _service.Insert(emp);
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

        // PUT: api/emp
        [HttpPut]
        [Route("")]
        public IHttpActionResult Put([FromBody]Emp emp)
        {
            var res = 0;
            if (emp == null)
            {
                return Content(HttpStatusCode.BadRequest, "empty value");
            }
            if (ModelState.IsValid)
            {
                res = _service.Update(emp);
            }

            return res == 1
                ? Ok(new ErrMsg())
                : Ok(new ErrMsg
                {
                    Err = true, 
                    Msg = "Update record fail"
                });
        }

        // DELETE: api/emp/id
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
    }
}