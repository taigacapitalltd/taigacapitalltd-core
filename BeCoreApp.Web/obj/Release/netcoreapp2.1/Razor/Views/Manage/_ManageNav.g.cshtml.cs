#pragma checksum "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6e4bef47ea05c78c755ed6eeca1e3240007069b7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Manage__ManageNav), @"mvc.1.0.view", @"/Views/Manage/_ManageNav.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Manage/_ManageNav.cshtml", typeof(AspNetCore.Views_Manage__ManageNav))]
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
#line 1 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#line 2 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Extensions;

#line default
#line hidden
#line 3 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Models;

#line default
#line hidden
#line 4 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Application.ViewModels.Product;

#line default
#line hidden
#line 5 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Application.ViewModels.Blog;

#line default
#line hidden
#line 6 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Models.AccountViewModels;

#line default
#line hidden
#line 7 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Models.ManageViewModels;

#line default
#line hidden
#line 8 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using BeCoreApp.Data.Entities;

#line default
#line hidden
#line 9 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Mvc.Localization;

#line default
#line hidden
#line 1 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
using BeCoreApp.Views.Manage;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e4bef47ea05c78c755ed6eeca1e3240007069b7", @"/Views/Manage/_ManageNav.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"5e9687c970b0a85b48325f199cadb19b9d1a74bb", @"/Views/_ViewImports.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e5c61ed1a67772e70fe4e6bfd743b6a573cc44b5", @"/Views/Manage/_ViewImports.cshtml")]
    public class Views_Manage__ManageNav : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Index", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ChangePassword", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "ExternalLogins", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "TwoFactorAuthentication", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
  
    var hasExternalLogins = (await SignInManager.GetExternalAuthenticationSchemesAsync()).Any();

#line default
#line hidden
            BeginContext(182, 49, true);
            WriteLiteral("\r\n<ul class=\"nav nav-pills nav-stacked\">\r\n    <li");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 231, "\"", 281, 1);
#line 8 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
WriteAttributeValue("", 239, ManageNavPages.IndexNavClass(ViewContext), 239, 42, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(282, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(283, 33, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d71c58f2088c4d8abe40ab8543867ec1", async() => {
                BeginContext(305, 7, true);
                WriteLiteral("Profile");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(316, 14, true);
            WriteLiteral("</li>\r\n    <li");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 330, "\"", 389, 1);
#line 9 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
WriteAttributeValue("", 338, ManageNavPages.ChangePasswordNavClass(ViewContext), 338, 51, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(390, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(391, 43, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "f18045cf37394bd89a94f99c45a067af", async() => {
                BeginContext(422, 8, true);
                WriteLiteral("Password");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(434, 7, true);
            WriteLiteral("</li>\r\n");
            EndContext();
#line 10 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
     if (hasExternalLogins)
    {

#line default
#line hidden
            BeginContext(477, 11, true);
            WriteLiteral("        <li");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 488, "\"", 547, 1);
#line 12 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
WriteAttributeValue("", 496, ManageNavPages.ExternalLoginsNavClass(ViewContext), 496, 51, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(548, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(549, 50, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "67b0983c157f4daba71162b44d61171c", async() => {
                BeginContext(580, 15, true);
                WriteLiteral("External logins");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(599, 7, true);
            WriteLiteral("</li>\r\n");
            EndContext();
#line 13 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
    }

#line default
#line hidden
            BeginContext(613, 7, true);
            WriteLiteral("    <li");
            EndContext();
            BeginWriteAttribute("class", " class=\"", 620, "\"", 688, 1);
#line 14 "C:\TongHop\Others\TaigaCapitalLtd\BeCoreApp.Web\Views\Manage\_ManageNav.cshtml"
WriteAttributeValue("", 628, ManageNavPages.TwoFactorAuthenticationNavClass(ViewContext), 628, 60, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(689, 1, true);
            WriteLiteral(">");
            EndContext();
            BeginContext(690, 69, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "80765dd752a649e9b439f97474204dc3", async() => {
                BeginContext(730, 25, true);
                WriteLiteral("Two-factor authentication");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(759, 16, true);
            WriteLiteral("</li>\r\n</ul>\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public SignInManager<AppUser> SignInManager { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
