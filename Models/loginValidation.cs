using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace bank.Models
{
    public class loginVal
    {
        [Key]
        public int Userid { get; set; }

        [Display(Name="UserName:")]
        [Required]
        
        public string UserNameLogin { get; set; }
        
        [Display(Name="Password:")]
        [Required]
        
        
        public string passwordLogin { get; set; }
    }
}