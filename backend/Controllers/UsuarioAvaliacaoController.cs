using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFelizApi.Data;
using PetFelizApi.Models;
using PetFelizApi.Models.Enuns;

namespace Pet_Feliz_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioAvaliacaoController : ControllerBase
    {
        [HttpPost("AssociarProprietario")]
        public async Task<ActionResult> associarProprietarioAvaliacao()
        {
            UsuarioAvaliacao usuarioAvaliacao = new UsuarioAvaliacao();

            //Busca o usuário que está fazendo a requisição
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(usu => usu.Id == PegarIdUsuarioToken());

            //Se o usuário que estives fazendo a requisição for um dog walker
            if (usuario.TipoConta == TipoConta.DogWalker)
            {
                return BadRequest("Esta requisição serve para o proprietário. Tente novamente, tendo um.");
            }

            //Busca a última avaliação feita
            Avaliacao avaliacao = await _context.Avaliacao
                .Where(prop => prop.ProprietarioId == usuario.Id)
                .OrderBy(order => order.Id)
                .LastAsync();

            //Se não houver avaliação alguma
            if (avaliacao == null)
            {
                return BadRequest("Nenhuma avaliação encontrada.");
            }

            usuarioAvaliacao.Usuario = usuario;
            usuarioAvaliacao.Avaliacao = avaliacao;

            await _context.UsuarioAvaliacao.AddAsync(usuarioAvaliacao);
            await _context.SaveChangesAsync();

            return Ok(usuarioAvaliacao);
        }

        [HttpPost("AssociarDogWalker/{idDogW}")]
        public async Task<ActionResult> associarDogWalkerAvaliacao(int idDogW)
        {
            UsuarioAvaliacao usuarioAvaliacao = new UsuarioAvaliacao();

            //Busca o usuário que está fazendo a requisição
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(usu => usu.Id == PegarIdUsuarioToken());

            //Se o usuário que estives fazendo a requisição for um dog walker
            if (usuario.TipoConta == TipoConta.DogWalker)
            {
                return BadRequest("Esta requisição serve para o proprietário. Tente novamente, tendo um.");
            }

            //Busca o dog walker que receberá a avaliação
            Usuario dogWalker = await _context.Usuario.FirstOrDefaultAsync(dogw => dogw.Id == idDogW);

            //Busca a última avaliação feita
            Avaliacao avaliacao = await _context.Avaliacao
                .Where(prop => prop.ProprietarioId == usuario.Id)
                .OrderBy(order => order.Id)
                .LastAsync();

            //Se não houver avaliação alguma
            if (avaliacao == null)
            {
                return BadRequest("Nenhuma avaliação encontrada.");
            }

            usuarioAvaliacao.Usuario = dogWalker;
            usuarioAvaliacao.Avaliacao = avaliacao;

            await _context.UsuarioAvaliacao.AddAsync(usuarioAvaliacao);
            await _context.SaveChangesAsync();

            return Ok(usuarioAvaliacao);
        }

        [HttpGet("VerificarAvaliacao/{idDogW}")]
        public async Task<IActionResult> verificarAvaliacoes(int idDogW)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(usu => usu.Id == PegarIdUsuarioToken());

            Usuario dogWalker = await _context.Usuario.FirstOrDefaultAsync(dogW => dogW.Id == idDogW);


            //Se caso for um proprietário solicitando
            if (usuario.TipoConta == TipoConta.Proprietario)
            {
                //Busca a avaliação de um proprietário para um dog walker em especifico
                UsuarioAvaliacao avaliacao = await _context.UsuarioAvaliacao
                .Where(prop => prop.Avaliacao.ProprietarioId == usuario.Id && prop.Usuario.Id == idDogW)
                .FirstOrDefaultAsync();

                //Se não há avaliação alguma, então o dog walker pode ser avaliado
                if (avaliacao == null)
                {
                    return Ok(true);
                }
                //caso contrário, ele não pode
                else
                {
                    return Ok(false);
                }
            }
            //Se caso for um dog walker solicitando
            else
            {
                //Busca a avaliação do dog walker logado
                UsuarioAvaliacao avaliacao = await _context.UsuarioAvaliacao
                .Where(dogW => dogW.Usuario.Id == idDogW)
                .FirstOrDefaultAsync();

                if (avaliacao == null)
                {  
                    return Ok(true);
                }
                else
                {
                    return Ok(false);
                }
            }
        }

        //Retorna o Id do usuário logado
        private int PegarIdUsuarioToken()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioAvaliacaoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

    }
}