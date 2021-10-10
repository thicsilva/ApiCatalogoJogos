using ApiCatalogoJogo.Entities;
using ApiCatalogoJogo.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogoJogo.Repositories
{
    public class GameMysqlRepository : IGameRepository
    {
        private readonly MySqlConnection _mysqlConnection;

        public GameMysqlRepository(IConfiguration configuration)
        {
            _mysqlConnection = new MySqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task Create(Game game)
        {
            var command = "INSERT INTO GAMES (ID, NAME, PRODUCER, PRICE) VALUES (@Id, @Name, @Producer, @Price)";
            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", game.Id);
            sqlCommand.Parameters.AddWithValue("@Name", game.Name);
            sqlCommand.Parameters.AddWithValue("@Producer", game.Producer);
            sqlCommand.Parameters.AddWithValue("@Price", game.Price);
            await sqlCommand.ExecuteNonQueryAsync();
            await _mysqlConnection.CloseAsync();
        }

        public async Task Delete(Guid id)
        {
            var command = "DELETE FROM GAMES WHERE ID=@Id";
            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            await sqlCommand.ExecuteNonQueryAsync();
            await _mysqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            _mysqlConnection?.Close();
            _mysqlConnection?.Dispose();
        }

        public async Task<Game> Get(Guid id)
        {
            Game game = null;
            var command = "SELECT ID, NAME, PRODUCER, PRICE FROM GAMES WHERE ID=@Id";
            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", id);
            MySqlDataReader dataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                game = new Game
                {
                    Id = Guid.Parse((string)dataReader["ID"]),
                    Name = (string)dataReader["NAME"],
                    Producer = (string)dataReader["PRODUCER"],
                    Price = (double)dataReader["PRICE"]
                };
            }

            await _mysqlConnection.CloseAsync();
            return game;
        }

        public async Task<List<Game>> GetList(int page, int quantity)
        {
            var games = new List<Game>();
            var command = "SELECT ID, NAME, PRODUCER, PRICE FROM GAMES ORDER BY ID LIMIT @Page, @Quantity";

            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Page", (page - 1) * quantity);
            sqlCommand.Parameters.AddWithValue("@Quantity", quantity);
            MySqlDataReader dataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                games.Add(new Game
                {
                    Id = Guid.Parse((string)dataReader["ID"]),
                    Name = (string)dataReader["NAME"],
                    Producer = (string)dataReader["PRODUCER"],
                    Price = (double)dataReader["PRICE"]
                });
            }

            await _mysqlConnection.CloseAsync();
            return games;
        }

        public async Task<List<Game>> GetList(string name, string producer)
        {
            var games = new List<Game>();
            var command = "SELECT ID, NAME, PRODUCER, PRICE FROM GAMES WHERE UPPER(NAME)=@Name AND UPPER(PRODUCER)=@Producer";
            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Name", name.ToUpper());
            sqlCommand.Parameters.AddWithValue("@Producer", producer.ToUpper());
            MySqlDataReader dataReader = (MySqlDataReader)await sqlCommand.ExecuteReaderAsync();
            while (dataReader.Read())
            {
                games.Add(new Game
                {
                    Id = Guid.Parse((string)dataReader["ID"]),
                    Name = (string)dataReader["NAME"],
                    Producer = (string)dataReader["PRODUCER"],
                    Price = (double)dataReader["PRICE"]
                });
            }

            await _mysqlConnection.CloseAsync();
            return games;
        }

        public async Task Update(Game game)
        {
            var command = $"UPDATE GAMES SET NAME=@Name, PRODUCER=@Producer, PRICE=@Price WHERE ID=@Id";
            await _mysqlConnection.OpenAsync();
            var sqlCommand = new MySqlCommand(command, _mysqlConnection);
            sqlCommand.Parameters.AddWithValue("@Id", game.Id);
            sqlCommand.Parameters.AddWithValue("@Name", game.Name);
            sqlCommand.Parameters.AddWithValue("@Producer", game.Producer);
            sqlCommand.Parameters.AddWithValue("@Price", game.Price);
            await sqlCommand.ExecuteNonQueryAsync();
            await _mysqlConnection.CloseAsync();
        }
    }
}
