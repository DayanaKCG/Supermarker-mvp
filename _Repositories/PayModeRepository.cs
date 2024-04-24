using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Data;
//using Supermarker_mvp.Model;
using System.Data;
using Supermarker_mvp.Models;
using Microsoft.Data.SqlClient;

namespace Supermarker_mvp._Repositories
{
    internal class PayModeRepository : BaseRepository, IPayModeRepository

    {public PayModeRepository(string connetionString)
        {
            this.connectionString = connetionString;
        }
        public void Add(PayModeModel payModeModel)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var Command = new SqlCommand())
            {
                connection.Open();
                Command.Connection = connection;
                Command.CommandText = "INSERT INTO PayMode VALUES (@name, @observation)";
                Command.Parameters.Add("@name", SqlDbType.NVarChar).Value = payModeModel.Name;
                Command.Parameters.Add("@observation", SqlDbType.NVarChar).Value = payModeModel.Observation;
                Command.ExecuteNonQuery();

            }
        }

        public void Delete(int id)
        {
            using (var connection= new SqlConnection(connectionString))
            using (var Command = new SqlCommand())
            {
                connection.Open();
                Command.Connection = connection;
                Command.CommandText = "DELETE FROM PayMode WHERE Pay_Mode_Id = @id";
                Command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                Command.ExecuteNonQuery();
            }
        }


        public void Edit(PayModeModel payModeModel)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var Command = new SqlCommand())
            {
                connection.Open();
                Command.Connection = connection;
                Command.CommandText = @"UPDATE PayMode
                                        SET Pay_Mode_Name =@name,
                                        Pay_Mode_Observation = @observation
                                        WHERE Pay_Mode_Id = @id";
                Command.Parameters.Add("@name", SqlDbType.NVarChar).Value= payModeModel.Name;
                Command.Parameters.Add("@observation",SqlDbType.NVarChar).Value = payModeModel.Observation;
                Command.Parameters.Add("@id", SqlDbType.Int).Value = payModeModel.Id;
                Command.ExecuteNonQuery();

            }
        }

        public IEnumerable<PayModeModel> Get(string value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PayModeModel> GetAll()
        {
            var payModeList =new List<PayModeModel>();
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM PayMode ORDER BY Pay_Mode_Id DESC";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var payModeModel = new PayModeModel();
                        payModeModel.Id = (int)reader["Pay_Mode_Id"];
                        payModeModel.Name = reader["Pay_Mode_Name"].ToString();
                        payModeModel.Observation = reader["Pay_Mode_Observation"].ToString();
                        payModeList.Add(payModeModel);

                    }
                }
            }
            return payModeList;
        }

        public IEnumerable<PayModeModel> GetByValue(string Value)
        {
           var payModeList = new List<PayModeModel>();
           int payModeId = int.TryParse(Value, out _) ? Convert.ToInt32(Value) : 0;
           string payModeName = Value;
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"SELECT * FROM PayMode
                WHERE Pay_Mode_Id=@id or Pay_Mode_Name LIKE @name+ '%'
                ORDER By Pay_Mode_Id DESC";
                command.Parameters.Add("@id", SqlDbType.Int).Value = payModeId;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = payModeName;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var payModeModel = new PayModeModel();
                        payModeModel.Id = (int)reader["Pay_Mode_Id"];
                        payModeModel.Name = reader["Pay_Mode_Name"].ToString();
                        payModeModel.Observation = reader["Pay_Mode_Observation"].ToString();
                        payModeList.Add(payModeModel);
                    }
                }
            }
            return payModeList;
        }

    }
}
