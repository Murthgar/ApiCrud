using ApiCrud.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiCrud.Estudantes
{
    public static class EstudantesRotas
    {
        public static void AddRotasEstudantes(this WebApplication app) 
        {
            var rotasEstudantes = app.MapGroup("estudantes");
            //Para criar se usa o post
            rotasEstudantes.MapPost("", async (AddEstudanteRequest request, AppDbContext context, CancellationToken ct) =>
            {
                var jaExiste = await context.Estudantes.AnyAsync(estudante => estudante.Nome == request.Nome);
                if (jaExiste) { return Results.Conflict("Ja existe"); }
                var novoEstudante = new Estudante(request.Nome);
                await context.Estudantes.AddAsync(novoEstudante);
                await context.SaveChangesAsync();
                var estudanteRetorno = new EstudanteDto(novoEstudante.Id, novoEstudante.Nome);
                return Results.Ok(estudanteRetorno);
            });

            //Retornar todos os estudantes cadastrados
            rotasEstudantes.MapGet("", async (AppDbContext context, CancellationToken ct) =>
            {
                var estudantes = await context.Estudantes.Where(estudante => estudante.Ativo).Select(estudante => new EstudanteDto(estudante.Id,estudante.Nome)).ToListAsync();
                return estudantes;
            });

            //Atualizar nome estudante
            rotasEstudantes.MapPut("{id:guid}", async (Guid id, UpdateEstudanteRequest request, AppDbContext context, CancellationToken ct) => 
            {
                var estudante = await context.Estudantes//Dbset<Estudante>
                .SingleOrDefaultAsync(estudante => estudante.Id == id);//Task<Estudante>
                if (estudante == null)
                    return Results.NotFound();
                estudante.AtualizarNome(request.Nome);
                await context.SaveChangesAsync();
                return Results.Ok(new EstudanteDto(estudante.Id,estudante.Nome));
            });

            //Deletar
            rotasEstudantes.MapDelete("{id}", async (Guid id, AppDbContext context, CancellationToken ct) =>
            {
                var estudante = await context.Estudantes.SingleOrDefaultAsync(estudante => estudante.Id == id);
                if (estudante == null)
                    return Results.NotFound();
                estudante.Desativar();
                await context.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
