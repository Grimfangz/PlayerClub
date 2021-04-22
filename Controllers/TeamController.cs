using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PlayerClub.Infrastructure;
using PlayerClub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayerClub.Controllers
{

    public class TeamController : Controller
    {
        private PlayerClubContext context;
        public TeamController(PlayerClubContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult> Index()
        {
            IQueryable<Team> teams = from t in context.Team orderby t.Name select t;

            List<Team> teamList = await teams.ToListAsync();

            return View(teamList);
        }

        //GET /team/createteam
        public IActionResult CreateTeam() => View();
        //POST /team/createteam
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateTeam(Team team)
        {
            if (ModelState.IsValid)
            {
                context.Add(team);
                await context.SaveChangesAsync();

                TempData["Success"] = team.Name + " has been added";

                return RedirectToAction("Index");
            }
            return View(team);
        }

        //GET /player/deleteteam
        public async Task<ActionResult> DeleteTeam(string name)
        {
            Team team = await context.Team.FindAsync(name);
            if (team == null)
            {
                TempData["Error"] = "Can't find the team =(";
            }
            else
            {
                context.Team.Remove(team);
                await context.SaveChangesAsync();

                TempData["Success"] = name + " has been deleted";
            }
            return RedirectToAction("Index");

        }

        //GET /team/teamplayers
        public async Task<ActionResult> TeamPlayers(string name)
        {
            Team team = await context.Team.FindAsync(name);
            IQueryable<Player> players = from p in context.Player.Include(p => p.Team) where p.Team.Name == name orderby p.Id select p;
            List<Player> playerList = await players.ToListAsync();

            return PartialView(playerList);
        }

        [Route("api/teams")]
        /// <response code="200">Returns all teams</response>
        /// <response code="400">If the teams return null</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<String> Teams()
        {
            IQueryable<Team> teams = from t in context.Team orderby t.Name select t;

            List<Team> teamList = await teams.ToListAsync();

            return JsonConvert.SerializeObject(teamList);

        }
    }
}
