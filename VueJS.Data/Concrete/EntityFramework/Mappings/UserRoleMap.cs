using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VueJS.Entities.Concrete;

namespace VueJS.Data.Concrete.EntityFramework.Mappings
{
    public class UserRoleMap : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.RoleId });
            builder.ToTable("UserRoles");

            builder.HasData(
                new UserRole
                {
                    UserId = 1,
                    RoleId = 1
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 2
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 3
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 4
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 5
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 6
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 7
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 8
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 9
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 10
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 11
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 12
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 13
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 14
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 15
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 16
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 17
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 18
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 19
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 20
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 21
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 22
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 23
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 24
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 25
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 26
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 27
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 28
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 29
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 30
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 31
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 32
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 33
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 34
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 35
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 36
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 37
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 38
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 39
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 40
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 41
                },
                new UserRole
                {
                    UserId = 1,
                    RoleId = 42
                });
        }
    }
}