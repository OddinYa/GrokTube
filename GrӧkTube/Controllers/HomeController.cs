using GrӧkTube.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;


namespace GrӧkTube.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index(UserModel userModel = null)
        {

            if (User.Identity.IsAuthenticated)
            {
                // Получаем данные из claims
                var username = User.Identity.Name;
                var userId = User.FindFirst("UserId")?.Value;

                
                
                ViewBag.User = new UserModel
                {
                    Username = username,
                    Id = int.Parse(userId),
                    AvatarUrl = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBwgHBgkIBwgKCgkLDRYPDQwMDRsUFRAWIB0iIiAdHx8kKDQsJCYxJx8fLT0tMTU3Ojo6Iys/RD84QzQ5OjcBCgoKDQwNGg8PGjclHyU3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3Nzc3N//AABEIAKgAtAMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAAAgMEBQYBBwj/xAA8EAABAwIEAwUGBAUDBQAAAAABAAIDBBEFEiExE0FRBiJhcYEyQqGxwfAUI5HRB1JicuEVQ4Izg5Ky8f/EABkBAAIDAQAAAAAAAAAAAAAAAAAEAQIDBf/EACMRAAMAAgIDAAIDAQAAAAAAAAABAgMREiEEMUETUQUyYSL/2gAMAwEAAhEDEQA/APDkIQgAQhCABCEIAF3knqKknrJhDTRPkkPutF1rsJ/h9iFVrXyikZ0dZx+BV4x1b6RnkzY8a/7ZjY2OeQ1jS4nYAK2puzGLVMYkipHFh2NwvVcJ7K4XhjWflxyyN3eWalW2eCAHJGwD+m2idx+DT7o5uX+TSeoWzx+m7D9oKl4bDh73A+9cAfqSl1XYHtPShzn4TM5jdzEWyfAFesjEW57sbcjmpsWJPIBDy0qa8OfjKL+Ry/ZPnipp5qaQx1EMkTwdWyNLT+hTS+hayqwzF5BT4zQQ1Ufuukbq3yI1Waxz+FVBVxun7O1ZY4glsExuD5Hl6pa/HqRuPOiv7dHkB2SVMxTDqzC6l1JX00lPMw2cx7bH06jx2UNLjye+wQhCABCEIAEIQgAQhCABCEIAEIQgDrd9FZYJhFRi9YyCBpyk9950a0eag00T552RRi73GwC9RwOgpsIo2QxtBleLySHmfDwW+DFzYt5Of8U9eyxwfs7hOCw9xhqHnVzpHE6+Q0+CszVxsbZmRjTyAVVUSyBtmEaa2uLqD+Medxc+S6i4wtI4risj3T2XVTiDWMvnB12Chfi+I0vL9OSgU4M7y5wsLqXCWgtjYLh2miHbYLGpFOqR3shsebUh1c82aDa67LBlzOy+18dBr8VBDZC8Oy6AqlbNZ0WAqL5cztjYqypq+ohaOA+4v3Tew9fBU8WaVxszujUpRkdFlAdpcA/IKNsGjQ11NQdqaJ9FjMeSYC0VRlu6M/f1Xjnazs1W9mMRNJWDMxwzRTNHdkb1H7L0+mxAN1vYh2vyP7q7rsKoe2mANpas5Z2G8UrT3onW38R1CwzYFa3Ps0w+S8Far+p89FcVn2hwarwLEpKCvZlmZaxGxHUeCrFz2tHZTVLa9AhC6EEg0FxsEt1mjLzXCMoskc0AF0IQgAQhCABdCArPCMOfUTsfLG7hDXUaOVpTb6IdJLsv+ymExwgVtYLvI7kf1Kv55XPlLr2PLwUSOQhlhp4n6JiacMBNtep3T8JROkcq93W2SZ6idp0dp52QydskZa4gu8Qq/iAuZmvZ3XdPVTeCwZd9x5KeRHBa0WDau7GsBcHbac1Pp2vi70rXm19RyWPNc6CVriSNb3buvSZof9Qw1hZlZO1gLg0aOuNCPSxHmFeK5GWaeGl+ytFQ03DyLO2CjzjJKXMNr7KN+GliA4ocNdC0/f3dJmI2Mh3Oql0RMr4LiqHd5oGXqo887o9L3vom3ytaDrr7xG10l0jXcN2bmFTkbKB+KbOwW3J19d/ktDg2J/hZGhpyEODSFl4yY5mSXuA4H5pU0wMwsSAXC1uqtNGV41fTLv8AivhYxnA6bHKNt5aTuTgb5CfodvMrx0i3ovc+x1W2ukqKKq78dQxzC12xBv8AuvGMZozh+KVlGbngTOjueYBNik/JlJ7Q74NvTxv4QShBQlh8EIQgAQhCABBQhAD9HCZ5gwLVU7GwtaDu0KmwNmUSS89grW999zqUxi6WxbN29E8TiQ2bvZOtpja5t6pikAtqbKaWtyaG/wAPomEKU9FdK3K7lp0SbvcNdlNZSl50Txpu4QBc22U8WRyWzO4i1jXBoeC4jYrW9hcYZMWYbVTlsjdIS4+2z+UeIOoWQxKAGQi2Ug3y7+qrXx1UZlkjZKWQEGR7QSGXNhc8rlYfkeOhl4FmjR7nXULZoHx2AcCSCBa2+3rf48wVia2J7XObHrlPPQj71Vn2V7QR4zhkbeORVwsyzRPIJNrd9vMg8xyI6WtzE7Bznd2+9xsf/qc2rnaOZCrFfFmbDCCTK7KV20YDonHlun6i073N2vsFXyxvaS14vZYtaG12OF7m5msdcBPQjOMhNie9fpZRo3szubl1IsptK6Fl2k3JHtffmoTJqejR9k7QVPGzkEOJAHnsqf8AiL2dmxDtzG6ljDIayBkr5OTAB3nH0CscDrWw1DWyMzBzrBvhe+q3eMmCfAKueRzGiKAkl1rXGozHmOoNr+CnLPKBeMjxZk/2fNtY1jKmZkWbhiQhue2awOl7aXTCcndne5xJLnG5JNybptc87YIQhAAhCEACcYBfXbml0sEk8oZE3M7oramw2GE5qqXO4H2I9QPVWUtlatSO4bHwqRrn+1IbqdE5uYJIHFH5d8oHLkuwcNju/IB4DVMpaE6e+yfTudc2dlCkBzj4+qhxytOYtsfEqRFJce0W+mi2kwolR3uLgjyKlRROkzFt7+KjRsDrX1vsbq7o4ow0NJABGubf0WqWzC60jIYrRyNmdI42AB1t10+qz1fLUxPnbDLIyKoI4kYcQ19jcBw2Nt/jzXplfQfiWBgeBY907H1WRxTC8khAL233IF2n0S2bA32hzxvKS6Zk6d89PKJqd7o3s2c06hauh7XPkjEOKwBzSLcRg1HmFDiwWoqI87AHMGl2tAHw2UefCpYtx3eu6xhZMfZvkrFk60XU1ZRynNSVObkAdLJk1QHcfrz2WedTyMkAbfMToOq0GBYU3EyYpKljJgPy2EkC/Q9SprPr2Vnxl8Eipjc+zBbmnKdkjpC9xtbW99kw/D30lc+mrWmB17NefdPIm3Lr4KskFV+IdHUZmSMcQ5m2UjkojMqZbJgaXs2eHRsjMcj542ZjYvzcvXX9FQds+178YibhlCOFhcJ7recpHvH6D9bq07O4I6vLmzSFodE4Ek7XFlg66A01XNAf9t5bv0Kvmp60Z+Pjh1v6hh1zuuIQlR0EIQgAQhCALuKqhaMjTlA91jb3S/xL3aQwZT1dqfik0tIxnsNz6e042UxsQPdJJ/oYLBMrYrTSZDe6pkGWaU/230+Gik0dnd25u3kGqXT0QlflGUabgXt6Jupw6RvfY1zm2vmdpf8AdWUtdmbtPo690jXaX8LhdjqnNd+ak0hnLS10wDvdD3fK64/8VGe+7NoNgNVff0prfRaw4iC2zfa5KbBiDw3nm52Wa/FgHWEtd4aq5oXNlyOZdwtqD1V4vv2ZZMel6LVs8sgD+8lVc5aGk0kbzbeVub4KTSui4LQRr8k+OG8EERk25pnjtexPbl+jAYlJiBqHSVzTVRg6FkhygdA0bfoE9RV9G4NEtJE4DYOcXA+htZaqsoqeZ1sjL25Cyy+K4PZ5dGCddwkMvjV7R1MPlS+ibPJhkjC3hCmzC22Uf+WyjR0L6ZzXwO7u4I5hUwdPTXjc4lvNpF07T1BjN6eV0P8ASPZ9WpGsbQ+siZvwIe02GCmqcrMRgH5Uh3eByPiqeWgfV0wrOFero3COuYfaLNmSjrbRp/4qupMVLXNc8cN4/wB6M9315harDsRFTVRVQjZJM1hZPHoW1UJFnDTfRZy3FJl6SuWibQSUuBwz1VcbQxxZ8zPeBOW3ndeL1TmvqJXMuGueS2+9ivTu3UD8M7P1tKx75IXujdC93vRF2Zp8+R8QV5cN/NPZq3oR8WNbYhC6d1xYDQIQhAAhCEAaxkZABmcQDpY7+gU2OnIZ37Qs5Zh3j6KNSuIs7KXvOoP8oT4c6QnvBwv3na5R5dfknkjmtsfjbG2xaQ1rTcue7dTSaaqZxHPfYDRpaLnxA2A8VAio2zOzO1tpd3ujwCntayJnDibby1P+VvCMMlIiyUMMkoDnNLCL75b/AK/PmolTQiAl9iH7BoBB+/BWMVDNNO1kXEzOOlhZSMRBoBmrmsY8i9s+aQ+PUBS4WtkLJ3ozUtHVOeOI890foEqmqJKOdhY53DBs6+xUo13GEgjgkdGTezeXgVHsat+Rkbg1ouc3ILCpXwYm39LmLFWcN3K5vddhxWPN3jlHVUUlIYtXbnS31+SOAfw73gZcgQnc+wcxXo1kVWx0gLJBlPXmlzZJoXtygOvyWSpaxrW3L7FW1HXl7b2uFtOXl0zC8Ll7RHxOgfxCGC+uqqpKItde1itMalkgy2sUzII93bFVeOWWnJa6Kemo3FwHyVph8FTSVAlonDuHNlvbXwP2Eh1QynNwLgJ1mIZC2Rh9oi4WN4cb6NZzZU9o3faGow/tD2RidVUklPUwjhzw5CAWnW7DtodR0uQvDcVoZMOrHQSHMPaY8Cwe06gheqHE31dA0MNg1wv8Fne1OHNqsPl4OUzUP5gA34btXN8rd4f9wclXJhUxpF8GeqyNUtbMCuJTuWqSlB8EIQgAQhCANTRyunYDMSWn3OvmpUtUGNyxnO/YhuzPvoqmniJtndcW71jZqlvyshNtuV2/Ick5LfwRqVssmYoC8RUEOd9gA6X2W+iscNp+M055M2XWSaQd1pPT9lX4ZQugtxg/iPGbhjQ/4G6vJLU1O2SvkbDGAQwHa/MNaP8APimce/bFMuvUoelqzSU0n4EOYA2z53Cz3f2/yj4qkwbCf9RldW1Yf+GDuv8A1D0J5efNJxDEnYlJHhtDCYYC7vFxHEf4nkPL58rdjnsYynja1oY2zWE7Acz081fat9+jLVY5/wBYrEZIKSkMVPGxhI0aw6NHUqmhaI6fQ3MpzOHM9PvxRicnFlEQN2usXO/m/wADkProHmMEbTIw3yaN5DP19N+t8qG9smVpEaZl3tY33L3PV3P9vROuhYygrWuNs7W5fO6UxrrhjWgXNhZM1bhmbGw34ZIc7qenoqM0XvRTSwOEmo0IvdKhbK1+VvRS6/8ALdDm9twuVyID8Q1x6aBLa7G1XQ62R0bMpNiQmXSTZ8jjfKUvOZJjn2DT6aW+qdkHsuIuQ0X81pr/AEpyX6K2UyOzA9UumbLmJfuNQL7hT2WLHki3eI28UpwbEGy7ZdSbbKvEORa4VJkDL5iy9iSdG9FE7RzyUEcNbCLgZoZGnnG65APSx/8AZKp5BFCWubc2It1J1CRijo66hmgfvI24cfdIsb/AH0V8i3GjLH1kTPPnW5bJKXKx0b3MeCHtJDgeRSFzmdYEoN0uus0XHFACSEIQgC+a821Nmjc9ExNiDcxZH7I1zdSquSR0mrj6LsTc7wP1Wjv9GSxL6aGkxnE5W9ycsaSLujYA4221UxrnEcWR8kkzxcPc4uIHUnfy/XoqqFwYBYXa3S3VPmcude/LUrea/Yvc99F5gkBfJLJGSSfy2Bp1PW1tf3VhVyR0zTHGQRvK/kSOX9ot6nrYWpMIxekpqR7JpZGSZnWcG3Nj08VFrcRdiEn5TOFTh3cZz0G58Vv+SVPQs8NVe2WVI9ssz5XbM0DQNSfv5qxsCGRBuUAWsOp3++llVYfIGsj1s49+/jy+icqsQELBFAfz3Gw/pHX4/BXm0kVqO+ifNNFTtDIiDPlJdbUMHL529VAa0Na8vNwBmd4nqm4g2NgbuXWc49enw19VGxCobcRt2bofPmPT6qtVpFojvQxNMZ6h0x56J1jvZPQ3UN1xsiOU5rHkl+XYzx6LOMWe55NgR9/RNOkyvNze+hCY41xdNyyZmFS7IUFnG5vD7vs7k9bpD5xq12xFvRRo6gCIAm2g+Sh1E4a19jewJUVZMx2WUNW1rckzrWsL9QEmCqL6dsjR3txfqNwoIfnYHWvcBR+N+Fc+wuzMD5fe3qo5lvx7GsXgHEL2crDXmLd0+fI+SrL635q5rHtyh18zB3T4tP8An5qpmZkeW722PVYV7N49CXm5SUIVDQEIQgAT0TspumV26lATWyrpqTazd1CzLodqrcinElxkHQb7lTon90NVZG+yktk0VpZSpLEVLgLMNiNL9B9/Ncp3XmLr300+pUJsl0kzXOVX5mfAtX1uRpEer+buijv7tmb5BYnxOp/b0UZkltemqDLc366qzvZCjRIc7LCSm2yXaB4KPNPrZN8bZUbNFJJc4t0Cb4hvqmpJ9U0ZlVsspJTp7jL1TUzrROP/ABUfi6hcmfm0UNllJY0suanZ4aJM7s128iLKDTy5AQuyT30Ub6I49gydzIzERcck3K/PbS1tEh2puuKmzQEIQgAQhCABdALjohCAOuta3NJQhACg6yUHoQpQAJEcRdQpI0dEqOMhCjbDQl0l0nOhCNknC5GZCFAC2gDUpDrX0QhAHEIQgAQhCABCEIAEIQgD/9k="

                };

                return View();
            }
            if (userModel != null && !string.IsNullOrEmpty(userModel.Username))
            {
              

                var claims = new List<Claim>
                   {
                        new Claim(ClaimTypes.Name, userModel.Username),
                        new Claim("UserId", userModel.Id.ToString())
                  };

                var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                     "CookieAuth",
                     new ClaimsPrincipal(identity),
                     new AuthenticationProperties
                     {
                         IsPersistent = true,
                         ExpiresUtc = DateTime.UtcNow.AddHours(2)
                     });

                ViewBag.User = userModel;
                return View();
            }

            return View();       
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        
    }
}
