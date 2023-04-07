using Microsoft.VisualBasic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Lucas_Gaspard_projet_mvc.Data.Model
{
    //Add-Migration "commentaire"
    //Update-Database
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255, ErrorMessage = "Limite de caractère dépassé.")]
        public string Titre { get; set; }
        public string Fabricant { get; set; }
        public int Prix { get; set; }
        public string Info { get; set; }

        //public enum Type
        //{
        //    [Description("Carrosserie")]
        //    Carrosserie,
        //    [Description("Peinture")]
        //    Peinture,
        //    [Description("Moteur")]
        //    Moteur
        //}
        public string Type { get; set; }
        //private readonly List<string> _type = new() { "Carrosserie", "Peinture", "Moteur" };
        //public string Type
        //{
        //    get { return _type.Value; }
        //    set { _type.Value = value; }
        //}
    }
}
