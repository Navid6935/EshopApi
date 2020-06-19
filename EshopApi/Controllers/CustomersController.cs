using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EshopApi.Contracts;
using EshopApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace EshopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private ICustomerRepository _customerRepository;
       

        public CustomersController
            (ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        
        }

        [HttpGet]
        public IActionResult GetCustomer()
        { 

            var result =  new ObjectResult
                (_customerRepository.GetAll())
            {
                StatusCode = (int) HttpStatusCode.OK
            };
            Request.HttpContext.Response.Headers.Add("X-Count",
                _customerRepository.CountCustomer().ToString());
            Request.HttpContext.Response.Headers.Add
                ("X-Name","Iman Madaeny");

            return result;
        }

        /// <summary>
        /// This Methode Get All Customer
        /// </summary>
        /// <param name="id">This Int Get Customer Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] int id)
        {
            if ( await CustomerExists(id))
            {
         
                var customer = await _customerRepository.Find(id);
                return Ok(customer);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("iman",Name = "Iman")]
        public IActionResult GetIman()
        {
            return Content("Iman Madaeny");
        }

        private async Task<bool> CustomerExists(int id)
        {
            return await _customerRepository.IsExists(id);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer
            ([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _customerRepository.Add(customer);
            return CreatedAtAction("GetCustomer",
                new {id = customer.CustomerId}, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer
            ([FromRoute] int id, [FromBody] Customer customer)
        {
            await _customerRepository.Update(customer);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer
            ([FromRoute] int id)
        {
            await _customerRepository.Remove(id);
            return Ok();
        }
    }
}