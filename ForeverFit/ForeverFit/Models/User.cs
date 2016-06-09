﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ForeverFit.Models
{
    public class User
    {

        [Required]
        [Display(Name = "Id")]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }


        [Required]
        [Display(Name = "Genero")]
        public int Genero { get; set; }

        [Required]
        [Display(Name = "Peso")]
        public float Weight { get; set; }


        [Required]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Frecuencia cardiaca máxima")]
        public float MaxHeartRate { get; set; }

        [Required]
        [Display(Name = "Frecuencia cardiaca en reposo")]
        public float RestingHeartRate { get; set; }


        public User IsValid(string _username, string _password)
        {
            using (var cn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeverFitDB"].ConnectionString))


            {
                string _sql = @"SELECT * FROM [dbo].[User] " +
                       @"WHERE [Username] = @u AND [Password] = @p";
                var cmd = new SqlCommand(_sql, cn);
                cmd.Parameters
                    .Add(new SqlParameter("@u", SqlDbType.NVarChar))
                    .Value = _username;
                cmd.Parameters
                    .Add(new SqlParameter("@p", SqlDbType.NVarChar))
                    .Value = Helpers.SHA1.Encode(_password);
                cn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    var u = mappingUser(reader); 
                    reader.Dispose();
                    cmd.Dispose();
                    return u;
                }
                else
                {
                    reader.Dispose();
                    cmd.Dispose();
                    return null;
                }
            }
        }

        private User mappingUser(SqlDataReader reader)
        {
            User u = new User();
            while (reader.Read())
            {      
                u.Id = (int) reader["Id"];
                u.UserName = reader["UserName"].ToString();
                u.Password = reader["Password"].ToString();
                u.Genero = (bool)reader["Genero"] ? 1 : 0;
                u.BirthDate = (DateTime)reader["BirthDate"];
                u.MaxHeartRate = (float)(double) reader["MaxHeartRate"];
                u.RestingHeartRate = (float)(double) reader["RestingHeartRate"];
                u.Weight = (float)(double) reader["Weight"];
            }
            return u;
        }
    }
}