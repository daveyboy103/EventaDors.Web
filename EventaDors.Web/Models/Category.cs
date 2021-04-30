using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rocky.Models
{
    public class Category
    {
        public Category()
        {
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Order of Display")]
        public int DisplayOrder { get; set; }
    }
}
