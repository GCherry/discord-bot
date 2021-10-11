using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tutorial.DataAccess.MSSQL.Context;
using Tutorial.DataAccess.MSSQL.Interfaces;
using Tutorial.Domain.Entities;

namespace Tutorial.DataAccess.MSSQL.Repositories
{
    public class ServerRepository : IServerRepository
    {
        private readonly TutorialDbContext _context;

        public ServerRepository(TutorialDbContext context)
        {
            _context = context;
        }

        public async Task ModifyGuildPrefix(ulong id, string prefix, string name)
        {
            var server = await _context.Servers
                .FindAsync(id);

            if (server == null)
            {
                _context.Add(new Server
                {
                    Id = id,
                    Prefix = prefix,
                    Name = name
                });
            }
            else
            {
                server.Prefix = prefix;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<string> GetGuildPrefix(ulong id)
        {
            var prefix = await _context.Servers
                .Where(x => x.Id == id)
                .Select(x => x.Prefix)
                .FirstOrDefaultAsync();

            return await Task.FromResult(prefix);
        }

    }
}
