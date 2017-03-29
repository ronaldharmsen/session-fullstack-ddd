using System.Collections;
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

    protected HALResponse HAL(string collectionName, IEnumerable collectionToEmbed)
    {
        var collection = new List<HALResponse>();
        foreach (var item in collectionToEmbed)
        {
            var halItem = new HALResponse(item);
            collection.Add(halItem);
        }

        var hal = new HALResponse(new CollectionSummary { Count = collection.Count() })
                        .AddEmbeddedCollection(collectionName, collection)
                        .AddSelfLink(HttpContext.Request);

        return hal;
    }

    // protected Link CommandLink<T>(Action<T> act) {
    //     return CommandLink<T>(cmd => act(cmd));
    // }
    
    protected Link CommandLink<T>(Expression<Action<T>> fun)
        {
            var mce = (MethodCallExpression)fun.Body;
            
            string controllerName = this.GetType().Name.Replace("Controller", string.Empty);
            string actionName = mce.Method.Name;
            string commandName = mce.Arguments[0].Type.Name;

            // Get title from attribute or default to command name
            string commandTitle = commandName;                        
            var commandAttribute = mce.Method.GetCustomAttributes<CommandAttribute>(true).FirstOrDefault();            
            if (commandAttribute != null && !string.IsNullOrWhiteSpace(commandAttribute.Title))
                commandTitle = commandAttribute.Title;

            return new Link(
                $"cmd.{commandName}", 
                Url.Link("Default", new { Controller = controllerName, Action = actionName }), 
                commandTitle, 
                "POST");
        }
}
