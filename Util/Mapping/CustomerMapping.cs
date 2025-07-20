using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Models;

namespace Util.Mapping
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(e => e.Id); // Define a chave prmária
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder
                  .Property(e => e.Nome)
                  .HasColumnName("Nome")
                  .HasMaxLength(100)
                  .IsRequired(true);

            builder
                .Property(e => e.CPF)
                .HasColumnName("CPF")
                .HasMaxLength(20)
                .IsRequired(true);

            builder.ToTable("Cliente");
        }
    }
}
