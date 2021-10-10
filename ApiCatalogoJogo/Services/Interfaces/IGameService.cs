using ApiCatalogoJogo.InputModel;
using ApiCatalogoJogo.ViewModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogoJogo.Services.Interfaces
{
    public interface IGameService:IDisposable
    {
        Task<List<GameViewModel>> GetList(int page, int quantity);
        Task<GameViewModel> Get(Guid id);
        Task<GameViewModel> Create(GameInputModel gameInputModel);
        Task Update(Guid id, GameInputModel gameInputModel);
        Task Update(Guid id, double price);
        Task Delete(Guid id);

    }
}
