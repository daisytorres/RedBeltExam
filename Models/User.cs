#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedBeltExam.Models;


public class User
{    
    [Key]  
    public int UserId { get; set; }
    
    [Required]    
    public string UserName { get; set; } 
    
    [Required]
    [EmailAddress]
    [UniqueEmail]
    public string Email { get; set; }    
    
    [Required]
    [DataType(DataType.Password)] //makes it display as a password on the form
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
    public string Password { get; set; } 

    [NotMapped] //makes it so that it does not get stored in the DB
    
    [DataType(DataType.Password)]
    [Compare("Password")] //validation to match password
    public string PasswordConfirm { get; set; }
    
    public DateTime CreatedAt {get;set;} = DateTime.Now;   
    public DateTime UpdatedAt {get;set;} = DateTime.Now;

    //nav props go here
    public List<Coupon> Sales { get; set; } = new(); 
    public List<UserCouponerUsed> ConfirmedCoupons { get; set; } = new(); 


}

//validation for it to not be an email that exists in the DB
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
    	// Though we have Required as a validation, sometimes we make it here anyways
    	// In which case we must first verify the value is not null before we proceed
        if(value == null)
        {
    	    // If it was, return the required error
            return new ValidationResult("Email is required!");
        }
    
    	// This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
    	if(_context.Users.Any(e => e.Email == value.ToString()))
        {
    	    // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        } else {
    	    // If no, proceed
            return ValidationResult.Success;
        }
    }
}