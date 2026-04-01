namespace datttwebapi.Config
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var handler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
                    await handler.HandleAsync(context);
                });
            });
        }
    }
}
