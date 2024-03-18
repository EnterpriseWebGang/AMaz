using Microsoft.AspNetCore.Mvc;
using AMaz.Service;
using AMaz.Common;
using AutoMapper;

namespace AMaz.Web.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public AccountController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        // GET: AccountController
        public ActionResult Index()
        {
            return View();
        }

        //// GET: AccountController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var request = _mapper.Map<CreateAccountRequest>(model);
                var result = await _userService.CreateAccount(request);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                ViewBag.Error = result.ToString();
                return View();
            }

            ViewBag.Error = "Invalid Input!";
            return View();
        }

        //// GET: AccountController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AccountController/Edit/5
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

        //// GET: AccountController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AccountController/Delete/5
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
