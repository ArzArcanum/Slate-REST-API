using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlateAPI.DTOs;

namespace SlateAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        //public TestController(MessageContext context)
        //{
        //}

        //// GET: /Test
        //[HttpGet]
        //public ActionResult GetTest()
        //{
        //    var placeholder = new
        //    {
        //        testMessage = "Hello there!"
        //    };
        //    return Ok(placeholder);
        //}

        // GET: /Test
        [HttpGet]
        public string GetTest()
        {
            var placeholder = new
            {
                testMessage = "Hello there!"
            };
            return "hello";
        }
    }
}
