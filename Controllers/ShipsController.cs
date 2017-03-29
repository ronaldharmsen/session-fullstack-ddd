using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Halcyon.HAL;
using Halcyon.Web.HAL;

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

            var response = HAL(nameof(ships), ships.ToHALResponses((i) => 
                new Link[] { 
                    new Link("_self", CreateShipLink(i.Id)),                    
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
            return this.HAL(new Ship { Name = "C" }, new Link[] { CommandLink<SellShipCommand>(cmd=> SellShip(id,cmd)) }, addSelfLinkIfNotExists: true);
        }

        //We are creating a new purchaseorder'document'
        [HttpPost("PurchaseOrder")]
		[Command(Title="Purchase")]
        public IActionResult BuyShip(BuyShipCommand cmd)
        {
            return Created(CreateShipLink(0), null);
        }

        [HttpPost("{id}/sale")]
        [Command(Title="Sell")]
        public IActionResult SellShip(int id, SellShipCommand cmd)
        {            
            return Ok();
        }

        private string CreateShipLink(int id)
        {
            return Url.Link(null, new { Controller = "Ships", Action = "Get", Id = id });
        }
    }
}
