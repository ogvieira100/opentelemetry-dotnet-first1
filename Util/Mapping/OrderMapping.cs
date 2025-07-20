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
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(e => e.Id); // Define a chave prmária
            builder.Property(e => e.Id).ValueGeneratedOnAdd();

            builder
                  .Property(e => e.Description)
                  .HasColumnName("Descricao")
                  .HasMaxLength(100)
                  .IsRequired(true);

            builder
                .HasOne(e => e.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CustomerId);

            builder
                .HasOne(e => e.Employee)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.EmployeeId);


            builder.ToTable("Pedido");
        }
    }
}
