using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ap.EntityFramework
{
    public class IdentityBaseMapper<T> : EntityTypeConfiguration<T> where T : IdentityBase
    {
        public IdentityBaseMapper()
        {
            HasKey(i => i.Id);
            Property(i => i.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(i => i.UniqueIdentifier)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }
    }
}
