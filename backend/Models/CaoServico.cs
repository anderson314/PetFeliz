namespace PetFelizApi.Models
{
    public class CaoServico
    {
        public int CaoId { get; set; }
        public Cao Cao { get; set; }
        public int ServicoId { get; set; }
        public Servico Servico { get; set; }
    }
}