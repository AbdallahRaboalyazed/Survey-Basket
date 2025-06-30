namespace SurveyBasket.Persistence.EntitiesConfiguration;

public class VoteAnswerConfiguration : IEntityTypeConfiguration<VoteAnswer>
{
    public void Configure(EntityTypeBuilder<VoteAnswer> builder)
    {
        builder.HasIndex(x => new { x.VoteId, x.QuestionId }).IsUnique(); // معناه ان مينفعش تبعتلي نفس السؤال مرتين لنفس استطلاع الرأي
    }
}
