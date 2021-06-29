using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetFelizApi.Data;
using PetFelizApi.Models;
using System.Collections.Generic;
using PetFelizApi.Models.Enuns;

namespace PetFelizApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CaoServicoController : ControllerBase
    {

        [HttpGet("{idServico}")]
        public async Task<ActionResult> buscarCaesServico(int idServico)
        {

            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(idU => idU.Id == PegarIdUsuarioToken());

            List<CaoServico> caesServico = await _context.CaesServico
                .Where(idS => idS.ServicoId == idServico)
                .Include(cao => cao.Cao)
                    .ThenInclude(p => p.Peso)
                .ToListAsync();

            return Ok(caesServico);
        }
        
        [HttpPost("AssociarCaoServico/{idCao}")]
        //Deverá somente mandar os ids dos cães nas solicitações
        public async Task<IActionResult> AdicionarCaoServico(int idCao)
        {
            CaoServico novoCaoServico = new CaoServico();

            Usuario Proprietario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            //Buscar o cão passado no JSON
            Cao cao = await _context.Cao.Include(prop => prop.Proprietario)
                .FirstOrDefaultAsync(cao => cao.Id == idCao);

            //Pegar o id do proprietário responsável pelo cão
            int idProprietario = cao.Proprietario.Id;
            
            if(Proprietario.TipoConta != TipoConta.Proprietario)
            {
                return BadRequest("Este usuário não tem permissão para esta ação.");
            }

            if(Proprietario.Id != idProprietario)
            {
                return BadRequest("O cão não pertence a " + Proprietario.Nome);
            }

            //Pegar o último serviço solicitado pelo Proprietário, para associar o cão a este serviço
            Servico servico = await _context.Servico
                .Include(usua => usua.Usuarios)
                .Where(id => id.ProprietarioId == PegarIdUsuarioToken())
                .OrderBy(it => it.Id)
                .LastAsync();


            //O servico a qual o cão está sendo associado será o serviço buscado acima
            novoCaoServico.Servico = servico;
            novoCaoServico.Cao = cao;

            await _context.CaesServico.AddAsync(novoCaoServico);
            await _context.SaveChangesAsync();

            return Ok("Cão adicionado ao servico com sucesso.");
        }


        //Retornará o Id do usuário logado
        private int PegarIdUsuarioToken()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }
        

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CaoServicoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}