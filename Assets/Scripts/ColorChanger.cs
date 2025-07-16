using UnityEngine;

public class ColorChanger
{
    public  void SetDefaultColor(Renderer renderer)
    {
        renderer.material.color = Color.white;
    }

    public  void SetRandomColor(Renderer renderer)
    {
        renderer.material.color = Random.ColorHSV();
    }
}
