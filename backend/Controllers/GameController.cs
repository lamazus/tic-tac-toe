using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;

namespace backend.Controllers;

[ApiController]
[Route("game")]
public class GameController : ControllerBase
{
    private readonly TicTacContext _context;
    public GameController(TicTacContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Route("/{sessionId}")]
    public ActionResult Get(int sessionId)
    {
        var response = _context.Sessions.Include(p=>p.Cells).SingleOrDefault(p => p.Id == sessionId);
        if(response == null)
            return NotFound();

        if(response != null && response.isFinished)
            return Ok($"Данная игровая сессия завершена. Победил Игрок {response.Winner}");

        return Ok(response);
    }

    [HttpPost]
    [Route("new-game")]
    public ActionResult NewGame()
    {   
        var session = new Session();
        _context.Sessions.Add(session);
        _context.SaveChangesAsync();

        List<Cell> cells = new List<Cell>();

        for(int i = 0; i < 9; i++)
        {
            cells.Add(new Cell{CellNumber = i+1, SessionId = session.Id});

        }
        _context.Cells.AddRange(session.Cells);
        _context.SaveChangesAsync();

        session.Cells = cells;
        _context.Sessions.Update(session);
        _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(Get), new { sessionId = session.Id}, session.Id );
    }

    [HttpPut]
    [Route("put-sign/{sessionId}/")]
    public ActionResult PutSign(int sessionId, CellDto cell)
    {
        var game = _context.Sessions.Include(p=>p.Cells).SingleOrDefault(p=>p.Id == sessionId);
        if(game == null || game.isFinished == true)
            return BadRequest("Игровая сессия не найдена или завершена");


        var cellToEdit = game.Cells.SingleOrDefault(p=>p.CellNumber == cell.ChoosenCellId);
        if(cellToEdit == null)
            return BadRequest("Введите корректный номер ячейки");

        if(cellToEdit!.Status == '-')
        {
            switch(game.ActivePlayer){
                case 1: cellToEdit.Status = 'X';
                        game.ActivePlayer = 2;
                        break;
                case 2: cellToEdit.Status = 'O';
                        game.ActivePlayer = 1;
                        break;
                default:
                    break;
            }
        }
        else
        {
            return BadRequest("Выберите пустую ячейку");
        }
        _context.Sessions.Update(game);
        _context.SaveChangesAsync();

        if(CheckWinner(game, out int winner))
        {
            game.Winner = winner;
            game.isFinished = true;
            _context.Sessions.Update(game);
            _context.SaveChangesAsync();
        };

       return Ok();
    }

    [NonAction]
    public bool CheckWinner(Session game, out int winner)
    {
        int[,] winLines = new int[,]
        {
            {1, 2, 3},
            {4, 5, 6},
            {7, 8, 9},
            {1, 4, 7},
            {2, 5, 8},
            {3, 6, 9},
            {1, 5, 9},
            {3, 5, 7}
        };

        winner = 0;

        for(int i = 0; i < winLines.GetUpperBound(1); i++)
        {
            Cell c1 = game.Cells.Find(p=>p.CellNumber == winLines[i,0])!;
            Cell c2 = game.Cells.Find(p=>p.CellNumber == winLines[i,0])!;
            Cell c3 = game.Cells.Find(p=>p.CellNumber == winLines[i,0])!;

            if(c1.Status != '-' && c1.Status == c2.Status && c1.Status == c3.Status)
            {   
                if(c1.Status == 'X')
                    winner = 1;

                if(c1.Status == 'O')
                    winner = 2;

                return true;
            }
        }

        return false;
    }

}