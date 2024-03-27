using College_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class StaffController : Controller
{
	private readonly UserManager<User> userManager;
	private readonly SignInManager<User> signInManager;
	private readonly AppDbContext _context;

	public StaffController(UserManager<User> userManager,
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
	public async Task<IActionResult> Register(StaffRegister model)
	{
		if (ModelState.IsValid)
		{
			var user = new User
			{
				UserName = model.Email,
				Email = model.Email,
				StaffId = model.StaffId,
				Name = model.Name,
				Surname = model.Surname,
				PhoneNumber = model.PhoneNumber
			};

			var result = await userManager.CreateAsync(user, model.Password);

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "Staff");

				await signInManager.SignInAsync(user, isPersistent: false);
				return RedirectToAction("Index");
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
    public async Task<IActionResult> Login(StaffLogin model, string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Staff");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }


    [HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Logout()
	{
		await signInManager.SignOutAsync();
		return RedirectToAction("Login");
	}

	[Authorize(Roles = "Staff")]
	public async Task<IActionResult> Index()
    {
        var subjects = await _context.Subjects.ToListAsync();
        return View(subjects);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Code,Description,Location,Status")] Subject subjectObj)
    {
        if (ModelState.IsValid)
        {
            _context.Add(subjectObj);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(subjectObj);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subjectObj = await _context.Subjects.FindAsync(id);
        if (subjectObj == null)
        {
            return NotFound();
        }
        return View(subjectObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Code,Description,Location,Status")] Subject subjectObj)
    {
        if (id != subjectObj.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(subjectObj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(subjectObj.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(subjectObj);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var subjectObj = await _context.Subjects.FirstOrDefaultAsync(m => m.Id == id);
        if (subjectObj == null)
        {
            return NotFound();
        }

        return View(subjectObj);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var subjectObj = await _context.Subjects.FindAsync(id);
        _context.Subjects.Remove(subjectObj);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SubjectExists(int id)
    {
        return _context.Subjects.Any(e => e.Id == id);
    }
}
