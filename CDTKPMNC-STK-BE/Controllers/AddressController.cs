using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.DataAccess.Repositories.AddressRepository;
using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AddressController : CommonController
    {
        private readonly AddressService _addressService;
        public AddressController(AddressService addressService) 
        {
            _addressService = addressService;
        }

        // GET api/<AddressController>/Provines
        [HttpGet("Provines")]
        public IActionResult GetAllProvines()
        {
            var provines = _addressService.GetAllProvines();
            return Ok(new ResponseMessage(true, "OK", new { Provines = provines }));
        }

        // GET api/<AddressController>/District/79
        [HttpGet("District/ProvineId")]
        public IActionResult GetDistrictsByProvineId(string ProvineId)
        {
            var provine = _addressService.GetProvinceById(ProvineId);
            if (provine != null)
            {
                var districts = _addressService.GetDistrictsByProvineId(ProvineId);
                return Ok(new ResponseMessage(true, "OK", new { Districts = districts }));
            }
            return BadRequest(new ResponseMessage(false, "ProvineId not exist."));
        }

        // GET /<AddressController>/Wards/760
        [HttpGet("Ward/DistrictId")]
        public IActionResult GetWardsByDistrictId(string DistrictId)
        {
            var district = _addressService.GetDistrictById(DistrictId);
            if (district != null)
            {
                var wards = _addressService.GetWardsByDistrict(district);
                return Ok(new ResponseMessage
                {
                    Success = true,
                    Message = "OK",
                    Data = new { Wards = wards }
                });
            }
            return BadRequest(new ResponseMessage(false, "DistrictId not exist."));
        }
    }
}
