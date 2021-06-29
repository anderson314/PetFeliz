using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PetFelizApi.Models.Enuns;

namespace PetFelizApi.Models
{
    public class Usuario
    {
       public int Id                { get; set; }
       public TipoConta TipoConta { get; set; }
       public string Nome           { get; set; }
       public DateTime DataNascimento { get; set; }
       public DateTime DataCadastro { get; set; }
       public string FotoPerfil          { get; set; }
       public string Email              { get; set; }
       public string WhatsApp   { get; set; }
       public List<Cao> Caes        { get; set; }
       public Boolean Disponivel { get; set; }
       public List<UsuariosServico> Servicos { get; set; }
       public InformacoesServicoDogWalker ServicoDogWalker { get; set; }
       public Double Latitude { get; set; }
       public Double Longitude { get; set; }
       public byte[] PasswordHash { get; set; }
       public byte[] PasswordSalt { get; set; }
       [NotMapped]
       public string PasswordString { get; set; }
       public List<UsuarioAvaliacao> UsuarioAvaliacao { get; set; }

    }
}