#pragma checksum "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1c47c5fb2322f8ab2093e140c3ba4eb0a6ccf4d5"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_AccessControl_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/AccessControl/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/AccessControl/Index.cshtml", typeof(AspNetCore.Areas_Admin_Views_AccessControl_Index))]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp;

#line default
#line hidden
#line 3 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Models;

#line default
#line hidden
#line 4 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Models.AccountViewModels;

#line default
#line hidden
#line 5 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Models.ManageViewModels;

#line default
#line hidden
#line 6 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Application.ViewModels.System;

#line default
#line hidden
#line 7 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Application.ViewModels.BlockChain;

#line default
#line hidden
#line 8 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Application.ViewModels.Valuesshare;

#line default
#line hidden
#line 9 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\_ViewImports.cshtml"
using BeCoreApp.Data.Entities;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1c47c5fb2322f8ab2093e140c3ba4eb0a6ccf4d5", @"/Areas/Admin/Views/AccessControl/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"68994ac50316bb5bc7adb5dd726610ea311dbfdf", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_AccessControl_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<AccessControlViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", "~/admin-app/controllers/accesscontrol/index.js", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
  
    ViewData["Title"] = "Quyền Hạn";

#line default
#line hidden
            DefineSection("Styles", async() => {
                BeginContext(92, 4, true);
                WriteLiteral("\r\n\r\n");
                EndContext();
            }
            );
            DefineSection("Scripts", async() => {
                BeginContext(116, 6, true);
                WriteLiteral("\r\n    ");
                EndContext();
                BeginContext(122, 96, false);
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f7ceb4b606ba4ffb90857a22b359edb7", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ScriptTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.Src = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
#line 9 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
__Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion = true;

#line default
#line hidden
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-append-version", __Microsoft_AspNetCore_Mvc_TagHelpers_ScriptTagHelper.AppendVersion, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                EndContext();
                BeginContext(218, 134, true);
                WriteLiteral("\r\n    <script>\r\n        var accessControlObj = new AccessControlController();\r\n        accessControlObj.initialize();\r\n    </script>\r\n");
                EndContext();
            }
            );
            BeginContext(355, 357, true);
            WriteLiteral(@"
<div class=""app-content icon-content"">
    <div class=""section"">
        <!-- Page-header opened -->
        <div class=""page-header"">
            <ol class=""breadcrumb"">
                <li class=""breadcrumb-item""><a href=""#""><i class=""ti-package mr-1""></i> Trang Chủ</a></li>
                <li class=""breadcrumb-item active"" aria-current=""page"">");
            EndContext();
            BeginContext(713, 17, false);
#line 22 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                  Write(ViewData["Title"]);

#line default
#line hidden
            EndContext();
            BeginContext(730, 1496, true);
            WriteLiteral(@"</li>
            </ol>
            <div class=""mt-3 mt-lg-0"">
                <div class=""d-flex align-items-center flex-wrap text-nowrap"">
                    <button type=""button"" class=""btn btn-success btn-icon-text mr-2 mb-2 mb-md-0""><i class=""fe fe-download""></i>Import</button>
                    <button type=""button"" class=""btn btn-secondary btn-icon-text mr-2 mb-2 mb-md-0""><i class=""fe fe-printer""></i>Print</button>
                    <button type=""button"" class=""btn btn-primary btn-icon-text mb-2 mb-md-0""><i class=""fe fe-download-cloud ""></i>Download</button>
                </div>
            </div>
        </div>
        <!-- Page-header closed -->
        <!-- row opened -->
        <div class=""row"">
            <div class=""col-md-12 col-lg-12"">
                <div class=""card"">
                    <div class=""card-header"">
                        <div class=""card-title"">Danh Sách Quyền Hạn</div>
                        <div class=""card-options"">
                            <a");
            WriteLiteral(@" href=""#"" class=""card-options-collapse"" data-toggle=""card-collapse""><i class=""fe fe-chevron-up""></i></a>
                            <a href=""#"" class=""card-options-fullscreen"" data-toggle=""card-fullscreen""><i class=""fe fe-maximize""></i></a>
                            <a href=""#"" class=""card-options-remove"" data-toggle=""card-remove""><i class=""fe fe-x""></i></a>
                        </div>
                    </div>
                    <div class=""card-body"">
");
            EndContext();
#line 46 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                         using (Html.BeginForm("SetPermission", "/Admin/AccessControl", FormMethod.Post, new { enctype = "multipart/form-data" }))
                        {

#line default
#line hidden
            BeginContext(2401, 701, true);
            WriteLiteral(@"                            <div class=""table-responsive product-datatable"">
                                <div id=""example_wrapper"" class=""dataTables_wrapper dt-bootstrap4 no-footer"">
                                    <div class=""row"">

                                        <div class=""col-sm-12"">

                                            <table id=""example"" class=""table table-striped table-bordered dataTable no-footer"" role=""grid"" aria-describedby=""example_info"">
                                                <thead>
                                                    <tr style=""background:#393939"">
                                                        <th>Module</th>
");
            EndContext();
#line 58 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                         foreach (var role in Model.AppRoles)
                                                        {

#line default
#line hidden
            BeginContext(3256, 268, true);
            WriteLiteral(@"                                                            <th>
                                                                <label class=""custom-control custom-checkbox"">
                                                                    <input type=""checkbox""");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 3524, "\"", 3540, 1);
#line 62 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 3532, role.Id, 3532, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(3541, 150, true);
            WriteLiteral(" class=\"custom-control-input CheckedAllById\">\r\n                                                                    <span class=\"custom-control-label\">");
            EndContext();
            BeginContext(3692, 9, false);
#line 63 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                                                  Write(role.Name);

#line default
#line hidden
            EndContext();
            BeginContext(3701, 150, true);
            WriteLiteral("</span>\r\n                                                                </label>\r\n                                                            </th>\r\n");
            EndContext();
#line 66 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                        }

#line default
#line hidden
            BeginContext(3910, 200, true);
            WriteLiteral("                                                    </tr>\r\n                                                </thead>\r\n                                                <tbody id=\"table-access-control\">\r\n");
            EndContext();
#line 70 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                      
                                                        int quantityUserRoles = Model.AppRoles.Count + 1;
                                                    

#line default
#line hidden
            BeginContext(4328, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 74 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                     foreach (var item in Model.AccessControlDTOs)
                                                    {
                                                        if (item.Root)
                                                        {


#line default
#line hidden
            BeginContext(4618, 150, true);
            WriteLiteral("                                                            <tr class=\"trparent\">\r\n                                                                <td");
            EndContext();
            BeginWriteAttribute("colspan", " colspan=\"", 4768, "\"", 4796, 1);
#line 80 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 4778, quantityUserRoles, 4778, 18, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(4797, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(4799, 9, false);
#line 80 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                                            Write(item.Name);

#line default
#line hidden
            EndContext();
            BeginContext(4808, 74, true);
            WriteLiteral("</td>\r\n                                                            </tr>\r\n");
            EndContext();
#line 82 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                        }
                                                        else
                                                        {

#line default
#line hidden
            BeginContext(5062, 183, true);
            WriteLiteral("                                                            <tr>\r\n                                                                <td><i class=\"fa fa-long-arrow-right ml-5 mr-3\"></i> ");
            EndContext();
            BeginContext(5246, 9, false);
#line 86 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                                                                Write(item.Name);

#line default
#line hidden
            EndContext();
            BeginContext(5255, 7, true);
            WriteLiteral("</td>\r\n");
            EndContext();
#line 87 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                  
                                                                    int stt = 0;
                                                                

#line default
#line hidden
            BeginContext(5479, 64, true);
            WriteLiteral("                                                                ");
            EndContext();
#line 90 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                 foreach (var role in Model.AppRoles)
                                                                {

#line default
#line hidden
            BeginContext(5649, 74, true);
            WriteLiteral("                                                                    <td>\r\n");
            EndContext();
#line 93 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                         if (item.IsPermissions[stt])
                                                                        {

#line default
#line hidden
            BeginContext(5901, 226, true);
            WriteLiteral("                                                                            <label class=\"custom-control custom-checkbox\">\r\n                                                                                <input type=\"checkbox\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 6127, "\"", 6147, 1);
#line 96 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 6135, item.Action, 6135, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 6148, "\"", 6194, 3);
            WriteAttributeValue("", 6156, "custom-control-input", 6156, 20, true);
            WriteAttributeValue(" ", 6176, "disables_", 6177, 10, true);
#line 96 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 6186, role.Id, 6186, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("name", "\r\n                                                                                       name=\"", 6195, "\"", 6329, 4);
            WriteAttributeValue("", 6290, "disables_", 6290, 9, true);
#line 97 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 6299, role.Id, 6299, 8, false);

#line default
#line hidden
#line 97 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 6307, Html.Raw("-"), 6307, 14, false);

#line default
#line hidden
#line 97 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 6321, item.Id, 6321, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(6330, 231, true);
            WriteLiteral(" checked=\"checked\">\r\n                                                                                <span class=\"custom-control-label\"></span>\r\n                                                                            </label>\r\n");
            EndContext();
#line 100 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                        }
                                                                        else
                                                                        {

#line default
#line hidden
            BeginContext(6789, 226, true);
            WriteLiteral("                                                                            <label class=\"custom-control custom-checkbox\">\r\n                                                                                <input type=\"checkbox\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 7015, "\"", 7035, 1);
#line 104 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 7023, item.Action, 7023, 12, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("class", " class=\"", 7036, "\"", 7082, 3);
            WriteAttributeValue("", 7044, "custom-control-input", 7044, 20, true);
            WriteAttributeValue(" ", 7064, "disables_", 7065, 10, true);
#line 104 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 7074, role.Id, 7074, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginWriteAttribute("name", "\r\n                                                                                       name=\"", 7083, "\"", 7217, 4);
            WriteAttributeValue("", 7178, "disables_", 7178, 9, true);
#line 105 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 7187, role.Id, 7187, 8, false);

#line default
#line hidden
#line 105 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 7195, Html.Raw("-"), 7195, 14, false);

#line default
#line hidden
#line 105 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
WriteAttributeValue("", 7209, item.Id, 7209, 8, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(7218, 213, true);
            WriteLiteral(">\r\n                                                                                <span class=\"custom-control-label\"></span>\r\n                                                                            </label>\r\n");
            EndContext();
#line 108 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                                        }

#line default
#line hidden
            BeginContext(7506, 75, true);
            WriteLiteral("                                                                    </td>\r\n");
            EndContext();
#line 110 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"

                                                                    stt++;
                                                                }

#line default
#line hidden
            BeginContext(7726, 67, true);
            WriteLiteral("                                                            </tr>\r\n");
            EndContext();
#line 114 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                                                        }
                                                    }

#line default
#line hidden
            BeginContext(7907, 750, true);
            WriteLiteral(@"                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                    <div class=""row"">
                                        <div class=""col-sm-12 mt-5 text-center"">
                                            <button type=""submit"" class=""btn btn-secondary btn-icon-text mr-2 mb-2 mb-md-0"">
                                                Cập Nhật
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
");
            EndContext();
#line 130 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Areas\Admin\Views\AccessControl\Index.cshtml"
                        }

#line default
#line hidden
            BeginContext(8684, 472, true);
            WriteLiteral(@"                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<style>
    .trparent {
        background: #e13600 !important;
        color: white;
        font-weight: bold;
    }

    table.dataTable thead th {
        color: white;
    }

    .custom-checkbox .custom-control-input:checked ~ .custom-control-label::before {
        background-color: #515151;
        border: 1px solid #515151;
    }
</style>");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<AccessControlViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
