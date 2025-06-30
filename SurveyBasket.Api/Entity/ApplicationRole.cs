namespace SurveyBasket.Entity;
public class ApplicationRole : IdentityRole
{
    public bool IsDefault { get; set; }
    public bool IsDeleted { get; set; }
}