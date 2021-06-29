using System;
using System.ComponentModel.DataAnnotations.Schema;
using PetFelizApi.Models.Enuns;

namespace PetFelizApi.Models
{
    public class Cao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Raca { get; set; }
        public DateTime? DataNascimento { get; set; }
        public PorteCao Porte { get; set; }
        public PesoCao Peso { get; set; }
        public int PesoId { get; set; }
        public Usuario Proprietario { get; set; }
    }
}