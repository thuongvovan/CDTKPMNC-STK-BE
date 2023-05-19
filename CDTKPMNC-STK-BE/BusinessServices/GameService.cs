using CDTKPMNC_STK_BE.BusinessServices.Common;
using CDTKPMNC_STK_BE.DataAccess.Repositories;
using CDTKPMNC_STK_BE.DataAccess;
using CDTKPMNC_STK_BE.Models;
using CDTKPMNC_STK_BE.BusinessServices.Records;
using CDTKPMNC_STK_BE.BusinessServices.RecordValidators;
using CDTKPMNC_STK_BE.Utilities;
using Microsoft.EntityFrameworkCore;

namespace CDTKPMNC_STK_BE.BusinessServices
{
    public class GameService : CommonService
    {
        private readonly IGameRepository _gameRepo;

        public GameService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _gameRepo = _unitOfWork.GameRepo;
        }

        public List<Game> GetAllGame()
        {
            return _gameRepo.GetAll().ToList();
        }

        public List<Game> GetAvailableGame()
        {
            return _gameRepo.GetAvailable().ToList();
        }

        public Game? GetById(Guid gameId)
        {
            return _gameRepo.GetById(gameId);
        }

        public bool DeleteGame(Game? game)
        {
            if (game != null && game.Campaigns != null && game.Campaigns.Count == 0)
            {
                _gameRepo.Delete(game);
                return true;
            }
            return false;
        }

        public bool DeleteGame(Guid gameId)
        {
            var game = _gameRepo.GetById(gameId);
            return DeleteGame(game);
        }

        public ValidationSummary ValidateGameRecord(GameRecord? gameRecord)
        {
            if (gameRecord == null)
            {
                return new ValidationSummary(false, "Token is required.");
            }
            var validator = new GameRecordValidator();
            var result = validator.Validate(gameRecord);
            return result.GetSummary();
        }

        public bool VerifyGameRecord(GameRecord gameRecord)
        {
            var game = _gameRepo.GetByName(gameRecord.Name!);
            if (game != null) return true;
            return false;
        }

        public Game CreateGame(GameRecord gameRecord)
        {
            var game = new Game
            {
                Name = gameRecord.Name!.ToTitleCase(),
                Description = gameRecord.Description!,
                Instruction = gameRecord.Instruction!,
                IsEnable = gameRecord.IsEnable!.Value,
                CreatedAt = DateTime.Now
            };
            _gameRepo.Add(game);
            return game;
        }

        public Game UpdateGame(Game game, GameRecord gameRecord)
        {
            game.Name = gameRecord.Name!.ToTitleCase();
            game.Description = gameRecord.Description!;
            game.Instruction = gameRecord.Instruction!;
            game.IsEnable = gameRecord.IsEnable!.Value;
            _gameRepo.Update(game);
            return game;
        }
    }
}
