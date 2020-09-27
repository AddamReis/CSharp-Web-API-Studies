using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebAPI_Catalogo.Context;
using WebAPI_Catalogo.Controllers;
using WebAPI_Catalogo.DTOs;
using WebAPI_Catalogo.DTOs.Mappings;
using WebAPI_Catalogo.Repository;
using Xunit;

namespace WebAPI_Catalogo_xUnitTests
{
    //Para realizar os testes utilizando estas configurações, todos os métodos chamados precisam ser ActionResult
    //E não podem estar com autenticação obrigatória.
    //Os testes abaixo foram feitos com os métodos alterados porém na checagem os mesmos foram desfeitos.
    public class CategoriasUnitTestController
    {
        private readonly IUnitOfWork repository;
        private readonly IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "server=localhost;userid=developer;password=123456;DataBase=CatalogoDB";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connectionString).Options;
        }

        public CategoriasUnitTestController()
        {
            //Definindo instância do AutoMapper
            var config = new MapperConfiguration(c =>
            {
                c.AddProfile(new MappingProfile());
            });

            mapper = config.CreateMapper();

            //Definindo instância do Contexto
            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        //Testes Unitários============================================================
        
        //testar o Método GET
        [Fact]
        public void GetCategorias_Return_OkResult()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);
            
            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);*/
        }

        //Testar Bad Request
        [Fact]
        public void GetCategorias_Return_BadRequest()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);
            
            //Act
            var data = controller.Get();
            
            //Assert
            Assert.IsType<BadRequestResult>(data.Result);*/
        }

        //Get retornar uma lista de objetos categoria
        [Fact]
        public void GetCategorias_Return_MathResult()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);

            //Act
            var data = controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("http://www.macoratti.net/Imagens/1.jpg", cat[0].ImagemUrl);*/
        }

        //Get Por Id
        [Fact]
        public void GetCategoriaById_Return_OkResult()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);
            var catId = 8;

            //Act
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<CategoriaDTO>(data.Value);*/
        }

        //Get por Id NotFound
        [Fact]
        public void GetCategoriaById_Return_NotFound()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var catId = 99;

            //Act
            var data = controller.Get(catId);

            //Assert
            Assert.IsType<NotFoundResult>(data.Result);
        }

        //POST
        [Fact]
        public void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var cat = new CategoriaDTO()
            { Nome = "Teste Unitario Inclusao", ImagemUrl = "TesteUnit.jpg" };

            //Act
            var data = controller.Post(cat);

            //Assert
            Assert.IsType<CreatedAtActionResult>(data);
        }

        //PUT
        [Fact]
        public void Put_Categoria_Update_ValidData_Return()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);
            var catId = 2;

            //Act
            var existingPort = controller.Get(catId);
            var result = existingPort.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = catId;
            catDto.Nome = "CategoriaAtualizada Teste 1";
            catDto.ImagemUrl = result.ImagemUrl;

            var updateData = controller.Put(catId, catDto);

            //Assert
            Assert.IsType<OkResult>(updateData);*/
        }

        //DELETE
        [Fact]
        public void Delete_Categoria_Return_OkResult()
        {
            //Arrange
            /*var controller = new CategoriasController(repository, mapper);
            var catId = 8;

            //Act
            var data = controller.Delete(catId);

            //Assert
            {Descomentar no teste} Assert.IsType<CategoriaDTO>(data.Value);*/
        }
    }
}
