using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tutorial.Domain.Entities;

namespace Tutorial.DataAccess.MSSQL.EntityConfigurations
{
    public class ServerEntityConfiguration : IEntityTypeConfiguration<Server>
    {
        public void Configure(EntityTypeBuilder<Server> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Server");
        }
    }
}
