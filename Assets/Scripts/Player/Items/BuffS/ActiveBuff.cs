using Unity;

/// <summary>
/// TAKE A BUFFSO AND CONVERTS IT INTO A USABLE RUNTIME OBJECT 
/// ACTIVE BUFF WILL INSTANTIATE ON BUFF MANAGER AND ITS 
/// DATA CAN BE FED TO STATADJUSTMENTER. ALLOWS BUFFSO TO EXIST IN GAMEPLAY
/// </summary>

public class ActiveBuff
{
    public BuffSO data;           // which buff is this
    public float timeRemaining;   // how long until it expires
    public int stacks;            // how many times applied

    public ActiveBuff(BuffSO data)
    {
        ///CONSTRUCTOR GETS CALLAED IN BUFFMANAGER WHEN ACTIVEBUFF IS INSTANTIATED 
        ///IT IS INSTANTIATED OCNE THE BUFF IS ADDED TO THE ACTIVEBUFFLIST
        this.data = data;
        this.timeRemaining = data.duration;
        this.stacks = 1;
    }

    public float FlatTotal()
    {
        return data.flatValue * stacks;
    }

    public float PercentTotal()
    {
        return data.percentValue * stacks;
    }
}