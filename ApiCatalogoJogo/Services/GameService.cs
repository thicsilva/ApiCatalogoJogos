using ApiCatalogoJogo.Entities;
using ApiCatalogoJogo.Exceptions;
using ApiCatalogoJogo.InputModel;
using ApiCatalogoJogo.Repositories.Interfaces;
using ApiCatalogoJogo.Services.Interfaces;
using ApiCatalogoJogo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogo.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<GameViewModel> Create(GameInputModel gameInputModel)
        {
            var games = await _gameRepository.GetList(gameInputModel.Name, gameInputModel.Producer);
            if (games.Count > 0)
                throw new GameAlreadyRegisteredException();
            var game = new Game 
            {
                Id = Guid.NewGuid(),
                Name = gameInputModel.Name,
                Producer = gameInputModel.Producer,
                Price = gameInputModel.Price
            };
            await _gameRepository.Create(game);
            return new GameViewModel 
            { 
                Id=game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task Delete(Guid id)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                throw new GameNotRegisteredException();
            await _gameRepository.Delete(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                return null;
            return new GameViewModel 
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<List<GameViewModel>> GetList(int page, int quantity)
        {
            var games = await _gameRepository.GetList(page, quantity);
            return games.Select(game => new GameViewModel
            {
                Id=game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            }).ToList();
        }

        public async Task Update(Guid id, GameInputModel gameInputModel)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                throw new GameNotRegisteredException();
            game.Name = gameInputModel.Name;
            game.Producer = gameInputModel.Producer;
            game.Price = gameInputModel.Price;
            await _gameRepository.Update(game);
        }

        public async Task Update(Guid id, double price)
        {
            var game = await _gameRepository.Get(id);
            if (game == null)
                throw new GameNotRegisteredException();
            game.Price = price;
            await _gameRepository.Update(game);
        }
    }
}
