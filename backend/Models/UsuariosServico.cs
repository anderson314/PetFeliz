namespace PetFelizApi.Models
{
    public class UsuariosServico
    {
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public int ServicoId { get; set; }
        public Servico Servico { get; set; }
    }
}