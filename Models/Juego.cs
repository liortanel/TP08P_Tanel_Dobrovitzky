using System.Collections.Generic;

public static class Juego
{
    public static string Username { get; set; }
    public static int PuntajeActual { get; set; }
    public static int CantidadPreguntasCorrectas { get; set; }
    public static int ContadorNroPreguntaActual { get; set; }
    public static Pregunta PreguntaActual { get; set; }
    public static List<Pregunta> ListaPreguntas { get; set; } = new List<Pregunta>();
    public static List<Respuesta> ListaRespuestas { get; set; } = new List<Respuesta>();

    public static void InicializarJuego()
    {
        PuntajeActual = 0;
        CantidadPreguntasCorrectas = 0;
        ContadorNroPreguntaActual = 0;
        PreguntaActual = null;
        ListaPreguntas.Clear();
        ListaRespuestas.Clear();
    }

    public static void CargarPartida(string username, int dificultad, int categoria)
    {
        InicializarJuego();
        Username = username;
        ListaPreguntas = BD.ObtenerPreguntas(dificultad, categoria);
        if (ListaPreguntas != null && ListaPreguntas.Count > 0)
        {
            PreguntaActual = ListaPreguntas[0];
        }
    }

    public static Pregunta ObtenerProximaPregunta()
    {
        if (ListaPreguntas != null && ContadorNroPreguntaActual < ListaPreguntas.Count)
        {
            return ListaPreguntas[ContadorNroPreguntaActual];
        }
        return null;
    }

    public static List<Respuesta> ObtenerProximasRespuestas(int idPregunta)
    {
        ListaRespuestas = BD.ObtenerRespuestas(idPregunta);
        return ListaRespuestas;
    }

    public static bool VerificarRespuesta(int idRespuesta)
    {
        bool esCorrecta = false;
        foreach (var respuesta in ListaRespuestas)
        {
            if (respuesta.ID == idRespuesta)
            {
                esCorrecta = respuesta.Correcta;
                break;
            }
        }
        if (esCorrecta)
        {
            PuntajeActual += 10;
            CantidadPreguntasCorrectas++;
        }
        ContadorNroPreguntaActual++;
        if (ContadorNroPreguntaActual < ListaPreguntas.Count)
        {
            PreguntaActual = ListaPreguntas[ContadorNroPreguntaActual];
        }
        else
        {
            PreguntaActual = null;
        }
        return esCorrecta;
    }
}
