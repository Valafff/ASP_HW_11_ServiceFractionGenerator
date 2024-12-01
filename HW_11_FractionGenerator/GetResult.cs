namespace HW_11_FractionGenerator
{
	public class GetResult
	{
		RequestDelegate next;
		IReader reader;

        public GetResult(RequestDelegate next, IReader reader)
        {
			this.next = next;
			this.reader = reader;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Path == "/result")
				await context.Response.WriteAsync($"Fraction: {reader.ReadFraction()}");
			else
				await next.Invoke(context);
		}
	}
}
