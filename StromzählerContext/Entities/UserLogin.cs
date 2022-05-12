using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StromzählerContext;

public class UserLogin
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}