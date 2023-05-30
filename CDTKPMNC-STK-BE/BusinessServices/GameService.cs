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
        private readonly string _uploadRequestPath = Environment.GetEnvironmentVariable("UPLOAD_REQUEST_PATH")!;
        private readonly string _uploadDirectory = Environment.GetEnvironmentVariable("UPLOAD_DIRECTORY")!;

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
            if (game == null) return true;
            return false;
        }

        public string? CopyImageUrl(Guid gameId, GameRecord gameRecord)
        {
            var sourceFileName = gameRecord.ImageUrl!.Split('/').Last();
            var sourceFilePath = Path.Combine(_uploadDirectory, "TempImages", sourceFileName);
            string fileExtension = Path.GetExtension(sourceFilePath);
            var destinationFileName = $"{gameId}{fileExtension}";
            if (File.Exists(sourceFilePath))
            {
                var directoryPath = Path.Combine(_uploadDirectory, "Game");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                var destinationFilePath = Path.Combine(directoryPath, destinationFileName);
                File.Copy(sourceFilePath, destinationFilePath, true);
                try
                {
                    File.Delete(sourceFilePath);
                }
                catch{}
                return _uploadRequestPath + "/Game/" + destinationFileName;
            }
            return null;
        }

        public Game CreateGame(GameRecord gameRecord)
        {
            Guid gameId = Guid.NewGuid();
            var imageUrl = CopyImageUrl(gameId, gameRecord);
            var game = new Game
            {
                Id = gameId,
                Name = gameRecord.Name!.ToTitleCase(),
                Description = gameRecord.Description!,
                Instruction = gameRecord.Instruction!,
                IsEnable = gameRecord.IsEnable!.Value,
                ImageUrl = imageUrl,
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

            if (game.ImageUrl != gameRecord.ImageUrl)
            {
                var imageUrl = CopyImageUrl(game.Id, gameRecord);
                game.ImageUrl = imageUrl;
            }
            _gameRepo.Update(game);
            return game;
        }

        public Game DisableGame(Game game)
        {
            game.IsEnable = false;
            _gameRepo.Update(game);
            return game;
        }

        public Game EnableGame(Game game)
        {
            game.IsEnable = true;
            _gameRepo.Update(game);
            return game;
        }

        #region Lucly Wheel
        public bool PlayLuclyWheel(int winRate)
        {
            var randomNum = RandomHelper.RandomWithin(0, 100);
            if (randomNum <= winRate) return true;
            return false;
        }

        #endregion

        #region Over Under
        // Xỉu: 3 - 10
        // Tài: 11 - 18
        public bool PlayOverUnder(int winRate, bool userIsOver, out OverUnderData overUnderData)
        {
            var overUnderGame = new OverUnder();
            return overUnderGame.PlayV2(winRate, userIsOver, out overUnderData);
        }

        public record OverUnderDices(int Dice_1, int Dice_2, int Dice_3);
        public record OverUnderData(bool GameIsOver, bool UserIsOver, int SumScore, OverUnderDices Dices);
        /// <summary>
        /// Class tổ chức game tài xỉu
        /// </summary>
        public class OverUnder
        {
            public bool PlayV2(int winRate, bool userIsOver, out OverUnderData overUnderData)
            {
                int min, max;
                bool isWinner = RandomHelper.RandomWithin(1, 100) <= winRate;
                if (isWinner)
                {
                    if (userIsOver)
                    {
                        min = 11;
                        max = 18;
                    }
                    else
                    {
                        min = 3;
                        max = 10;
                    }
                }
                else
                {
                    if (userIsOver)
                    {
                        min = 3;
                        max = 10;
                    }
                    else
                    {
                        min = 11;
                        max = 18;
                    }
                }
                var sumScore = RandomHelper.RandomWithin(min, max);
                var dices = GenerateDicesResult(sumScore);
                var gameIsOver = sumScore >= 11;
                overUnderData = new OverUnderData(gameIsOver, userIsOver, sumScore, dices);
                if (gameIsOver == userIsOver) return true;
                return false;

            }
            public bool PlayV1(int winRate, bool userIsOver, out OverUnderData overUnderData)
            {
                int min = 3;
                int max = 18;
                int range = max - min + 1;
                int adjustmentRate = Math.Abs(50 - winRate);
                int adjustmentValue = (int)Math.Round(((double)(range * adjustmentRate) / 100));

                if (userIsOver) // Tài
                {
                    if (winRate > 50) min += adjustmentValue;
                    else if (winRate < 50) max -= adjustmentValue;
                }
                else // Xỉu
                {
                    if (winRate > 50) max -= adjustmentValue;
                    else if (winRate < 50) min += adjustmentValue;
                }

                var sumScore = RandomHelper.RandomWithin(min, max);


                var dices = GenerateDicesResult(sumScore);
                var gameIsOver = sumScore >= 11;
                overUnderData = new OverUnderData(gameIsOver, userIsOver, sumScore, dices);
                if (gameIsOver == userIsOver) return true;
                return false;
            }

            public OverUnderDices GenerateDicesResult(int sumScore)
            {
                int sum = 0;
                int dice_1 = 0;
                int dice_2 = 0;
                int dice_3 = 0;

                while (sum != sumScore || dice_3 < 1 || dice_3 > 6)
                {
                    dice_1 = RandomHelper.RandomWithin(1, 6);
                    dice_2 = RandomHelper.RandomWithin(1, 6);
                    dice_3 = sumScore - dice_1 - dice_2;
                    sum = (dice_1 + dice_2 + dice_3);
                    // Console.WriteLine($"IN WHILE ({sumScore}): {dice_1} - {dice_2} - {dice_3}");
                }

                return new OverUnderDices(dice_1, dice_2, dice_3);
            }
        }

        // ====== TESTING ======
        //int nWin = 0;
        //for (int i = 0; i < 1000; i++)
        //{
        //    bool userPlay = RandomHelper.RandomWithin(0, 1) == 0;
        //    var overUnderGame = new OverUnder();
        //    var ressult = overUnderGame.Play(70, userPlay, out var d);
        //    if (ressult) nWin++;
        //    Console.WriteLine($"Is Winner: {ressult} | GameIsOver: {d.GameIsOver} | UserIsOver: {d.UserIsOver} | ${d.SumScore}({d.Dices.Dice_1}, {d.Dices.Dice_2}, {d.Dices.Dice_3})");
        //}

        //Console.WriteLine("=======TOTAL==========");
        //Console.WriteLine($"{nWin}/1000");

        #endregion




    }
}
