using Domain.Entities.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.CarritoCompra
{
    public class CartSession
    {
        public int CartSessionId { get; set; }
        public DateTime? CreationDate { get; set; }
        public required virtual ICollection<CartSessionDetail> DetailList { get; set; }

    }
}
