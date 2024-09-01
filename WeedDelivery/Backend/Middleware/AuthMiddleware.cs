namespace WeedDelivery.Backend.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Пример логики промежуточного ПО для авторизации
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrEmpty(token))
            {
                // Обработка токена, если он присутствует
            }

            await _next(context);
        }
    }
}
