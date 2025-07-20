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
    public class EmployeeMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasKey(e => e.Id); // Define a chave prmária
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder
                  .Property(e => e.NomeFantasia)
                  .HasColumnName("NomeFantasia")
                  .HasMaxLength(100)
                  .IsRequired(true);

            builder
                .Property(e => e.CNPJ)
                .HasColumnName("CNPJ")
                .HasMaxLength(20)
                .IsRequired(true);


            builder.ToTable("Fornecedor");
        }
    }
}
