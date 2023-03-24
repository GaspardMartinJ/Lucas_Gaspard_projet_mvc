using Microsoft.VisualBasic;

namespace Lucas_Gaspard_projet_mvc.Data.Model
{
    //Add-Migration "commentaire"
    //Update-Database
    public class Product
    {
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Fabricant { get; set; }
        public int Prix { get; set; }
        public string Info { get; set; }
        public string Type { get; set; }
    }
}
