using Microsoft.Data.SqlClient;
using Dapper;
public static class BD
{
    private static string _connectionString = @"Server=localhost; DataBase=Recordatorios; Integrated Security=True; TrustServerCertificate=True;";

    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(_connectionString);
    }
    public static List<Categoria> ObtenerCategorias()
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = "SELECT * FROM Categoria";
            var categorias = connection.Query<Categoria>(query).ToList();
            return categorias;
        }
    }
    public static List<Dificultad> ObtenerDificultades()
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = "SELECT * FROM Dificultad";
            var dificultades = connection.Query<Dificultad>(query).ToList();
            return dificultades;
        }
    }
    public static List<Pregunta> ObtenerPreguntas(int dificultad, int categoria)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            List<Pregunta> preguntas;

            if (dificultad == -1 && categoria == -1)
            {
                var query = "SELECT * FROM Pregunta";
                preguntas = connection.Query<Pregunta>(query).ToList();
            }
            else if (dificultad == -1)
            {
                var query = "SELECT * FROM Pregunta WHERE IDCategoria = @categoria";
                preguntas = connection.Query<Pregunta>(query, new { categoria }).ToList();
            }
            else if (categoria == -1)
            {
                var query = "SELECT * FROM Pregunta WHERE IDDificultad = @dificultad";
                preguntas = connection.Query<Pregunta>(query, new { dificultad }).ToList();
            }
            else
            {
                var query = "SELECT * FROM Pregunta WHERE IDDificultad = @dificultad AND IDCategoria = @categoria";
                preguntas = connection.Query<Pregunta>(query, new { dificultad, categoria }).ToList();
            }

            return preguntas;
        }
    }
    public static List<Respuesta> ObtenerRespuestas(int pregunta)
    {
        using (SqlConnection connection = ObtenerConexion())
        {
            var query = "SELECT * FROM Respuesta WHERE IDPregunta = @pregunta";
            var respuestas = connection.Query<Respuesta>(query).ToList();
            return respuestas;
        }
    }
}