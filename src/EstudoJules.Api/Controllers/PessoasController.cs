using EstudoJules.Application.Interfaces; // IPessoaAppService e DTOs
using Microsoft.AspNetCore.Mvc;

namespace EstudoJules.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PessoasController : ControllerBase
{
    private readonly IPessoaAppService _pessoaAppService;

    public PessoasController(IPessoaAppService pessoaAppService)
    {
        _pessoaAppService = pessoaAppService ?? throw new ArgumentNullException(nameof(pessoaAppService));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarPessoa([FromBody] CriarPessoaDto criarPessoaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pessoaDto = await _pessoaAppService.AdicionarAsync(criarPessoaDto);

        if (pessoaDto == null)
        {
            // Isso pode acontecer se houver um erro de validação nos ValueObjects (Email, Cpf)
            // ou outro problema na lógica de AdicionarAsync que retorne null.
            return BadRequest("Não foi possível criar a pessoa. Verifique os dados fornecidos.");
        }

        return CreatedAtAction(nameof(ObterPessoaPorId), new { id = pessoaDto.Id }, pessoaDto);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPessoaPorId(Guid id)
    {
        var pessoaDto = await _pessoaAppService.ObterPorIdAsync(id);
        if (pessoaDto == null)
        {
            return NotFound();
        }
        return Ok(pessoaDto);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObterTodasPessoas()
    {
        var pessoasDto = await _pessoaAppService.ObterTodosAsync();
        return Ok(pessoasDto);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AtualizarPessoa(Guid id, [FromBody] AtualizarPessoaDto atualizarPessoaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var pessoaDto = await _pessoaAppService.AtualizarAsync(id, atualizarPessoaDto);

        if (pessoaDto == null)
        {
            // Pode ser que a pessoa não foi encontrada, ou houve um erro de validação.
            // O serviço AppService deveria idealmente distinguir esses casos.
            // Por agora, assumimos que null pode significar "não encontrado" ou "bad request" dependendo do contexto.
            // Vamos verificar se a pessoa existe primeiro para retornar 404 se for o caso.
            var existe = await _pessoaAppService.ObterPorIdAsync(id);
            if (existe == null)
            {
                return NotFound($"Pessoa com ID {id} não encontrada.");
            }
            // Se existe mas mesmo assim retornou null, foi um erro de validação nos dados.
            return BadRequest("Não foi possível atualizar a pessoa. Verifique os dados fornecidos.");
        }

        return Ok(pessoaDto);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverPessoa(Guid id)
    {
        var sucesso = await _pessoaAppService.RemoverAsync(id);
        if (!sucesso)
        {
            return NotFound();
        }
        return NoContent();
    }
}
