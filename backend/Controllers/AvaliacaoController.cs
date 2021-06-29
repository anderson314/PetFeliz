using System;
using System.Collections.Generic;
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
    public class AvaliacaoController : ControllerBase
    {  

        //Enviar somente a nota,comentário e a data no JSON
        [HttpPost("AvaliarServico")]
        public async Task<IActionResult> cadastrarAvaliacao(Avaliacao avaliacao)
        {
            //Busca o usuário que está fazendo a requisição
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(prop => prop.Id == PegarIdUsuarioToken());

            //Busca o último serviço finalizado
            Servico servico = await _context.Servico
                //Busca o serviço que está finalizado e que seja do usuário que esteja fazendo a requisição
                .Where(estado => estado.Estado == EstadoSolicitacao.Finalizado && estado.ProprietarioId == usuario.Id)
                .OrderByDescending(idS => idS.Id)
                .FirstAsync();

            //Caso não houver serviço algum finalizado
            if (servico == null)
            {
                return BadRequest("Este proprietário não possui serviços finalizados");
            }

            //Se quem estiver fazendo a requisição for um Dog Walker
            if (usuario.TipoConta == TipoConta.DogWalker)
            {
                return BadRequest("Dog Walkers não podem avaliar serviços.");
            }

            // DateTime dataAtual = DateTime.Today;

            // avaliacao.DataAvaliacao = dataAtual;
            avaliacao.ProprietarioId = usuario.Id;

            await _context.Avaliacao.AddAsync(avaliacao);
            await _context.SaveChangesAsync();

            return Ok(avaliacao);
        }
        
        [HttpGet("ListarAvaliacoes/{idDogW}")]
        public async Task<IActionResult> listarAvaliacoes(int idDogW)
        {
            //Garante que as solicitações 
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(usu => usu.Id == PegarIdUsuarioToken());

            //Busca as avaliações do dog walker
            List<UsuarioAvaliacao> avaliacoes = await _context.UsuarioAvaliacao
                .Where(avali => avali.Usuario.Id == idDogW)
                .Include(dogw => dogw.Usuario)
                .Include(usus => usus.Avaliacao.UsuarioAvaliacao)
                .ThenInclude(usu => usu.Usuario)
                .OrderBy(idAval => idAval.AvaliacaoId)
                .ToListAsync();

            if (avaliacoes == null)
            {
                return BadRequest("Este dog walker não possui avaliações.");
            }

            return Ok(avaliacoes);
        }

        private int PegarIdUsuarioToken()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AvaliacaoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

    }
}