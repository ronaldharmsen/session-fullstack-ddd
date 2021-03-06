﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL;
using Halcyon.Web.HAL;

namespace SessionGen
{
    [Route("api/[controller]")]
    public class ShipsController : HALController
    {
        static List<Ship> ships = new List<Ship>
            {
                new Ship { Id=0, Name = "MS A", Location="Tel Aviv" },
                new Ship { Id=1, Name = "MS B", Location="Rome" },
                new Ship { Id=2, Name = "MS C", Location="Amsterdam" }
            };

		public ShipsController() : base()
		{
			
		}
        [HttpGet()]
        public IActionResult Get()
        {        
            //Add metadata explicitly
            var response = HAL(ships.ToHALResponses((i) => 
                new Link[] { 
                    new Link("_self", CreateShipLink(i.Id)),   
                    CommandLink<SellShipCommand>(cmd=> SellShip(i.Id,cmd))                    
                }
                ));

            response.AddLinks(
                CommandLink<BuyShipCommand>(cmd => BuyShip(cmd))
			);

            return Ok(response);
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            return this.HAL(
                new Ship { Name = "C" }, 
                new Link[] { CommandLink<SellShipCommand>(cmd=> SellShip(id,cmd)) }, 
                addSelfLinkIfNotExists: true);
        }

        //We are creating a new purchaseorder'document'
        [HttpPost("PurchaseOrder")]
		[Command(Title="Purchase")]
        public IActionResult BuyShip(BuyShipCommand cmd)
        {
            //Hand off to backend / CQRS Command bus here...
            int newId = 0;
            return Created(CreateShipLink(newId), null);
        }

        [HttpPost("{id}/sale")]
        [Command(Title="Sell")]
        public IActionResult SellShip(int id, SellShipCommand cmd)
        {            
            //Hand off to backend / CQRS Command bus here...
            return Ok();
        }

        private string CreateShipLink(int id)
        {
            return Url.Link(null, new { Controller = "Ships", Action = "Get", Id = id });
        }
    }
}
