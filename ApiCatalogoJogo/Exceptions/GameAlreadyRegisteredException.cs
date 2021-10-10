using System;

namespace ApiCatalogoJogo.Exceptions
{
    public class GameAlreadyRegisteredException : Exception
    {
        public GameAlreadyRegisteredException():base("Game already registered in database")
        {
        }
    }
}
