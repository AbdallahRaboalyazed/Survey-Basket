namespace SurveyBasket.Contracts.Authentication;

public record AuthRefreshRequest(string Token, string RefreshToken);