Environment.SetEnvironmentVariable("FeatureManagement__NewFeatureFlag", "false", EnvironmentVariableTarget.Process);


// we should add this line in programs.cs
builder.Services.AddFeatureManagement().AddFeatureFilter<PercentageFilter>();

// add this percentage filter in appsetting.json
// "EnabledFor" is a property that take a list of parameters to configure the features. 
//    “Percentage” is built-in with feature management library. 
//    The “Value” which makes the feature flag decision.
"NewPercentageFilter": {
      "EnabledFor": [
        {
          "Name": "Percentage", // this is name of built-in filter as we use PercentageFilter s
          "Parameters": { // PercentageFilter acccept 
            "Value": 50
          }
        }
      ]
    }

[HttpGet("TestPercentageFilter")]
[FeatureGate("NewPercentageFilter")] // add this if you want to return only A if 'NewPercentageFilter' is true
public async Task<string> TestPercentageFilter()
{
  // check if NewPercentageFilter is true or not
  // Looks like the algorithms are switching between "A" and "B" on an average of 50 percentage
  var newAlgorithm = await _featureManager.IsEnabledAsync("NewPercentageFilter");
  return newAlgorithm ? "A" : "B";
}

//Custom Filter
public class BrowserFilterSettings
 {
     // this property will use as Parameters of filter in appsetting.json
     public string[] Allowed { get; set; }
 }

 // create custom filter BrowserFilter that implement IFeatureFilter
// FilterAlias 'Browser' this alias used in name of fitler key in appsetting.json
// use IHttpContextAccessor to get information about the request and the current user.

[FilterAlias("Browser")]
public class BrowserFilter : IFeatureFilter
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public BrowserFilter(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
  {
    if (_httpContextAccessor.HttpContext != null)
    {
      //  
      var userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
      var _browserSettings = context.Parameters.Get<BrowserFilterSettings>();
      return Task.FromResult(_browserSettings.Allowed.Any(x => userAgent.Contains(x)));
    }
      return Task.FromResult(false);
  }
}

// add filter key in appsetting.json 
// filter has name with same name of FilterAlias in BrowserFilter.cs
// filter has Parameters that has 'Allowed' same as BrowserFilterSettings.cs
  "BrowserFlag": {
      "EnabledFor": [
        {
          "Name": "Browser",
          "Parameters": {
            "Allowed": [ "Edg" ]
          }
        }
      ]
    }

// add FeatureGate with name of filter key that has define in appsetting.json
[HttpGet("ExcuteOnEdgeOnly")]
[FeatureGate("BrowserFlag")]
public async Task<ActionResult> ExcuteOnEdgeOnly()
{
    return Ok();
}

// add AddHttpContextAccessor in programs.cs
// add BrowserFilter in programs.cs
builder.Services.AddHttpContextAccessor();
builder.Services.AddFeatureManagement().AddFeatureFilter<BrowserFilter>();