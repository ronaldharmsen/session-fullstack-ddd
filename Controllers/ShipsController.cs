using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using System;
using System.Linq.Expressions;

namespace SessionGen
{
    [Route("api/[controller]")]
    public class ShipsController : HALController
    {
		public ShipsController() : base()
		{
			
		}
        [HttpGet()]
        public IActionResult Get()
        {
            var ships = new List<Ship>
            {
                new Ship { Id=0, Name = "MS A", Location="Tel Aviv" },
                new Ship { Id=1, Name = "MS B", Location="Rome" }
            };

            var response = HAL(nameof(ships), ships);
            response.AddLinks(
                CommandLink<BuyShipCommand>(cmd => BuyShip(cmd))
			);

            return Ok(response);
        }

        

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return this.HAL(new Ship { Name = "C" }, new Link[] { }, addSelfLinkIfNotExists: true);
        }

        //We are creating a new purchaseorder'document'
        [HttpPost("PurchaseOrder")]
		[Command(Title="Purchase")]
        public IActionResult BuyShip(BuyShipCommand cmd)
        {
            return Created(CreateShipLink(0), null);
        }

        private string CreateShipLink(int id)
        {
            return Url.Link("Default", new { Controller = "ShipsController", Action = "Get", Id = id });
        }

        [HttpPost("")]
        public IActionResult PostSomethingElse(string name)
        {
            return Created("", null);
        }
    }
}
