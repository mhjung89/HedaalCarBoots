﻿using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello, world");
        }
    }
}