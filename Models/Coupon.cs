#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RedBeltExam.Models;

public class Coupon
{
    [Key]  
    public int CouponId { get; set; }

    [Required]
    public string Code { get; set; }

    [Required]
    public string Website { get; set; }

    [Required]
    [MinLength(10)]
    public string Description { get; set; }

    public int UserId { get; set; }
    public User? Couponer { get; set; }
    public List<UserCouponerUsed> UsersConfirmed { get; set; } = new();

    public DateTime CreatedAt {get;set;} = DateTime.Now;   
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}