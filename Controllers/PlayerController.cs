using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlayerClub.Infrastructure;
using PlayerClub.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Controllers
{
    public class PlayerController : Controller
    {
        private PlayerClubContext context;

        public PlayerController(PlayerClubContext context)
        {
            this.context = context;
        }

        //GET /
        public async Task<ActionResult> Index()
        {
            IQueryable<Player> players = from p in context.Player.Include(p => p.Team) orderby p.Id select p;

            List<Player> playerList = await players.ToListAsync();

            return View(playerList);
        }

        //GET /player/createplayer
        public IActionResult CreatePlayer() => View();
        //POST /player/createplayer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreatePlayer(Player player)
        {
            if (ModelState.IsValid)
            {
                context.Add(player);
                await context.SaveChangesAsync();

                TempData["Success"] = player.Name + " has been added";

                return RedirectToAction("Index");
            }
            return View(player);
        }

        //GET /player/deleteplayer
        public async Task<ActionResult> DeletePlayer(int id)
        {

            Player player = await context.Player.FindAsync(id);
            if (player == null)
            {
                TempData["Error"] = "Can't find the player =(";
            }
            else
            {
                context.Player.Remove(player);
                await context.SaveChangesAsync();

                TempData["Success"] = player.Name + " has been deleted";
            }
            return RedirectToAction("Index");

        }

        //GET /player/playerdetails
        public async Task<ActionResult> PlayerDetails(int id)
        {
            Player player = (await (from p in context.Player.Include(p => p.Team) where p.Id == id select p).ToListAsync()).First();
            if (player == null)
            {
                TempData["Error"] = "Something went wrong";
            }
            else
            {
                return PartialView(player);
            }
            return RedirectToAction("Index");
        }

        //GET /player/signplayer
        public async Task<ActionResult> SignPlayer(int id)
        {
            Sign signingmodel = new Sign();
            IQueryable<Team> teams = from t in context.Team orderby t.Name select t;
            List<Team> allTeams = await teams.ToListAsync();
            Player player = await context.Player.FindAsync(id);
            
            if (player == null || allTeams == null)
            {
                TempData["Error"] = "Something went wrong";
            }
            else
            {
                signingmodel.Player = player;
                signingmodel.Teams = allTeams;
                return PartialView(signingmodel);
            }
            return RedirectToAction("Index");
        }

        //GET /player/signtoteam
        public async Task<ActionResult> SignToTeam(int id, string name)
        {
            Team team = await context.Team.FindAsync(name);
            Player player = await context.Player.FindAsync(id);

            if (player == null || team == null)
            {
                TempData["Error"] = "Something went wrong";
            }
            else
            {
                player.Team = team;
                context.Update(player);
                Console.WriteLine(player.Team.Name);
                context.SaveChanges();

                Player player2 = await context.Player.FindAsync(player.Id);

            }
            return RedirectToAction("Index");
        }

        [Route("api/players")]
        /// <response code="200">Returns all players</response>
        /// <response code="400">If the players return null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<String> Players()
        {
            IQueryable<Player> players = from p in context.Player orderby p.Id select p;

            List<Player> playerList = await players.ToListAsync();

            return JsonConvert.SerializeObject(playerList);

        }
    }
}
