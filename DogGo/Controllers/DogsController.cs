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
    public class DogsController : Controller
    {

        private readonly IDogRepository _dogRepo;
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give us an instance of our Owner Repository. This is called "Dependency Injection"
        public DogsController(IDogRepository dogRepository, IOwnerRepository ownerRepository)
        {
            _dogRepo = dogRepository;
            _ownerRepo = ownerRepository;
        }
        // GET: DogsController1
        public IActionResult Index()
        {
            // GET: Dogs

            List<Dog> dogs = _dogRepo.GetAllDogs();

            return View(dogs);
        }

        // GET: DogsController1/Details/5
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogsController1/Create
        public ActionResult Create()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = new Dog(),
                Owners = owners
            };
            return View(vm);
        }

        // POST: Dogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Dog dog)
        {
            try
            {
                _dogRepo.AddDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Edit/5
        public ActionResult Edit(int id)
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();
            DogFormViewModel vm = new DogFormViewModel()
            {
                Dog = _dogRepo.GetDogById(id),
                Owners = owners
            };
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: Dogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                _dogRepo.UpdateDog(dog);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }

        // GET: DogsController/Delete/5
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            return View(dog);
        }

        // POST: Dogs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                _dogRepo.DeleteDog(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(dog);
            }
        }
    }
}
