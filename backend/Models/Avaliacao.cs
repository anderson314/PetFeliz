using System;
using System.Collections.Generic;

namespace PetFelizApi.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public DateTime DataAvaliacao { get; set; }
        public int ProprietarioId { get; set; }
        public List<UsuarioAvaliacao> UsuarioAvaliacao { get; set; }

    }
}