using System.ComponentModel;
using Microsoft.SemanticKernel;

namespace CursoSK.Api.Plugins;

public class MathPlugin
{
    [KernelFunction("sumar")]
    [Description("Suma dos números")]
    public double Sumar(
        [Description("Primer número")] double a,
        [Description("Segundo número")] double b) => a + b;

    [KernelFunction("restar")]
    [Description("Resta dos números (a - b)")]
    public double Restar(
        [Description("Primer número")] double a,
        [Description("Segundo número")] double b) => a - b;

    [KernelFunction("multiplicar")]
    [Description("Multiplica dos números")]
    public double Multiplicar(
        [Description("Primer número")] double a,
        [Description("Segundo número")] double b) => a * b;

    [KernelFunction("dividir")]
    [Description("Divide dos números (a / b)")]
    public string Dividir(
        [Description("Dividendo")] double a,
        [Description("Divisor")] double b)
        => b == 0 ? "Error: no se puede dividir entre cero" : (a / b).ToString("F4");
}
