using CMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CMS.Infrastructure.Persistence.Configurations;

public class DoctorConfig : IEntityTypeConfiguration<Doctor>
{
    public void Configure(EntityTypeBuilder<Doctor> builder)
    {
        builder.ToTable("Doctors");


        // Removed HasBaseType configuration to use default TPC/TPT or just mapped superclass behavior implicitly.
        // BaseEntity is not in DbSet, so EF Core treats it as mapped superclass by default for its properties.

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Specialization)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(x => x.User)
            .WithOne()
            .HasForeignKey<Doctor>(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
            // Cascade delete on User might delete Doctor. Restrict is safer if we want manual deletion.
            // But usually Cascade delete from User -> Doctor makes sense if User is deleted.
            // Let's stick with Restrict or NoAction to be safe unless specified otherwise, but cascade is more typical for ownership.
            // However, User entity doesn't have a navigation to Doctor. So this is unidirectional.
            
        builder.HasMany(x => x.Appointments)
            .WithOne(x => x.Doctor)
            .HasForeignKey(x => x.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
