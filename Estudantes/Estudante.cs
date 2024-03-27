namespace ApiCrud.Estudantes
{
    public class Estudante
    {
        public Guid Id { get; init; }
        public string Nome { get; private set; }
        public bool Ativo {  get; private set; }

        public Estudante(String nome) 
        {
            Nome = nome;
            Id = Guid.NewGuid();
            Ativo = true;
        }
        public void AtualizarNome(String nome)
        {
            Nome = nome;
        }
        public void Desativar()
        {
            Ativo = false;
        }
    }
}
