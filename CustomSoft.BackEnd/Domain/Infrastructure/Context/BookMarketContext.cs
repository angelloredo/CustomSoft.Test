using Domain.Entities.Book;
using Domain.Entities.CarritoCompra;
using Domain.Entities.ShoppingCart;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Infrastructure.Context
{
    public class BookMarketContext : DbContext
    {

        public BookMarketContext(DbContextOptions<BookMarketContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones del modelo
        }

        public DbSet<Book> Book { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<AcademicGrade> AcademicGrade { get; set; }
        public DbSet<CartSession> CartSession { get; set; }
        public DbSet<CartSessionDetail> CartSessionDetalle { get; set; }

      
    }
}
