using Microsoft.EntityFrameworkCore;

namespace NoteKeeper.Infra.Orm.Compartilhado;

public static class MigradorBancoDados
{
    public static bool AtualizarBancoDados(DbContext dbContext)
    {
      var migracoesPendentes =  dbContext.Database.GetPendingMigrations().Count();

      if (migracoesPendentes == 0)
      {
          Console.WriteLine("Nenhuma migracao pendente, continando...");
          return false;
      }
      
      Console.WriteLine("Aplicando migracoes pedentes, aguarde um instante...");
      dbContext.Database.Migrate();
      return true;
    }
}