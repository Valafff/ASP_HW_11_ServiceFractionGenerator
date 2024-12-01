using Microsoft.AspNetCore.Mvc;
using HW_11_FractionGenerator;
using System;

var builder = WebApplication.CreateBuilder(args);

var value = new Value();

builder.Services.AddSingleton<IGeneration>(value);
builder.Services.AddSingleton<IReader>(value);

var app = builder.Build();

app.UseMiddleware<GetResult>();
app.UseMiddleware<GenMid>();

app.Run();


class GenMid
{
	RequestDelegate next;
	IGeneration generation;

	public GenMid(RequestDelegate next, IGeneration generation)
	{
		this.next = next;
		this.generation = generation;
	}
	public async Task InvokeAsync(HttpContext context)
	{
		if (context.Request.Path == "/generation")
			await context.Response.WriteAsync($"New fraction: {generation.GenerateFraction()}");
		else
			await next.Invoke(context);
	}
}

class Value : IGeneration, IReader
{
	string fraction = "none";
	string value = "";
	int numerator;
	int denominator;
	public string GenerateFraction()
	{
		numerator = new Random().Next(1, 1001);
		denominator = new Random().Next(1, 1001);
		int nod = GCD(numerator, denominator);
		numerator /= nod;
		denominator /= nod;

		if (numerator == denominator && numerator / denominator != 0)
		{
			return fraction = "1";
		}
		else if (numerator == 0)
		{
			return fraction = "0";
		}
		else if (numerator > denominator)
		{
			int value = numerator / denominator;
			int temp = numerator % denominator;
			if (temp != 0)
			{
				return $"{value} {temp}/{denominator}";
			}
			else
			{
				return $"{value}";
			}

		}
		else
		{
			return fraction = $"{numerator}/{denominator}";
		}
	}
	public string ReadFraction()
	{
		return fraction;
	}

	//алгоритм Евклида для нахождения НОД двух чисел
	private int GCD(int numerator, int denominator)
	{
		while (denominator != 0)
		{
			int t = denominator;
			denominator = numerator % denominator;
			numerator = t;
		}
		return numerator;
	}
}





