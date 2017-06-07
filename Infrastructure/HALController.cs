using System.Collections.Generic;
using Halcyon.HAL;
using Halcyon.Web.HAL;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Linq.Expressions;

public static class MethodInfoExtentions
{
    public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MethodInfo methodInfo, bool inherit)
     where TAttribute : Attribute
    {
        IEnumerable<Attribute> attributeObjects = methodInfo.GetCustomAttributes(typeof(TAttribute), inherit);

        return attributeObjects.Cast<TAttribute>();
    }
}

public static class IEnumerableExtensions
{
    public static IEnumerable<HALResponse> ToHALResponses<T>(this IEnumerable<T> collection, Func<T, IEnumerable<Link>> linkGenerator = null)
    {
        var halCollection = new List<HALResponse>();

        foreach (var item in collection)
        {
            var halItem = new HALResponse(item);
            if (linkGenerator != null)
            {
                var linkList = linkGenerator(item);
                foreach (var x in linkList)
                    System.Console.WriteLine(x.Href);
                halItem.AddLinks(linkList);
            }

            halCollection.Add(halItem);
        }

        return halCollection;
    }
}

public class HALController : Controller
{
    private class CollectionSummary
    {
        public string ResourceType { get; } = "List";
        public int Count { get; set; }
    }

    public HALController() : base()
    {

    }

    protected HALResponse HAL(IEnumerable<HALResponse> collectionToEmbed, string collectionName="data")
    {
        var hal = new HALResponse(new CollectionSummary { Count = collectionToEmbed.Count() })
                        .AddEmbeddedCollection(collectionName, collectionToEmbed)
                        .AddSelfLink(HttpContext.Request);

        return hal;
    }

    protected Link CommandLink<T>(Expression<Action<T>> fun)
    {
        var mce = (MethodCallExpression)fun.Body;

        string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
        string actionName = mce.Method.Name;
        var cmdArgument = mce.Arguments.Single(x => x.Type == typeof(T));
        string commandName = cmdArgument.Type.Name;

        // Get title from attribute or default to command name
        string commandTitle = commandName;
        var commandAttribute = mce.Method.GetCustomAttributes<CommandAttribute>(true).FirstOrDefault();
        if (commandAttribute != null && !string.IsNullOrWhiteSpace(commandAttribute.Title))
            commandTitle = commandAttribute.Title;

        var link = Url.Link(null, new { Controller = controllerName, Action = actionName });
        
        var idArgument = mce.Arguments.FirstOrDefault(x => x.Type == typeof(Int32));
        if (idArgument != null)
            link= Url.Link(null, new { Controller = controllerName, Action = actionName, Id = 0 });

        return new Link(
            $"cmd.{commandName}",
            link,
            commandTitle,
            "POST");
    }
}
