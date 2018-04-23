﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HackneyAddressesAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values?id=5
        [HttpGet("/get/{id}/{id2}")]
        public string Get(int id, int id2)
        {
            return "value";
        }

        //get api/values?uprn=57658
        [HttpGet("/get2/{id}/{id2}")]
        public string Get2(int uprn, int uprn2)
        {
            return "value";
        }

        //get api/values?uprn=57658
        [HttpGet("/get3/{id}/{id2}")]
        public string Get3(int uprn, int uprn2)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
