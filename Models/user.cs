using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace bank.Models
{
    public class User
    {
        [Key]
        public int User_id { get; set; }
        
        [Display(Name="First Name:")]
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Display(Name="Username")]
        [MinLength(3)]
        public string UserName {get; set;}
        
        [Display(Name="Last Name:")]
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        
        [Display(Name="Email Address:")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Display(Name="Password:")]
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        
        public int Wallet  {get; set;} = 1000;

        public List<Function> AllPosts{get; set;}
        
        
        public List<Key> Bid { get; set; }
        
        public User()
        {
            Bid = new List<Key>();
            AllPosts = new List<Function>();
        }
        
    }
}