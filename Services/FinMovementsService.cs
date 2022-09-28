using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Data.Common;
using System.Threading.Tasks;
using FinBalance2.Models;

namespace FinBalance2.Services
{
    class FinMovementsService : IDisposable
    {
        private const string CONNECTION_STRING = @"Driver={Microsoft Access Driver (*.mdb, *.accdb)}; Dbq=Data\Balance.mdb;";
        private OdbcConnection conn;
        private bool isConnectionOwner;

        public FinMovementsService(OdbcConnection? optConnection = null)
        {
            conn = optConnection ?? new OdbcConnection(CONNECTION_STRING);
            isConnectionOwner = optConnection == null;
        }

        public async Task<IEnumerable<FinMovement>> GetAllMovements()
        {
            string query = @"SELECT * FROM Movimentos";
            OdbcCommand command = new(query, conn);
            List<FinMovement> output = new();

            conn.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    FinMovement move = new();
                    move.Id = reader.SafeGetInt(0);
                    move.Name = reader.SafeGetString(1);
                    move.Date = reader.SafeGetDate(2);
                    move.Value = reader.SafeGetDecimal(3);
                    move.Notes = reader.SafeGetString(4);
                    output.Add(move);
                }
            }
            conn.Close();
            return output;
        }

        public async Task<IEnumerable<FinMovement>> GetFromPeriod(DateTime begin, DateTime end)
        {
            string query = @"SELECT * FROM Movimentos WHERE Data >= ? AND Data <= ?";
            OdbcCommand command = new(query, conn);
            command.Parameters.Add("beginDate", OdbcType.Date, 0).Value = begin;
            command.Parameters.Add("endDate", OdbcType.Date, 0).Value = end;

            List<FinMovement> output = new();
            conn.Open();

            using (var reader = await command.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    FinMovement move = new();
                    move.Id = reader.SafeGetInt(0);
                    move.Name = reader.SafeGetString(1);
                    move.Date = reader.SafeGetDate(2);
                    move.Value = reader.SafeGetDecimal(3);
                    move.Notes = reader.SafeGetString(4);
                    output.Add(move);
                }
            }
            conn.Close();
            return output;
        }

        public async Task<int> SaveChanges(IEnumerable<FinMovement> changedMovements, IEnumerable<FinMovement> deletedMovements)
        {
            int affectedRows = 0;
            conn.Open();

            foreach (FinMovement move in deletedMovements)
            {
                if (move.Id == null) continue;

                string query = @"DELETE FROM Movimentos WHERE ID = ? ";
                OdbcCommand command = new(query, conn);
                command.Parameters.Add("Id", OdbcType.Int, 0).Value = move.Id;

                affectedRows += await command.ExecuteNonQueryAsync();
            }

            foreach (FinMovement move in changedMovements)
            {
                if (move.Id == null)
                {
                    string query = @"INSERT INTO Movimentos (Nome, Data, `Valor Movimentado`, Notas) VALUES (?, ?, ?, ?) ";
                    OdbcCommand command = new(query, conn);
                    command.Parameters.Add("Name", OdbcType.VarChar, 140).Value = move.Name;
                    command.Parameters.Add("Date", OdbcType.Date, 0).Value = move.Date;
                    command.Parameters.Add("Value", OdbcType.Double, 0).Value = move.Value;
                    command.Parameters.Add("Notes", OdbcType.VarChar, 0).Value = move.Notes;

                    affectedRows += await command.ExecuteNonQueryAsync();

                    string getNewIdQuery = @"SELECT @@IDENTITY ";
                    OdbcCommand command2 = new(getNewIdQuery, conn);
                    int newId = Convert.ToInt32(await command2.ExecuteScalarAsync());

                    move.Id = newId;
                }
                else
                {
                    string query = @"UPDATE Movimentos SET Nome = ?, Data = ?, `Valor Movimentado` = ?, Notas = ? WHERE ID = ?";
                    OdbcCommand command = new(query, conn);
                    command.Parameters.Add("Name", OdbcType.VarChar, 140).Value = move.Name;
                    command.Parameters.Add("Date", OdbcType.Date, 0).Value = move.Date;
                    command.Parameters.Add("Value", OdbcType.Double, 0).Value = move.Value;
                    command.Parameters.Add("Notes", OdbcType.VarChar, 0).Value = move.Notes;
                    command.Parameters.Add("Id", OdbcType.Int, 0).Value = move.Id;

                    affectedRows += await command.ExecuteNonQueryAsync();
                }
            }
            conn.Close();

            return affectedRows;
        }

        public void Dispose()
        {
            if (isConnectionOwner)
                conn.Dispose();
        }
    }
}
