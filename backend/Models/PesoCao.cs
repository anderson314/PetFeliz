using System.Collections.Generic;

namespace PetFelizApi.Models
{
    public class PesoCao
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public List<Cao> Caes { get; set; }
    }
}