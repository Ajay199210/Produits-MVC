namespace Produits.MVC.Models
{
    public class GestionProduits
    {
        private List<Produit> _produits;
        private string _cheminFichier = "produits.csv";
        public GestionProduits()
        {
            _produits = new List<Produit>();
        }

        // Ajouter un nouveau produit
        public async Task Ajouter(Produit produit)
        {
            await ObtenirTout();

            // Génération du nouveau ID
            int id = 1;
            if (_produits.Any())
            {
                id = _produits.Max(p => p.Id) + 1;
            }
            produit.Id = id;

            if(produit.EstVedette)
            {
                _produits.ForEach(p => p.EstVedette = false);
            }

            await EnregistrerProduit(produit);
            await EnregistrerTout();
        }

        // Supprimer un produit
        public async Task Supprimer(Produit produit)
        {
            await ObtenirTout();

            Produit? produitASupprimer = ObtenirProduitSelonId(produit.Id);
            if (produitASupprimer != null)
            {
                _produits.Remove(produitASupprimer);
                await EnregistrerTout();
            }
        }

        // Modifier un produit
        public async Task Modifier(Produit produit)
        {
            await ObtenirTout();

            Produit? produitAModifier = ObtenirProduitSelonId(produit.Id);
            if (produitAModifier != null)
            {
                produitAModifier.Nom = produit.Nom;
                produitAModifier.Description = produit.Description;
                produitAModifier.Prix = produit.Prix;
                produitAModifier.Quantite = produit.Quantite;
                produitAModifier.Image = produit.Image;
                produitAModifier.EstVedette = produit.EstVedette;
                
                // Si le produit modifié est en vedette, on doit déactiver le statut de vedette pour l'autre produit
                if (produitAModifier.EstVedette)
                {
                    _produits.FindAll(p => p.Id != produitAModifier.Id).ForEach(p => p.EstVedette = false);
                }
                
                await EnregistrerTout();
            }
        }

        // Obtenir un produit selon son Id
        public Produit? ObtenirProduitSelonId(int id)
        { 
            return _produits.SingleOrDefault(p => p.Id == id);
        }

        // Filtrer les produits
        public List<Produit> FiltrerProduitsSelonNom(string filtre)
        {
            List<Produit> produitsRecherches = _produits.FindAll(p => p.Nom.ToLower().Contains(filtre.ToLower().Trim()));
            
            // Si le produit vedette n'est pas inclus dans la recherche, on doit l'ajouter...
            // ...afin que la page d'accueil l'affiche correctement au début de la page d'accueil
            if(produitsRecherches.All(p => !p.EstVedette))
            {
                Produit produitEnVedette = _produits.Single(p => p.EstVedette);
                produitsRecherches.Add(produitEnVedette);
            }

            return produitsRecherches;
        }

        // Lire le fichier de produits
        private async Task LireFichier()
        {
            using (StreamReader sr = new StreamReader(_cheminFichier))
            {
                sr.ReadLine(); // file headers
                string ligne;
                while ((ligne = await sr.ReadLineAsync()) != null)
                {
                    string[] champs = ligne.Split(";");
                    Produit produit = new Produit();
                    produit.Id = int.Parse(champs[0]);
                    produit.Nom = champs[1];
                    produit.Description = champs[2];
                    produit.Prix = int.Parse(champs[3]);
                    produit.Quantite = int.Parse(champs[4]);
                    produit.Image = champs[5];
                    produit.EstVedette = bool.Parse(champs[6]);
                    _produits.Add(produit);
                }
            }
        }

        // Obtenir tous les produits
        public async Task<List<Produit>> ObtenirTout()
        {
            await LireFichier();
            return _produits;
        }

        // Enregistrer un produit
        public async Task EnregistrerProduit(Produit produit)
        {
            _produits.Add(produit);
            using (StreamWriter sw = new StreamWriter(_cheminFichier, true))
            {
                await sw.WriteLineAsync(produit.Id + ";" + produit.Nom + ";" + produit.Description + ";" + produit.Prix + ";" + produit.Quantite + ";" + produit.Image + ";" + produit.EstVedette);
            }
        }

        // Enregistrer tous les produits
        public async Task EnregistrerTout()
        {
            using (StreamWriter sw = new StreamWriter(_cheminFichier))
            {
                sw.WriteLine("Id;Nom;Description;Prix;Quantité;Image;En Vedette");
                foreach (Produit produit in _produits)
                {
                    await sw.WriteLineAsync(produit.Id + ";" + produit.Nom + ";" + produit.Description + ";" + produit.Prix + ";" + produit.Quantite + ";" + produit.Image + ";" + produit.EstVedette);
                }
            }
        }
    }
}
