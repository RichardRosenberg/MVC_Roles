using College_Management_System;
using College_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class StudentController : Controller
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly AppDbContext _context;

    public StudentController(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              AppDbContext context)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        _context = context;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(StudentRegister model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                StudentId = model.StudentId,
                Name = model.Name,
                Surname = model.Surname,
                Address = model.Address,
                PostalCode = model.PostalCode
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Student");

                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(StudentLogin model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Student");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Login", "Student");
    }

    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Index()
    {
        var studentSubjects = await _context.StudentSubjects.ToListAsync();
        return View(studentSubjects);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Code,Description,Status")] StudentSubject studentSubject)
    {
        if (ModelState.IsValid)
        {
            _context.Add(studentSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(studentSubject);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studentSubject = await _context.StudentSubjects.FindAsync(id);
        if (studentSubject == null)
        {
            return NotFound();
        }
        return View(studentSubject);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Description,Status")] StudentSubject studentSubject)
    {
        if (id != studentSubject.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(studentSubject); 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentSubjectExists(studentSubject.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        return View(studentSubject);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var studentSubject = await _context.StudentSubjects.FirstOrDefaultAsync(m => m.Id == id);
        if (studentSubject == null)
        {
            return NotFound();
        }

        return View(studentSubject);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var studentSubject = await _context.StudentSubjects.FindAsync(id);
        _context.StudentSubjects.Remove(studentSubject);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentSubjectExists(int id)
    {
        return _context.StudentSubjects.Any(e => e.Id == id);
    }
}
