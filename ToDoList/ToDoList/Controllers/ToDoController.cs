using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;
      public ToDoController(ToDoContext context)
        {
            this.context = context;
        }
        //GET/
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.ToDoList orderby i.Id select i;
            List<TodoList> todoList = await items.ToListAsync();
            return View(todoList);
        }
        // GET/ TODO/EKLE
        public IActionResult Create() => View();
        
        // POST/TODO/CREATE
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "Görev eklendi!";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET/todo/edit
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST/TODO/Edit/2
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();
                TempData["Success"] = "Görev güncellendi!";
                return RedirectToAction("Index");
            }
            return View(item);
        }
        //GET/todo/sil
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "Görev mevcut değil!";
            }
            else
            {
                context.ToDoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "Görev silindi!";
            }

            return RedirectToAction("Index");
        }
    }
}
