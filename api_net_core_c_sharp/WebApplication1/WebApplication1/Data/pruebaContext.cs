using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Data
{
    public partial class pruebaContext : DbContext
    {
        public pruebaContext(DbContextOptions<pruebaContext> options) : base(options)
        {
        }
        
        public virtual DbSet<MarcasAutos> marcasAutos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Se agregan propiedades adicionales de los campos de la tabla
            //al crearse el modelo
            //también pudo hacerse en MarcasAutos.cs en la definición de la entity
            //campo IdMarca primaryKey
            //campo Marca required
            modelBuilder.Entity<MarcasAutos>(entity =>
            {
                entity.HasKey(e => e.IdMarca);
                entity.Property(e => e.IdMarca)
                    .HasColumnName("IdMarca")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Marca)
                .HasColumnName("Marca")
                .IsRequired();
            });

            //Implementación del mecanismo de Data Seed para cargar la tabla con al menos
            //tres ejemplos de marcas de autos.

            modelBuilder.Entity<MarcasAutos>().HasData(
            new MarcasAutos { IdMarca = 1,Marca = "Toyota" },
            new MarcasAutos { IdMarca = 2, Marca = "Ford" },
            new MarcasAutos { IdMarca = 3, Marca = "Honda" }
        );

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
