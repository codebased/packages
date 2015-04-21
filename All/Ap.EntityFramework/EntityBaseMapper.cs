namespace Ap.EntityFramework
{
    public class EntityBaseMapper<T> :
         IdentityBaseMapper<T> where T : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the EntityBaseMapper class.
        /// </summary>
        public EntityBaseMapper()
        {
            Property(i => i.DateModified).HasColumnType("smalldatetime")
                .IsRequired();

            Property(i => i.DateCreated).HasColumnType("smalldatetime")
                .IsRequired();
        }
    }
}
