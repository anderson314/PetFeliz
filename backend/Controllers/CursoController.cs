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
    public class CursoController : ControllerBase
    {
        
        [HttpPost("AdicionarCurso")]
        public async Task<IActionResult> adicionarCurso(Curso novoCurso)
        {
            Usuario usuario = await _context.Usuario
                .Include(i => i.ServicoDogWalker)
                .FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            novoCurso.InfoServDogW = usuario.ServicoDogWalker;

            await _context.Curso.AddAsync(novoCurso);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("ListarCursos/{idDogW}")]
        public async Task<IActionResult> listarCursos(int idDogW)
        {
            Usuario usuario = await _context.Usuario
                .FirstOrDefaultAsync(u => u.Id == PegarIdUsuarioToken());

            if (usuario.TipoConta == TipoConta.DogWalker)
            {
                List<Curso> cursos = await _context.Curso
                .Where(f => f.InfoServDogW.DogWalkerId == usuario.Id)
                .OrderByDescending(f => f.Id)
                .ToListAsync();

                return Ok(cursos);

            }
            else
            {
                List<Curso> cursos = await _context.Curso
                .Where(f => f.InfoServDogW.DogWalkerId == idDogW)
                .OrderByDescending(f => f.Id)
                .ToListAsync();

                return Ok(cursos);

            }

            

            
        }

        [HttpPut("AlterarCurso/{cursoId}")]
        public async Task<IActionResult> alterarCurso(int cursoId, Curso curso)
        {

            Curso _curso = await _context.Curso.FirstOrDefaultAsync(id => id.Id == cursoId);

            _curso.AnoConclusao = curso.AnoConclusao;
            _curso.Nome = curso.Nome;

            _context.Update(_curso);
            await _context.SaveChangesAsync();

            return Ok(_curso);
        }

        [HttpDelete("RemoverCurso/{cursoId}")]
        public async Task<IActionResult> removerCurso(int cursoId)
        {
            Curso curso = await _context.Curso.FirstOrDefaultAsync(id => id.Id == cursoId);

            _context.Remove(curso);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //Retornará o Id do usuário logado
        private int PegarIdUsuarioToken()
        {
            return int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
        }

        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CursoController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

    }
}