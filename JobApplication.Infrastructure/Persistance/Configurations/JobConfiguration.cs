using JobApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplication.Infrastructure.Persistance.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(j => j.IsActive)
                .IsRequired();

            builder.Property(j => j.CreatedAt)
                .IsRequired();

            builder.Property(j => j.UpdatedAt)
                .IsRequired();

            builder.HasMany(j => j.Applications)
    .WithOne(a => a.Job)
    .HasForeignKey(a => a.JobId)
    .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Recruiter)
    .WithMany()
    .HasForeignKey(x => x.RecruiterId)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}