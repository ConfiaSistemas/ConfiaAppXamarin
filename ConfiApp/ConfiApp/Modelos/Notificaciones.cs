using System;
using System.Collections.Generic;
using System.Text;

namespace ConfiApp.Modelos
{
    class Notificaciones
    {
        public int id { get; set; }
        public string Tipo { get; set; }
        public bool Notificacion { get; set; }
        public string Usuario { get; set; }
        public string UsuarioDestino { get; set; }
        public string Valor { get; set; }
        public string Mensaje { get; set; }
        public string Comentario { get; set; }
        public bool Aplicado { get; set; }
        public int idSesion { get; set; }
        public string Fecha { get; set; }
        public DateTime Hora { get; set; }
        public bool Visto { get; set; }
        public string FechaAplicacion { get; set; }
        public DateTime HoraAplicacion { get; set; }
        public string Empresa { get; set; }
        public string ComentarioUsuarioDestino { get; set; }
        public string Estado { get; set; }
    }
}
