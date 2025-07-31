using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MvcAdoDemo.Models;
using MvcAdoDemo.Data;

namespace MvcAdoDemo.Controllers
{
    public class AccountController : Controller
    {
        AccountDataAccessLayer objemployee = new AccountDataAccessLayer();

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("role") != "admin")
                return RedirectToAction("Index", "Account");

            List<Account> lstEmployee = objemployee.GetAllAccounts().ToList();
            return View(lstEmployee);
        }

        public IActionResult Info()
        {
            string username = HttpContext.Session.GetString("username");
            // TODO: Lấy thông tin người dùng theo username nếu cần
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Account account)
        {
            if (ModelState.IsValid)
            {
                objemployee.AddAccount(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }
        [HttpPost]
        public IActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                objemployee.UpdateAccount(account); // hoặc tên khác bạn dùng để update
                return RedirectToAction("Index");
            }

            return View(account);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind] Account account)
        {
            if (id != account.UserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                objemployee.UpdateAccount(account);
                return RedirectToAction("Index");
            }
            return View(account);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Account account = objemployee.GetAccountData(id);

            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Account account = objemployee.GetAccountData(id);

            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            objemployee.DeleteAccount(id);
            return RedirectToAction("Index");
        }
    }
}
