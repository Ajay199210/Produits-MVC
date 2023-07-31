using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Produits.MVC.Models
{
    public class Produit
    {
        public int Id { get; set; } // Identifiant auto-incrémenté

        [Required(ErrorMessage = "Le nom est obligatoire")]
        [MaxLength(100)]
        [RegularExpression(@"^[^+/*<>^?={}\'""\[\]\\!@#$%\(\)_]+$", ErrorMessage = "Le nom peut contenir des lettres, chiffres, espaces et les tirets")]
        public string? Nom { get; set; }

        [Required(ErrorMessage = "La description est obligatoire")]
        [MaxLength(250)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Le prix est obligatoire")]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Le prix doit être un entier positif")]
        public int Prix { get; set; }

        [Required(ErrorMessage = "La quantité est obligatoire")]
        [Range(1, 150, ErrorMessage = "La quantité doit être comprise entre 0 et 150")]
        [RegularExpression(@"^[^.]+$", ErrorMessage = "La quantité doit être un entier")]
        [DisplayName("Quantité")]
        public int Quantite { get; set; }

        [Required(ErrorMessage = "Le nom de l'image est obligatoire")]
        [RegularExpression(@"^.+\.(png|jpg)$", ErrorMessage = "Seulement les extensions .png et .jpg sont acceptées")]
        public string? Image { get; set; }

        public bool EstVedette { get; set; } // Permettre de définir le produit comme produit vedette
    }
}
