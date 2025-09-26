using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public static class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=TP_08_ PreguntadOrt; Integrated Security=True; TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(_connectionString);
    }

    public static List<Categoria> ObtenerCategorias()
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = @"SELECT IdCategoria AS IDCategoria, Nombre FROM Categorias";
            var categorias = connection.Query<Categoria>(query).ToList();
            return categorias;
        }
    }

    public static List<Dificultad> ObtenerDificultades()
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = "SELECT IdDificultad AS IDDificultad, Nombre FROM Dificultades";
            var dificultades = connection.Query<Dificultad>(query).ToList();
            return dificultades;
        }
    }

    public static List<Pregunta> ObtenerPreguntas(int dificultad, int categoria)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            string baseQuery = @"SELECT IdPreguntas AS IDPregunta, Enunciado, IdCategoria AS IDCategoria, IdDificultad AS IDDificultad
                                 FROM Preguntas";

            if (dificultad == -1 && categoria == -1)
            {
                var preguntas = connection.Query<Pregunta>(baseQuery).ToList();
                return preguntas;
            }
            else if (dificultad == -1)
            {
                var query = baseQuery + " WHERE IdCategoria = @categoria";
                var preguntas = connection.Query<Pregunta>(query, new { categoria }).ToList();
                return preguntas;
            }
            else if (categoria == -1)
            {
                var query = baseQuery + " WHERE IdDificultad = @dificultad";
                var preguntas = connection.Query<Pregunta>(query, new { dificultad }).ToList();
                return preguntas;
            }
            else
            {
                var query = baseQuery + " WHERE IdDificultad = @dificultad AND IdCategoria = @categoria";
                var preguntas = connection.Query<Pregunta>(query, new { dificultad, categoria }).ToList();
                return preguntas;
            }
        }
    }

    public static List<Respuesta> ObtenerRespuestas(int pregunta)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = @"SELECT IdRespuestas AS ID, IdPregunta AS IdPregunta, Opcion, Contenido AS Texto, Correcta AS Correcta
                          FROM Respuestas
                          WHERE IdPregunta = @pregunta
                          ORDER BY Opcion";
            var respuestas = connection.Query<Respuesta>(query, new { pregunta }).ToList();
            return respuestas;
        }
    }

    public static void GuardarPuntaje(DateTime fechaHora, string username, int puntaje)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var insertSql = @"INSERT INTO HighScores (FechaHora, Username, Puntaje) VALUES (@fechaHora, @username, @puntaje)";
            connection.Execute(insertSql, new { fechaHora, username, puntaje });
        }
    }

    public static List<HighScore> ObtenerHighScores()
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var selectSql = @"SELECT TOP 20 Id AS ID, FechaHora, Username, Puntaje FROM HighScores ORDER BY Puntaje DESC, FechaHora ASC";
            var lista = connection.Query<HighScore>(selectSql).ToList();
            return lista;
        }
    }

    public static int ObtenerRankingPorPuntaje(int puntaje)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var sql = @"SELECT COUNT(*) FROM HighScores WHERE Puntaje > @puntaje";
            int mayores = connection.ExecuteScalar<int>(sql, new { puntaje });
            return mayores + 1;
        }
    }
}
