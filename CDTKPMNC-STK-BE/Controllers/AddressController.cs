using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAddressRepository _addressRepository;
        public AddressController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _addressRepository = _unitOfWork.AddressRepository;
        }

        // GET api/<AddressController>/Provines
        [HttpGet("Provines")]
        public IActionResult GetAllProvines()
        {
            var provines = _addressRepository.GetAllProvines();
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { Provines = provines }
            });
        }

        // GET api/<AddressController>/District/79
        [HttpGet("District/ProvineId")]
        public IActionResult GetDistrictsByProvine(string ProvineId)
        {
            var districts = _addressRepository.GetDistrictsByProvineId(ProvineId);
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { Districts = districts }
            });
        }

        // GET /<AddressController>/Wards/760
        [HttpGet("Ward/DistrictId")]
        public IActionResult GetWardsByDistrict(string DistrictId)
        {
            var wards = _addressRepository.GetWardsByDistrictId(DistrictId);
            return Ok(new ResponseMessage
            {
                Success = true,
                Message = "OK",
                Data = new { Wards = wards }
            });
        }
    }
}
