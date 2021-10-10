using System;

namespace ApiCatalogoJogo.Exceptions
{
    public class GameNotRegisteredException : Exception
    {
        public GameNotRegisteredException(): base("Game not registered in database")
        {
        }
    }
}
