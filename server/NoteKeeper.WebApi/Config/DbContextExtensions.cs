using NoteKeeper.Dominio.Compartilhado;
using NoteKeeper.Infra.Orm.Compartilhado;

namespace NoteKeeper.WebApi.Config
{
    public static class DbContextExtensions
    {
        public static bool AutoMigrateDataBase(this IApplicationBuilder applicationBuilder)
        {
            {
                using var scope = applicationBuilder.ApplicationServices.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<IContextoPersistencia>();

                bool migracaoConcluida = false;

                if (dbContext is NoteKeeperDbContext noteKeeperDbContext)
                {
                   migracaoConcluida =  MigradorBancoDados.AtualizarBancoDados(noteKeeperDbContext);
                }

                return migracaoConcluida;

            }
        }
    }
}
