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

namespace PetFelizApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ServicoController : ControllerBase
    {

        //Nome : Solicitar servico
        //OBS: O Id do proprietário terá de ser mandado na requisição
        //O proprietário deverá ser adicionado primeiro
        [HttpPost("Solicitar")]
        public async Task<IActionResult> solicitarServico(Servico novoServico)
        {

            Usuario Proprietario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            //Verifica se o usuário que está fazendo a solicitação é um Dog Walker
            if (Proprietario.TipoConta == TipoConta.DogWalker)
                return BadRequest("O Dog Walker não pode fazer uma solicitação de serviço");

            //Guarda o Id do Proprietário que está fazendo a solicitação
            novoServico.ProprietarioId = PegarIdUsuarioToken();

            DateTime dataAtual = DateTime.Today;
            //Adicionará duas horas pois o servidor está duas horas adiantadas
            DateTime horaAtual = DateTime.Now.AddHours(2);
            
            //Atribuir a data e hora acima à requisição o JSON
            novoServico.DataSolicitacao = dataAtual.ToString("dd/MM/yyyy");
            novoServico.HoraSolicitacao = horaAtual.ToString("HH:mm");

            //O estado do serviço será marcado automaticamente como solicitado
            novoServico.Estado = EstadoSolicitacao.Solicitado;

            await _context.Servico.AddAsync(novoServico);
            await _context.SaveChangesAsync();

            return Ok(novoServico);
        }

        // Nome : Listar serviços gerais
        // Atores : Proprietário e Dog Walker
        // OBS : nenhuma
        [HttpGet("ListarServicosGerais")]
        public async Task<IActionResult> listarServicosGerais()
        {
            
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            if(usuario.TipoConta == TipoConta.Proprietario)
            {
                List<UsuariosServico> servicosGerais = await _context.UsuariosServico
                .Where(usu => usu.Usuario == usuario && usu.Servico.Estado != EstadoSolicitacao.Finalizado)
                //Ordem em que os serviços aparecerão
                .OrderByDescending(e => e.Servico.Estado == EstadoSolicitacao.Solicitado)
                    .ThenByDescending(e => e.Servico.Estado == EstadoSolicitacao.EmAndamento)
                    .ThenByDescending(e => e.Servico.Estado == EstadoSolicitacao.Aceito)
                    .ThenByDescending(e => e.Servico.Estado == EstadoSolicitacao.Recusado)
                    .ThenByDescending(i => i.Servico.Id)
                .Include("Servico.Usuarios.Usuario.ServicoDogWalker")
                .ToListAsync();


                return Ok(servicosGerais);
            }
            else
            {
               
                List<UsuariosServico> servicosGerais = await _context.UsuariosServico
                .Where(usu => usu.Usuario == usuario && usu.Servico.Estado != EstadoSolicitacao.Finalizado && usu.Servico.Estado != EstadoSolicitacao.Solicitado)
                //Ordem em que os serviços aparecerão
                .OrderByDescending(e => e.Servico.Estado == EstadoSolicitacao.Aceito)
                    .ThenByDescending(e => e.Servico.Estado == EstadoSolicitacao.EmAndamento)
                    .ThenByDescending(e => e.Servico.Estado == EstadoSolicitacao.Recusado)
                    .ThenByDescending(i => i.Servico.Id)
                .Include("Servico.Usuarios.Usuario")
                .ToListAsync();

                return Ok(servicosGerais);
            }

        }

        // Nome : Listar serviços finalizados
        // Atores : Proprietário e Dog Walker
        // OBS : nenhuma
        [HttpGet("ListarServicosFinalizados")]
        public async Task<IActionResult> listarServicosFinalizados()
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            List<UsuariosServico> servicosFinalizados = await _context.UsuariosServico
            .Where(usu => usu.Usuario == usuario && usu.Servico.Estado == EstadoSolicitacao.Finalizado)
            .OrderByDescending(dt => dt.Servico.Id)
            .Include("Servico.Usuarios.Usuario.ServicoDogWalker")
            .ToListAsync();

            return Ok(servicosFinalizados);
        }

        // Nome : Listar serviços Solicitados
        // Atores : Dog Walker
        // OBS : nenhuma
        [HttpGet("ListarServicosSolicitados")]
        public async Task<IActionResult> listarServicosSolicitados()
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            if(usuario.TipoConta == TipoConta.DogWalker)
            {
                List<UsuariosServico> servicosSolicitados = await _context.UsuariosServico
                .Where(usu => usu.Usuario == usuario && usu.Servico.Estado == EstadoSolicitacao.Solicitado)
                .OrderByDescending(dt => dt.Servico.Id)
                .Include("Servico.Caes.Cao")
                .Include(s => s.Servico)
                .ThenInclude(usu => usu.Usuarios)
                .ThenInclude(u => u.Usuario)
                .ToListAsync();

                return Ok(servicosSolicitados);
            }
            else 
                return BadRequest("Esta ação é somente permitida para um Dog Walker.");

        }

        
        //Nome : Cancelar Servico
        //Atore : proprietário
        //OBS: O id DEVE vir pela url
        [HttpPut("Cancelar/{id}")]
        public async Task<IActionResult> cancelarServico(int id)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            //Buscar serviço pelo id do path
            int idServico = id;
            Servico servico = await _context.Servico.FirstOrDefaultAsync(id => id.Id == idServico);

                //Verificar se, o serviço que está a ser cencelado, está em estado de aceito ou solicitado
                if(servico.Estado == EstadoSolicitacao.Aceito || servico.Estado == EstadoSolicitacao.Solicitado)
                {
                     servico.Estado = EstadoSolicitacao.Cancelado;
                    
                    _context.Servico.Update(servico);
                    await _context.SaveChangesAsync();

                    List<UsuariosServico> servicosGerais = await _context.UsuariosServico
                    .Where(usu => usu.Usuario == usuario && usu.Servico.Estado != EstadoSolicitacao.Finalizado)
                    .OrderByDescending(dt => dt.Servico.Id)
                    .Include(s => s.Servico)
                    .ThenInclude(usu => usu.Usuarios)
                    .ThenInclude(u => u.Usuario)
                    .ThenInclude(sd => sd.ServicoDogWalker)
                    .ToListAsync();

                    return Ok(servicosGerais);
                }
                else
                    return BadRequest("O serviço não pode ser cancelado");
            
            
            
        }
        
        

        // Nome : Iniciar Servico
        // Atore : Proprietário
        // OBS : O id DEVE vir pela url
        [HttpPut("Iniciar/{id}")]
        public async Task<IActionResult> iniciarServico(int id)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            int idServico = id;
            Servico servico = await _context.Servico.FirstOrDefaultAsync(id => id.Id == idServico);

            if(usuario.TipoConta == TipoConta.Proprietario)
            {
                //Verifica se o estado do serviço não está como aceito
                if(servico.Estado != EstadoSolicitacao.Aceito)
                {
                     return BadRequest("Impossível iniciar o serviço. O serviço deve estar em estado de Aceito.");
                }
                else
                {
                    //Atribuir hora do inicio do servico
                    //Adiciona 2 horas, pois o servidor está duas horas adiantadas
                    DateTime horaAtual = DateTime.Now.AddHours(2);
                    servico.HoraInicio = horaAtual.ToString("HH:mm");

                    servico.Estado = EstadoSolicitacao.EmAndamento;

                    _context.Servico.Update(servico);
                    await _context.SaveChangesAsync();

                    return Ok(servico);
                }
            }
            else
                return BadRequest("Este usuário não tem permissão para iniciar este serviço");
            
        }


        // Nome : Finalizar Servico
        // Atore : Proprietário
        // OBS : O id DEVE vir pela url
        [HttpPut("Finalizar/{id}")]
        public async Task<IActionResult> finalizarServico(int id)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            int idServico = id;
            Servico servico = await _context.Servico.FirstOrDefaultAsync(id => id.Id == idServico);

            if(usuario.TipoConta == TipoConta.Proprietario)
            {
                if(servico.Estado != EstadoSolicitacao.EmAndamento)
                {
                    return BadRequest("Não é possível finalizar o serviço, ele deve ter sido iniciado, antes");
                }
                else
                {
                    //Atribuir hora do fim do servico
                    //Adiciona 2 horas, pois o servidor está duas horas adiantadas
                    DateTime horaAtual = DateTime.Now.AddHours(2);
                    servico.HoraTermino = horaAtual.ToString("HH:mm");

                    servico.Estado = EstadoSolicitacao.Finalizado;

                    _context.Servico.Update(servico);
                    await _context.SaveChangesAsync();

                    return Ok(servico);
                }
            }
            else
                return BadRequest("Este usuário não tem permissão para finalizar este serviço.");
        }

        // Nome : Aceitar Servico
        // Atore : Dog Walker
        // OBS : O id DEVE vir pela url
        [HttpPut("Aceitar/{id}")]
        public async Task<IActionResult> aceitarServico(int id)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            int idServico = id;
            Servico servico = await _context.Servico.FirstOrDefaultAsync(id => id.Id == idServico);
            
            if(usuario.TipoConta == TipoConta.DogWalker)
            {
                if(servico.Estado != EstadoSolicitacao.Solicitado)
                    return BadRequest("Não foi possível iniciar o serviço.");
                else
                {
                    servico.Estado = EstadoSolicitacao.Aceito;

                    _context.Servico.Update(servico);
                    await _context.SaveChangesAsync();

                    return Ok(servico);
                }
            }
            else
                return BadRequest("Este usuário não tem permissão para aceitar este serviço.");

        }

        // Nome : Recusar Servico
        // Atore : Dog Walker
        // OBS : O id DEVE vir pela url
        [HttpPut("Recusar/{id}")]
        public async Task<IActionResult> recusarServico(int id)
        {
            Usuario usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            int idServico = id;
            Servico servico = await _context.Servico.FirstOrDefaultAsync(id => id.Id == idServico);
            
            if(usuario.TipoConta == TipoConta.DogWalker)
            {
                if(servico.Estado != EstadoSolicitacao.Solicitado)
                    return BadRequest("Não foi possível recusar o serviço.");
                else
                {
                    servico.Estado = EstadoSolicitacao.Recusado;

                    _context.Servico.Update(servico);
                    await _context.SaveChangesAsync();

                    return Ok(servico);
                }
            }
            else
                return BadRequest("Este usuário não tem permissão para recusar o serviço.");

            
        }
        

        //Retornará o Id do usuário logado
        private int PegarIdUsuarioToken()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }


        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ServicoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

    }
}