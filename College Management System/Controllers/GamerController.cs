using College_Management_System.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    public class GamerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public GamerController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(GamerRegister model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    GamerId = model.GamerId,
                    Age = model.Age
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Gamer");
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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(GamerLogin model, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
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

        [Authorize(Roles = "Gamer")]
        [HttpGet]
        public ViewResult Index()
        {
            var board = new TicTacToeBoard();

            foreach (Cell cell in board.Cells)
            {
                cell.Mark = TempData[cell.Id]?.ToString()!;
            }
            board.CheckForWinner();

            var model = new TicTacToeViewModel
            {
                Cells = board.Cells,
                Selected = new Cell { Mark = TempData["nextTurn"]?.ToString() ?? "X" },
                IsGameOver = board.HasWinner || board.HasAllCellsSelected
            };

            if (model.IsGameOver)
            {
                TempData["nextTurn"] = "X";
                TempData["message"] = (board.HasWinner) ? $"{board.WinningMark} wins!" : "It's a tie!";
            }
            else
            {
                TempData.Keep();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult Index(TicTacToeViewModel vm)
        {
            TempData[vm.Selected.Id] = vm.Selected.Mark;
            TempData["nextTurn"] = (vm.Selected.Mark == "X") ? "O" : "X";
            return RedirectToAction("Index");
        }
    }
}
