namespace SurveyBasket.Persistence.EntitiesConfiguration;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();  // معناه ان اليوزر ممكن يصوت مرة واحدة فقط على الاستفتاء مش اكتر
    }
}
