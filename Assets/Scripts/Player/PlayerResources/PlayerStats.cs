using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ResourcePool health = new ResourcePool(100f);
    public ResourcePool mana = new ResourcePool(75f);

    private void Update()
    {
        health.Regen(Time.deltaTime);
        TestingInput();
    }

    public void TestingInput()
    {
        if (InputManager.Instance.zKeyPressed)
        {
            health.Spend(10f);
            Debug.Log("Health: " + health.current);
        }

        if (InputManager.Instance.xKeyPressed)
        {
            if (mana.HasEnough(15f))
            {
                mana.Spend(15f);
                Debug.Log("Mana: " + mana.current);
            }
            else
            {
                Debug.Log("Not enough mana");
            }
        }
    }


}
