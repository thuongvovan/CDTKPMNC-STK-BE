using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.Repositories;
using CDTKPMNC_STK_BE.Utilities.Validator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameRepository _gameRepository;
        public GameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _gameRepository = _unitOfWork.GameRepo;
        }

        // POST /<UserController>
        [HttpPost()]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult AddGame([FromBody] GameInfo gameInfo)
        {
            var validator = new GameValidator();
            ValidationResult? validateResult;
            try
            {
                validateResult = validator.Validate(gameInfo);
            }
            catch (Exception)
            {

                return BadRequest(new ResponseMessage { Success = false, Message = "Unable to verify data" });
            }

            if (!validateResult.IsValid)
            {
                string? ErrorMessage = validateResult.Errors?.FirstOrDefault()?.ErrorMessage;
                return BadRequest(new ResponseMessage { Success = false, Message = ErrorMessage! });
            }
            return Ok(new ResponseMessage { Success = true, Message = "Create new game successfuly." });
        }

        // GET /<UserController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllGame()
        {
            var games = _unitOfWork.GameRepo.GetAll();
            if (games != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of game successfuly.", Data = new { Games = games } });
            }
            games = new List<Game>();
            return Ok(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Games = games } });
        }

        // GET /<UserController>/Available
        [HttpGet("Available")]
        [Authorize(AuthenticationSchemes = "Admin&Partner")]
        public IActionResult GetAvailableGame()
        {
            var games = _unitOfWork.GameRepo.GetAvailable();
            if (games != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Get the list of available game successfuly.", Data = new { Games = games } });
            }
            games = new List<Game>();
            return Ok(new ResponseMessage { Success = true, Message = "The list is empty.", Data = new { Games = games } });
        }

        // GET /<UserController>/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpGet("{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Account")]
        public IActionResult GetGameDetail(Guid gameId)
        {
            var game = _unitOfWork.GameRepo.GetById(gameId);
            if (game != null)
            {
                return Ok(new ResponseMessage { Success = true, Message = "Successful", Data = game });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not correct." });
        }

        // GET /<UserController>/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpDelete("{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DeleteGame(Guid gameId)
        {
            var game = _unitOfWork.GameRepo.GetById(gameId);
            if (game != null)
            {
                _unitOfWork.GameRepo.Delete(game);
                _unitOfWork.Commit();
                return Ok(new ResponseMessage { Success = true, Message = "Deleted game" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not correct." });
        }
    }
}
