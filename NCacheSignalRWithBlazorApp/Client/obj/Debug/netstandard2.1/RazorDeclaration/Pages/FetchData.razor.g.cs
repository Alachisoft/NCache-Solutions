#pragma checksum "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\Pages\FetchData.razor" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0017ce00bbb55344df8ed6d4890848b406696c0d"
// <auto-generated/>
#pragma warning disable 1591
#pragma warning disable 0414
#pragma warning disable 0649
#pragma warning disable 0169

namespace BlazorSignalRApp.Client.Pages
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Components;
#nullable restore
#line 1 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using System.Net.Http;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using System.Net.Http.Json;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Forms;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Routing;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using Microsoft.AspNetCore.Components.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 6 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using Microsoft.JSInterop;

#line default
#line hidden
#nullable disable
#nullable restore
#line 7 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using BlazorSignalRApp.Client;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\_Imports.razor"
using BlazorSignalRApp.Client.Shared;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\Pages\FetchData.razor"
using BlazorSignalRApp.Shared;

#line default
#line hidden
#nullable disable
    [Microsoft.AspNetCore.Components.RouteAttribute("/fetchdata")]
    public partial class FetchData : Microsoft.AspNetCore.Components.ComponentBase
    {
        #pragma warning disable 1998
        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder __builder)
        {
        }
        #pragma warning restore 1998
#nullable restore
#line 38 "C:\Users\obaid_ur_rehman\Source\Repos\NCache-Solutions\NCacheSignalRWithBlazorApp\Client\Pages\FetchData.razor"
       
    private WeatherForecast[] forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("WeatherForecast");
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Components.InjectAttribute] private HttpClient Http { get; set; }
    }
}
#pragma warning restore 1591
