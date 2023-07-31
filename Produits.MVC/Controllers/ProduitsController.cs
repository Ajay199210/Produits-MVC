using Microsoft.AspNetCore.Mvc;
using Produits.MVC.Models;

namespace Produits.MVC.Controllers
{
    public class ProduitsController : Controller
    {
        private GestionProduits gestionProduits = new GestionProduits();

        // GET: ProduitsController
        public async Task<ActionResult> Index(string filtre)
        {
            var stock = await gestionProduits.ObtenirTout();

            if (!string.IsNullOrEmpty(filtre))
            {
                return View(gestionProduits.FiltrerProduitsSelonNom(filtre));
            }

            return View(stock);
        }

        //GET: ProduitsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProduitsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Produit produit)
        {
            if (ModelState.IsValid)
            {
                await gestionProduits.Ajouter(produit);
                return RedirectToAction(nameof(Index));
            }

            return View(produit);
        }

        // GET: ProduitsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            await gestionProduits.ObtenirTout();
            //Produit? produit = stock.SingleOrDefault(p => p.Id == id);
            Produit? produit = gestionProduits.ObtenirProduitSelonId(id);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: ProduitsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Produit produit)
        {
            if (ModelState.IsValid)
            {
                await gestionProduits.Modifier(produit);
                return RedirectToAction(nameof(Index));
            }
            return View(produit);
        }

        // GET: ProduitsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var stock = await gestionProduits.ObtenirTout();
            //Produit? produit = stock.SingleOrDefault(p => p.Id == id);
            Produit? produit = gestionProduits.ObtenirProduitSelonId(id);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: ProduitsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Produit produit)
        {
            await gestionProduits.Supprimer(produit);
            return RedirectToAction(nameof(Index));
        }
    }
}