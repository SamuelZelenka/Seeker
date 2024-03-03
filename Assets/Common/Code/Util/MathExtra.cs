public static class MathExtra
{
	public static int WrapModulo(int value, int max)
	{
		return (value % max + max) % max;
	}
}
