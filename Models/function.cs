using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace bank.Models
{
    public class Function
    {
        [Key]
        public int Functionid {get; set;}
        [Required]
        [MinLength(3)]
        public string Product {get; set;}
        [Required]
        [MinLength(10)]
        public string Description {get; set;}
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date {get; set;}
        [Required]
        public int StartBid{get; set;}

        public int Userid {get; set;}

        public User user {get; set;} 
        public List<Key> bid { get; set; }
        public Function() {
            bid = new List<Key>();
        }
    }
}