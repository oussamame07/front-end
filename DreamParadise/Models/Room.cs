#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Add this using statement to access NotMapped
using System.ComponentModel.DataAnnotations.Schema;
namespace DreamParadise.Models;
public class Room
{
    [Key]
    public int RoomId { get; set; }






    //* ======= Navigation ============
    public User? UserWhoReserved {get;set;}





    
}


