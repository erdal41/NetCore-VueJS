using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UserTokenMap : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(ut => new { ut.UserId, ut.LoginProvider, ut.Name });
            builder.Property(ut => ut.LoginProvider).HasMaxLength(256);
            builder.Property(ut => ut.Name).HasMaxLength(256);
            builder.ToTable("UserTokens");
        }
    }
}