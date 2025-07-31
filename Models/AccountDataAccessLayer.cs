using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using MvcAdoDemo.Models;

namespace MvcAdoDemo.Data
{
    public class AccountDataAccessLayer
    {
        private readonly string connectionString = "Server=localhost\\SQLEXPRESS01;Database=EmployeeDB2;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

        // Lấy tất cả tài khoản
        public IEnumerable<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllAccounts", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        accounts.Add(new Account
                        {
                            UserId = Convert.ToInt32(rdr["UserId"]),
                            Username = rdr["Username"].ToString(),
                            Password = rdr["Password"].ToString(),
                            Role = rdr["Role"].ToString(),
                            StudentId = rdr["StudentId"].ToString(),
                            Name = rdr["Name"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            City = rdr["City"].ToString()
                        });
                    }
                }
            }
            return accounts;
        }

        // Thêm tài khoản
        public void AddAccount(Account account)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@Role", account.Role);
                cmd.Parameters.AddWithValue("@StudentId", account.StudentId);
                cmd.Parameters.AddWithValue("@Name", account.Name);
                cmd.Parameters.AddWithValue("@Gender", account.Gender);
                cmd.Parameters.AddWithValue("@City", account.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Cập nhật tài khoản
        public void UpdateAccount(Account account)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spUpdateAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", account.UserId);
                cmd.Parameters.AddWithValue("@Username", account.Username);
                cmd.Parameters.AddWithValue("@Password", account.Password);
                cmd.Parameters.AddWithValue("@Role", account.Role);
                cmd.Parameters.AddWithValue("@StudentId", account.StudentId);
                cmd.Parameters.AddWithValue("@Name", account.Name);
                cmd.Parameters.AddWithValue("@Gender", account.Gender);
                cmd.Parameters.AddWithValue("@City", account.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Lấy 1 tài khoản theo ID
        public Account GetAccountData(int? id)
        {
            if (id == null) return null;

            Account account = null;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sql = "SELECT * FROM Account WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        account = new Account
                        {
                            UserId = Convert.ToInt32(rdr["UserId"]),
                            Username = rdr["Username"].ToString(),
                            Password = rdr["Password"].ToString(),
                            Role = rdr["Role"].ToString(),
                            StudentId = rdr["StudentId"].ToString(),
                            Name = rdr["Name"].ToString(),
                            Gender = rdr["Gender"].ToString(),
                            City = rdr["City"].ToString()
                        };
                    }
                }
            }
            return account;
        }

        // Xóa tài khoản
        public void DeleteAccount(int? id)
        {
            if (id == null) return;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteAccount", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
