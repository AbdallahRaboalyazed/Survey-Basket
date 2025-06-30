using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.Authentication.Filters;
// not nessary to write above the controller or the endpoint [HasPermissionAttribute(......)]
// , you can wtite it without 'Attribute' at the end like this [HasPermission(......)]
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
{
}