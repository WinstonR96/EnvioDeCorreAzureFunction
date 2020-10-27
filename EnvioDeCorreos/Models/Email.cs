using System.Collections.Generic;

namespace EnvioDeCorreos.Models
{
    public class Email
    {
        public Credenciales credenciales { get; set; }
        public List<string> destinatarios { get; set; }
        public List<string> cc { get; set; }
        public string asunto { get;set; }
        public List<string> RutaAdjunto { get; set; }
        public string cuerpo { get; set; }
    }
}
