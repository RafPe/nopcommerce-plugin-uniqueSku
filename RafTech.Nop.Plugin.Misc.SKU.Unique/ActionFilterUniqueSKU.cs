using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RafTech.Nop.Plugin.Misc.SKU.Unique
{
    using System.Web.Mvc;

    using global::Nop.Admin.Controllers;
    using global::Nop.Admin.Models.Catalog;
    using global::Nop.Core.Domain.Directory;
    using global::Nop.Core.Infrastructure;
    using global::Nop.Services.Catalog;

    /// <summary>
    /// Filter making sure SKU is unique
    /// </summary>
    public class ActionFilterUniqueSKU : ActionFilterAttribute, IFilterProvider
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            if ((actionDescriptor.ControllerDescriptor.ControllerType == typeof(ProductController)) &&
                (actionDescriptor.ActionName.Equals("Create") &&
                controllerContext.HttpContext.Request.HttpMethod == "POST"))
            {
                return new List<Filter>() { new Filter(this, FilterScope.Action, 0) }; 
            }

            return new Filter[] { };
        }



        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            try
            {


                var product2create = filterContext.ActionParameters["model"] as ProductModel; // Our object model

                var _productservice = EngineContext.Current.Resolve<IProductService>();       // Our serice - we are stateless in filter :) 

                if (_productservice.GetProductBySku(product2create.Sku) != null )
                {

                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Plugins/RafTech.Nop.Plugin.Misc.SKU.Unique/Views/UniqueSKU/NotFound.cshtml",
                    };


                }
                else
                {
                    base.OnActionExecuting(filterContext);    
                }

                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("could not process SKU filter", ex.Message,ex.InnerException);
            }


            
        }
    }

 
}