namespace Zhankui_Wang_ProblemAssignment2.Services
{ 
    using Microsoft.AspNetCore.Http;

    public class HandleCookies
    {
        private DateTime FirstStartTime;
        private readonly IHttpContextAccessor _httpContextAccessor;

  

        public HandleCookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public void SetFirstStartTime(string userId)
        {
            string encodedUserId = Uri.EscapeDataString(userId);
            FirstStartTime = DateTime.Now;

            var httpContext = _httpContextAccessor.HttpContext;
            var isFirstVisit = !httpContext.Request.Cookies.ContainsKey($"FirstStartTime_{encodedUserId}");

            if (isFirstVisit)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTimeOffset.MaxValue // Set to a far-future date
                };
               
                httpContext.Response.Cookies.Append($"FirstStartTime_{encodedUserId}", FirstStartTime.ToString("o"), options); // "o" is the round-trip date/time pattern
            }
        }

        public DateTime? GetFirstVisitTime(string userId)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string encodedUserId = Uri.EscapeDataString(userId);
            if (httpContext.Request.Cookies.TryGetValue($"FirstStartTime_{encodedUserId}", out var cookieValue))
            {
                if (DateTime.TryParse(cookieValue, out var firstVisitTime))
                {
                    return firstVisitTime;
                }

            }
            return null;
        }
    }
}
