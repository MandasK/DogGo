using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Models;
using DogGo.Models.ViewModels;
using DogGo.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DogGo.Controllers
{
    
    public class WalksController : Controller
    {
        private readonly IWalksRepository _walksRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;

        public WalksController(IWalksRepository walksRepository,
                               IDogRepository dogRepository,
                               IWalkerRepository walkerRepository)
        {
            _walksRepo = walksRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
        }
        // GET: WalksCOntroller
        public IActionResult Index()
        {
            List<Walks> walks = _walksRepo.GetAllWalks();
            return View(walks);
        }

        // GET: WalksCOntroller/Details/5
        public ActionResult Details(int id)
        {
            Walks walk = _walksRepo.GetWalkByIdDetails(id);
            return View(walk);
        }

        // GET: WalksCOntroller/Create
        public ActionResult Create()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();
            List<Dog> dogs = _dogRepo.GetAllDogs();
            WalkFormModel vm = new WalkFormModel()
            {
                Walks = new Walks(),
                Dogs = dogs,
                Walkers = walkers
            };
            return View(vm);
        }

        //// POST: WalksCOntroller/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Walks Walks)
        {
            try
            {
                _walksRepo.AddWalk(Walks);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                List<Walker> walkers = _walkerRepo.GetAllWalkers();
                List<Dog> dogs = _dogRepo.GetAllDogs();
                WalkFormModel vm = new WalkFormModel()
                {
                    Walks = new Walks(),
                    Dogs = dogs,
                    Walkers = walkers
                };
                return View(vm);
            }
        }

        // GET: WalksCOntroller/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //// POST: WalksCOntroller/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: WalksCOntroller/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksCOntroller/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
