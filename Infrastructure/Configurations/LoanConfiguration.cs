using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => l.Id);
            builder.Property(l => l.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");
            builder.Property(l => l.Status)
                   .IsRequired();
            builder.Property(l => l.CreatedAt)
                   .IsRequired();

            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(l => l.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
