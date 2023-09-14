#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RedBeltExam.Models;

public class UserCouponerUsed
{
    [Key]  
    public int UserCouponerUsedId { get; set; }

    public int UserId { get; set; }
    public User? UserUsed { get; set; }

    public int CouponId { get; set; }
    public Coupon? CouponUsed { get; set; }

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

}