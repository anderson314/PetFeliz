using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetFelizApi.Models
{
    public class InformacoesServicoDogWalker
    {
        public int Id { get; set; }
        public Usuario DogWalker { get; set; }
        public int DogWalkerId { get; set; }
        
        [Column (TypeName = "decimal(5, 2)")]
        [Required]
        public decimal AvaliacaoMedia { get; set; }
        public string Sobre { get; set; }
        public string Preferencias { get; set; }

        [Column (TypeName = "decimal(4,2)")]
        public decimal ValorServico { get; set; }
        public Boolean AceitaCartao { get; set; }
        public List<Curso> Cursos { get;set; }
        
    }
}