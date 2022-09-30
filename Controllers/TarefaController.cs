using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using Microsoft.EntityFrameworkCore;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var tarefa = _context.Tarefas.FirstOrDefault(x => x.Id == id);

            if(tarefa == null)
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            var tarefas = _context.Tarefas.ToList();
            if(!tarefas.Any())
                return NotFound();

            return Ok(tarefas);
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            var tarefa = _context.Tarefas.Where(x => x.Titulo.Contains(titulo));

            if(!tarefa.Any())
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
            
            if(!tarefa.Any())
                return NotFound();

            return Ok(tarefa);
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            var tarefa = _context.Tarefas.Where(x => x.Status == status);

            if(!tarefa.Any())
                return NotFound();

            return Ok(tarefa);
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {

            try
            {
                if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
            
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();

                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception ex) 
            {                
                return BadRequest(new { Erro = ex.Message });
            }            
            
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {            
            try
            {
                var tarefaBD = _context.Tarefas.FirstOrDefault(x => x.Id == id);

                if (tarefaBD == null)
                    return NotFound();

                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

                tarefa.Id = tarefaBD.Id;

                _context.Entry(tarefaBD).State = EntityState.Detached;

                _context.Tarefas.Update(tarefa);
                _context.SaveChanges();
                
                return Ok();
            }
            catch (Exception ex)
            {
                
                return BadRequest(new { Erro = ex.Message});
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {        
            try
            {
                var tarefaBD = _context.Tarefas.FirstOrDefault(x => x.Id == id);

                if (tarefaBD == null)
                    return NotFound();

                _context.Entry(tarefaBD).State = EntityState.Detached;
                _context.Tarefas.Remove(tarefaBD);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                
                return BadRequest(new {Erro = ex.Message});
            }
        }
    }
}
