using ElectronicStore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ElectronicStore.Infrastructure
{

    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {


        public PagingInfo PageModel { get; set; }
        public string PageActionAndController { get; set; }
        public int CurrentPage { get; set; }
        public string Urlpage { get; set; }

        public HttpContext urlcontext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (Urlpage.Count() > 0)
            {
                if (Urlpage.IndexOf("&page=") > 0)
                {
                    Urlpage = Urlpage.Substring(0, Urlpage.IndexOf("&page="));
                }
            }
            else
            {
                Urlpage = "?";
            }
            PageActionAndController = PageActionAndController + Urlpage;

            TagBuilder result = new TagBuilder("ul");
            result.AddCssClass("store-pagination");

            if (CurrentPage < 6)
            {
                if (PageModel.TotalPages > 7)
                {
                    for (int i = 1; i <= 6; i++)
                    {
                        TagBuilder item = new TagBuilder("li");
                        TagBuilder tag = new TagBuilder("a");
                        string page = PageActionAndController + '&' + $"page={i}";
                        tag.Attributes["href"] = page;
                        if (CurrentPage == i)
                        {
                            item.AddCssClass("active");

                        }
                        tag.InnerHtml.Append(i.ToString());
                        item.InnerHtml.AppendHtml(tag);
                        result.InnerHtml.AppendHtml(item);
                    }
                    TagBuilder itemhelp = new TagBuilder("li");
                    TagBuilder taghelp = new TagBuilder("a");
                    itemhelp.InnerHtml.AppendHtml("...");
                    result.InnerHtml.AppendHtml(itemhelp);

                    //end Page
                    itemhelp = new TagBuilder("li");
                    string Endpage = PageActionAndController + '&' + $"page={PageModel.TotalPages}";
                    taghelp.Attributes["href"] = Endpage;
                    taghelp.InnerHtml.Append(PageModel.TotalPages.ToString());
                    itemhelp.InnerHtml.AppendHtml(taghelp);
                    result.InnerHtml.AppendHtml(itemhelp);
                }
                else
                {
                    for (int i = 1; i <= PageModel.TotalPages; i++)
                    {
                        TagBuilder item = new TagBuilder("li");
                        TagBuilder tag = new TagBuilder("a");
                        string page = PageActionAndController + '&' + $"page={i}";
                        tag.Attributes["href"] = page;
                        if (CurrentPage == i)
                        {
                            item.AddCssClass("active");

                        }
                        tag.InnerHtml.Append(i.ToString());
                        item.InnerHtml.AppendHtml(tag);
                        result.InnerHtml.AppendHtml(item);
                    }
                }
            }
            else
            {
                if (PageModel.CurrentPage == PageModel.TotalPages)
                {
                    if (PageModel.TotalPages <= 7)
                    {
                        for (int i = 1; i <= PageModel.TotalPages; i++)
                        {
                            TagBuilder item = new TagBuilder("li");
                            TagBuilder tag = new TagBuilder("a");
                            string page = PageActionAndController + '&' + $"page={i}";
                            tag.Attributes["href"] = page;
                            if (CurrentPage == i)
                            {
                                item.AddCssClass("active");

                            }
                            tag.InnerHtml.Append(i.ToString());
                            item.InnerHtml.AppendHtml(tag);
                            result.InnerHtml.AppendHtml(item);
                        }
                    }
                    else
                    {
                        //First Page 
                        TagBuilder itemhelp = new TagBuilder("li");
                        TagBuilder taghelp = new TagBuilder("a");
                        string Startpage = PageActionAndController + '&' + $"page={1}";
                        taghelp.Attributes["href"] = Startpage;
                        taghelp.InnerHtml.Append("1");
                        itemhelp.InnerHtml.AppendHtml(taghelp);
                        result.InnerHtml.AppendHtml(itemhelp);

                        // Enter  
                        itemhelp = new TagBuilder("li");
                        taghelp = new TagBuilder("a");
                        itemhelp.InnerHtml.AppendHtml("...");
                        result.InnerHtml.AppendHtml(itemhelp);
                        for (int i = PageModel.TotalPages - 5; i <= PageModel.TotalPages; i++)
                        {
                            TagBuilder item = new TagBuilder("li");
                            TagBuilder tag = new TagBuilder("a");
                            string page = PageActionAndController + '&' + $"page={i}";
                            tag.Attributes["href"] = page;
                            if (CurrentPage == i)
                            {
                                item.AddCssClass("active");

                            }
                            tag.InnerHtml.Append(i.ToString());
                            item.InnerHtml.AppendHtml(tag);
                            result.InnerHtml.AppendHtml(item);
                        }
                    }
                }
                else
                {

                    //First Page 
                    TagBuilder itemhelp = new TagBuilder("li");
                    TagBuilder taghelp = new TagBuilder("a");
                    string Startpage = PageActionAndController + '&' + $"page={1}";
                    taghelp.Attributes["href"] = Startpage;
                    taghelp.InnerHtml.Append("1");
                    itemhelp.InnerHtml.AppendHtml(taghelp);
                    result.InnerHtml.AppendHtml(itemhelp);

                    // Enter  
                    itemhelp = new TagBuilder("li");
                    taghelp = new TagBuilder("a");
                    itemhelp.InnerHtml.AppendHtml("...");
                    result.InnerHtml.AppendHtml(itemhelp);

                    //afte enter 
                    int beforEndPage = CurrentPage + 2;
                    if (beforEndPage >= PageModel.TotalPages)
                    {
                        for (int i = CurrentPage - 2; i <= PageModel.TotalPages; i++)
                        {
                            TagBuilder item = new TagBuilder("li");
                            TagBuilder tag = new TagBuilder("a");
                            string page = PageActionAndController + '&' + $"page={i}";
                            tag.Attributes["href"] = page;
                            if (CurrentPage == i)
                            {
                                item.AddCssClass("active");

                            }
                            tag.InnerHtml.Append(i.ToString());
                            item.InnerHtml.AppendHtml(tag);
                            result.InnerHtml.AppendHtml(item);
                        }
                    }
                    else
                    {
                        for (int i = CurrentPage - 2; i <= beforEndPage; i++)
                        {
                            TagBuilder item = new TagBuilder("li");
                            TagBuilder tag = new TagBuilder("a");
                            string page = PageActionAndController + '&' + $"page={i}";
                            tag.Attributes["href"] = page;
                            if (CurrentPage == i)
                            {
                                item.AddCssClass("active");

                            }
                            tag.InnerHtml.Append(i.ToString());
                            item.InnerHtml.AppendHtml(tag);
                            result.InnerHtml.AppendHtml(item);
                        }
                        //aftepages
                        itemhelp = new TagBuilder("li");
                        taghelp = new TagBuilder("a");
                        itemhelp.InnerHtml.AppendHtml("...");
                        result.InnerHtml.AppendHtml(itemhelp);

                        //end page
                        itemhelp = new TagBuilder("li");
                        taghelp = new TagBuilder("a");
                        string Endpage = PageActionAndController + '&' + $"page={PageModel.TotalPages}";
                        taghelp.Attributes["href"] = Endpage;
                        taghelp.InnerHtml.Append(PageModel.TotalPages.ToString());
                        itemhelp.InnerHtml.AppendHtml(taghelp);
                        result.InnerHtml.AppendHtml(itemhelp);
                    }
                }



            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}

