using Domain.Entities.CarritoCompra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ShoppingCart
{
    public class CartSessionDetail
    {

        public int CartSessionDetailId { get; set; }
        public DateTime? CreationDate { get; set; }
        public required string SelectedProduct { get; set; }
        public int CartSessionId { get; set; }
        public CartSession? CartSession { get; set; }
    }
}
