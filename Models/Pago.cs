using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace appejemplo2.Models
{ [Table("t_pago")]
    public class Pago
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("Id")]
        public int Id { get; set; }
        public DateTime PaymentDate { get; set; }
        public String NombreTarjeta { get; set; }
        public String NumeroTarjeta { get; set; }
        [NotMapped]
        public String DueDateYYMM { get; set; }
        [NotMapped]
        public String Cvv { get; set; }
        public Decimal MontoTotal{ get; set; }
        public String UserID{ get; set; }
  
    }

}