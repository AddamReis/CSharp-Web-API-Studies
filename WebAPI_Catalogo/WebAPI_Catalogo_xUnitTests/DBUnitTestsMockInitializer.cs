using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Models;

namespace WebAPI_Catalogo_xUnitTests
{
   public class DBUnitTestsMockInitializer
    {
        public DBUnitTestsMockInitializer(){ }

        public void Seed(AppDbContext context)
        {
            context.Categorias.Add
                (new Categoria { CategoriaId = 999, Nome = "Bebidas999", ImagemUrl = "imagem999.jpg"});

            context.Categorias.Add
                (new Categoria { CategoriaId = 998, Nome = "Bebidas998", ImagemUrl = "imagem998.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 997, Nome = "Bebidas997", ImagemUrl = "imagem997.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 996, Nome = "Bebidas996", ImagemUrl = "imagem996.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 995, Nome = "Bebidas995", ImagemUrl = "imagem995.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 994, Nome = "Bebidas994", ImagemUrl = "imagem994.jpg" });

            context.Categorias.Add
                (new Categoria { CategoriaId = 993, Nome = "Bebidas993", ImagemUrl = "imagem993.jpg" });
        }
    }
}
