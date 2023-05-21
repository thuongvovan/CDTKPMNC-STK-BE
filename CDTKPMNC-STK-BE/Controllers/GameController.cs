﻿using CDTKPMNC_STK_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using CDTKPMNC_STK_BE.BusinessServices;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.AccountServices;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CDTKPMNC_STK_BE.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : CommonController
    {
        private readonly GameService _gameService;
        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // POST /<UserController>/Create
        [HttpPost("Create")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult AddGame([FromBody] GameRecord gameRecord)
        {
            var validateSummary = _gameService.ValidateGameRecord(gameRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            bool IsVerified = _gameService.VerifyGameRecord(gameRecord);
            if (!IsVerified)
            {
                
                var game = _gameService.CreateGame(gameRecord);
                return Ok(new ResponseMessage { Success = true, Message = "Create new game successfuly.", Data = new { Game = game } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Game is really exiseted" });            
        }

        // GET /<UserController>/All
        [HttpGet("All")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult GetAllGame()
        {
            var games = _gameService.GetAllGame();
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
            var games = _gameService.GetAllGame();
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
            var game = _gameService.GetById(gameId);
            if (game != null)
            {
                if (game.IsEnable)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Successful", Data = new { Game = game } });
                }
                if (UserType == AccountType.Admin)
                {
                    return Ok(new ResponseMessage { Success = true, Message = "Successful", Data = new { Game = game } });
                }
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not correct." });
        }

        // DELETE /<UserController>/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpDelete("{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DeleteGame(Guid gameId)
        {
            var isSuccess = _gameService.DeleteGame(gameId);
            if (isSuccess)
            {
                 return Ok(new ResponseMessage { Success = true, Message = "Deleted game" });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not correct." });
        }

        // PUT /<UserController>/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult UpdateGame(Guid gameId, [FromBody] GameRecord gameRecord)
        {
            var validateSummary = _gameService.ValidateGameRecord(gameRecord);
            if (!validateSummary.IsValid)
            {
                return BadRequest(new ResponseMessage(false, validateSummary.ErrorMessage));
            }
            var game = _gameService.GetById(gameId);
            if (game != null)
            {
                _gameService.UpdateGame(game, gameRecord);
                return Ok(new ResponseMessage { Success = true, Message = "Updated game", Data = new { Game = game }});
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "gameId is not correct." });
        }

        // PUT /<UserController>/Disable/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Disable/{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult DisableGame(Guid gameId)
        {
            var game = _gameService.GetById(gameId);
            if (game != null && game.IsEnable)
            {
                _gameService.DisableGame(game);
                return Ok(new ResponseMessage { Success = true, Message = "The game has been disabled", Data = new { Game = game } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }

        // PUT /<UserController>/Disable/ECE26B11-E820-4184-2D7A-08DB4FD1F7BC
        [HttpPut("Enable/{gameId:Guid}")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public IActionResult EnableGame(Guid gameId)
        {
            var game = _gameService.GetById(gameId);
            if (game != null && game.IsEnable)
            {
                _gameService.EnableGame(game);
                return Ok(new ResponseMessage { Success = true, Message = "The game has been enabled", Data = new { Game = game } });
            }
            return BadRequest(new ResponseMessage { Success = false, Message = "Invalid request." });
        }
    }
}
