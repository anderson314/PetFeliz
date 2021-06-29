using System;
using System.Collections.Generic;

namespace PetFelizApi.Models
{
    public class UsuarioAvaliacao
    {
        public int AvaliacaoId { get; set; }
        public Avaliacao Avaliacao { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}