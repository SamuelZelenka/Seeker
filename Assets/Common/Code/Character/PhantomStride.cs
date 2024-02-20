using UnityEngine;

[CreateAssetMenu(menuName = "Skills/PhantomStride")]
public class PhantomStride : Skill
{
	public override void Perform()
	{
		Debug.Log("Performed Skill");
	}
}
