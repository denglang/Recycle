using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recycle.Models
{
    [Table("RECYCLINGDIRECTORY_WEBSITE")]
    public class RecycleSites
    {
        [Key]
      public int ObjectID { get; set; }
      //public object Shape { get; set; }
     
        [Required]
      public  string Member_Name{ get; set;}
        [Required]
        public  string Contact_Email{ get; set;}
        [Required]
        public  string Phone_Number{ get; set;}
      public  string Website{ get; set;}
      public  string Street{ get; set;}
      public  string City{ get; set;}
      public int? Zip_Code{ get; set;}
      public string County{ get; set;}
      public string State{ get; set;}
      public string Status_1{ get; set;}
      public string Profile_Hours{ get; set;}
      public string Construction_Demolition{ get; set;}
      public string Hazardous_Waste{ get; set;}
      public string Organics{ get; set;}
      public string Solid_Waste{ get; set;}
      public string Address_Website{ get; set;}
      public string Electronics{ get; set;}
    }
}