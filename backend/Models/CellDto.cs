
using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class CellDto
{
    [Range(1, 9, ErrorMessage = "Введите корректное значение ячейки (1-9)")]
    public int ChoosenCellId { get; set; }
}