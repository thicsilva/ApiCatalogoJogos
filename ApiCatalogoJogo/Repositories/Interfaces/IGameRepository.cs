using ApiCatalogoJogo.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogoJogo.Repositories.Interfaces
{
    public interface IGameRepository:IDisposable
    {
        Task<List<Game>> GetList(int page, int quantity);
        Task<List<Game>> GetList(string name, string producer);
        Task<Game> Get(Guid id);
        Task Create(Game game);
        Task Update(Game game);
        Task Delete(Guid id);
    }
}
