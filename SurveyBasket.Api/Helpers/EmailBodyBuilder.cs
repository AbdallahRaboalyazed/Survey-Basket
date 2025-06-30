namespace SurveyBasket.Helpers;

public static class EmailBodyBuilder
{
    public static string GenearteEmailbody(string template, Dictionary<string,string>  templatemodel )
    {
        var templatepath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";
        var streamreader = new StreamReader(templatepath) ;
        var body = streamreader.ReadToEnd();
        streamreader.Close();

        foreach (var item in templatemodel)
        {
            body = body.Replace(item.Key,item.Value);
        }

        return body;



    }
}
